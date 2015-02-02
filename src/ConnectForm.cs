using System;
using System.Windows.Forms;
using mk.Logic;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;
namespace zenQuery
{
    /// <summary>
    /// Connection Form - this requests connection details from the user and then creates
    ///  and configures DbClient and Browser objects, which can be obtained through
    ///  properties of the form.
    /// </summary>
    public class ConnectForm : System.Windows.Forms.Form
    {
        #region User Fields
        // Save the config file location in a static field so we can always refer to the same file
        //static string configFile = "";
        DbClient dbClient = null;
        IBrowser browser = null;
        string connectionName;//[connectionName] varchar(50)
        string server;        //[server] VARCHAR(200)  NULL,
        string database;       //[database] VARCHAR(255)  NULL,
        bool trusted;          //[trusted] BOOLEAN  NULL,
        string login;          //[login] VARCHAR(255)  NULL,
        string password;       //[password] VARCHAR(255)  NULL,
        string provider;       //[provider] VARCHAR(255)  NULL,
        string connectString;
        static string configFilePath = "config.cfg";
        //  string connectDescription;

        #endregion

        #region Designer Fields

        private System.Windows.Forms.Button btnConnect;
        private System.ComponentModel.IContainer components;
        internal DataGridView dgCONN;
        internal Button btnsave;
        private Panel panel1;
        private Label lblapp;
        private Label label3;

        private DataGridViewTextBoxColumn serverDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn databaseDataGridViewTextBoxColumn;
        private DataGridViewCheckBoxColumn trustedDataGridViewCheckBoxColumn;
        private DataGridViewTextBoxColumn loginDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn passwordDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn providerDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn connectionNameDataGridViewTextBoxColumn;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TextBox txtpassword;
        private TextBox txtlogin;
        private TextBox txtdatabase;
        private TextBox txtserver;
        private TextBox txtconnectionname;
        internal Button btndelete;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label1;
        private ComboBox txtprovider;
        private CheckBox txttrusted;
        private Label label9;
        private TextBox txtconnectionString;
        private TextBox txtinfo;
        internal Button button1;
        private Label label2;
        private OpenFileDialog openFileDialog1;
        private Label txtstatus;
        private System.Windows.Forms.CheckBox chkLowBandwidth;
        #endregion

        #region Public Properties

        /// <summary>
        /// The database client object which is used to talk to the database server.
        /// This should be queried after the form is closed (following a DialogResult.OK)
        /// </summary>
        public DbClient DbClient
        {
            get { return dbClient; }
        }

        /// <summary>
        /// The database browser object which is used in producing a TreeView of objects.
        /// This should be queried after the form is closed (following a DialogResult.OK)
        /// This property will be null if no browser is available for the database provider.
        /// </summary>
        public IBrowser Browser
        {
            get { return browser; }
        }

        public bool LowBandwidth
        {
            get { return chkLowBandwidth.Checked; }
        }

        #endregion

        #region Constructor
        public ConnectForm()
        {
            mk.Logic.simpleDebug.dump();
            this.Text = zenQuery.Logic.mMisc.appVersion;


            InitializeComponent();
            Icon = null;
            dgCONN.SelectionChanged += new EventHandler(dgCONN_SelectionChanged);
            dgCONN.CellDoubleClick += new DataGridViewCellEventHandler(dgCONN_CellDoubleClick);

            this.KeyUp += new KeyEventHandler(ConnectForm_KeyUp);
            dgCONN.CellFormatting += new DataGridViewCellFormattingEventHandler(dgCONN_CellFormatting); //gwiazdki zamiast hasla
            txttrusted.CheckStateChanged += new EventHandler(txttrusted_CheckStateChanged);
            txtprovider.SelectedValueChanged += new EventHandler(txtprovider_SelectedValueChanged);  //jesli sie zmieni providera powinien sie zaktualizowac connectionstring

            CreateDatabase(); //utworzenie bazy danych 
            fillDG(); //wypelnienie danych
            crycore.actions.fillActions(); //wypelneinie akcji 
        }

        void txtprovider_SelectedValueChanged(object sender, EventArgs e)
        {
            mk.Logic.simpleDebug.dump();
            styleManage();
            setconnectString();
        }

        void txttrusted_CheckStateChanged(object sender, EventArgs e)
        {
            mk.Logic.simpleDebug.dump();
            styleManage();
            setconnectString();
        }

        void dgCONN_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            mk.Logic.simpleDebug.dump();
            connect();
        }

        void ConnectForm_KeyUp(object sender, KeyEventArgs e)
        {
            mk.Logic.simpleDebug.dump();
            setconnectString();
        }




        /// <summary>
        /// Zaslania odslania texboxy
        /// </summary>
        void styleManage()
        {
            mk.Logic.simpleDebug.dump();
            string option = txtprovider.SelectedText;
            switch (option)
            {
                case "ORACLE":
                    txttrusted.Enabled = false; //w oraclu trusted nei obslugujemy
                    txttrusted.Checked = false;
                    break;

                case "MSSQL":
                    txttrusted.Enabled = true;
                    if (txttrusted.Checked)
                    {
                        txtlogin.Enabled = false;
                        txtpassword.Enabled = false;
                    }
                    else
                    {
                        txtlogin.Enabled = true;
                        txtpassword.Enabled = true;
                    }
                    break;

                case "SQLite":
                    txttrusted.Enabled = false; //w oraclu trusted nei obslugujemy
                    txttrusted.Checked = false;
                    txtlogin.Enabled = false;
                    txtpassword.Enabled = false;
                    break;

                default: break;
            }




        }

        #endregion

        #region Misc Methods
        // Enable / disable controls appropriate to UI selections


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #endregion



        #region Event Handlers

        private void btnConnect_Click(object sender, System.EventArgs e)
        {
            mk.Logic.simpleDebug.dump();
            connect();
        }

        private void ConnectForm_Closed(object sender, System.EventArgs e)
        {
            Dispose();
        }

        #endregion

        #region MKConnection

        ///<summary>
        ///delete connection
        ///</summary>
        private int ConnectionDelete(string server)
        {
            mk.Logic.simpleDebug.dump();
            //Sprawdzenie czy juz nie ma takiej nazwy jesli tak wtedy pytanie czy nadpisac
            String strsql;
            int result;
            strsql = @"delete from conn where server = '" + server + "' ";
            result = mk.msqllite.ExecuteNonQuery(strsql);
            return result;
        }

        #endregion

        /// <summary>
        /// Uruchomienie 
        /// 1.sprawdzenie czy istnieje plik z konfiguracja
        /// 2.odswiezenie dg 
        /// </summary>
        private void CreateDatabase()
        {
            mk.Logic.simpleDebug.dump();
            //utworzenie configa

            if (System.IO.File.Exists(configFilePath)) //czy plik istnieje i czy jest wiekszy od 0
            {
                System.IO.FileInfo fileinfo = new System.IO.FileInfo(configFilePath);
                if (fileinfo.Length > 0)
                    return;
                else
                    System.IO.File.Delete(configFilePath);
            }


            mk.msqllite.ExecuteNonQuery(zenQuery.Logic.s3dbconfig.conn);
            mk.msqllite.ExecuteNonQuery(zenQuery.Logic.s3dbconfig.sqlhist);
            mk.msqllite.ExecuteNonQuery(zenQuery.Logic.s3dbconfig.tblsnipitem);

        }

        void connectionSave()
        {
            mk.Logic.simpleDebug.dump();

            string queryResult;
            List<SQLiteParameter> paramss = new List<SQLiteParameter>();
            paramss.Add(new SQLiteParameter("@oldconnectionname", connectionName));//poprzednai wartosc tu trzymana
            paramss.Add(new SQLiteParameter("@connectionname", txtconnectionname.Text));
            paramss.Add(new SQLiteParameter("@server", txtserver.Text));
            paramss.Add(new SQLiteParameter("@database", txtdatabase.Text));
            paramss.Add(new SQLiteParameter("@trusted", (txttrusted.Checked ? 1 : 0)));
            paramss.Add(new SQLiteParameter("@login", txtlogin.Text));
            paramss.Add(new SQLiteParameter("@password", txtpassword.Text));
            paramss.Add(new SQLiteParameter("@provider", txtprovider.Text));

            //RUN
            queryResult = mk.msqllite.mSQLLiteExecute(false, "update conn  set connectionname = @connectionname,server=@server,database=@database,trusted=@trusted,login=@login,password=@password,provider=@provider where connectionname = @oldconnectionname", paramss, null);


            if (queryResult == "0") //insert
            {
                mk.Logic.simpleDebug.dump();
                List<SQLiteParameter> paramss1 = new List<SQLiteParameter>();
                paramss1.Add(new SQLiteParameter("@connectionname", txtconnectionname.Text));
                paramss1.Add(new SQLiteParameter("@server", txtserver.Text));
                paramss1.Add(new SQLiteParameter("@database", txtdatabase.Text));
                paramss1.Add(new SQLiteParameter("@trusted", (txttrusted.Checked ? 1 : 0)));
                paramss1.Add(new SQLiteParameter("@login", txtlogin.Text));
                paramss1.Add(new SQLiteParameter("@password", txtpassword.Text));
                paramss1.Add(new SQLiteParameter("@provider", txtprovider.Text));

                //RUN
                 mk.msqllite.mSQLLiteExecute(false, "insert into conn  (connectionname ,server,database,trusted,login,password,provider) values (@connectionname ,@server,@database,@trusted,@login,@password,@provider) ", paramss1, null);
            }


            fillDG();
        }



        private void btnsave_Click(object sender, EventArgs e)
        {
            mk.Logic.simpleDebug.dump();
            connectionSave();
        }


        /// <summary>
        /// Zwrac aconnectionstirng
        /// </summary>
        void setconnectString()
        {
            mk.Logic.simpleDebug.dump();
            //Initial Catalog = txtdatabase.Text ;
            if (txtprovider.SelectedItem != null)
                switch (txtprovider.SelectedItem.ToString())
                {
                    case "MSSQL":	// SQL Server

                        connectString = "Data Source=" + txtserver.Text + ";" + (String.IsNullOrEmpty(txtdatabase.Text) ? "" : "Initial Catalog=" + txtdatabase.Text + ";") + "app=zenQuery";

                        if (trusted)
                            connectString += ";Integrated Security=SSPI;Connect Timeout=15";
                        else
                            connectString +=
                                ";User ID=" + txtlogin.Text +
                                ";Password=" + txtpassword.Text + ";Connect Timeout=15";

                        break;

                    case "ORACLE":

                        provider = "ORACLE";
                        connectString = string.Format("user Id={0};Password={1};Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={2})(PORT=1521))(CONNECT_DATA=(SERVICE_NAME={3})));", txtlogin.Text, txtpassword.Text, txtserver.Text, txtdatabase.Text);

                        break;
                    case "SQLite":

                        provider = "SQLite";
                        connectString = string.Format("Data Source={0};", txtdatabase.Text);

                        break;
                }
            txtconnectionString.Text = connectString;
        }



        /// <summary>
        /// Connect to database
        /// 1.Create DbClient (database client) object
        /// 2.Create a browser object (if available for the provider)
        /// </summary>
        void connect()
        {


            simpleDebug.dump();
            // 1.Create DbClient (database client) object

            IClientFactory clientFactory;


            switch (provider)
            {
                case "MSSQL":
                    clientFactory = new SqlFactory();

                    break;

                case "ORACLE":
                    clientFactory = new OleDbFactory();
                    break;
                case "SQLite":
                    clientFactory = new SQLiteFactory();
                    break;

                default: return;
            }

            dbClient = new DbClient(clientFactory, connectString, connectionName, provider, server, login);
            // frmLoader c = new frmLoader("Connecting...");
            // c.Show();
            // c.Refresh();
            lblapp.Text += " Connecting...." + connectionName;
            bool success = dbClient.Connect();
            // c.Close();


            if (!success)
            {
                lblapp.Text = zenQuery.Logic.mMisc.appVersion;
                MessageBox.Show("Unable to connect: " + dbClient.Error, "zenQuery", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dbClient.Dispose();
                return;
            }

            //2. Create a browser object (if available for the provider)

            if (provider == "MSSQL")					// SQL Server
                browser = new SqlBrowser(dbClient);

            if (provider == "ORACLE")					// Oracle
                browser = new OracleBrowser(dbClient);

            if (provider == "SQLite")					// Oracle
                browser = new SQLite(dbClient);


            DialogResult = DialogResult.OK;
        }

        #region ObslugaDatagridview

        /// <summary>
        /// Gwiazdki zamiast tekstu w wyswietlaniu
        /// </summary>
        private void dgCONN_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if ((e.ColumnIndex != -1 && dgCONN.Columns[e.ColumnIndex].Name == "password"))
                if (e.Value != null)
                    e.Value = "********";
        }

        /// <summary>
        /// odswiezenei datagrid
        /// </summary>
        void fillDG()
        {
            mk.Logic.simpleDebug.dump();
            string strsql = "select connectionname,server,database,trusted,login,password,provider from conn order by connectionname";

            if (dgCONN == null)
                return;

            int row = -1;
            if (dgCONN.CurrentRow != null)
                row = dgCONN.CurrentRow.Index;

            DataTable dt = new DataTable();

            dt = mk.msqllite.GetDataTable(strsql);
            dgCONN.AutoGenerateColumns = true;
            dgCONN.DataSource = dt;
            dt.Dispose();
            if (row > -1 && dgCONN.RowCount > 0)
            {
                dgCONN.CurrentCell.Selected = false;
                dgCONN.Rows[row].Cells[0].Selected = true;
                dgCONN.CurrentCell = dgCONN.SelectedCells[0];
                dgRefresh();
            }

        }

        /// <summary>
        /// Event przy zmianie beizacego wiersza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgCONN_SelectionChanged(object sender, EventArgs e)
        {
            mk.Logic.simpleDebug.dump();
            dgRefresh();
        }


        /// <summary>
        /// Odswiezenei datagridview
        /// </summary>
        void dgRefresh()
        {
            mk.Logic.simpleDebug.dump();
            //inicjalize value
            if (dgCONN.CurrentRow == null) return;

            txtstatus.Text = "";
            btndelete.Visible = true;

            connectionName = Convert.ToString(dgCONN.CurrentRow.Cells[0].Value);//[connectionName] varchar(50)
            server = Convert.ToString(dgCONN.CurrentRow.Cells[1].Value);        //[server] VARCHAR(200)  NULL,
            database = Convert.ToString(dgCONN.CurrentRow.Cells[2].Value);       //[database] VARCHAR(255)  NULL,
            trusted = (bool)dgCONN.CurrentRow.Cells[3].Value;          //[trusted] BOOLEAN  NULL,
            login = Convert.ToString(dgCONN.CurrentRow.Cells[4].Value);          //[login] VARCHAR(255)  NULL,
            password = Convert.ToString(dgCONN.CurrentRow.Cells[5].Value);       //[password] VARCHAR(255)  NULL,
            provider = Convert.ToString(dgCONN.CurrentRow.Cells[6].Value);       //[provider] VARCHAR(255)  NULL,

            //set textbox
            txtconnectionname.Text = connectionName;
            txtserver.Text = server;
            txtdatabase.Text = database;

            txtlogin.Text = login;
            txtpassword.Text = password;
            txtprovider.SelectedItem = provider;
            txttrusted.Checked = trusted; //uwag ato musi byc za providerem bo sie provider resetowal
            setconnectString();
            styleManage();

        }
        #endregion


        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.chkLowBandwidth = new System.Windows.Forms.CheckBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.dgCONN = new System.Windows.Forms.DataGridView();
            this.serverDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.databaseDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trustedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.loginDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.passwordDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.providerDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.connectionNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnsave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblapp = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtstatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtinfo = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtconnectionString = new System.Windows.Forms.TextBox();
            this.txtprovider = new System.Windows.Forms.ComboBox();
            this.txttrusted = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btndelete = new System.Windows.Forms.Button();
            this.txtpassword = new System.Windows.Forms.TextBox();
            this.txtlogin = new System.Windows.Forms.TextBox();
            this.txtdatabase = new System.Windows.Forms.TextBox();
            this.txtserver = new System.Windows.Forms.TextBox();
            this.txtconnectionname = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dgCONN)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkLowBandwidth
            // 
            this.chkLowBandwidth.Location = new System.Drawing.Point(1218, 583);
            this.chkLowBandwidth.Name = "chkLowBandwidth";
            this.chkLowBandwidth.Size = new System.Drawing.Size(232, 34);
            this.chkLowBandwidth.TabIndex = 1;
            this.chkLowBandwidth.Text = "L&ow bandwidth";
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnConnect.ForeColor = System.Drawing.Color.Green;
            this.btnConnect.Location = new System.Drawing.Point(12, 535);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(294, 82);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "Connect to database";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
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
            this.dgCONN.Location = new System.Drawing.Point(0, 0);
            this.dgCONN.MultiSelect = false;
            this.dgCONN.Name = "dgCONN";
            this.dgCONN.ReadOnly = true;
            this.dgCONN.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgCONN.ShowEditingIcon = false;
            this.dgCONN.Size = new System.Drawing.Size(1462, 524);
            this.dgCONN.TabIndex = 14;
            // 
            // serverDataGridViewTextBoxColumn
            // 
            this.serverDataGridViewTextBoxColumn.DataPropertyName = "server";
            this.serverDataGridViewTextBoxColumn.HeaderText = "server";
            this.serverDataGridViewTextBoxColumn.Name = "serverDataGridViewTextBoxColumn";
            this.serverDataGridViewTextBoxColumn.ReadOnly = true;
            this.serverDataGridViewTextBoxColumn.Width = 61;
            // 
            // databaseDataGridViewTextBoxColumn
            // 
            this.databaseDataGridViewTextBoxColumn.DataPropertyName = "database";
            this.databaseDataGridViewTextBoxColumn.HeaderText = "database";
            this.databaseDataGridViewTextBoxColumn.Name = "databaseDataGridViewTextBoxColumn";
            this.databaseDataGridViewTextBoxColumn.ReadOnly = true;
            this.databaseDataGridViewTextBoxColumn.Width = 76;
            // 
            // trustedDataGridViewCheckBoxColumn
            // 
            this.trustedDataGridViewCheckBoxColumn.DataPropertyName = "trusted";
            this.trustedDataGridViewCheckBoxColumn.HeaderText = "trusted";
            this.trustedDataGridViewCheckBoxColumn.Name = "trustedDataGridViewCheckBoxColumn";
            this.trustedDataGridViewCheckBoxColumn.ReadOnly = true;
            this.trustedDataGridViewCheckBoxColumn.Width = 45;
            // 
            // loginDataGridViewTextBoxColumn
            // 
            this.loginDataGridViewTextBoxColumn.DataPropertyName = "login";
            this.loginDataGridViewTextBoxColumn.HeaderText = "login";
            this.loginDataGridViewTextBoxColumn.Name = "loginDataGridViewTextBoxColumn";
            this.loginDataGridViewTextBoxColumn.ReadOnly = true;
            this.loginDataGridViewTextBoxColumn.Width = 54;
            // 
            // passwordDataGridViewTextBoxColumn
            // 
            this.passwordDataGridViewTextBoxColumn.DataPropertyName = "password";
            this.passwordDataGridViewTextBoxColumn.HeaderText = "password";
            this.passwordDataGridViewTextBoxColumn.Name = "passwordDataGridViewTextBoxColumn";
            this.passwordDataGridViewTextBoxColumn.ReadOnly = true;
            this.passwordDataGridViewTextBoxColumn.Width = 77;
            // 
            // providerDataGridViewTextBoxColumn
            // 
            this.providerDataGridViewTextBoxColumn.DataPropertyName = "provider";
            this.providerDataGridViewTextBoxColumn.HeaderText = "provider";
            this.providerDataGridViewTextBoxColumn.Name = "providerDataGridViewTextBoxColumn";
            this.providerDataGridViewTextBoxColumn.ReadOnly = true;
            this.providerDataGridViewTextBoxColumn.Width = 70;
            // 
            // connectionNameDataGridViewTextBoxColumn
            // 
            this.connectionNameDataGridViewTextBoxColumn.DataPropertyName = "connectionName";
            this.connectionNameDataGridViewTextBoxColumn.HeaderText = "connectionName";
            this.connectionNameDataGridViewTextBoxColumn.Name = "connectionNameDataGridViewTextBoxColumn";
            this.connectionNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.connectionNameDataGridViewTextBoxColumn.Width = 113;
            // 
            // btnsave
            // 
            this.btnsave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnsave.Location = new System.Drawing.Point(208, 526);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(218, 81);
            this.btnsave.TabIndex = 16;
            this.btnsave.Text = "Save";
            this.btnsave.UseVisualStyleBackColor = true;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.ForestGreen;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lblapp);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(739, 120);
            this.panel1.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(1226, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(252, 25);
            this.label3.TabIndex = 1;
            this.label3.Text = "(c)  Marcin Kotynia  2009";
            // 
            // lblapp
            // 
            this.lblapp.AutoSize = true;
            this.lblapp.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblapp.ForeColor = System.Drawing.Color.White;
            this.lblapp.Location = new System.Drawing.Point(24, 18);
            this.lblapp.Name = "lblapp";
            this.lblapp.Size = new System.Drawing.Size(319, 73);
            this.lblapp.TabIndex = 0;
            this.lblapp.Text = "zenQuery";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 120);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(739, 308);
            this.tabControl1.TabIndex = 19;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgCONN);
            this.tabPage1.Controls.Add(this.chkLowBandwidth);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.btnConnect);
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(731, 270);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Connection List";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button1.Location = new System.Drawing.Point(416, 535);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(208, 82);
            this.button1.TabIndex = 28;
            this.button1.Text = "New connection";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtstatus);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.txtinfo);
            this.tabPage2.Controls.Add(this.label9);
            this.tabPage2.Controls.Add(this.txtconnectionString);
            this.tabPage2.Controls.Add(this.txtprovider);
            this.tabPage2.Controls.Add(this.txttrusted);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.btndelete);
            this.tabPage2.Controls.Add(this.txtpassword);
            this.tabPage2.Controls.Add(this.btnsave);
            this.tabPage2.Controls.Add(this.txtlogin);
            this.tabPage2.Controls.Add(this.txtdatabase);
            this.tabPage2.Controls.Add(this.txtserver);
            this.tabPage2.Controls.Add(this.txtconnectionname);
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(731, 270);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Manage Connection";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtstatus
            // 
            this.txtstatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtstatus.ForeColor = System.Drawing.Color.Red;
            this.txtstatus.Location = new System.Drawing.Point(498, 11);
            this.txtstatus.Name = "txtstatus";
            this.txtstatus.Size = new System.Drawing.Size(374, 39);
            this.txtstatus.TabIndex = 31;
            this.txtstatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(884, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 25);
            this.label2.TabIndex = 30;
            this.label2.Text = "* must be unique";
            // 
            // txtinfo
            // 
            this.txtinfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtinfo.Location = new System.Drawing.Point(208, 430);
            this.txtinfo.Multiline = true;
            this.txtinfo.Name = "txtinfo";
            this.txtinfo.ReadOnly = true;
            this.txtinfo.Size = new System.Drawing.Size(1220, 85);
            this.txtinfo.TabIndex = 29;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 347);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(179, 25);
            this.label9.TabIndex = 27;
            this.label9.Text = "connection String";
            // 
            // txtconnectionString
            // 
            this.txtconnectionString.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtconnectionString.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtconnectionString.Location = new System.Drawing.Point(208, 347);
            this.txtconnectionString.Multiline = true;
            this.txtconnectionString.Name = "txtconnectionString";
            this.txtconnectionString.ReadOnly = true;
            this.txtconnectionString.Size = new System.Drawing.Size(1220, 72);
            this.txtconnectionString.TabIndex = 26;
            // 
            // txtprovider
            // 
            this.txtprovider.DisplayMember = "MSSQL";
            this.txtprovider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtprovider.FormattingEnabled = true;
            this.txtprovider.Items.AddRange(new object[] {
            "MSSQL",
            "ORACLE",
            "SQLite"});
            this.txtprovider.Location = new System.Drawing.Point(208, 11);
            this.txtprovider.Name = "txtprovider";
            this.txtprovider.Size = new System.Drawing.Size(242, 33);
            this.txtprovider.TabIndex = 25;
            this.txtprovider.ValueMember = "MSSQL";
            // 
            // txttrusted
            // 
            this.txttrusted.AutoSize = true;
            this.txttrusted.Location = new System.Drawing.Point(208, 207);
            this.txttrusted.Name = "txttrusted";
            this.txttrusted.Size = new System.Drawing.Size(110, 29);
            this.txttrusted.TabIndex = 24;
            this.txttrusted.Text = "trusted";
            this.txttrusted.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(106, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 25);
            this.label8.TabIndex = 23;
            this.label8.Text = "provider";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(98, 305);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 25);
            this.label7.TabIndex = 22;
            this.label7.Text = "password";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(144, 257);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 25);
            this.label6.TabIndex = 21;
            this.label6.Text = "login";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(94, 162);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 25);
            this.label5.TabIndex = 20;
            this.label5.Text = "database";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(120, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 25);
            this.label4.TabIndex = 19;
            this.label4.Text = "Server";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 25);
            this.label1.TabIndex = 18;
            this.label1.Text = "ConnectionName";
            // 
            // btndelete
            // 
            this.btndelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btndelete.ForeColor = System.Drawing.Color.Red;
            this.btndelete.Location = new System.Drawing.Point(438, 526);
            this.btndelete.Name = "btndelete";
            this.btndelete.Size = new System.Drawing.Size(218, 81);
            this.btndelete.TabIndex = 17;
            this.btndelete.Text = "DELETE";
            this.btndelete.UseVisualStyleBackColor = true;
            this.btndelete.Click += new System.EventHandler(this.btndelete_Click);
            // 
            // txtpassword
            // 
            this.txtpassword.Location = new System.Drawing.Point(208, 299);
            this.txtpassword.Name = "txtpassword";
            this.txtpassword.Size = new System.Drawing.Size(664, 31);
            this.txtpassword.TabIndex = 4;
            // 
            // txtlogin
            // 
            this.txtlogin.Location = new System.Drawing.Point(208, 251);
            this.txtlogin.Name = "txtlogin";
            this.txtlogin.Size = new System.Drawing.Size(664, 31);
            this.txtlogin.TabIndex = 3;
            // 
            // txtdatabase
            // 
            this.txtdatabase.Location = new System.Drawing.Point(208, 157);
            this.txtdatabase.Name = "txtdatabase";
            this.txtdatabase.Size = new System.Drawing.Size(664, 31);
            this.txtdatabase.TabIndex = 2;
            // 
            // txtserver
            // 
            this.txtserver.Location = new System.Drawing.Point(208, 109);
            this.txtserver.Name = "txtserver";
            this.txtserver.Size = new System.Drawing.Size(664, 31);
            this.txtserver.TabIndex = 1;
            // 
            // txtconnectionname
            // 
            this.txtconnectionname.Location = new System.Drawing.Point(208, 61);
            this.txtconnectionname.Name = "txtconnectionname";
            this.txtconnectionname.Size = new System.Drawing.Size(664, 31);
            this.txtconnectionname.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // ConnectForm
            // 
            this.AcceptButton = this.btnConnect;
            this.AutoScaleBaseSize = new System.Drawing.Size(10, 24);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(739, 428);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ZenQuery";
            this.Closed += new System.EventHandler(this.ConnectForm_Closed);
            ((System.ComponentModel.ISupportInitialize)(this.dgCONN)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion


        //uusniecie
        private void btndelete_Click(object sender, EventArgs e)
        {
            mk.Logic.simpleDebug.dump();
            List<SQLiteParameter> paramss = new List<SQLiteParameter>();
            paramss.Add(new SQLiteParameter("@oldconnectionname", connectionName));//poprzednai wartosc tu trzymana
            txtinfo.Text = mk.msqllite.mSQLLiteExecute(false, "delete from conn where connectionname = @oldconnectionname", paramss, null);
            fillDG();
        }

        //DOdanie nowego
        private void button1_Click(object sender, EventArgs e)
        {

            tabControl1.SelectedIndex = 1;
            txtconnectionname.Text = "NEW";
            txtstatus.Text = "New connection";
            btndelete.Visible = false;


        }

    }
}
