namespace zenQuery
{
    partial class frmHistory
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnrefresh = new System.Windows.Forms.Button();
            this.btnclear = new System.Windows.Forms.Button();
            this.dgsqlhist = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rtsqlhist = new System.Windows.Forms.RichTextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgsqlhist)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Controls.Add(this.btnrefresh);
            this.panel1.Controls.Add(this.btnclear);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(835, 51);
            this.panel1.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(296, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Context search";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(84, 7);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(206, 20);
            this.txtSearch.TabIndex = 3;
            // 
            // btnrefresh
            // 
            this.btnrefresh.Location = new System.Drawing.Point(585, 4);
            this.btnrefresh.Name = "btnrefresh";
            this.btnrefresh.Size = new System.Drawing.Size(75, 23);
            this.btnrefresh.TabIndex = 2;
            this.btnrefresh.Text = "Refresh";
            this.btnrefresh.UseVisualStyleBackColor = true;
            this.btnrefresh.Click += new System.EventHandler(this.btnrefresh_Click);
            // 
            // btnclear
            // 
            this.btnclear.Location = new System.Drawing.Point(504, 3);
            this.btnclear.Name = "btnclear";
            this.btnclear.Size = new System.Drawing.Size(75, 23);
            this.btnclear.TabIndex = 1;
            this.btnclear.Text = "Clear History";
            this.btnclear.UseVisualStyleBackColor = true;
            this.btnclear.Click += new System.EventHandler(this.btnclear_Click);
            // 
            // dgsqlhist
            // 
            this.dgsqlhist.AllowUserToAddRows = false;
            this.dgsqlhist.AllowUserToDeleteRows = false;
            this.dgsqlhist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgsqlhist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgsqlhist.Location = new System.Drawing.Point(0, 51);
            this.dgsqlhist.MultiSelect = false;
            this.dgsqlhist.Name = "dgsqlhist";
            this.dgsqlhist.ReadOnly = true;
            this.dgsqlhist.Size = new System.Drawing.Size(835, 362);
            this.dgsqlhist.TabIndex = 1;
            this.dgsqlhist.SizeChanged += new System.EventHandler(this.dgsqlhist_SizeChanged);
            this.dgsqlhist.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgsqlhist_CellFormatting);
            this.dgsqlhist.SelectionChanged += new System.EventHandler(this.dgsqlhist_SelectionChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rtsqlhist);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 416);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(835, 139);
            this.panel2.TabIndex = 2;
            // 
            // rtsqlhist
            // 
            this.rtsqlhist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtsqlhist.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.rtsqlhist.Location = new System.Drawing.Point(0, 0);
            this.rtsqlhist.Name = "rtsqlhist";
            this.rtsqlhist.Size = new System.Drawing.Size(835, 139);
            this.rtsqlhist.TabIndex = 0;
            this.rtsqlhist.Text = "";
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 413);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(835, 3);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // frmHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 555);
            this.Controls.Add(this.dgsqlhist);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "frmHistory";
            this.ShowInTaskbar = false;
            this.TabText = "Queries History";
            this.Text = "Queries History";
            this.Load += new System.EventHandler(this.HistoryForm_Load);
            this.Activated += new System.EventHandler(this.frmHistory_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmHistory_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgsqlhist)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgsqlhist;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox rtsqlhist;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button btnclear;
        private System.Windows.Forms.Button btnrefresh;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearch;
    }
}