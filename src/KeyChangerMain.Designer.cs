namespace MyKeyChangerForAppleWireless {
    partial class KeyChangerMain {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeyChangerMain));
            this.cNotify = new System.Windows.Forms.NotifyIcon(this.components);
            this.cAppMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cAppMenuStart = new System.Windows.Forms.ToolStripMenuItem();
            this.cAppMenuStop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cAppMenuReset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cAppMenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.cAppMenu.SuspendLayout();
            // 
            // cNotify
            // 
            this.cNotify.ContextMenuStrip = this.cAppMenu;
            this.cNotify.Icon = ((System.Drawing.Icon)(resources.GetObject("cNotify.Icon")));
            this.cNotify.Text = "MyKeyChanger";
            this.cNotify.Visible = true;
            // 
            // cAppMenu
            // 
            this.cAppMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cAppMenuStart,
            this.cAppMenuStop,
            this.toolStripSeparator1,
            this.cAppMenuReset,
            this.toolStripSeparator2,
            this.cAppMenuExit});
            this.cAppMenu.Name = "cAppMenu";
            this.cAppMenu.Size = new System.Drawing.Size(103, 104);
            // 
            // cAppMenuStart
            // 
            this.cAppMenuStart.CheckOnClick = true;
            this.cAppMenuStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cAppMenuStart.Name = "cAppMenuStart";
            this.cAppMenuStart.Size = new System.Drawing.Size(102, 22);
            this.cAppMenuStart.Text = "Start";
            // 
            // cAppMenuStop
            // 
            this.cAppMenuStop.CheckOnClick = true;
            this.cAppMenuStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cAppMenuStop.Name = "cAppMenuStop";
            this.cAppMenuStop.Size = new System.Drawing.Size(102, 22);
            this.cAppMenuStop.Text = "Stop";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(99, 6);
            // 
            // cAppMenuReset
            // 
            this.cAppMenuReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cAppMenuReset.Name = "cAppMenuReset";
            this.cAppMenuReset.Size = new System.Drawing.Size(102, 22);
            this.cAppMenuReset.Text = "Reset";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(99, 6);
            // 
            // cAppMenuExit
            // 
            this.cAppMenuExit.Name = "cAppMenuExit";
            this.cAppMenuExit.Size = new System.Drawing.Size(102, 22);
            this.cAppMenuExit.Text = "Exit";
            this.cAppMenu.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon cNotify;
        private System.Windows.Forms.ContextMenuStrip cAppMenu;
        private System.Windows.Forms.ToolStripMenuItem cAppMenuStart;
        private System.Windows.Forms.ToolStripMenuItem cAppMenuStop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cAppMenuReset;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cAppMenuExit;
    }
}
