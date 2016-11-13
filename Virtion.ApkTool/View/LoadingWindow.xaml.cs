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
using System.Windows.Shapes;

namespace Virtion.ApkTool.View
{
    public partial class LoadingWindow : Window
    {
        public LoadingWindow()
        {
            InitializeComponent();
        }

        public void Log(string word)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.TB_Tip.Text = word + "\n";
            }));
        }

        internal void FinishCallBack()
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.Close();
            }));
        }
    }
}
