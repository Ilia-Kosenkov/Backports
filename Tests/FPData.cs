#nullable enable
using System;
using System.Globalization;

namespace Tests
{
    // ReSharper disable once InconsistentNaming
    public readonly struct FPData
    {
        private static int    SizeofHalf  { get; } = sizeof(float) / 2;
        public         byte[] HalfBytes   { get; }
        public         byte[] SingleBytes { get; }
        public         byte[] DoubleBytes { get; }

        public string StrRep { get; }

        public FPData(ReadOnlySpan<byte> half, ReadOnlySpan<byte> single, ReadOnlySpan<byte> @double, string strRep)
        {
            HalfBytes = new byte[SizeofHalf];
            SingleBytes = new byte[sizeof(float)];
            DoubleBytes = new byte[sizeof(double)];

            half.CopyTo(HalfBytes);
            single.CopyTo(SingleBytes);
            @double.CopyTo(DoubleBytes);
            StrRep = strRep;
        }

#if NETCOREAPP3_0_OR_GREATER

        public static FPData FromString(string input)
        {
            if (input.Length < 32)
            {
                throw new ArgumentException("Input string too short.", nameof(input));
            }

            var chars = input.AsSpan();
            Span<byte> buff = stackalloc byte[SizeofHalf + sizeof(float) + sizeof(double)];
            var strRep = chars[31..].Trim().ToString();
            if (!BitConverter.IsLittleEndian)
            {

                buff[13] = byte.Parse(chars[28..30], NumberStyles.HexNumber);
                buff[12] = byte.Parse(chars[26..28], NumberStyles.HexNumber);
                buff[11] = byte.Parse(chars[24..26], NumberStyles.HexNumber);
                buff[10] = byte.Parse(chars[22..24], NumberStyles.HexNumber);
                buff[9] = byte.Parse(chars[20..22], NumberStyles.HexNumber); 
                buff[8] = byte.Parse(chars[18..20], NumberStyles.HexNumber);
                buff[7] = byte.Parse(chars[16..18], NumberStyles.HexNumber);
                buff[6] = byte.Parse(chars[14..16], NumberStyles.HexNumber);
                
                buff[5] = byte.Parse(chars[11..13], NumberStyles.HexNumber);
                buff[4] = byte.Parse(chars[9..11], NumberStyles.HexNumber);
                buff[3] = byte.Parse(chars[7..9], NumberStyles.HexNumber);
                buff[2] = byte.Parse(chars[5..7], NumberStyles.HexNumber);

                buff[1] = byte.Parse(chars[2..4], NumberStyles.HexNumber);
                buff[0] = byte.Parse(chars[..2], NumberStyles.HexNumber);

            }
            else
            {
                // Default arch
                buff[13] = byte.Parse(chars[14..16], NumberStyles.HexNumber);
                buff[12] = byte.Parse(chars[16..18], NumberStyles.HexNumber);
                buff[11] = byte.Parse(chars[18..20], NumberStyles.HexNumber);
                buff[10] = byte.Parse(chars[20..22], NumberStyles.HexNumber);
                buff[9] = byte.Parse(chars[22..24], NumberStyles.HexNumber);
                buff[8] = byte.Parse(chars[24..26], NumberStyles.HexNumber);
                buff[7] = byte.Parse(chars[26..28], NumberStyles.HexNumber);
                buff[6] = byte.Parse(chars[28..30], NumberStyles.HexNumber);

                buff[5] = byte.Parse(chars[5..7], NumberStyles.HexNumber);
                buff[4] = byte.Parse(chars[7..9], NumberStyles.HexNumber);
                buff[3] = byte.Parse(chars[9..11], NumberStyles.HexNumber);
                buff[2] = byte.Parse(chars[11..13], NumberStyles.HexNumber);

                buff[1] = byte.Parse(chars[..2], NumberStyles.HexNumber);
                buff[0] = byte.Parse(chars[2..4], NumberStyles.HexNumber);
            }
            return new FPData(buff[..2], buff[2..6], buff[6..14], strRep);
        }
#else
        public static FPData FromString(string input)
        {
            if (input.Length < 32)
            {
                throw new ArgumentException("Input string too short.", nameof(input));
            }

            var chars = input.AsSpan();
            Span<byte> buff = stackalloc byte[SizeofHalf + sizeof(float) + sizeof(double)];
            var strRep = chars.Slice(31).Trim().ToString();
            if (!BitConverter.IsLittleEndian)
            {
                buff[13] = byte.Parse(chars.Slice(28, 2).ToString(), NumberStyles.HexNumber);
                buff[12] = byte.Parse(chars.Slice(26, 2).ToString(), NumberStyles.HexNumber);
                buff[11] = byte.Parse(chars.Slice(24, 2).ToString(), NumberStyles.HexNumber);
                buff[10] = byte.Parse(chars.Slice(22, 2).ToString(), NumberStyles.HexNumber);
                buff[9] = byte.Parse(chars.Slice(20, 2).ToString(), NumberStyles.HexNumber);
                buff[8] = byte.Parse(chars.Slice(18, 2).ToString(), NumberStyles.HexNumber);
                buff[7] = byte.Parse(chars.Slice(16, 2).ToString(), NumberStyles.HexNumber);
                buff[6] = byte.Parse(chars.Slice(14, 2).ToString(), NumberStyles.HexNumber);

                buff[5] = byte.Parse(chars.Slice(11, 2).ToString(), NumberStyles.HexNumber);
                buff[4] = byte.Parse(chars.Slice(9, 2).ToString(), NumberStyles.HexNumber);
                buff[3] = byte.Parse(chars.Slice(7, 2).ToString(), NumberStyles.HexNumber);
                buff[2] = byte.Parse(chars.Slice(5, 2).ToString(), NumberStyles.HexNumber);

                buff[1] = byte.Parse(chars.Slice(2, 2).ToString(), NumberStyles.HexNumber);
                buff[0] = byte.Parse(chars.Slice(0, 2).ToString(), NumberStyles.HexNumber);

            }
            else
            {
                // Default arch

                buff[13] = byte.Parse(chars.Slice(14, 2).ToString(), NumberStyles.HexNumber);
                buff[12] = byte.Parse(chars.Slice(16, 2).ToString(), NumberStyles.HexNumber);
                buff[11] = byte.Parse(chars.Slice(18, 2).ToString(), NumberStyles.HexNumber);
                buff[10] = byte.Parse(chars.Slice(20, 2).ToString(), NumberStyles.HexNumber);
                buff[9] = byte.Parse(chars.Slice(22, 2).ToString(), NumberStyles.HexNumber);
                buff[8] = byte.Parse(chars.Slice(24, 2).ToString(), NumberStyles.HexNumber);
                buff[7] = byte.Parse(chars.Slice(26, 2).ToString(), NumberStyles.HexNumber);
                buff[6] = byte.Parse(chars.Slice(28, 2).ToString(), NumberStyles.HexNumber);

                buff[5] = byte.Parse(chars.Slice(5, 2).ToString(), NumberStyles.HexNumber);
                buff[4] = byte.Parse(chars.Slice(7, 2).ToString(), NumberStyles.HexNumber);
                buff[3] = byte.Parse(chars.Slice(9, 2).ToString(), NumberStyles.HexNumber);
                buff[2] = byte.Parse(chars.Slice(11, 2).ToString(), NumberStyles.HexNumber);

                buff[1] = byte.Parse(chars.Slice(0, 2).ToString(), NumberStyles.HexNumber);
                buff[0] = byte.Parse(chars.Slice(2, 2).ToString(), NumberStyles.HexNumber);
            }
            return new FPData(buff.Slice(0, SizeofHalf), buff.Slice(2, sizeof(float)), buff.Slice(6, sizeof(double)), strRep);
        }
#endif
    }
}
