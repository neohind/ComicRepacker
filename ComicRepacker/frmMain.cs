using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ComicRepacker
{
    public partial class frmMain : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        string m_sTempFolder = string.Empty;
        string m_sSourceFolder = string.Empty;
        string m_sTargetFolder = string.Empty;
        public frmMain()
        {
            InitializeComponent();

            m_sTempFolder = Environment.GetEnvironmentVariable("Temp");
            m_sTempFolder = Path.Combine(m_sTempFolder, "ComicRepacker");
            m_sSourceFolder = Path.Combine(m_sTempFolder, "Source");
            m_sTargetFolder = Path.Combine(m_sTempFolder, "Target");
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (Directory.Exists(m_sTempFolder))
                {
                    try
                    {
                        Directory.Delete(m_sTempFolder, true);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
                Directory.CreateDirectory(m_sTempFolder);

                RefreshList();
                this.UseWaitCursor = true;
                timerProgress.Start();
                bgWorkerLoading.RunWorkerAsync(dlg.FileNames);
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            this.UseWaitCursor = true;
            timerProgress.Start();
            bgWorkerProcessing.RunWorkerAsync();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".zip";
            dlg.FileName = txtBaseName.Text + ".zip";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string sTargetPath = Path.Combine(m_sTargetFolder, "*.*");
                ProcessStartInfo info = new ProcessStartInfo("7za.exe");
                info.Arguments = string.Format("a  \"{0}\" -r \"{1}\"", dlg.FileName, sTargetPath);
                info.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(info).WaitForExit();

                MessageBox.Show("Completed");
            }
        }


        private void ExtractChildArchive(FileInfo[] tempFileInfos, string sSourceFolder)
        {
            foreach (FileInfo zipfileInfo in tempFileInfos)
            {
                string sFilename = zipfileInfo.Name.Replace(zipfileInfo.Extension, "");
                string sTempDirName = Path.Combine(sSourceFolder, sFilename);
                ProcessStartInfo info = new ProcessStartInfo("7za.exe");
                info.Arguments = string.Format("x  \"{0}\" -r -o\"{1}\"", zipfileInfo.FullName, sTempDirName);
                info.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(info).WaitForExit();
                zipfileInfo.Delete();
            }
        }

        private void SlideFile(DirectoryInfo source, DirectoryInfo target)
        {
            if (target.Exists == false)
                target.Create();

            FileInfo[] allFiles = source.GetFiles();
            foreach (FileInfo info in allFiles)
            {
                string sExt = info.Extension.ToLower();

                if (".jpeg.jpg.png.gif".Contains(sExt))
                {
                    Image img = Image.FromFile(info.FullName);
                    if (img.Width > img.Height)
                    {
                        string sFilenameSideA = info.Name.Replace(info.Extension, "") + "_A.jpg" ;
                        string sFilenameSideB = info.Name.Replace(info.Extension, "") + "_B.jpg" ;
                        sFilenameSideA = Path.Combine(target.FullName, sFilenameSideA);
                        sFilenameSideB = Path.Combine(target.FullName, sFilenameSideB);

                        Bitmap imgA = new Bitmap(img.Width / 2, img.Height);
                        imgA.SetResolution(img.HorizontalResolution, img.VerticalResolution);


                        Graphics g = Graphics.FromImage(imgA);
                        g.DrawImage(img, 0, 0, new Rectangle(0, 0, img.Width / 2, img.Height), GraphicsUnit.Pixel);
                        g.Dispose();

                        Bitmap imgB = new Bitmap(img.Width - (img.Width / 2 + 1), img.Height);
                        imgB.SetResolution(img.HorizontalResolution, img.VerticalResolution);


                        g = Graphics.FromImage(imgB);
                        g.DrawImage(img, 0, 0, new Rectangle(img.Width / 2 + 1, 0, img.Width, img.Height), GraphicsUnit.Pixel);
                        g.Dispose();

                        imgA.Save(sFilenameSideA, System.Drawing.Imaging.ImageFormat.Jpeg);
                        imgB.Save(sFilenameSideB, System.Drawing.Imaging.ImageFormat.Jpeg);

                        imgA.Dispose();
                        imgB.Dispose();
                    }
                }
            }

            DirectoryInfo[] childInfos = source.GetDirectories();
            foreach (DirectoryInfo childInfo in childInfos)
            {
                DirectoryInfo childTargetInfo = new DirectoryInfo(Path.Combine(target.FullName, childInfo.Name));
                SlideFile(childInfo, childTargetInfo);
            }

        }

        private void btnOpenWorkingFolder_Click(object sender, EventArgs e)
        {
            string sSourceFolder = Path.Combine(m_sTempFolder, "Source");
            if (Directory.Exists(sSourceFolder))
            {
                ProcessStartInfo info = new ProcessStartInfo("explorer.exe");
                info.Arguments = sSourceFolder;
                Process.Start(info);
            }
        }

        private void bgWorkerLoading_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] aryFilenames = (string[])e.Argument;


            string sBaseName = string.Empty;
            foreach (string sFilename in aryFilenames)
            {
                FileInfo fileInfo = new FileInfo(sFilename);
                fileInfo.CopyTo(Path.Combine(m_sTempFolder, fileInfo.Name));
                sBaseName = fileInfo.Name.Replace(fileInfo.Extension, "");
            }


            DirectoryInfo dirInfoRoot = new DirectoryInfo(m_sTempFolder);
            DirectoryInfo dirInfoSource = new DirectoryInfo(m_sSourceFolder);
            dirInfoSource.Create();

            FileInfo[] rootFiles = dirInfoRoot.GetFiles("*.zip", SearchOption.AllDirectories);
            while (rootFiles.Length > 0)
            {
                ExtractChildArchive(rootFiles, m_sSourceFolder);
                rootFiles = dirInfoRoot.GetFiles("*.zip", SearchOption.AllDirectories);
            }

            ReorderSource(dirInfoSource);
            e.Result = sBaseName;
        }


        private void ReorderSource(DirectoryInfo info)
        {
            DirectoryInfo[] aryAllDirs = info.GetDirectories("*.*", SearchOption.AllDirectories);
            foreach (DirectoryInfo curInfo in aryAllDirs)
            {
                if (curInfo.GetDirectories().Length == 0)
                {
                    if (curInfo.GetFiles("*.jpg").Length > 0
                        || curInfo.GetFiles("*.jpeg").Length > 0
                        || curInfo.GetFiles("*.gif").Length > 0
                        || curInfo.GetFiles("*.png").Length > 0)
                    {
                        string sTargetPath = curInfo.Parent.Parent.FullName;
                        if (!string.Equals(info.FullName, curInfo.Parent.FullName))
                        {
                            if (string.Equals(curInfo.Name, curInfo.Parent.Name))
                            {
                                foreach (FileInfo file in curInfo.GetFiles())
                                    file.MoveTo(Path.Combine(sTargetPath, file.Name));
                            }
                            else
                            {
                                string sNewPath = Path.Combine(sTargetPath, curInfo.Name);
                                curInfo.MoveTo(sNewPath);
                            }
                        }
                    }
                    else
                        curInfo.Delete();

                }
            }

            aryAllDirs = info.GetDirectories("*.*", SearchOption.AllDirectories);
            foreach (DirectoryInfo curInfo in aryAllDirs)
            {
                if(curInfo.Parent.FullName.Equals(info.FullName) == false
                    || !(curInfo.GetFiles("*.jpg").Length > 0
                        || curInfo.GetFiles("*.jpeg").Length > 0
                        || curInfo.GetFiles("*.gif").Length > 0
                        || curInfo.GetFiles("*.png").Length > 0))
                {
                    ReorderSource(info);
                    break;
                }
            }

        }


        private void bgWorkerLoading_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            txtBaseName.Text = (string)e.Result;
            timerProgress.Stop();

            RefreshList();

            progressBar1.Value = 100;
            this.UseWaitCursor = false;
        }

        private void timerProgress_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value == 100)
                progressBar1.Value = 0;
            progressBar1.Value = progressBar1.Value + 1;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void RefreshList()
        {
            listTargetFiles.Items.Clear();
            DirectoryInfo info = new DirectoryInfo(m_sSourceFolder);
            if (info.Exists)
            {
                foreach (DirectoryInfo childInfo in info.GetDirectories())
                {
                    listTargetFiles.Items.Add(childInfo.Name);
                }
            }
        }

        private void bgWorkerProcessing_DoWork(object sender, DoWorkEventArgs e)
        {
            DirectoryInfo dirInfoSource = new DirectoryInfo(m_sSourceFolder);
            DirectoryInfo dirInfoTarget = new DirectoryInfo(m_sTargetFolder);

            dirInfoTarget.Create();
            SlideFile(dirInfoSource, dirInfoTarget);
        }

        private void bgWorkerProcessing_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Process Completed!");
            this.UseWaitCursor = false;
            timerProgress.Stop();
        }
    }
}
