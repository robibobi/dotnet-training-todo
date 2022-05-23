using System;
using System.IO;

namespace TodoApplication.Services
{
    internal class AppConfigService : IAppConfigService
    {
        const string TodoFileName = "todoItems.json";
        const string TagFileName = "tags.json";

        public FileInfo TodoItemFile { get; }
        public FileInfo TagItemFile { get; }

        public AppConfigService()
        { 
            TodoItemFile = GetTodoItemFile();
            TagItemFile = GetTagItemFile();
        }

        private FileInfo GetTagItemFile()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = System.IO.Path.Combine(path, "TodoApplication", TagFileName);
            return new FileInfo(path);
        }

        private FileInfo GetTodoItemFile()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = System.IO.Path.Combine(path, "TodoApplication", TodoFileName);
            return new FileInfo(path);
        }
    }
}
