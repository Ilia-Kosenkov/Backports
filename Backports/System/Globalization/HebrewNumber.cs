#if NETSTANDARD2_0
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using Backports.System.Text;

namespace Backports.System.Globalization
{
    ////////////////////////////////////////////////////////////////////////////
    //
    // Used in HebrewNumber.ParseByChar to maintain the context information (
    // the state in the state machine and current Hebrew number values, etc.)
    // when parsing Hebrew number character by character.
    //
    ////////////////////////////////////////////////////////////////////////////

    internal struct HebrewNumberParsingContext
    {
        // The current state of the state machine for parsing Hebrew numbers.
        internal HebrewNumber.HS State;
        // The current value of the Hebrew number.
        // The final value is determined when state is FoundEndOfHebrewNumber.
        internal int Result;

        public HebrewNumberParsingContext(int result)
        {
            // Set the start state of the state machine for parsing Hebrew numbers.
            State = HebrewNumber.HS.Start;
            Result = result;
        }
    }

    ////////////////////////////////////////////////////////////////////////////
    //
    // Please see ParseByChar() for comments about different states defined here.
    //
    ////////////////////////////////////////////////////////////////////////////

    internal enum HebrewNumberParsingState
    {
        InvalidHebrewNumber,
        NotHebrewDigit,
        FoundEndOfHebrewNumber,
        ContinueParsing,
    }

    ////////////////////////////////////////////////////////////////////////////
    //
    // class HebrewNumber
    //
    //  Provides static methods for formatting integer values into
    //  Hebrew text and parsing Hebrew number text.
    //
    //  Limitations:
    //      Parse can only handle value 1 ~ 999.
    //      Append() can only handle 1 ~ 999. If value is greater than 5000,
    //      5000 will be subtracted from the value.
    //
    ////////////////////////////////////////////////////////////////////////////

    internal static class HebrewNumber
    {
        ////////////////////////////////////////////////////////////////////////////
        //
        //  Append
        //
        //  Converts the given number to Hebrew letters according to the numeric
        //  value of each Hebrew letter, appending to the supplied StringBuilder.
        //  Basically, this converts the lunar year and the lunar month to letters.
        //
        //  The character of a year is described by three letters of the Hebrew
        //  alphabet, the first and third giving, respectively, the days of the
        //  weeks on which the New Year occurs and Passover begins, while the
        //  second is the initial of the Hebrew word for defective, normal, or
        //  complete.
        //
        //  Defective Year : Both Heshvan and Kislev are defective (353 or 383 days)
        //  Normal Year    : Heshvan is defective, Kislev is full  (354 or 384 days)
        //  Complete Year  : Both Heshvan and Kislev are full      (355 or 385 days)
        //
        ////////////////////////////////////////////////////////////////////////////

        internal static void Append(ref ValueStringBuilder outputBuffer, int number)
        {
            var outputBufferStartingLength = outputBuffer.Length;

            var cTens = '\x0';

            //
            //  Adjust the number if greater than 5000.
            //
            if (number > 5000)
            {
                number -= 5000;
            }

            Debug.Assert(number > 0 && number <= 999, "Number is out of range.");

            //
            //  Get the Hundreds.
            //
            var hundreds = number / 100;

            if (hundreds > 0)
            {
                number -= hundreds * 100;
                // \x05e7 = 100
                // \x05e8 = 200
                // \x05e9 = 300
                // \x05ea = 400
                // If the number is greater than 400, use the multiples of 400.
                for (var i = 0; i < hundreds / 4; i++)
                {
                    outputBuffer.Append('\x05ea');
                }

                var remains = hundreds % 4;
                if (remains > 0)
                {
                    outputBuffer.Append((char)('\x05e6' + remains));
                }
            }

            //
            //  Get the Tens.
            //
            var tens = number / 10;
            number %= 10;

            cTens = tens switch
            {
                0 => '\x0',
                1 => '\x05d9' // Hebrew Letter Yod
               ,
                2 => '\x05db' // Hebrew Letter Kaf
               ,
                3 => '\x05dc' // Hebrew Letter Lamed
               ,
                4 => '\x05de' // Hebrew Letter Mem
               ,
                5 => '\x05e0' // Hebrew Letter Nun
               ,
                6 => '\x05e1' // Hebrew Letter Samekh
               ,
                7 => '\x05e2' // Hebrew Letter Ayin
               ,
                8 => '\x05e4' // Hebrew Letter Pe
               ,
                9 => '\x05e6' // Hebrew Letter Tsadi
               ,
                _ => cTens
            };

            //
            //  Get the Units.
            //
            var cUnits = (char)(number > 0 ? (int)'\x05d0' + number - 1 : 0);

            if (cUnits == '\x05d4' && // Hebrew Letter He  (5)
                cTens  == '\x05d9')
            {              // Hebrew Letter Yod (10)
                cUnits = '\x05d5';                 // Hebrew Letter Vav (6)
                cTens = '\x05d8';                 // Hebrew Letter Tet (9)
            }

            if (cUnits == '\x05d5' && // Hebrew Letter Vav (6)
                cTens  == '\x05d9')
            {               // Hebrew Letter Yod (10)
                cUnits = '\x05d6';                 // Hebrew Letter Zayin (7)
                cTens = '\x05d8';                 // Hebrew Letter Tet (9)
            }

            //
            //  Copy the appropriate info to the given buffer.
            //

            if (cTens != '\x0')
                outputBuffer.Append(cTens);

            if (cUnits != '\x0')
                outputBuffer.Append(cUnits);

            if (outputBuffer.Length - outputBufferStartingLength > 1)
                outputBuffer.Insert(outputBuffer.Length - 1, '"', 1);
            else
                outputBuffer.Append('\'');
        }

        ////////////////////////////////////////////////////////////////////////////
        //
        // Token used to tokenize a Hebrew word into tokens so that we can use in the
        // state machine.
        //
        ////////////////////////////////////////////////////////////////////////////

        // ReSharper disable InconsistentNaming
        private enum HebrewToken : short
        {
            Invalid = -1,
            Digit400 = 0,
            Digit200_300 = 1,
            Digit100 = 2,
            Digit10 = 3,    // 10 ~ 90
            Digit1 = 4,     // 1, 2, 3, 4, 5, 8,
            Digit6_7 = 5,
            Digit7 = 6,
            Digit9 = 7,
            SingleQuote = 8,
            DoubleQuote = 9,
        }
        // ReSharper restore InconsistentNaming

        ////////////////////////////////////////////////////////////////////////////
        //
        // This class is used to map a token into its Hebrew digit value.
        //
        ////////////////////////////////////////////////////////////////////////////

        private readonly struct HebrewValue
        {
            internal readonly HebrewToken Token;
            internal readonly short       Value;
            internal HebrewValue(HebrewToken token, short value)
            {
                this.Token = token;
                this.Value = value;
            }
        }

        //
        // Map a Hebrew character from U+05D0 ~ U+05EA to its digit value.
        // The value is -1 if the Hebrew character does not have a associated value.
        //
        private static readonly HebrewValue[] SHebrewValues = {
            new(HebrewToken.Digit1, 1),         // '\x05d0
            new(HebrewToken.Digit1, 2),         // '\x05d1
            new(HebrewToken.Digit1, 3),         // '\x05d2
            new(HebrewToken.Digit1, 4),         // '\x05d3
            new(HebrewToken.Digit1, 5),         // '\x05d4
            new(HebrewToken.Digit6_7, 6),       // '\x05d5
            new(HebrewToken.Digit6_7, 7),       // '\x05d6
            new(HebrewToken.Digit1, 8),         // '\x05d7
            new(HebrewToken.Digit9, 9),         // '\x05d8
            new(HebrewToken.Digit10, 10),       // '\x05d9; // Hebrew Letter Yod
            new(HebrewToken.Invalid, -1),       // '\x05da;
            new(HebrewToken.Digit10, 20),       // '\x05db; // Hebrew Letter Kaf
            new(HebrewToken.Digit10, 30),       // '\x05dc; // Hebrew Letter Lamed
            new(HebrewToken.Invalid, -1),       // '\x05dd;
            new(HebrewToken.Digit10, 40),       // '\x05de; // Hebrew Letter Mem
            new(HebrewToken.Invalid, -1),       // '\x05df;
            new(HebrewToken.Digit10, 50),       // '\x05e0; // Hebrew Letter Nun
            new(HebrewToken.Digit10, 60),       // '\x05e1; // Hebrew Letter Samekh
            new(HebrewToken.Digit10, 70),       // '\x05e2; // Hebrew Letter Ayin
            new(HebrewToken.Invalid, -1),       // '\x05e3;
            new(HebrewToken.Digit10, 80),       // '\x05e4; // Hebrew Letter Pe
            new(HebrewToken.Invalid, -1),       // '\x05e5;
            new(HebrewToken.Digit10, 90),       // '\x05e6; // Hebrew Letter Tsadi
            new(HebrewToken.Digit100, 100),     // '\x05e7;
            new(HebrewToken.Digit200_300, 200), // '\x05e8;
            new(HebrewToken.Digit200_300, 300), // '\x05e9;
            new(HebrewToken.Digit400, 400),     // '\x05ea;
        };

        private const int MinHebrewNumberCh = 0x05d0;
        private static readonly char SMaxHebrewNumberCh = (char)(MinHebrewNumberCh + SHebrewValues.Length - 1);

        ////////////////////////////////////////////////////////////////////////////
        //
        // Hebrew number parsing State
        // The current state and the next token will lead to the next state in the state machine.
        // DQ = Double Quote
        //
        ////////////////////////////////////////////////////////////////////////////

        // ReSharper disable InconsistentNaming
        internal enum HS : sbyte
        {
            _err = -1,          // an error state
            Start = 0,
            S400 = 1,           // a Hebrew digit 400
            S400_400 = 2,       // Two Hebrew digit 400
            S400_X00 = 3,       // Two Hebrew digit 400 and followed by 100
            S400_X0 = 4,       // Hebrew digit 400 and followed by 10 ~ 90
            X00_DQ = 5,         // A hundred number and followed by a double quote.
            S400_X00_X0 = 6,
            X0_DQ = 7,          // A two-digit number and followed by a double quote.
            X = 8,              // A single digit Hebrew number.
            X0 = 9,            // A two-digit Hebrew number
            X00 = 10,           // A three-digit Hebrew number
            S400_DQ = 11,       // A Hebrew digit 400 and followed by a double quote.
            S400_400_DQ = 12,
            S400_400_100 = 13,
            S9 = 14,            // Hebrew digit 9
            X00_S9 = 15,        // A hundred number and followed by a digit 9
            S9_DQ = 16,         // Hebrew digit 9 and followed by a double quote
            END = 100,          // A terminal state is reached.
        }
        // ReSharper restore InconsistentNaming

        //
        // The state machine for Hebrew number parsing.
        //
        private static readonly HS[] SNumberParsingState =
        {
            // 400            300/200         100             90~10           8~1      6,       7,       9,          '           "
            /* 0 */
                             HS.S400,       HS.X00,         HS.X00,          HS.X0,          HS.X,    HS.X,    HS.X,    HS.S9,      HS._err,    HS._err,
            /* 1: S400 */
                             HS.S400_400,   HS.S400_X00,    HS.S400_X00,     HS.S400_X0,     HS._err, HS._err, HS._err, HS.X00_S9, HS.END,     HS.S400_DQ,
            /* 2: S400_400 */
                             HS._err,       HS._err,        HS.S400_400_100, HS.S400_X0,     HS._err, HS._err, HS._err, HS.X00_S9, HS._err,    HS.S400_400_DQ,
            /* 3: S400_X00 */
                             HS._err,       HS._err,        HS._err,         HS.S400_X00_X0, HS._err, HS._err, HS._err, HS.X00_S9, HS._err,    HS.X00_DQ,
            /* 4: S400_X0 */
                             HS._err,       HS._err,        HS._err,         HS._err,        HS._err, HS._err, HS._err, HS._err,    HS._err,    HS.X0_DQ,
            /* 5: X00_DQ */
                             HS._err,       HS._err,        HS._err,         HS.END,         HS.END,  HS.END,  HS.END,  HS.END,     HS._err,    HS._err,
            /* 6: S400_X00_X0 */
                             HS._err,       HS._err,        HS._err,         HS._err,        HS._err, HS._err, HS._err, HS._err,    HS._err,    HS.X0_DQ,
            /* 7: X0_DQ */
                             HS._err,       HS._err,        HS._err,         HS._err,        HS.END,  HS.END,  HS.END,  HS.END,     HS._err,    HS._err,
            /* 8: X */
                             HS._err,       HS._err,        HS._err,         HS._err,        HS._err, HS._err, HS._err, HS._err,    HS.END,     HS._err,
            /* 9: X0 */
                             HS._err,       HS._err,        HS._err,         HS._err,        HS._err, HS._err, HS._err, HS._err,    HS.END,     HS.X0_DQ,
            /* 10: X00 */
                             HS._err,       HS._err,        HS._err,         HS.S400_X0,     HS._err, HS._err, HS._err, HS.X00_S9,  HS.END,     HS.X00_DQ,
            /* 11: S400_DQ */
                             HS.END,        HS.END,         HS.END,          HS.END,         HS.END,  HS.END,  HS.END,  HS.END,     HS._err,    HS._err,
            /* 12: S400_400_DQ*/
                             HS._err,       HS._err,        HS.END,          HS.END,         HS.END,  HS.END,  HS.END,  HS.END,     HS._err,    HS._err,
            /* 13: S400_400_100*/
                             HS._err,       HS._err,        HS._err,         HS.S400_X00_X0, HS._err, HS._err, HS._err, HS.X00_S9,  HS._err,    HS.X00_DQ,
            /* 14: S9 */
                             HS._err,       HS._err,        HS._err,         HS._err,        HS._err, HS._err, HS._err, HS._err,    HS.END,     HS.S9_DQ,
            /* 15: X00_S9 */
                             HS._err,       HS._err,        HS._err,         HS._err,        HS._err, HS._err, HS._err, HS._err,    HS._err,    HS.S9_DQ,
            /* 16: S9_DQ */
                             HS._err,       HS._err,        HS._err,         HS._err,        HS._err, HS.END,  HS.END,  HS._err,    HS._err,    HS._err
        };

        // Count of valid HebrewToken, column count in the NumberParsingState array
        private const int HebrewTokenCount = 10;

        ////////////////////////////////////////////////////////////////////////
        //
        //  Actions:
        //      Parse the Hebrew number by passing one character at a time.
        //      The state between characters are maintained at HebrewNumberParsingContext.
        //  Returns:
        //      Return a enum of HebrewNumberParsingState.
        //          NotHebrewDigit: The specified ch is not a valid Hebrew digit.
        //          InvalidHebrewNumber: After parsing the specified ch, it will lead into
        //              an invalid Hebrew number text.
        //          FoundEndOfHebrewNumber: A terminal state is reached.  This means that
        //              we find a valid Hebrew number text after the specified ch is parsed.
        //          ContinueParsing: The specified ch is a valid Hebrew digit, and
        //              it will lead into a valid state in the state machine, we should
        //              continue to parse incoming characters.
        //
        ////////////////////////////////////////////////////////////////////////

        internal static HebrewNumberParsingState ParseByChar(char ch, ref HebrewNumberParsingContext context)
        {
            Debug.Assert(SNumberParsingState.Length == HebrewTokenCount * ((int)HS.S9_DQ + 1));

            HebrewToken token;
            if (ch == '\'')
            {
                token = HebrewToken.SingleQuote;
            }
            else if (ch == '\"')
            {
                token = HebrewToken.DoubleQuote;
            }
            else
            {
                var index = (int)ch - MinHebrewNumberCh;
                if (index >= 0 && index < SHebrewValues.Length)
                {
                    token = SHebrewValues[index].Token;
                    if (token == HebrewToken.Invalid)
                    {
                        return HebrewNumberParsingState.NotHebrewDigit;
                    }
                    context.Result += SHebrewValues[index].Value;
                }
                else
                {
                    // Not in valid Hebrew digit range.
                    return HebrewNumberParsingState.NotHebrewDigit;
                }
            }
            context.State = SNumberParsingState[(int)context.State * (int)HebrewTokenCount + (int)token];
            if (context.State == HS._err)
                // Invalid Hebrew state.  This indicates an incorrect Hebrew number.
                return HebrewNumberParsingState.InvalidHebrewNumber;
            if (context.State == HS.END)
                // Reach a terminal state.
                return HebrewNumberParsingState.FoundEndOfHebrewNumber;
            // We should continue to parse.
            return HebrewNumberParsingState.ContinueParsing;
        }

        ////////////////////////////////////////////////////////////////////////
        //
        // Actions:
        //  Check if the ch is a valid Hebrew number digit.
        //  This function will return true if the specified char is a legal Hebrew
        //  digit character, single quote, or double quote.
        // Returns:
        //  true if the specified character is a valid Hebrew number character.
        //
        ////////////////////////////////////////////////////////////////////////

        internal static bool IsDigit(char ch)
        {
            if (ch >= MinHebrewNumberCh && ch <= SMaxHebrewNumberCh)
            {
                return SHebrewValues[ch - MinHebrewNumberCh].Value >= 0;
            }
            return ch == '\'' || ch == '\"';
        }
    }
}
#endif