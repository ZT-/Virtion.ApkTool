using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Virtion.ApkTool.Executor;

namespace Virtion.ApkTool.View
{
    public partial class SignerPage : UserControl
    {
        private ApkSigner apkSigner;

        public SignerPage()
        {
            InitializeComponent();
        }

        private void TB_ApkPath_PreviewDragEnter(object sender, DragEventArgs e)
        {
            e.Handled = true;
        }

        private void TB_ApkPath_PreviewDrop(object sender, DragEventArgs e)
        {
            object text = e.Data.GetData(DataFormats.FileDrop);
            TextBox tb = sender as TextBox;
            tb.Text = "";
            ApkSigner.ApkList.Clear();

            if (tb != null)
            {
                var fileList = text as string[];

                foreach (var item in fileList)
                {
                    if (item.EndsWith(".apk") == true)
                    {
                        ApkSigner.ApkList.Add(item);
                        tb.Text += item + "\n";
                    }
                }
                //this.E_Console.IsExpanded = true;
                //this.TB_Console.Text = "识别到的apk数量：" + ApkSigner.ApkList.Count;
            }
            apkSigner.AutoSignAll();
        }

        private void B_OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                apkSigner.OutputFolder = folder.SelectedPath;
                this.TB_Output.Text = folder.SelectedPath;
            }
        }

        private void E_SignerOption_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ApkSigner.IsAutoMake = !this.E_SignerOption.IsExpanded;
        }

        private void TB_SignerMake_Click(object sender, RoutedEventArgs e)
        {
            apkSigner.SignAll();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.apkSigner = new ApkSigner();
        }
    }
}
