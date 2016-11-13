using System;
using System.Diagnostics;
using System.Windows;

namespace Virtion.ApkTool
{
    public partial class App : Application
    {
        public static Action<string> LoggerCallBack;

        public static string CurrentPath
        {
            get
            {
                return System.AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public static string CurrentExe
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.FileName;
            }
        }

        public new static MainWindow MainWindow
        {
            get
            {
                return App.Current.MainWindow as MainWindow;
            }
        }

        public static void Log(string word)
        {
            if (App.Current != null)
            {
                MainWindow.Log(word);
            }
            else if (LoggerCallBack != null)
            {
                LoggerCallBack(word);
            }
        }

    }
}
