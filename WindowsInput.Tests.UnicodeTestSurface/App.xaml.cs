using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WindowsInput.Tests.UnicodeTestSurface
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Filename = e.Args[0];
            if (File.Exists(Filename) == false) throw new FileNotFoundException("The expected file was not found", Filename);
        }

        public static string Filename { get; set; }
    }
}
