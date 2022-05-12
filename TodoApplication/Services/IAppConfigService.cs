using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplication.Services
{
    internal interface IAppConfigService
    {
        FileInfo TodoItemFile { get; }
    }
}
