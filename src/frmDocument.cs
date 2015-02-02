using System;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using mk.Logic;
using ScintillaNET;
using WeifenLuo.WinFormsUI.Docking;
using System.Collections.Generic;
using System.Text;
namespace zenQuery
{
    public partial class frmDocument : DockContent
    {
        public event EventHandler PropertyChanged;
        public Keys _runSnippet; //wykorzystany do funkcji snippetow
        public Keys _runAutocomplete; //wykorzystany do funkcji autocomplete

        #region Private Fields
        private const string DateTimeFormatString = "yyyy'-'MM'-'dd HH':'mm':'ss.fff";
        DbClient dbClient;									// DbClient object used to talk to database server
        IBrowser browser;									// Browser object for displaying object browser (may be null)
        DateTime queryStartTime;					// For  the timer showing running query time
        string fileName;										// Filename for when query is saved
        static int untitledCount = 1;					// For default new filenames (Untited-1, Untitled-2, etc)
        bool realFileName = false;					// true if default name of "untitled-x" - forces Save As... when Save is requested
        bool resultsInText = false;		//to do usuniecie			// text based results rather than grid based
        string resultType;
        RichTextBox txtResultsBox;				// handle to the rich textbox used to display text results
        bool hideBrowser = false;						// hide the treeview, if available
        bool initializing = true;							// to prevent multiple updates during startup
        bool error = false;									// true if an error was encountered
        string lastDatabase;								// ...so we can tell when the database has changed
        string resultsType ;  //tutaj z kcomboboxa	
        List<string[]> _Autocomplete;   //do autocomplete
        private long rowCount;//ilosc rekordow
        #endregion

        /// <summary>
        /// 1.Initialize Windows
        /// 2.Initialize Snippets
        /// </summary>
        /// <param name="dbClient"></param>
        /// <param name="browser"></param>
        /// <param name="hideBrowser"></param>
        /// <param name="provider"> MSSQL,ORACLE sluzy do wypelnienia nippetow</param>
        public frmDocument(DbClient dbClient, IBrowser browser, bool hideBrowser, string provider)
        {
            simpleDebug.dump();
            InitializeComponent();
            this.dbClient = dbClient;
            this.browser = browser;
            //this.autoComplete  = browser.
            lastDatabase = dbClient.Database;				// this is so we know when the current database changes
            HideResults = true;
            HideBrowser = hideBrowser || (browser == null);
            //FileName = "untitled" + untitledCount++.ToString() + ".sql";
            sciDocument.ConfigurationManager.Language = "mssql";//(dbClient.providerr.ToLower()== "mssql") ? "mssql" :"sql" ; //lexer posiada tylko 2 jezyki mssql sql
            
            sciDocument.Margins.Margin0.Width = 30;
            sciDocument.KeyDown += new KeyEventHandler(sciDocument_KeyDown); //klawisze
            sciDocument.KeyUp += new KeyEventHandler(sciDocument_KeyUp);
            sciDocument.AutoComplete.MaxHeight = 18; //ilosc pozycji

            //czas
            this.tmrExecTime.Interval = 1000;
            this.tmrExecTime.Tick += new System.EventHandler(this.tmrExecTime_Tick);

            statusBar.Font = new Font(statusBar.Font.Name, 8, FontStyle.Bold);

            //initialize snippets
            try
            {
                crycore.actions.fillSnippets(ref sciDocument, 1, provider);
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, ex.Message.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            //inicjalizacja autocomplete
            _Autocomplete = crycore.AUTOCOMPLETE.autoComplete(ref dbClient );
        }



        bool CloseQuery()
        {

            simpleDebug.dump();

            // Check to see if a query is running, and warn user that the query will be cancelled.
            if (RunState != RunState.Idle)
                if (MessageBox.Show(FileName + " is currently executing.\nWould you like to cancel the query?",
                    "zenQuery", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    // The Dispose method in DbClient will actually do the Cancel
                    return false;

            // If the query text has been modified, give option of saving changes.
            // Don't nag the user in the case of simple queries of less than 30 characters.
            if (sciDocument.Modified && sciDocument.Text.Length > 10)
            {
                DialogResult dr = MessageBox.Show("Save changes to " + FileName + "?", Text,
                    MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    if (!Save()) return false;
                }
                else if (dr == DialogResult.Cancel)
                    return false;
            }
            return true;
        }



        private string _filePath = null;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
            }
        }

        /// <summary>True if the "hide browser" option has been selected</summary>
        public bool HideBrowser
        {

            get { return hideBrowser; }
            set
            {
                if (Browser == null && !value) return;		// Can't show browser if not available!
                hideBrowser = value;
                panBrowser.Visible = !value;						// show/hide the browser panel containing the treeview
                splBrowser.Visible = !value;						// show/hide the splitter
                if (!value) PopulateBrowser();
                FirePropertyChanged();
            }
        }
        /// <summary>Returns the database browser object provided in construction</summary>
        public IBrowser Browser
        {
            get { return browser; }
        }
        public Scintilla Document
        {
            get
            {
                return sciDocument;
            }
        }

        /// <summary> If a Browser object is available, populate the treeview control on the left </summary>
        void PopulateBrowser()
        {
            simpleDebug.dump();
            if (Browser != null && !HideBrowser && !ClientBusy)
                try
                {
                    treeView.Nodes.Clear();
                    TreeNode[] tn = Browser.GetObjectHierarchy("doc");//mozliwosc odswiezenie
                    if (tn == null) HideBrowser = true;
                    else
                    {
                        treeView.Nodes.AddRange(tn);
                        treeView.Nodes[0].Expand();				// Expand the top level of hierarchy
                        cboDatabase.Items.Clear();
                        cboDatabase.Items.Add("<refresh objects...>");
                        cboDatabase.Items.AddRange(Browser.GetDatabases());
                        try { cboDatabase.Text = DbClient.Database; }
                        catch { }
                    }
                }
                catch { }
        }

        /// <summary> This is called once a cancel request has been completed </summary>
        void CancelDone()
        {
            simpleDebug.dump();
            SetRunning(false);
            panRunStatus.Text = "Query batch was cancelled.";
        }

        public bool Save()
        {
            simpleDebug.dump();
            if (String.IsNullOrEmpty(_filePath))
                return SaveAs();

            return save(_filePath);
        }

        public bool SaveAs()
        {
            simpleDebug.dump();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                _filePath = saveFileDialog.FileName;
                return save(_filePath);
            }
            else
            {
                return false;
            }
        }

        private bool save(string path)
        {
            simpleDebug.dump();
            using (FileStream fs = File.Create(path))
            using (BinaryWriter bw = new BinaryWriter(fs))
                bw.Write(sciDocument.RawText, 0, sciDocument.RawText.Length - 1); // Omit trailing NULL

            //sciDocument.IsDirty = false;
            return true;
        }


        private void sciDocument_SavePointReached(object sender, EventArgs e)
        {
            simpleDebug.dump();
            addOrRemoveAsteric();
        }

        private void sciDocument_SavePointLeft(object sender, EventArgs e)
        {
            simpleDebug.dump();
            addOrRemoveAsteric();
        }

        private void addOrRemoveAsteric()
        {
            simpleDebug.dump();

            if (Text.EndsWith(" *"))
                Text = Text.Substring(0, Text.Length - 2);
        }

        #region Query Execution Methods

        /// <summary>Format SQL by sqlhere.com ograniczenie do 5000 znakow</summary>
        public void Formatt()
        {
            simpleDebug.dump();


            //  frmLoader c = new frmLoader("Connecting... formatting your query by sqlhere.com");
            //  c.Show() ;
            //  c.Refresh();

            try
            {
                //TODO: MK do zrobienia


                int selectionLength;
                selectionLength = sciDocument.Selection.Length;
                string query = selectionLength == 0 ? sciDocument.Text : sciDocument.Selection.Text;
                if (query.Trim() == "") return;
                //if (query.Length > 4999)
               // {
                //    MessageBox.Show(this, "Sorry max 5000 chars can be formatted by this version", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
               // }

                if (selectionLength == 0)
                    sciDocument.Text = shEngine.cmain.mSTART(query, false, "TXTN", null);//Logic.mMisc.sqlhereWebservice(query);
                else
                    sciDocument.Selection.Text = shEngine.cmain.mSTART(query, false, "TXTN", null);
                
                
             

            }
            catch (Exception)
            {

                MessageBox.Show(this, "Error connecting to beauty provider sqlHere.com \nPosible problems: \nNo internet connection", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //throw;
            }
            finally
            {
                //  c.Close();

            }

        }


        /// <summary>Starts execution of  a query
        /// If the user has selected text within the query window, just execute the
        /// selected text.  Otherwise, execute the contents of the whole textbox.
        /// </summary>
        public void Execute(string resultType )
        {
            simpleDebug.dump();
            if (RunState != RunState.Idle)
                return;

            if (HideResults) HideResults = false;
            error = false;

            // Delete any previously defined tab pages and their child controls
            tabControl.TabPages.Clear();

            TabPage tabPage = new TabPage(ResultsInText ? "Results" : "Messages");
            // We'll need a rich textbox because an ordinary textbox has limited capacity
            txtResultsBox = new RichTextBox();
            txtResultsBox.AutoSize = false;
            txtResultsBox.Dock = DockStyle.Fill;

            txtResultsBox.Multiline = true;
            txtResultsBox.WordWrap = false;
            txtResultsBox.Font = new Font("Courier New", 9);
            txtResultsBox.ScrollBars = RichTextBoxScrollBars.Both;
            txtResultsBox.MaxLength = 0;
            txtResultsBox.Text = "";
            tabControl.TabPages.Add(tabPage);
            tabPage.Controls.Add(txtResultsBox);



            // If the user has selected text within the query window, just execute the
            // selected text.  Otherwise, execute the contents of the whole textbox.
            int selectionLength;
            selectionLength = sciDocument.Selection.End - sciDocument.Selection.Start;


            string query = selectionLength == 0 ? sciDocument.Text : sciDocument.Selection.Text;

            if (query.Trim() == "") return;

            // Use the database client class to execute the query.  Create delegates which will be invoked
            // when the query completes or cancels with an error.

            MethodInvoker results, done, failed;

            if (ResultsInText)
                results = new MethodInvoker(AddTextResults);
            else
                results = new MethodInvoker(AddGridResults);

            done = new MethodInvoker(QueryDone);
            failed = new MethodInvoker(QueryFailed);

            // dbClient.Execute runs asynchronously, so control will return immediately to the calling method.

            Cursor oldCursor = Cursor;
            Cursor = Cursors.WaitCursor;
            panRunStatus.Text = "Executing Query Batch...";
            dbClient.Execute(this, results, done, failed, query, ResultsInText,resultsType );		// this does the work


            SetRunning(true);
            Cursor = oldCursor;
        }

        /// <summary> Cancel a running query asynchronously </summary>
        public void Cancel()
        {
            simpleDebug.dump();
            panRunStatus.Text = "Cancelling...";
            dbClient.Cancel(new MethodInvoker(CancelDone));
            // Control will return immediately, and CancelDone will be invoked when the cancel is complete.
            FirePropertyChanged();
        }

        #endregion
        #region Misc Methods: SwitchPane,Clone,CheckDatabase,UpdateFormTExt,SetRunning,FirePropertyChanged

        /// <summary>
        /// Move the cursor into the next or previous window
        /// </summary>
        public void SwitchPane(bool forward)
        {
            simpleDebug.dump();
            if (ResultsInText)
            {
                if (sciDocument.Focused)
                    txtResultsBox.Focus();
                else
                    sciDocument.Focus();
                return;
            }
            if (forward)
            {
                if (sciDocument.Focused)
                {
                    tabControl.Focus();
                    tabControl.SelectedIndex = 0;
                }
                else
                {
                    if (tabControl.SelectedIndex < tabControl.TabCount - 1)
                        tabControl.SelectedIndex++;
                    else
                        sciDocument.Focus();
                }
            }
            else
            {
                if (sciDocument.Focused)
                {
                    tabControl.Focus();
                    tabControl.SelectedIndex = tabControl.TabCount - 1;
                }
                else
                {
                    if (tabControl.SelectedIndex > 0)
                        tabControl.SelectedIndex--;
                    else
                        sciDocument.Focus();
                }
            }
            if (!sciDocument.Focused)
                tabControl.SelectedTab.Controls[0].Focus();
        }

        /// <summary>
        /// Return a copy of the QueryForm object, with separate connection and browser objects
        /// </summary>
        public frmDocument Clone()
        {
            simpleDebug.dump();
            // Make a copy of the QueryForm's DbClient object.  We can't use the same object
            // object because this would mean sharing the same connection, preventing concurrent queries.
            DbClient d = DbClient.Clone();
            if (d.Connect())
            {
                d.Database = DbClient.Database;
                // We have to duplicate the Browser too, since it has a reference to the DbClient object.
                IBrowser b = null;
                if (Browser != null) try { b = Browser.Clone(d); }
                    catch { }
                frmDocument newQF = new frmDocument(d, b, HideBrowser, "MSSQL");
                newQF.ResultsInText = ResultsInText;
                return newQF;
            }
            else
            {
                MessageBox.Show("Unable to connect: " + d.Error, "zenQuery", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        /// <summary>
        /// Check the current database - if it has changed, update controls accordingly
        /// </summary>
        void CheckDatabase()
        {
            simpleDebug.dump();
            if (lastDatabase != dbClient.Database)
            {
                lastDatabase = dbClient.Database;
                UpdateFormText();
                PopulateBrowser();
            }
        }

        /// <summary>
        /// Update the form's caption to show the connection & selected database 
        /// </summary>
        void UpdateFormText()
        {
            simpleDebug.dump();
            Text = dbClient.ConnectDescription + " - " + dbClient.Database + " - " + fileName;
        }

        /// <summary>
        /// This should be called whenever a query is started or stopped
        /// </summary>
        void SetRunning(bool running)
        {
            simpleDebug.dump();
            // Start the timer in the status bar
            if (running)
            {
                queryStartTime = DateTime.Now;
                UpdateExecTime();
            }
            tmrExecTime.Enabled = running;
            if (!running) CheckDatabase();
            FirePropertyChanged();
        }

        void FirePropertyChanged()
        {
            if (!initializing && PropertyChanged != null)
                PropertyChanged(this, EventArgs.Empty);		// fire event
        }
        #endregion
        #region Methods to Populate Controls

        /// <summary>
        /// Create a new grid for each DataTable present in the results dataset
        /// </summary>
        void AddGridResults()
        {
            simpleDebug.dump();
            // Note: we give this method via a delegate to our DbClient object.  The DbClient object then
            // invokes this delegate from its worker thread, when results have become available.
            const int MaxResultSets = 20;

            // Create a new tab page and grid for each new result set.  In case this has already been called,
            // (as will be the case with multiple queries, separated with the 'GO' construct) only add tab
            // pages for new result sets.
            for (int page = tabControl.TabCount - 1; page < Math.Min(MaxResultSets, DSResults.Tables.Count); page++)
                AddGrid(DSResults.Tables[page]);
        }

        /// <summary>
        /// Paint the row number on the row header.
        /// The using statement automatically disposes the brush.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            simpleDebug.dump();
            string t = (e.RowIndex + 1).ToString(System.Globalization.CultureInfo.CurrentUICulture);
            using (SolidBrush b = new SolidBrush(((DataGridView)sender).RowHeadersDefaultCellStyle.ForeColor))
                e.Graphics.DrawString(t,
                                      e.InheritedRowStyle.Font,
                                      b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        /// <summary>
        /// Dodanie wyniku jako datagridview
        /// Formatowanie daty
        /// Create contextMenu
        /// </summary>
        private void AddGrid(DataTable dt)		// called by the method above
        {
            //TODO: pomyslec czy nie da sie zamienic na listview 
            // ListView lv = new ListView();


            simpleDebug.dump();
            DataGridView dataGrid = new DataGridView();

            // Due to a bug in the grid control, we must add the grid to the tabpage before assigning a datasource.
            // This bug was introduced in Beta 1, was fixed for Beta 2, then reared its ugly head again in RTM.
            TabPage tabPage = new TabPage("Result Set " + (tabControl.TabCount).ToString());
            //tabPage.Tag = ResultsTabType.GridResults;
            tabPage.Controls.Add(dataGrid);
            tabControl.TabPages.Add(tabPage);

            dataGrid.Dock = DockStyle.Fill;
            dataGrid.AllowUserToResizeRows = false;

            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; //MK
            dataGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;//MK
            dataGrid.EnableHeadersVisualStyles = false;//MK

            dataGrid.ReadOnly = true;
            dataGrid.AllowUserToAddRows = false;
            dataGrid.RowPostPaint += new DataGridViewRowPostPaintEventHandler(dataGrid_RowPostPaint); //dodanie numeracji
            dataGrid.DataError += new DataGridViewDataErrorEventHandler(dataGrid_DataError);
            dataGrid.DataSource = dt;




            rowCount = dt.Rows.Count;

            #region Create Context Menu
            dataGrid.ContextMenuStrip = Logic.EditManager.GetEditManager().GetContextMenuStrip(dataGrid,ref sciDocument   );
            dataGrid.ContextMenuStrip.Opening += delegate(object o, System.ComponentModel.CancelEventArgs e) { dataGrid.Focus(); };
            #endregion

            // Set datetime columns to show the time as well as the date
            DataGridViewCellStyle dateCellStyle;
            dateCellStyle = new DataGridViewCellStyle();
            dateCellStyle.Format = DateTimeFormatString;
        }

        //handle error
        private void dataGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        { }

        /// <summary>
        /// Create / append text results to the results window
        /// </summary>
        void AddTextResults()
        {
            simpleDebug.dump();
            // Note: we give this method via a delegate to our DbClient object.  The DbClient object then
            // invokes this delegate from its worker thread, as results become available.
            if (RunState == RunState.Cancelling) return;
            txtResultsBox.AppendText(dbClient.TextResults.ToString());
        }

        /// <summary>
        /// Called when a query has successfully finished executing.
        /// </summary>
        void QueryDone()
        {
            simpleDebug.dump();
            panRunStatus.Text = "Query batch completed" + (error ? " with errors" : ".");
            // If there were no results from query, display message to provide feedback to user
            if (!ResultsInText && !error && dbClient.Messages.Count == 0)
                txtResultsBox.AppendText("The command(s) completed successfully." );
            if (dbClient.Messages.Count > 0)
            {
                if (txtResultsBox.Text.Length > 0) txtResultsBox.AppendText("\r\n");
                foreach (string msg in dbClient.Messages)
                    txtResultsBox.AppendText(msg + "\r\n");
            }
            if (!ResultsInText)
                tabControl.SelectedIndex = error ? 0 : 1;
            ShowRowCount();
            SetRunning(false);
            sciDocument.Focus();
        }

        /// <summary>
        /// Called when a query has returned errors.
        /// </summary>
        void QueryFailed()
        {
            simpleDebug.dump();
            error = true;
            txtResultsBox.AppendText(dbClient.Error + "\r\n\r\n");
        }

        /// <summary> Display the number of rows retrieved in status bar </summary>
        void ShowRowCount()
        {
            simpleDebug.dump();
            if (ResultsInText || tabControl.SelectedIndex < 1)
                panRows.Text = "";
            else
            {
                int rows;
                if (DSResults.Tables.Count == 0 || tabControl.SelectedIndex < 0)
                    rows = 0;
                else
                    rows = DSResults.Tables[tabControl.SelectedIndex - 1].Rows.Count;
                panRows.Text = rows == 0 ? "" : rows.ToString() + " row" + (rows == 1 ? "" : "s");
            }
        }

        /// <summary> Show the elapsed time on the status bar </summary>
        void UpdateExecTime()
        {
            simpleDebug.dump();
            TimeSpan t = DateTime.Now.Subtract(queryStartTime);
            panExecTime.Text = String.Format("Exec Time: {0}:{1}:{2}"
                , t.Hours.ToString("00"), t.Minutes.ToString("00"), t.Seconds.ToString("00"));
        }

        #endregion

        #region Event Handlers

        private void QueryForm_Activated(object sender, System.EventArgs e)
        {
            simpleDebug.dump();
            FirePropertyChanged();
        }

        private void QueryForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            simpleDebug.dump();
            if (!CloseQuery()) e.Cancel = true;
        }

        private void tmrExecTime_Tick(object sender, System.EventArgs e)
        {
            simpleDebug.dump();
            UpdateExecTime();
        }

        private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            simpleDebug.dump();
            // Workaround: there's a bug in the grid control, whereby the scrollbars in grids in tabpages
            // don't resize when the parent control is resized with another tabpage active.
            this.Height += 1;
            this.Height -= 1;
            // If there is more than one result set, show the row count in the currently selected table.
            //tabControl.SelectedTab.inRefresh();
            ShowRowCount();
        }

        /// <summary>2.Wstawia do okna z browsera</summary>
        private void DoBrowserAction(object sender, EventArgs e)
        {
            simpleDebug.dump();
            sciDocument.SuspendLayout();


            MenuItem mi = (MenuItem)sender;
            // Ask the browser for the text to append, applicable to the selected node and menu item text.


            string retval = browser.GetActionText(treeView.SelectedNode, mi.Text);
       
            if (retval == null)
            {
                sciDocument.ResumeLayout();
                return;
            }

            int _length = retval.Length;
            //powodowalo ciemny ekran
            //if (_length > 200) HideResults = true;

            int pos = sciDocument.Selection.Start;
            if (sciDocument.Text.Length != 0)
            {
                sciDocument.Selection.Text = "\r\n" + retval;
                sciDocument.Selection.Start = pos + 2;
            }
            else
            {
                sciDocument.Selection.Text = retval;
                sciDocument.Selection.Start = pos;
            }

            // sciDocument.Modified = true;
            sciDocument.ResumeLayout();
            sciDocument.Focus();

        }

        private void sciDocument_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            simpleDebug.dump();
            if (e.Data.GetDataPresent(typeof(string)))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void sciDocument_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            simpleDebug.dump();
            if (e.Data.GetDataPresent(typeof(string)))
            {
                string s = (string)e.Data.GetData(typeof(string));
                // Have the newly inserted text highlighted
                int start = sciDocument.Selection.Start;
                //TODO:tu poprawic zaznaczenie
                //sciDocument.SelectedText = s;
                sciDocument.Selection.Start = start;
                sciDocument.Selection.End = start + s.Length;
                //sciDocument.Modified = true;
                sciDocument.Focus();
            }
        }

        private void btnCloseBrowser_Click(object sender, System.EventArgs e)
        {
            simpleDebug.dump();
            HideBrowser = true;
        }

        private void cboDatabase_Enter(object sender, System.EventArgs e)
        {
            simpleDebug.dump();
            if (ClientBusy) sciDocument.Focus();
        }

        private void splQuery_Resize(object sender, System.EventArgs e)
        {
            simpleDebug.dump();
            // Force a re-paint
            Invalidate();
        }

        #endregion



        #region Properties

        /// <summary>Returns the database client object provided in construction</summary>
        public DbClient DbClient
        {
            get { return dbClient; }
        }


        /// <summary>The current state of query execution</summary>
        public RunState RunState
        {
            get { return dbClient.RunState; }
        }

        /// <summary>The filename given to the SQL query</summary>
        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                UpdateFormText();
                FirePropertyChanged();
            }
        }


        public string ResultType
        {
            get { return resultsType; }
            set
            {
                resultsType = value;
                FirePropertyChanged();
            }
        }
        /// <summary>True if results should be displayed in textbox rather than in a grid</summary>
        public bool ResultsInText
        {
            get { return resultsInText; }
            set
            {
                resultsInText = value;
                FirePropertyChanged();
            }
        }

        /// <summary>True if the "hide results" option has been selected (manually or automatically)</summary>
        public bool HideResults
        {
            get { return !tabControl.Visible; }
            set
            {
                simpleDebug.dump();
                splQuery.Panel2Collapsed = !splQuery.Panel2Collapsed;
                FirePropertyChanged();
            }
        }


        // Private properties

        DataSet DSResults
        {
            get { return DbClient.DataSet; }
        }

        bool ClientBusy
        {
            get { return RunState != RunState.Idle; }
        }

        #endregion

        private void treeView_BeforeExpand_1(object sender, TreeViewCancelEventArgs e)
        {
            simpleDebug.dump();
            // If a browser has been installed, see if it has a sub object hierarchy for us at the point of expansion
            if (Browser == null) return;
            TreeNode[] subtree = Browser.GetSubObjectHierarchy(e.Node);
            if (subtree != null)
            {
                e.Node.Nodes.Clear();
                e.Node.Nodes.AddRange(subtree);
            }
        }

        private void treeView_ItemDrag_1(object sender, ItemDragEventArgs e)
        {
            simpleDebug.dump();
            // Allow objects to be dragged from the browser to the query textbox.
            if (e.Button == MouseButtons.Left && e.Item is TreeNode)
            {
                // Ask the browser object for a string applicable to dragging onto the query window.
                string dragText = Browser.GetDragText((TreeNode)e.Item);
                // We'll use a simple string-type DataObject
                if (dragText != "")
                    treeView.DoDragDrop(new DataObject(dragText), DragDropEffects.Copy);
            }
        }

        private void treeView_MouseDown_1(object sender, MouseEventArgs e)
        {
            simpleDebug.dump();
            // When right-clicking, first select the node under the mouse.
            if (e.Button == MouseButtons.Right)
            {
                TreeNode tn = treeView.GetNodeAt(e.X, e.Y);
                if (tn != null)
                    treeView.SelectedNode = tn;
            }
        }


        /// <summary>
        /// Generuj Menu
        /// </summary>
        private void treeView_MouseUp_1(object sender, MouseEventArgs e)
        {
            simpleDebug.dump();
            if (Browser == null) return;

            // Display a context menu if the browser has an action list for the selected node
            if (e.Button == MouseButtons.Right && treeView.SelectedNode != null)
            {
             
                System.Windows.Forms.ContextMenu cm = new ContextMenu();

                //Dodanie zwyklych akcji
                StringCollection actions = browser.GetActionList(treeView.SelectedNode, "simple");
                if (actions != null)
                    foreach (string action in actions)
                        cm.MenuItems.Add(action, new EventHandler(DoBrowserAction));


                MenuItem a;
                a = new MenuItem();
                a.BarBreak = true;
                cm.MenuItems.Add(a);

                //Dodanie Snippetow
                actions = browser.GetActionList(treeView.SelectedNode, "actions");
                if (actions != null)
                {
                    a = cm.MenuItems.Add("Snippets");
                    foreach (string action in actions)
                        a.MenuItems.Add(action, new EventHandler(DoBrowserAction));

                }

                //Dodanie Historii
                actions = browser.GetActionList(treeView.SelectedNode, "history" );
                if (actions != null)
                {
                    a = cm.MenuItems.Add("History for this item");
                    foreach (string action in actions)
                    {
                        a.MenuItems.Add(action, new EventHandler(DoBrowserAction));
                    }
                }

                cm.Show(treeView, new Point(e.X, e.Y));
            }
        }

        #region mkShortcuts Handle keywords


        /// <summary>
        /// Handler autocomplete
        /// </summary>
        void sciDocument_KeyUp(object sender, KeyEventArgs e)
        {
            int minLength = 6; //minimalna ilosc znakow zeby zaznaczcyc
            int documentLength = this.sciDocument.Text.Length;
  

            if (documentLength > 1)
            {
           
                if (e.KeyCode  == Keys.OemPeriod ) //kropka
                {
                    crycore.AUTOCOMPLETE.getAutocomplete(ref sciDocument, ref _Autocomplete);

                    e.Handled = true;
                }
            }

          int startMatch = this.sciDocument.Caret.Position;
            int endMatch;

           
            char charAfter;
            //40 (  41 )91 [93 ]123 {125 }46 kropka

            if (documentLength >startMatch)
            {
                charAfter = this.sciDocument.CharAt(startMatch);
                if (charAfter == 40 || charAfter == 123)
                {
                    endMatch = sciDocument.NativeInterface.BraceMatch(startMatch, 200);
                    if (endMatch - startMatch < minLength) return;

                    sciDocument.NativeInterface.BraceHighlight(startMatch, endMatch);
                    Range r = sciDocument.GetRange(0, documentLength);
                    r.ClearIndicator(3);

                    r = new Range(startMatch + 1, endMatch, sciDocument);
                    r.SetIndicator(3);
                }

                if (charAfter == 41 || charAfter == 125)
                {

                    endMatch = sciDocument.NativeInterface.BraceMatch(startMatch, 200);
                    if (startMatch - endMatch < minLength) return;

                    sciDocument.NativeInterface.BraceHighlight(endMatch, startMatch);
                }
            }

        }



        /// <summary>
        /// Obsluga klawiszy skrotow np wywolanie snippetow
        /// 1.snippety 
        /// 2.F5 execute
        /// 3.F4 format
        /// </summary>
        private void sciDocument_KeyDown(object sender, KeyEventArgs e)
        {
            //jezeli kropka

            if (_runAutocomplete == Keys.ControlKey & e.KeyCode == Keys.Space)
            {
                crycore.AUTOCOMPLETE.getAutocomplete(ref  sciDocument, ref _Autocomplete);
            }


            simpleDebug.dump();

            if (_runSnippet == Keys.OemQuestion & e.KeyCode == Keys.Tab)
            {
                //TODO: 200803 coz tutaj nic lepszego nie wymyslilem ale optymalnie powinno byc usuneicie one char before
                this.sciDocument.SuspendLayout();
                int carret;
                int line;
                line = this.sciDocument.Caret.LineNumber;
                carret = this.sciDocument.Caret.Position;
                this.sciDocument.Text = this.sciDocument.Text.Remove(carret - 1, 1);
                this.sciDocument.Caret.LineNumber = line + 2;
                this.sciDocument.Selection.Start = carret;
                this.sciDocument.Caret.Position = carret;
                this.sciDocument.ResumeLayout();
                this.sciDocument.Snippets.ShowSnippetList();
                e.Handled = true;
            }
            _runSnippet = e.KeyCode;
            _runAutocomplete = e.KeyCode;

            if (e.KeyCode == Keys.F5)
            {
                this.Execute(globals._resultType);
                e.Handled = true;
            }

            if (e.KeyCode == Keys.F3)
            {
                this.Formatt();
                e.Handled = true;
            }

        }

        private void QueryForm_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            simpleDebug.dump();
            // Check for Control+E keypress (alternative to F5 for executing a  query)
            // Because this keystroke does get received in the KeyPress event, we are obliged to trap
            // it here (rather than in KeyDown) so we can set Handled to true to prevent the
            // default behaviour (ie a beep).

            if (e.KeyChar == '\x005' && RunState == RunState.Idle)
            {
                Execute(this.MdiParent.Controls["tsResult"].Text);
                e.Handled = true;
            }

        }
        #endregion

        /// <summary>Zmiana bazy w combobox</summary>
        private void cboDatabase_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            simpleDebug.dump();
            if (cboDatabase.SelectedIndex == 0)
                PopulateBrowser();
            else
                DbClient.Database = cboDatabase.Text;
            CheckDatabase();
        }

        private void frmDocument_FormClosed(object sender, FormClosedEventArgs e)
        {
            dbClient.Dispose();
        }

        public void setText(string text)
        {
            sciDocument.InsertText(text);
        }
    }
}