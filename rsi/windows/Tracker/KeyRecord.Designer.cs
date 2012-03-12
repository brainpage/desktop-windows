namespace Tracker
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
            this.btnStop = new System.Windows.Forms.Button();
            this.timerFade = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLater = new System.Windows.Forms.Button();
            this.btnGood = new System.Windows.Forms.Button();
            this.pbClock = new System.Windows.Forms.PictureBox();
            this.lblTimeLeft = new System.Windows.Forms.Label();
            this.contextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClock)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStop.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
            this.btnStop.Location = new System.Drawing.Point(143, 222);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(254, 50);
            this.btnStop.TabIndex = 4;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "RSI Protect your health";
            this.notifyIcon.Visible = true;
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemSetting,
            this.toolStripSeparator1,
            this.menuItemExit});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(117, 54);
            // 
            // menuItemSetting
            // 
            this.menuItemSetting.Name = "menuItemSetting";
            this.menuItemSetting.Size = new System.Drawing.Size(116, 22);
            this.menuItemSetting.Text = "Setting";
            this.menuItemSetting.Click += new System.EventHandler(this.menuItemSetting_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(113, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.Size = new System.Drawing.Size(116, 22);
            this.menuItemExit.Text = "Exit";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // btnLater
            // 
            this.btnLater.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLater.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnLater.Image = ((System.Drawing.Image)(resources.GetObject("btnLater.Image")));
            this.btnLater.Location = new System.Drawing.Point(441, 222);
            this.btnLater.Name = "btnLater";
            this.btnLater.Size = new System.Drawing.Size(190, 50);
            this.btnLater.TabIndex = 6;
            this.btnLater.UseVisualStyleBackColor = true;
            this.btnLater.Click += new System.EventHandler(this.btnLater_Click);
            // 
            // btnGood
            // 
            this.btnGood.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGood.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnGood.Image = ((System.Drawing.Image)(resources.GetObject("btnGood.Image")));
            this.btnGood.Location = new System.Drawing.Point(218, 337);
            this.btnGood.Name = "btnGood";
            this.btnGood.Size = new System.Drawing.Size(225, 50);
            this.btnGood.TabIndex = 7;
            this.btnGood.UseVisualStyleBackColor = true;
            this.btnGood.Click += new System.EventHandler(this.btnGood_Click);
            // 
            // pbClock
            // 
            this.pbClock.Image = ((System.Drawing.Image)(resources.GetObject("pbClock.Image")));
            this.pbClock.InitialImage = ((System.Drawing.Image)(resources.GetObject("pbClock.InitialImage")));
            this.pbClock.Location = new System.Drawing.Point(266, 299);
            this.pbClock.Name = "pbClock";
            this.pbClock.Size = new System.Drawing.Size(34, 32);
            this.pbClock.TabIndex = 8;
            this.pbClock.TabStop = false;
            // 
            // lblTimeLeft
            // 
            this.lblTimeLeft.AutoSize = true;
            this.lblTimeLeft.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTimeLeft.ForeColor = System.Drawing.Color.Black;
            this.lblTimeLeft.Location = new System.Drawing.Point(304, 300);
            this.lblTimeLeft.Name = "lblTimeLeft";
            this.lblTimeLeft.Size = new System.Drawing.Size(60, 31);
            this.lblTimeLeft.TabIndex = 9;
            this.lblTimeLeft.Text = "--:--";
            // 
            // KeyRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(729, 541);
            this.Controls.Add(this.lblTimeLeft);
            this.Controls.Add(this.btnGood);
            this.Controls.Add(this.pbClock);
            this.Controls.Add(this.btnLater);
            this.Controls.Add(this.btnStop);
            this.Name = "KeyRecord";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.contextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbClock)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Timer timerFade;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem menuItemSetting;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.Button btnLater;
        private System.Windows.Forms.Button btnGood;
        private System.Windows.Forms.PictureBox pbClock;
        private System.Windows.Forms.Label lblTimeLeft;
  
    }
}

