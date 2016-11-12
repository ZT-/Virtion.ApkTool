using IWshRuntimeLibrary;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace DiPiPiApk
{
	public class WinMain : Form
	{
		private delegate int startCommandCallback(string[] cmd);

		private delegate void loadingStopSync(LoadingGraph lg);

		private IContainer components = null;

		private MenuStrip menuStrip1;

		private ToolStripMenuItem 帮助ToolStripMenuItem;

		private ToolStripMenuItem 在线帮助ToolStripMenuItem;

		private ToolStripMenuItem 关于ToolStripMenuItem;

		private TabControl tabControl1;

		private TabPage tabPage1;

		private Button tb1_btn_StartDecompiler;

		private Button tb1_btn_output_dir;

		private Button tb1_btn_apkfile;

		private TextBox tb1_txt_output_dir;

		private TextBox tb1_txt_apk_path;

		private Label label2;

		private Label label1;

		private TabPage tabPage2;

		private Button tb2_btn_StartSmaliToApk;

		private Button tb2_btn_smali_dir;

		private Button tb2_btn_apkfile;

		private TextBox tb2_txt_smali_dir;

		private TextBox tb2_txt_apk_path;

		private Label label3;

		private Label label4;

		private TabPage tabPage3;

		private Button tb3_btn_StartSign;

		private Button tb3_btn_apkfile;

		private TextBox tb3_txt_apk_path;

		private Label label6;

		private OpenFileDialog openFileDialog;

		private FolderBrowserDialog dirDialog;

		private SaveFileDialog saveFileDialog;

		private Label label5;

		private Label label7;

		private Label label8;

		private Label label9;

		private ToolStripMenuItem 创建桌面快捷方式ToolStripMenuItem;

		private TabPage tabPage4;

		private RichTextBox richTextBox1;

		private LoadingGraph tab1_loading;

		private LoadingGraph tab2_loading;

		private LoadingGraph tab3_loading;

		public WinMain()
		{
			this.InitializeComponent();
		}

		private void 在线帮助ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Process.Start("iexplore.exe", "http://taven.cnblogs.com");
		}

		private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new AboutMe().ShowDialog();
		}

		private void initForm()
		{
		}

		public int startCommand(IAsyncResult ar, string[] cmd)
		{
			return Starter.StartMain(cmd);
		}

		public void loadingStop(LoadingGraph ctr)
		{
			if (ctr.InvokeRequired)
			{
				WinMain.loadingStopSync method = new WinMain.loadingStopSync(this.loadingStop);
				base.Invoke(method, new object[]
				{
					ctr
				});
			}
			else
			{
				ctr.Visible = false;
				ctr.Stop();
			}
		}

		private void tb1_txt_apk_path_DragDrop(object sender, DragEventArgs e)
		{
			((TextBox)sender).Text = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
		}

		private void tb1_txt_apk_path_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Link;
		}

		private void tb1_txt_output_dir_DragDrop(object sender, DragEventArgs e)
		{
			((TextBox)sender).Text = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
		}

		private void tb1_txt_output_dir_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Link;
		}

		private void tb1_btn_output_dir_Click(object sender, EventArgs e)
		{
			DialogResult dialogResult = this.dirDialog.ShowDialog(this);
			string selectedPath = this.dirDialog.SelectedPath;
			if (!string.IsNullOrEmpty(selectedPath))
			{
				this.tb1_txt_output_dir.Text = selectedPath;
			}
		}

		private void tb1_btn_apkfile_Click(object sender, EventArgs e)
		{
			this.openFileDialog.Filter = "dex文件|*.dex|安卓程序|*.apk";
			DialogResult dialogResult = this.openFileDialog.ShowDialog(this);
			string fileName = this.openFileDialog.FileName;
			if (!string.IsNullOrEmpty(fileName))
			{
				this.tb1_txt_apk_path.Text = fileName;
			}
		}

		private void tb1_btn_StartDecompiler_Click(object sender, EventArgs e)
		{
			string text = this.tb1_txt_apk_path.Text;
			if (string.IsNullOrEmpty(text))
			{
				MessageBox.Show("文件路径不能为空");
			}
			else
			{
				string text2 = this.tb1_txt_output_dir.Text ?? "";
				if (string.IsNullOrEmpty(text2))
				{
					MessageBox.Show("输出路径不能为空");
				}
				else if (System.IO.File.Exists(text))
				{
					FileInfo fileInfo = new FileInfo(text);
					text2 = text2 + "\\" + fileInfo.Name.Replace(fileInfo.Extension, "");
					if (Directory.Exists(text2))
					{
					}
					this.tab1_loading.Visible = true;
					this.tab1_loading.Start();
					string[] cmd = new string[]
					{
						"-jar",
						Application.StartupPath + "\\lib\\baksmali.dll",
						"-o",
						text2,
						text
					};
					int result = -1;
					Thread thread = new Thread(delegate
					{
						result = Starter.StartMain(cmd);
						this.loadingStop(this.tab1_loading);
						if (result == 0)
						{
							MessageBox.Show("反编译成功", "消息提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						}
					});
					thread.Start();
					while (result != -1)
					{
						this.tab1_loading.Visible = false;
						this.tab1_loading.Stop();
					}
				}
				else
				{
					MessageBox.Show("文件路径不存在");
				}
			}
		}

		private void tb2_txt_smali_dir_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Link;
		}

		private void tb2_txt_smali_dir_DragDrop(object sender, DragEventArgs e)
		{
			((TextBox)sender).Text = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
		}

		private void tb2_txt_apk_path_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Link;
		}

		private void tb2_txt_apk_path_DragDrop(object sender, DragEventArgs e)
		{
			((TextBox)sender).Text = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
		}

		private void tb2_btn_smali_dir_Click(object sender, EventArgs e)
		{
			DialogResult dialogResult = this.dirDialog.ShowDialog(this);
			string selectedPath = this.dirDialog.SelectedPath;
			if (!string.IsNullOrEmpty(selectedPath))
			{
				this.tb2_txt_smali_dir.Text = selectedPath;
			}
		}

		private void tb2_btn_apkfile_Click(object sender, EventArgs e)
		{
			this.saveFileDialog.Filter = "dex文件|*.dex";
			DialogResult dialogResult = this.saveFileDialog.ShowDialog(this);
			string fileName = this.saveFileDialog.FileName;
			if (!string.IsNullOrEmpty(fileName))
			{
				this.tb2_txt_apk_path.Text = fileName;
			}
		}

		private void tb2_btn_StartSmaliToApk_Click(object sender, EventArgs e)
		{
			string text = this.tb2_txt_smali_dir.Text ?? "";
			if (string.IsNullOrEmpty(text))
			{
				MessageBox.Show("Smali文件夹路径不能为空");
			}
			else if (!Directory.Exists(text))
			{
				MessageBox.Show("文件夹不存在");
			}
			else
			{
				string text2 = this.tb2_txt_apk_path.Text;
				if (string.IsNullOrEmpty(text2))
				{
					MessageBox.Show("保存的文件路径不能为空");
				}
				else
				{
					this.tab2_loading.Visible = true;
					this.tab2_loading.Start();
					string[] cmd = new string[]
					{
						"-jar",
						Application.StartupPath + "\\lib\\smali.dll",
						text,
						"-o",
						text2
					};
					Thread thread = new Thread(delegate
					{
						int num = Starter.StartMain(cmd);
						this.loadingStop(this.tab2_loading);
						if (num == 0)
						{
							MessageBox.Show("编译Smali成功", "消息提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
						}
					});
					thread.Start();
				}
			}
		}

		private void tb3_txt_apk_path_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Link;
		}

		private void tb3_txt_apk_path_DragDrop(object sender, DragEventArgs e)
		{
			((TextBox)sender).Text = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
		}

		private void tb3_btn_apkfile_Click(object sender, EventArgs e)
		{
			this.openFileDialog.Filter = "安卓程序|*.apk|Java程序|*.jar";
			DialogResult dialogResult = this.openFileDialog.ShowDialog(this);
			string fileName = this.openFileDialog.FileName;
			if (!string.IsNullOrEmpty(fileName))
			{
				this.tb3_txt_apk_path.Text = fileName;
			}
		}

		private void tb3_btn_StartSign_Click(object sender, EventArgs e)
		{
			string text = this.tb3_txt_apk_path.Text;
			if (string.IsNullOrEmpty(text))
			{
				MessageBox.Show("签名的文件路径不能为空");
			}
			else if (System.IO.File.Exists(text))
			{
				FileInfo fileInfo = new FileInfo(text);
				string text2 = fileInfo.Name.Replace(fileInfo.Extension, "");
				this.tab3_loading.Visible = true;
				this.tab3_loading.Start();
				string[] cmd = new string[]
				{
					"-jar",
					Application.StartupPath + "\\lib\\signapk.dll",
					Application.StartupPath + "\\key\\publickey.x509.pem",
					Application.StartupPath + "\\key\\privatekey.pk8",
					text,
					string.Concat(new string[]
					{
						fileInfo.DirectoryName,
						"\\",
						text2,
						"-sign",
						fileInfo.Extension
					})
				};
				Thread thread = new Thread(delegate
				{
					int num = Starter.StartMain(cmd);
					this.loadingStop(this.tab3_loading);
					if (num == 0)
					{
						MessageBox.Show("签名Apk成功", "消息提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					}
				});
				thread.Start();
			}
			else
			{
				MessageBox.Show("文件路径不存在");
			}
		}

	}
}
