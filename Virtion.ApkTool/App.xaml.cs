using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Virtion.ApkTool
{

    public partial class App : Application
    {
        public static string CurrentPath
        {
            get
            {
                return System.Environment.CurrentDirectory;
            }
        }

        public static new MainWindow MainWindow
        {
            get
            {
              return   App.Current.MainWindow as MainWindow;
            }
        }

        public static void Log(string word)
        {
            MainWindow.Log(word);
        }

    }

}
