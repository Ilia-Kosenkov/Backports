using System;
using System.Collections.Generic;
using System.IO;


namespace Tests
{
    // ReSharper disable once InconsistentNaming
    internal class FPDataSetProvider
    {
        private readonly string _path;

        public FPDataSetProvider(string path)
        {   
            _ = path ?? throw new ArgumentNullException(nameof(path));
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(nameof(path), path);
            }
            
            _path = path;
        }

        public async IAsyncEnumerable<FPData> ReadTestData()
        {
            using var reader = new StreamReader(new FileStream(_path, FileMode.Open, FileAccess.Read));
            while (!reader.EndOfStream)
            {
                var str = await reader.ReadLineAsync();
                if(str is not null)
                {
                    yield return FPData.FromString(str);
                }
                    
            }
        }

    }
}
