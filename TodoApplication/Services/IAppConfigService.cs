using System.IO;

namespace TodoApplication.Services
{
    internal interface IAppConfigService
    {
        FileInfo TodoItemFile { get; }

        FileInfo TagItemFile { get; }
    }
}
