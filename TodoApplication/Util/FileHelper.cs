using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplication.Util
{
    static class FileHelper
    {
        public static async Task<string> ReadFileAsync(string fileName)
        {
            using (var streamReader = new StreamReader(File.OpenRead(fileName)))
            {
                return await streamReader.ReadToEndAsync();
            }
        }

        public static async Task WriteFileAsync(string fileName, string content)
        {
            using (var memStream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                using (var fileStream = File.Open(fileName, FileMode.Create))
                {
                    await memStream.CopyToAsync(fileStream);
                }
            }
        }
    }
}
