namespace ComicRepacker
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.listTargetFiles = new System.Windows.Forms.ListBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtBaseName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOpenWorkingFolder = new System.Windows.Forms.Button();
            this.bgWorkerLoading = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.timerProgress = new System.Windows.Forms.Timer(this.components);
            this.btnRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Location = new System.Drawing.Point(12, 12);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(75, 23);
            this.btnLoadFile.TabIndex = 0;
            this.btnLoadFile.Text = "Load File";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // listTargetFiles
            // 
            this.listTargetFiles.FormattingEnabled = true;
            this.listTargetFiles.Location = new System.Drawing.Point(13, 157);
            this.listTargetFiles.Name = "listTargetFiles";
            this.listTargetFiles.Size = new System.Drawing.Size(383, 173);
            this.listTargetFiles.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(12, 128);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save File";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtBaseName
            // 
            this.txtBaseName.Location = new System.Drawing.Point(100, 44);
            this.txtBaseName.Name = "txtBaseName";
            this.txtBaseName.Size = new System.Drawing.Size(172, 20);
            this.txtBaseName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Result Filename";
            // 
            // btnOpenWorkingFolder
            // 
            this.btnOpenWorkingFolder.Location = new System.Drawing.Point(235, 128);
            this.btnOpenWorkingFolder.Name = "btnOpenWorkingFolder";
            this.btnOpenWorkingFolder.Size = new System.Drawing.Size(161, 23);
            this.btnOpenWorkingFolder.TabIndex = 2;
            this.btnOpenWorkingFolder.Text = "Open Working Folder";
            this.btnOpenWorkingFolder.UseVisualStyleBackColor = true;
            this.btnOpenWorkingFolder.Click += new System.EventHandler(this.btnOpenWorkingFolder_Click);
            // 
            // bgWorkerLoading
            // 
            this.bgWorkerLoading.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerLoading_DoWork);
            this.bgWorkerLoading.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkerLoading_RunWorkerCompleted);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(0, 358);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(409, 23);
            this.progressBar1.TabIndex = 5;
            // 
            // timerProgress
            // 
            this.timerProgress.Tick += new System.EventHandler(this.timerProgress_Tick);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(154, 128);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 6;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 381);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBaseName);
            this.Controls.Add(this.btnOpenWorkingFolder);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.listTargetFiles);
            this.Controls.Add(this.btnLoadFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmMain";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.ListBox listTargetFiles;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtBaseName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpenWorkingFolder;
        private System.ComponentModel.BackgroundWorker bgWorkerLoading;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timerProgress;
        private System.Windows.Forms.Button btnRefresh;
    }
}

