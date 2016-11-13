using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Virtion.ApkTool.Executor;
using Virtion.ApkTool.View;

namespace Virtion.ApkTool
{
    public class MainApp
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                App app = new App();
                app.InitializeComponent();
                app.Run();
            }
            else
            {
                string apkPath = args[0];
                if (string.IsNullOrEmpty(apkPath) == false)
                {
                    if (File.Exists(apkPath) == true)
                    {

                        // MessageBox.Show(apkPath);
                        var window = new LoadingWindow();

                        var apkSigner = new ApkSigner();
                        apkSigner.SetApkPath(apkPath);
                        apkSigner.Execute();
                        apkSigner.FinishCallBack += window.FinishCallBack;
                        App.LoggerCallBack += window.Log;
                        
                        window.ShowDialog();

                    }
                }
            }
        }
    }
}
