using Autofac;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TodoApplication.Respositories;
using TodoApplication.Services;
using TodoApplication.ViewModels;

namespace TodoApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var log4netConfig = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config"));
            XmlConfigurator.Configure(log4netConfig);
        }
    }
}
