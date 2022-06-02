using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplication.Util
{
    static class FileHelper
    {
        public static async Task<Result<string>> ReadFileAsync(string fileName)
        {
            try
            {
                using (var streamReader = new StreamReader(File.OpenRead(fileName)))
                {
                    var fileContent = await streamReader.ReadToEndAsync();
                    return Result.CreateSuccess(fileContent);
                }
            } 
            catch(Exception ex)
            {
                return Result.CreateError<string>($"Failed to read file '{fileName}'. {ex.Message}");
            }
        }

        public static async Task<Result<Unit>> WriteFileAsync(string fileName, string content)
        {
            try
            {
                using (var memStream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
                {
                    using (var fileStream = File.Open(fileName, FileMode.Create))
                    {
                        await memStream.CopyToAsync(fileStream);
                        return Result.CreateSuccess();
                    }
                }
            } 
            catch(Exception ex)
            {
                return Result.CreateError($"Failed to write file '{fileName}'. {ex.Message}");
            }
        }
    }
}
