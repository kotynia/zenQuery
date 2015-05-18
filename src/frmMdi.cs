using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using mk.Logic;
using ScintillaNET;
namespace zenQuery
{
    public partial class frmMdi : Form
    {


        /// <summary>
        /// Initialize
        /// </summary>
        public frmMdi()
        {
            System.Threading.Thread.CurrentThread.Name = "Main Thread";		// to ease debugging
            //	I personally really dislike the OfficeXP look on Windows XP with the blue.
            ToolStripManager.RenderMode = ToolStripManagerRenderMode.Professional;
            ((ToolStripProfessionalRenderer)ToolStripManager.Renderer).ColorTable.UseSystemColors = true;

            // ToolStripComboBox tsx = (ToolStripComboBox)ts.Items["tsResult"];



            InitializeComponent();
        }

        #region Resizing Forms
        // Variables having to do with the resizing code follow
        protected bool bSizeMode = false;
        /// <summary>
        /// Windows Constants for WndProc call in BeginResize/EndResize events
        /// </summary>
        protected const int WM_SIZING = 0x214,
        WM_ENTERSIZEMOVE = 0x231,
        WM_EXITSIZEMOVE = 0x232,
        WM_SYSCOMMAND = 0x112,
        SC_SIZE = 0xF000;
        /// <summary>
        /// Fires once at the beginning of a resizing drag.
        /// </summary>
        public event EventHandler BeginResize;
        /// <summary>
        /// Fires once at the end of a resizing drag.
        /// </summary>
        public event EventHandler EndResize;
        /// <summary>
        /// Process Windows messages (in this case to provide Begin/EndResize events
        /// </summary>
        /// <param name="m"></param>
        //protected override void WndProc(ref Message m)
        //{
        //    simpleDebug.dump();
        //    switch (m.Msg)
        //    {
        //        case WM_SIZING:
        //            // Resize event already provided
        //            break;
        //        case WM_SYSCOMMAND:
        //            bSizeMode = ((m.WParam.ToInt32() & 0xFFF0) == SC_SIZE);
        //            break;
        //        case WM_ENTERSIZEMOVE:
        //            if (bSizeMode)
        //            {
        //                OnBeginResize(EventArgs.Empty);
        //            }
        //            break;
        //        case WM_EXITSIZEMOVE:
        //            {
        //                if (bSizeMode)
        //                {
        //                    OnEndResize(EventArgs.Empty);
        //                }
        //                break;
        //            }

        //    } // end switch
        //    base.WndProc(ref m);
        //}



        /// <summary>
        /// Wylaczone Raise BeginResize event wylaczone
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnBeginResize(System.EventArgs e)
        {
            simpleDebug.dump();
            if (BeginResize != null)
                BeginResize(this, e);
        }

        /// <summary>
        /// Raise EndResize event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnEndResize(System.EventArgs e)
        {
            simpleDebug.dump();
            if (EndResize != null)
                EndResize(this, e);
        }


        private void frmMdi_ResizeBegin(object sender, EventArgs e)
        {
            simpleDebug.dump();
            OnBeginResize(e);
        }

        private void frmMdi_ResizeEnd(object sender, EventArgs e)
        {
            simpleDebug.dump();
            OnEndResize(e);
        }

        #endregion

        #region mkObslugaFormularzy: isFormAlreadyLoaded, Laduj Formularz x
        /// <summary>
        /// czy zaladowany formularz
        /// </summary>
        /// <param name="formToLoadName"></param>
        /// <returns></returns>
        private bool isFormAlreadyLoaded(string formToLoadName)
        {
            foreach (Form frmChild in this.MdiChildren)
            {
                if (frmChild.Name == formToLoadName)
                { frmChild.Activate(); return true; }
            } return false;
        }
        /// <summary>Laduj frmHistory</summary>
        private void tsQueryHistory_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            if (!isFormAlreadyLoaded("frmHistory"))
            {
                frmHistory c = new frmHistory();
                c.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;

                c.Text = "zenQuery History";
                c.Show(dockPanel);
            }
            setFrmDocumentMenu();
        }
        /// <summary>Laduj frmsnippets</summary>
        private void tsSnippets_Click(object sender, EventArgs e)
        {

            if (!isFormAlreadyLoaded("frmaction"))
            {
                frmaction c = new frmaction();
                c.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
                c.Text = "Actions";
                c.Show(dockPanel);

                //refresh akcji

            }
            setFrmDocumentMenu();

        }


        #endregion

        #region Menu menuStripMain , czyli pasek menu u gory standardowe opcje

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Close();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.Printing.Print();
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();

            ActiveDocument.Document.Printing.PrintPreview();
        }

        private void frmMdi_Load(object sender, EventArgs e)
        {
            simpleDebug.dump();
            DoConnect();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.SaveAs();
        }

        private void saveAllStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            foreach (frmDocument doc in dockPanel.Documents)
            {
                doc.Activate();
                doc.Save();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            OpenFile();
        }

        public bool OpenFile()
        {

            if (IsChildActive())
            {

                //wroc jesli nie wybrales ok 
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    return false;

                byte[] data = null;
                using (FileStream fs = File.Open(openFileDialog.FileName, FileMode.Open))
                {
                    data = new byte[fs.Length];
                    fs.Read(data, 0, (int)fs.Length);
                }
                UTF8Encoding temp = new UTF8Encoding(true);

                GetQueryChild().setText(temp.GetString(data));
                return true;

            }
            return false;
        }


        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        { simpleDebug.dump(); ActiveDocument.Document.UndoRedo.Undo(); }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.UndoRedo.Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.Clipboard.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();

            ActiveDocument.Document.Clipboard.Copy();
            ActiveDocument.Document.AutoComplete.List.Insert(0, "asdasd");
            ActiveDocument.Document.AutoComplete.Show();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.Clipboard.Paste();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.Selection.SelectAll();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.FindReplace.ShowFind();
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.FindReplace.ShowReplace();
        }



        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.GoTo.ShowGoToDialog();
        }

        private void toggleBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.Lines.Current.AddMarker(0);
        }

        private void previosBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            //	 I've got to redo this whole FindNextMarker/FindPreviousMarker Scheme
            Line l = ActiveDocument.Document.Lines.Current.FindPreviousMarker(1);
            if (l != null)
                l.Goto();
        }

        private void nextBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            //	 I've got to redo this whole FindNextMarker/FindPreviousMarker Scheme
            Line l = ActiveDocument.Document.Lines.Current.FindNextMarker(1);
            if (l != null)
                l.Goto();
        }

        private void clearBookmarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.Markers.DeleteAll(0);
        }



        private void collectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.DropMarkers.Collect();
        }

        private void makeUpperCaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.Commands.Execute(BindableCommand.UpperCase);
        }

        private void makeLowerCaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.Commands.Execute(BindableCommand.LowerCase);
        }

        private void commentStreamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.Commands.Execute(BindableCommand.StreamComment);
        }

        private void commentLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.Commands.Execute(BindableCommand.LineComment);
        }

        private void uncommentLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.Commands.Execute(BindableCommand.LineUncomment);
        }



        private void insertSnippetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.Snippets.ShowSnippetList();
        }

        private void surroundWithToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.Snippets.ShowSurroundWithList();
        }

        private void whiteSpaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            whiteSpaceToolStripMenuItem.Checked = !whiteSpaceToolStripMenuItem.Checked;

            //if (whiteSpaceToolStripMenuItem.Checked)
            //{
            //    ActiveDocument.Document.WhiteSpace.Mode = WhiteSpaceMode.VisibleAlways;
            //}
            //else
            //{
            //    ActiveDocument.Document.WhiteSpace.Mode = WhiteSpaceMode.Invisible;
            //}
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            wordWrapToolStripMenuItem.Checked = !wordWrapToolStripMenuItem.Checked;


            //TODO find new method
            //if (wordWrapToolStripMenuItem.Checked)
            //{
            //    ActiveDocument.Document.LineWrapping.Mode = LineWrapping.Word;
            //}
            //else
            //{
            //    ActiveDocument.Document.LineWrap.Mode = WrapMode.None;
            //}
        }

        private void endOfLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            endOfLineToolStripMenuItem.Checked = !endOfLineToolStripMenuItem.Checked;
            ActiveDocument.Document.EndOfLine.IsVisible = endOfLineToolStripMenuItem.Checked;
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.ZoomIn();
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.ZoomOut();
        }

        private void resetZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.ZoomFactor = 0;
        }

        private void foldLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.Lines.Current.FoldExpanded = true;
        }

        private void unfoldLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.Lines.Current.FoldExpanded = false;
        }

        private void foldAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            foreach (Line l in ActiveDocument.Document.Lines)
            {
                l.FoldExpanded = true;
            }
        }

        private void unfoldAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            foreach (Line l in ActiveDocument.Document.Lines)
            {
                l.FoldExpanded = true;
            }
        }

        private void cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.ConfigurationManager.Language = "cs";
            ActiveDocument.Document.Indentation.SmartIndentType = SmartIndent.CPP;
        }

        private void plainTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.ConfigurationManager.Language = "";
        }

        private void hTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.ConfigurationManager.Language = "html";
        }

        private void mSSQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.ConfigurationManager.Language = "mssql";
        }

        private void vBScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.ConfigurationManager.Language = "vbscript";
        }

        private void pythonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.ConfigurationManager.Language = "python";
        }

        private void xMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.ConfigurationManager.Language = "xml";
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            new frmAbout().ShowDialog(this);
        }

        private void lineNumbersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            lineNumbersToolStripMenuItem.Checked = !lineNumbersToolStripMenuItem.Checked;
            if (lineNumbersToolStripMenuItem.Checked)
                ActiveDocument.Document.Margins.Margin0.Width = 20;
            else
                ActiveDocument.Document.Margins.Margin0.Width = 0;
        }

        private void navigateForwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.DocumentNavigation.NavigateForward();
        }

        private void navigateBackwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            ActiveDocument.Document.DocumentNavigation.NavigateBackward();
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            //NewDocument();
        }

        #endregion

        #region SCIDE


        /// <summary>
        /// Numer dokumentu w programie
        /// </summary>
        public int _newDocumentCount = 0;

        /// <summary>
        /// Aktywny dokument w mdi
        /// </summary>
        public frmDocument ActiveDocument
        {
            get
            {
                return dockPanel.ActiveDocument as frmDocument;
            }
        }
        #endregion

        /// <summary>
        /// Uruchamijac sprawdza jaki typ okna jest czy jest to dokument
        /// czy nie i ukrywa menu etc
        /// </summary>
        private void setFrmDocumentMenu()
        {
            if (ActiveMdiChild == null || ActiveMdiChild.Name != "frmDocument")
            {
                frmDocumentMenu(false);
            }
            else
            {
                frmDocumentMenu(true);
            }
        }


        private void frmDocumentMenu(bool _enabled)
        {
            tsDisconnect.Enabled = _enabled;
            tsNew.Enabled = _enabled;
            tsOpen.Enabled = _enabled;
            tsSave.Enabled = _enabled;
            tsExecute.Enabled = _enabled;
            tsCancel.Enabled = _enabled;
            tsResult.Enabled = _enabled;
            tsHideResults.Enabled = _enabled;
            tsHideBrowser.Enabled = _enabled;
            tsBeautyCode.Enabled = _enabled;
            editToolStripMenu.Enabled = _enabled;
            viewStripMenuItem.Enabled = _enabled;
            newToolStripMenuItem.Enabled = _enabled;
            openToolStripMenuItem.Enabled = _enabled;
            saveToolStripMenuItem.Enabled = _enabled;
            saveAsToolStripMenuItem.Enabled = _enabled;
            saveAllStripMenuItem.Enabled = _enabled;
            printToolStripMenuItem.Enabled = _enabled;
            printPreviewToolStripMenuItem.Enabled = _enabled;
        }

        /// <summary>
        /// Wywolanie polaczenia
        /// </summary>
        private void tsConnect_Click(object sender, EventArgs e)
        {
            DoConnect();
        }

        private void DoConnect()
        {
            simpleDebug.dump();
            ConnectForm cf = new ConnectForm();
            if (cf.ShowDialog() == DialogResult.OK)
            {
                //MK 
                frmDocument qf = new frmDocument(cf.DbClient, cf.Browser, cf.LowBandwidth, "MSSQL");
                qf.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
                qf.Text = "Query" + ++_newDocumentCount;
                qf.Show(dockPanel);


            }
        }


        /// <summary>Sprzawdza czy biezacy formularz to frmDocument</summary>
        bool IsChildActive()
        {

            if (ActiveMdiChild != null)
            {
                if (ActiveMdiChild.Name == "frmDocument")
                    return true;
            }

            return false;
        }


        /// <summary>
        /// biezacy aktywny dokument
        /// </summary>
        /// <returns></returns>
        frmDocument GetQueryChild()
        {
            return (frmDocument)ActiveMdiChild;
        }

        private void tsDisconnect_Click(object sender, EventArgs e)
        {
            if (IsChildActive()) GetQueryChild().Close();
        }

        private void tsOpen_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            //TODO: openfile
            if (IsChildActive()) OpenFile();

        }


        private void tsSave_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            if (IsChildActive()) GetQueryChild().Save();
        }

        private void tsExecute_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            if (IsChildActive()) GetQueryChild().Execute(tsResult.Text);
        }

        private void tsCancel_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            if (IsChildActive()) GetQueryChild().Cancel();
        }

        private void tsHideResults_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            if (IsChildActive()) GetQueryChild().HideResults = !GetQueryChild().HideResults;
        }

        private void tsHideBrowser_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            if (IsChildActive()) GetQueryChild().HideBrowser = !GetQueryChild().HideBrowser;

        }



        private void tsBeautyCode_Click(object sender, EventArgs e)
        {
            simpleDebug.dump();
            try
            {
                if (IsChildActive()) GetQueryChild().Formatt();
            }
            catch (Exception)
            {
                MessageBox.Show(this, "Nothing to beauty or engine error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        #region Output result
        /// <summary>
        /// Wybranie output text lub grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsResult_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (tsResult.SelectedItem.ToString() == "Result Grid" || tsResult.SelectedItem.ToString() == "Result Grid Trans.")
            { DoResultsInGrid(); }
            else { DoResultsInText(); }

            globals._resultType = tsResult.SelectedItem.ToString();
        }
        private void DoResultsInText()
        {
            // Changing the value of this property will automatically invoke the QueryForm's
            // PropertyChanged event, which we've wired to EnableControls().
            if (IsChildActive())
            {
                GetQueryChild().ResultsInText = true;
                GetQueryChild().ResultType = tsResult.SelectedItem.ToString();
            }

        }

        private void DoResultsInGrid()
        {
            // Changing the value of this property will automatically invoke the QueryForm's
            // PropertyChanged event, which we've wired to EnableControls().
            if (IsChildActive())
            {
                GetQueryChild().ResultType = tsResult.SelectedItem.ToString();
                GetQueryChild().ResultsInText = false;
            }
        }

        #endregion

        private void tsNew_Click(object sender, EventArgs e)
        {

            simpleDebug.dump();
            if (IsChildActive())
            {
                // Change the cursor to an hourglass while we're doing this, in case establishing the
                // new connection takes some time.
                Cursor oldCursor = Cursor;
                Cursor = Cursors.WaitCursor;
                frmDocument newQF = GetQueryChild().Clone();
                if (newQF != null)																// could be null if new connection failed
                {
                    newQF.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
                    newQF.Text = "Query" + ++_newDocumentCount;

                    newQF.Show(dockPanel);
                    // This is so that we can update the toolbar and menu as the state of the QueryForm changes.
                    //newQF.PropertyChanged += new EventHandler(ChildPropertyChanged);

                }
                Cursor = oldCursor;
            }
        }


        private void btnsearch_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(txtsearch.Text.Trim()))
            {
                MessageBox.Show(this, "You must provide search string.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtsearch.Focus();
                return;
            }

                foreach (Form frmChild in this.MdiChildren)
                {
                    if (frmChild.Name == "frmDocument")
                    {
                        frmDocument y = (frmDocument)frmChild;
                        TreeView treeView = y.tree;

                        treeView.BeginUpdate();
                        treeView.Nodes.Clear();
                        y.PopulateBrowser();

                        if (this.txtsearch.Text != string.Empty)
                        {
                            foreach (TreeNode _parentNode in treeView.Nodes)
                            {
                                int nodescount = _parentNode.Nodes.Count;

                                for (int i = 0; i < nodescount; i++)
                                {
                                    if (nodescount == 0)
                                        break;

                                    TreeNode x = _parentNode.Nodes[i];
                                    if (x == null)
                                        break;
                                    if (x.Text.IndexOf(this.txtsearch.Text, StringComparison.InvariantCultureIgnoreCase) == -1)
                                    {
                                        _parentNode.Nodes[i].Remove();
                                        i--;
                                        nodescount--;
                                    }


                                }
                            }
                        }

                        //enables redrawing tree after all objects have been added
                        treeView.EndUpdate();
                    }

                }
          
                Cursor oldCursor = Cursor;
                Cursor = Cursors.WaitCursor;

                //frmDocument qf = new frmDocument(cf.DbClient, cf.Browser, cf.LowBandwidth, "MSSQL");
                //qf.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
                //qf.Text = "Query" + ++_newDocumentCount;
                //qf.Show(dockPanel);


                frmSearch c = new frmSearch(chkobjecttext.Checked, txtsearch.Text, GetQueryChild().DbClient);
                c.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
                c.Text = "Search Result" + ++_newDocumentCount;
                c.Show(dockPanel);
                //c.Show();
                Cursor = oldCursor;
          
        }



        private void chkobjectname_CheckedChanged(object sender, EventArgs e)
        {

            chkobjecttext.Checked = false;
         
        }

        private void chkobjecttext_CheckedChanged(object sender, EventArgs e)
        {

            chkobjectname.Checked = false;
         
        }


        private void dockPanel_ActiveDocumentChanged_1(object sender, EventArgs e)
        {
            simpleDebug.dump();
            setFrmDocumentMenu();
        }

    




        //private void DoNew()
        //{
        //    if (IsChildActive())
        //    {
        //        // Change the cursor to an hourglass while we're doing this, in case establishing the
        //        // new connection takes some time.
        //        Cursor oldCursor = Cursor;
        //        Cursor = Cursors.WaitCursor;
        //        frmDocument newQF = GetQueryChild().Clone();
        //        if (newQF != null)																// could be null if new connection failed
        //        {
        //            ((Form)newQF).MdiParent = this;
        //            newQF.PropertyChanged += new EventHandler<EventArgs>(PropertyChanged);
        //            ((Form)newQF).Show();
        //        }
        //        Cursor = oldCursor;
        //    }
        //    else
        //        DoConnect();
        //}



    }
}