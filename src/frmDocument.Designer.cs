namespace zenQuery
{
	partial class frmDocument
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDocument));
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tmrExecTime = new System.Windows.Forms.Timer(this.components);
            this.panRows = new System.Windows.Forms.StatusBarPanel();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveResultsDialog = new System.Windows.Forms.SaveFileDialog();
            this.splBrowser = new System.Windows.Forms.Splitter();
            this.panExecTime = new System.Windows.Forms.StatusBarPanel();
            this.treeview_imageList = new System.Windows.Forms.ImageList(this.components);
            this.panRunStatus = new System.Windows.Forms.StatusBarPanel();
            this.miRefresh = new System.Windows.Forms.MenuItem();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.cmRefresh = new System.Windows.Forms.ContextMenu();
            this.panBrowser = new System.Windows.Forms.Panel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.panDatabase = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cboDatabase = new System.Windows.Forms.ComboBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.splQuery = new System.Windows.Forms.SplitContainer();
            this.sciDocument = new ScintillaNet.Scintilla();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panRows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panExecTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panRunStatus)).BeginInit();
            this.panBrowser.SuspendLayout();
            this.panDatabase.SuspendLayout();
            this.splQuery.Panel1.SuspendLayout();
            this.splQuery.Panel2.SuspendLayout();
            this.splQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sciDocument)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrExecTime
            // 
            this.tmrExecTime.Interval = 1000;
            // 
            // panRows
            // 
            this.panRows.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.panRows.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
            this.panRows.MinWidth = 60;
            this.panRows.Name = "panRows";
            this.panRows.Width = 60;
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "SQL";
            this.openFileDialog.Filter = "SQL files|*.sql|Text files|*.txt|All files|*.*";
            // 
            // saveResultsDialog
            // 
            this.saveResultsDialog.Filter = "CSV Format|*.csv|XML|*.xml|All files|*.*";
            this.saveResultsDialog.Title = "Save Query Results";
            // 
            // splBrowser
            // 
            this.splBrowser.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.splBrowser.Location = new System.Drawing.Point(168, 0);
            this.splBrowser.Name = "splBrowser";
            this.splBrowser.Size = new System.Drawing.Size(5, 335);
            this.splBrowser.TabIndex = 11;
            this.splBrowser.TabStop = false;
            // 
            // panExecTime
            // 
            this.panExecTime.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.panExecTime.MinWidth = 80;
            this.panExecTime.Name = "panExecTime";
            this.panExecTime.Width = 80;
            // 
            // treeview_imageList
            // 
            this.treeview_imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeview_imageList.ImageStream")));
            this.treeview_imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.treeview_imageList.Images.SetKeyName(0, "closed-folder.gif");
            this.treeview_imageList.Images.SetKeyName(1, "table.gif");
            // 
            // panRunStatus
            // 
            this.panRunStatus.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.panRunStatus.Name = "panRunStatus";
            this.panRunStatus.Text = "Ready";
            this.panRunStatus.Width = 482;
            // 
            // miRefresh
            // 
            this.miRefresh.Index = 0;
            this.miRefresh.Text = "&Refresh Browser";
            // 
            // statusBar
            // 
            this.statusBar.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBar.Location = new System.Drawing.Point(168, 335);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.panRunStatus,
            this.panExecTime,
            this.panRows});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(622, 19);
            this.statusBar.SizingGrip = false;
            this.statusBar.TabIndex = 9;
            // 
            // cmRefresh
            // 
            this.cmRefresh.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.miRefresh});
            // 
            // panBrowser
            // 
            this.panBrowser.Controls.Add(this.treeView);
            this.panBrowser.Controls.Add(this.panDatabase);
            this.panBrowser.Dock = System.Windows.Forms.DockStyle.Left;
            this.panBrowser.Location = new System.Drawing.Point(0, 0);
            this.panBrowser.Name = "panBrowser";
            this.panBrowser.Size = new System.Drawing.Size(168, 354);
            this.panBrowser.TabIndex = 10;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.treeview_imageList;
            this.treeView.Location = new System.Drawing.Point(0, 40);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(168, 314);
            this.treeView.TabIndex = 0;
            this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeExpand_1);
            this.treeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseUp_1);
            this.treeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseDown_1);
            this.treeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag_1);
            // 
            // panDatabase
            // 
            this.panDatabase.ContextMenu = this.cmRefresh;
            this.panDatabase.Controls.Add(this.label1);
            this.panDatabase.Controls.Add(this.cboDatabase);
            this.panDatabase.Dock = System.Windows.Forms.DockStyle.Top;
            this.panDatabase.Location = new System.Drawing.Point(0, 0);
            this.panDatabase.Name = "panDatabase";
            this.panDatabase.Size = new System.Drawing.Size(168, 40);
            this.panDatabase.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Browser:";
            // 
            // cboDatabase
            // 
            this.cboDatabase.ContextMenu = this.cmRefresh;
            this.cboDatabase.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cboDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDatabase.DropDownWidth = 128;
            this.cboDatabase.ItemHeight = 13;
            this.cboDatabase.Location = new System.Drawing.Point(0, 19);
            this.cboDatabase.MaxDropDownItems = 20;
            this.cboDatabase.Name = "cboDatabase";
            this.cboDatabase.Size = new System.Drawing.Size(168, 21);
            this.cboDatabase.TabIndex = 1;
            this.cboDatabase.SelectedIndexChanged += new System.EventHandler(this.cboDatabase_SelectedIndexChanged_1);
            // 
            // tabControl
            // 
            this.tabControl.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(3, 3);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(607, 100);
            this.tabControl.TabIndex = 9;
            this.tabControl.TabStop = false;
            // 
            // splQuery
            // 
            this.splQuery.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splQuery.Location = new System.Drawing.Point(173, 0);
            this.splQuery.Name = "splQuery";
            this.splQuery.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splQuery.Panel1
            // 
            this.splQuery.Panel1.Controls.Add(this.sciDocument);
            // 
            // splQuery.Panel2
            // 
            this.splQuery.Panel2.Controls.Add(this.tabControl);
            this.splQuery.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splQuery.Size = new System.Drawing.Size(617, 335);
            this.splQuery.SplitterDistance = 220;
            this.splQuery.SplitterWidth = 5;
            this.splQuery.TabIndex = 14;
            // 
            // sciDocument
            // 
            this.sciDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sciDocument.Location = new System.Drawing.Point(0, 0);
            this.sciDocument.Name = "sciDocument";
            this.sciDocument.Size = new System.Drawing.Size(613, 216);
            this.sciDocument.TabIndex = 0;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "table.gif");
            // 
            // frmDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 354);
            this.Controls.Add(this.splQuery);
            this.Controls.Add(this.splBrowser);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.panBrowser);
            this.Name = "frmDocument";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmDocument_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.panRows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panExecTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panRunStatus)).EndInit();
            this.panBrowser.ResumeLayout(false);
            this.panDatabase.ResumeLayout(false);
            this.panDatabase.PerformLayout();
            this.splQuery.Panel1.ResumeLayout(false);
            this.splQuery.Panel2.ResumeLayout(false);
            this.splQuery.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sciDocument)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Timer tmrExecTime;
        private System.Windows.Forms.StatusBarPanel panRows;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveResultsDialog;
        private System.Windows.Forms.Splitter splBrowser;
        private System.Windows.Forms.StatusBarPanel panExecTime;
        private System.Windows.Forms.ImageList treeview_imageList;
        private System.Windows.Forms.StatusBarPanel panRunStatus;
        private System.Windows.Forms.MenuItem miRefresh;
        private System.Windows.Forms.StatusBar statusBar;
        private System.Windows.Forms.ContextMenu cmRefresh;
        private System.Windows.Forms.Panel panBrowser;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.Panel panDatabase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboDatabase;
        //private ScintillaNet.Scintilla scintilla1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.SplitContainer splQuery;
        private System.Windows.Forms.ImageList imageList;
        private ScintillaNet.Scintilla sciDocument;
        //private ScintillaNet.Scintilla scintilla2;
	}
}