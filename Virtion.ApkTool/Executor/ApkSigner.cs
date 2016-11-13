using IKVM.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace Virtion.ApkTool.Executor
{
    public class ApkSigner : CommondExecutor
    {
        public static List<string> ApkList = new List<string>();
        public static bool IsAutoMake = true;
        private string apkPath;

        public void AutoSignAll()
        {
            if (IsAutoMake == true)
            {
                foreach (var item in ApkList)
                {
                    try
                    {
                        this.apkPath = item;
                        this.Execute();
                    }
                    catch (Exception ex)
                    {
                        App.Log(ex.ToString());
                    }
                }
            }
        }

        public void SignAll()
        {
            foreach (var item in ApkList)
            {
                try
                {
                    this.apkPath = item;
                    this.Execute();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    //throw;
                }
            }
        }

        private String GetSourceApkSize(string srcApk)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(srcApk);
            return file.Length.ToString();
        }

        private String GetDestApkSize(string destApk)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(destApk);
            return file.Length.ToString();
        }


        public void SetApkPath(string fileFullPath)
        {
            this.apkPath = fileFullPath;
        }

        public override bool Execute()
        {
            String fileFullPath = this.apkPath;

            App.Log("signing apk :" + fileFullPath);

            if (System.IO.File.Exists(fileFullPath) == false)
            {
                MessageBox.Show("文件路径不存在");
                return false;
            }

            FileInfo fileInfo = new FileInfo(fileFullPath);
            string apkFileName = fileInfo.Name.Replace(fileInfo.Extension, "");

            string apkDirectory;
            if (string.IsNullOrEmpty(OutputFolder) == true)
            {
                apkDirectory = fileInfo.DirectoryName;
            }
            else
            {
                if (System.IO.Directory.Exists(OutputFolder) == false)
                {
                    MessageBox.Show("文件路径不存在" + OutputFolder);
                    return false;
                }
                apkDirectory = OutputFolder;
            }

            string outputApk = string.Concat(new string[]
				{
					apkDirectory,
					"\\",
					apkFileName,
					"-sign",
					fileInfo.Extension
				});

            string[] cmd = new string[]
			{
				"-jar",
                App.CurrentPath+  "lib\\signapk.dll",
				App.CurrentPath+ "key\\publickey.x509.pem",
				App.CurrentPath+ "key\\privatekey.pk8",
				fileFullPath,
                outputApk
			};

            Thread thread = new Thread(() =>
            {
                try
                {
                    int num = Starter.StartMain(cmd);
                    if (num == 0)
                    {
                        if (this.FinishCallBack!=null)
                        {
                            this.FinishCallBack();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            });
            thread.Start();

            return true;
        }

    }
}
