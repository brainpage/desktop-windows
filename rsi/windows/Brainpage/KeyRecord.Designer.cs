namespace Brainpage
{
    partial class KeyRecord
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeyRecord));
            this.timerFade = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemView = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.btnGood = new System.Windows.Forms.Button();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Brainpage: your smart data";
            this.notifyIcon.Visible = true;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemView,
            this.menuItemSetting,
            this.toolStripSeparator1,
            this.menuItemExit});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(173, 98);
            // 
            // menuItemView
            // 
            this.menuItemView.Name = "menuItemView";
            this.menuItemView.Size = new System.Drawing.Size(172, 22);
            this.menuItemView.Text = "Go to Charts ...";
            this.menuItemView.Click += new System.EventHandler(this.menuItemView_Click);
            // 
            // menuItemSetting
            // 
            this.menuItemSetting.Name = "menuItemSetting";
            this.menuItemSetting.Size = new System.Drawing.Size(172, 22);
            this.menuItemSetting.Text = "Go to Settings ...";
            this.menuItemSetting.Click += new System.EventHandler(this.menuItemSetting_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(169, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.Size = new System.Drawing.Size(172, 22);
            this.menuItemExit.Text = "Exit Brainpage";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // btnGood
            // 
            this.btnGood.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGood.ForeColor = System.Drawing.Color.Black;
            this.btnGood.Location = new System.Drawing.Point(250, 439);
            this.btnGood.Name = "btnGood";
            this.btnGood.Size = new System.Drawing.Size(225, 36);
            this.btnGood.TabIndex = 7;
            this.btnGood.Text = "Stop Break";
            this.btnGood.UseVisualStyleBackColor = true;
            this.btnGood.Click += new System.EventHandler(this.btnGood_Click);
            // 
            // webBrowser
            // 
            this.webBrowser.Location = new System.Drawing.Point(43, 31);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(649, 386);
            this.webBrowser.TabIndex = 8;
            this.webBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser_DocumentCompleted);
            // 
            // KeyRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(729, 541);
            this.Controls.Add(this.webBrowser);
            this.Controls.Add(this.btnGood);
            this.Name = "KeyRecord";
            this.ShowInTaskbar = false;
            this.Text = "reminder";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timerFade;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuItemSetting;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.Button btnGood;
        private System.Windows.Forms.ToolStripMenuItem menuItemView;
        private System.Windows.Forms.WebBrowser webBrowser;
  
    }
}

