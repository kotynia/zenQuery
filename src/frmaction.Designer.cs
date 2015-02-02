namespace zenQuery
{
    partial class frmaction
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmaction));
            this.tab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgCONN = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txtinfo = new System.Windows.Forms.TextBox();
            this.btnnew = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtsnipitemid = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtobjectmask = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtobjecttype = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btndelete = new System.Windows.Forms.Button();
            this.txtprovider = new System.Windows.Forms.TextBox();
            this.btnsave = new System.Windows.Forms.Button();
            this.txtdatabase = new System.Windows.Forms.TextBox();
            this.txttype = new System.Windows.Forms.TextBox();
            this.txtstrsql = new System.Windows.Forms.TextBox();
            this.txtdescription = new System.Windows.Forms.TextBox();
            this.btnopenActions = new System.Windows.Forms.Button();
            this.btnRefreshActions = new System.Windows.Forms.Button();
            this.tab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgCONN)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab
            // 
            this.tab.Controls.Add(this.tabPage1);
            this.tab.Controls.Add(this.tabPage2);
            this.tab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab.Location = new System.Drawing.Point(0, 0);
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            this.tab.Size = new System.Drawing.Size(1085, 644);
            this.tab.TabIndex = 20;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1077, 618);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Action List";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgCONN);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 51);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1071, 564);
            this.panel2.TabIndex = 16;
            // 
            // dgCONN
            // 
            this.dgCONN.AllowUserToAddRows = false;
            this.dgCONN.AllowUserToDeleteRows = false;
            this.dgCONN.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dgCONN.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgCONN.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgCONN.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgCONN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgCONN.Location = new System.Drawing.Point(0, 0);
            this.dgCONN.MultiSelect = false;
            this.dgCONN.Name = "dgCONN";
            this.dgCONN.ReadOnly = true;
            this.dgCONN.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgCONN.ShowEditingIcon = false;
            this.dgCONN.Size = new System.Drawing.Size(1071, 564);
            this.dgCONN.TabIndex = 15;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRefreshActions);
            this.panel1.Controls.Add(this.btnopenActions);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1071, 48);
            this.panel1.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 13);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(243, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "To add,edit Actions , edit files in \\Action  Directory";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Controls.Add(this.txtinfo);
            this.tabPage2.Controls.Add(this.btnnew);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.txtsnipitemid);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.txtobjectmask);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.txtobjecttype);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.btndelete);
            this.tabPage2.Controls.Add(this.txtprovider);
            this.tabPage2.Controls.Add(this.btnsave);
            this.tabPage2.Controls.Add(this.txtdatabase);
            this.tabPage2.Controls.Add(this.txttype);
            this.tabPage2.Controls.Add(this.txtstrsql);
            this.tabPage2.Controls.Add(this.txtdescription);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1077, 618);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Manage Snippets";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(104, 370);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(882, 173);
            this.textBox1.TabIndex = 34;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            this.textBox1.WordWrap = false;
            // 
            // txtinfo
            // 
            this.txtinfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtinfo.Enabled = false;
            this.txtinfo.Location = new System.Drawing.Point(442, 320);
            this.txtinfo.Multiline = true;
            this.txtinfo.Name = "txtinfo";
            this.txtinfo.ReadOnly = true;
            this.txtinfo.Size = new System.Drawing.Size(327, 44);
            this.txtinfo.TabIndex = 33;
            // 
            // btnnew
            // 
            this.btnnew.Enabled = false;
            this.btnnew.Location = new System.Drawing.Point(334, 320);
            this.btnnew.Name = "btnnew";
            this.btnnew.Size = new System.Drawing.Size(102, 44);
            this.btnnew.TabIndex = 32;
            this.btnnew.Text = "ADD";
            this.btnnew.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(80, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(18, 13);
            this.label9.TabIndex = 31;
            this.label9.Text = "ID";
            // 
            // txtsnipitemid
            // 
            this.txtsnipitemid.Location = new System.Drawing.Point(104, 9);
            this.txtsnipitemid.Name = "txtsnipitemid";
            this.txtsnipitemid.ReadOnly = true;
            this.txtsnipitemid.Size = new System.Drawing.Size(82, 20);
            this.txtsnipitemid.TabIndex = 30;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 294);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "OBJECTMASK";
            // 
            // txtobjectmask
            // 
            this.txtobjectmask.Enabled = false;
            this.txtobjectmask.Location = new System.Drawing.Point(104, 294);
            this.txtobjectmask.Name = "txtobjectmask";
            this.txtobjectmask.Size = new System.Drawing.Size(332, 20);
            this.txtobjectmask.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 271);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "OBJECTTYPE";
            // 
            // txtobjecttype
            // 
            this.txtobjecttype.Enabled = false;
            this.txtobjecttype.Location = new System.Drawing.Point(104, 268);
            this.txtobjecttype.Name = "txtobjecttype";
            this.txtobjecttype.Size = new System.Drawing.Size(332, 20);
            this.txtobjecttype.TabIndex = 26;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(35, 242);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "PROVIDER";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(34, 216);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "DATABASE";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(63, 193);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "TYPE";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(51, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "ACTION";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "DESCRIPTION";
            // 
            // btndelete
            // 
            this.btndelete.Enabled = false;
            this.btndelete.Location = new System.Drawing.Point(219, 320);
            this.btndelete.Name = "btndelete";
            this.btndelete.Size = new System.Drawing.Size(109, 44);
            this.btndelete.TabIndex = 17;
            this.btndelete.Text = "DELETE";
            this.btndelete.UseVisualStyleBackColor = true;
            // 
            // txtprovider
            // 
            this.txtprovider.Enabled = false;
            this.txtprovider.Location = new System.Drawing.Point(104, 242);
            this.txtprovider.Name = "txtprovider";
            this.txtprovider.Size = new System.Drawing.Size(332, 20);
            this.txtprovider.TabIndex = 4;
            // 
            // btnsave
            // 
            this.btnsave.Enabled = false;
            this.btnsave.Location = new System.Drawing.Point(104, 320);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(109, 44);
            this.btnsave.TabIndex = 16;
            this.btnsave.Text = "Save";
            this.btnsave.UseVisualStyleBackColor = true;
            // 
            // txtdatabase
            // 
            this.txtdatabase.Enabled = false;
            this.txtdatabase.Location = new System.Drawing.Point(104, 216);
            this.txtdatabase.Name = "txtdatabase";
            this.txtdatabase.Size = new System.Drawing.Size(332, 20);
            this.txtdatabase.TabIndex = 3;
            // 
            // txttype
            // 
            this.txttype.Enabled = false;
            this.txttype.Location = new System.Drawing.Point(104, 190);
            this.txttype.Name = "txttype";
            this.txttype.Size = new System.Drawing.Size(332, 20);
            this.txttype.TabIndex = 2;
            // 
            // txtstrsql
            // 
            this.txtstrsql.Enabled = false;
            this.txtstrsql.Location = new System.Drawing.Point(104, 59);
            this.txtstrsql.Multiline = true;
            this.txtstrsql.Name = "txtstrsql";
            this.txtstrsql.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtstrsql.Size = new System.Drawing.Size(882, 125);
            this.txtstrsql.TabIndex = 1;
            // 
            // txtdescription
            // 
            this.txtdescription.Enabled = false;
            this.txtdescription.Location = new System.Drawing.Point(104, 33);
            this.txtdescription.Name = "txtdescription";
            this.txtdescription.Size = new System.Drawing.Size(332, 20);
            this.txtdescription.TabIndex = 0;
            // 
            // btnopenActions
            // 
            this.btnopenActions.Location = new System.Drawing.Point(297, 13);
            this.btnopenActions.Name = "btnopenActions";
            this.btnopenActions.Size = new System.Drawing.Size(155, 23);
            this.btnopenActions.TabIndex = 1;
            this.btnopenActions.Text = "Open Actions Folder";
            this.btnopenActions.UseVisualStyleBackColor = true;
            this.btnopenActions.Click += new System.EventHandler(this.btnopenActions_Click);
            // 
            // btnRefreshActions
            // 
            this.btnRefreshActions.Location = new System.Drawing.Point(458, 13);
            this.btnRefreshActions.Name = "btnRefreshActions";
            this.btnRefreshActions.Size = new System.Drawing.Size(155, 23);
            this.btnRefreshActions.TabIndex = 2;
            this.btnRefreshActions.Text = "Refresh Actions from files";
            this.btnRefreshActions.UseVisualStyleBackColor = true;
            this.btnRefreshActions.Click += new System.EventHandler(this.btnRefreshActions_Click);
            // 
            // frmaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1085, 644);
            this.Controls.Add(this.tab);
            this.Name = "frmaction";
            this.TabText = "frmaction";
            this.Text = "frmaction";
            this.tab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgCONN)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button btndelete;
        private System.Windows.Forms.TextBox txtprovider;
        internal System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.TextBox txtdatabase;
        private System.Windows.Forms.TextBox txttype;
        private System.Windows.Forms.TextBox txtstrsql;
        private System.Windows.Forms.TextBox txtdescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtobjectmask;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtobjecttype;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtsnipitemid;
        private System.Windows.Forms.Button btnnew;
        private System.Windows.Forms.TextBox txtinfo;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel2;
        internal System.Windows.Forms.DataGridView dgCONN;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnopenActions;
        private System.Windows.Forms.Button btnRefreshActions;
    }
}