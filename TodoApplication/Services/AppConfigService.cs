using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace TodoApplication.Services
{
    internal class AppConfigService : IAppConfigService
    {
        const string TodoFileName = "todoItems.json";

        public FileInfo TodoItemFile { get; }

        public AppConfigService()
        { 
            TodoItemFile = GetTodoItemFile();
        }

        private FileInfo GetTodoItemFile()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = System.IO.Path.Combine(path, "TodoApplication", TodoFileName);
            return new FileInfo(path);
        }
    }
}
