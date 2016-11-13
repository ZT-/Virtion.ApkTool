using System.Windows;
using System.Windows.Controls;
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
            TextBox textBox = sender as TextBox;
            ApkSigner.ApkList.Clear();
            if (textBox != null)
            {
                textBox.Text = "";
                var fileList = text as string[];
                if (fileList == null)
                {
                    return;
                }

                foreach (var item in fileList)
                {
                    if (item.EndsWith(".apk") == true)
                    {
                        ApkSigner.ApkList.Add(item);
                        textBox.Text += item + "\n";
                    }
                }

                //this.E_Console.IsExpanded = true;
                //this.TB_Console.Text = "识别到的apk数量：" + ApkSigner.ApkList.Count;
            }
            this.apkSigner.AutoSignAll();
        }

        private void B_OpenFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folder = new System.Windows.Forms.FolderBrowserDialog();
            if (folder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.apkSigner.OutputFolder = folder.SelectedPath;
                this.TB_Output.Text = folder.SelectedPath;
            }
        }

        private void E_SignerOption_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ApkSigner.IsAutoMake = !this.E_SignerOption.IsExpanded;
        }

        private void TB_SignerMake_Click(object sender, RoutedEventArgs e)
        {
            this.apkSigner.SignAll();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.apkSigner = new ApkSigner();
        }
    }
}
