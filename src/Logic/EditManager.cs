using System;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Collections; //hashtable

namespace zenQuery.Logic
{
    /// <summary>
    /// A mediator for managing Edit menu commands (copy, cut, paste, etc)
    /// </summary>
    public class EditManager //: Component
    {
        // MenuItems to which to connect
        ToolStripMenuItem miEdit, miUndo, miCopy, miCopyWithHeaders, miUpdate, miInsert, miDelete, miSelect, miCut, miPaste, miSelectAll;

        private static EditManager _EditManagerInstance = new EditManager();

        private EditManager()
        {
            MenuItemCopy = null;
            MenuItemCopyWithHeaders = null;
            MenuItemUpdate = null;
            MenuItemCut = null;
            MenuItemEdit = null;
            MenuItemPaste = null;
            MenuItemSelectAll = null;
            MenuItemUndo = null;

        }

        public static EditManager GetEditManager()
        {
            return _EditManagerInstance;
        }

        // Menu item implementing Edit submenu.  Attach/detach event handler
        // to popup event so we can enable/disable sub-items when menu is activated.
        public ToolStripMenuItem MenuItemEdit
        {
            get { return miEdit; }
            set
            {
                if (miEdit != null)
                {
                    //                    miEdit.Click -= new EventHandler(miEdit_Popup);
                    miEdit.DropDownOpening -= new EventHandler(miEdit_Popup);
                }
                miEdit = value;
                if (miEdit != null)
                {
                    //                    miEdit.Click += new EventHandler(miEdit_Popup);
                    miEdit.DropDownOpening += new EventHandler(miEdit_Popup);
                }
            }
        }

        public ToolStripMenuItem MenuItemUndo
        {
            get { return miUndo; }
            set
            {
                if (miUndo != null)
                    miUndo.Click -= new EventHandler(miUndo_Click);
                miUndo = value;
                if (miUndo != null)
                    miUndo.Click += new EventHandler(miUndo_Click);
            }
        }

        public ToolStripMenuItem MenuItemCopy
        {
            get { return miCopy; }
            set
            {
                if (miCopy != null)
                    miCopy.Click -= new EventHandler(miCopy_Click);
                miCopy = value;
                if (miCopy != null)
                    miCopy.Click += new EventHandler(miCopy_Click);
            }
        }

        public ToolStripMenuItem MenuItemCopyWithHeaders
        {
            get { return miCopyWithHeaders; }
            set
            {
                if (miCopyWithHeaders != null)
                    miCopyWithHeaders.Click -= new EventHandler(miCopyWithHeaders_Click);
                miCopyWithHeaders = value;
                if (miCopyWithHeaders != null)
                    miCopyWithHeaders.Click += new EventHandler(miCopyWithHeaders_Click);
            }
        }
        /// <summary>
        /// Update
        /// </summary>
        public ToolStripMenuItem MenuItemUpdate
        {
            get { return miUpdate; }
            set
            {
                if (miUpdate != null)
                    miUpdate.Click -= new EventHandler(miUpdate_Click);
                miUpdate = value;
                if (miUpdate != null)
                    miUpdate.Click += new EventHandler(miUpdate_Click);
            }
        }

        /// <summary>
        /// Insert
        /// </summary>     
        public ToolStripMenuItem MenuItemInsert
        {
            get { return miInsert; }
            set
            {
                if (miInsert != null)
                    miInsert.Click -= new EventHandler(miInsert_Click);
                miInsert = value;
                if (miInsert != null)
                    miInsert.Click += new EventHandler(miInsert_Click);
            }
        }


        private ScintillaNet.Scintilla document;


        public ToolStripMenuItem MenuItemCut
        {
            get { return miCut; }
            set
            {
                if (miCut != null)
                    miCut.Click -= new EventHandler(miCut_Click);
                miCut = value;
                if (miCut != null)
                    miCut.Click += new EventHandler(miCut_Click);
            }
        }

        public ToolStripMenuItem MenuItemPaste
        {
            get { return miPaste; }
            set
            {
                if (miPaste != null)
                    miPaste.Click -= new EventHandler(miPaste_Click);
                miPaste = value;
                if (miPaste != null)
                    miPaste.Click += new EventHandler(miPaste_Click);
            }
        }

        public ToolStripMenuItem MenuItemSelectAll
        {
            get { return miSelectAll; }
            set
            {
                if (miSelectAll != null)
                    miSelectAll.Click -= new EventHandler(miSelectAll_Click);
                miSelectAll = value;
                if (miSelectAll != null)
                    miSelectAll.Click += new EventHandler(miSelectAll_Click);
            }
        }


        //Utworzenie  menu
        public ContextMenuStrip GetContextMenuStrip(Control parent, ref ScintillaNet.Scintilla doc)
        {
            document = doc;

            mk.Logic.simpleDebug.dump();
            parent.SuspendLayout();

            ContextMenuStrip cm = new ContextMenuStrip();
            //MenuStrip cm = new MenuStrip();
            //cm.Parent = parent;

            cm.Opened += new EventHandler(miEdit_Popup);

            cm.Items.AddRange(new ToolStripItem[] { new ToolStripMenuItem("Copy",  img.Resources.copy , new EventHandler(miCopy_Click)),
            new ToolStripMenuItem("CopyWithHeaders",  img.Resources.copy, new EventHandler(miCopyWithHeaders_Click)) ,
            new ToolStripMenuItem("SelectAll",  img.Resources.copy, new EventHandler(miSelectAll_Click)) ,
            new ToolStripMenuItem(),
            new ToolStripMenuItem("Generate Select" ,  img.Resources.copy, new EventHandler(miSelect_Click)),
            new ToolStripMenuItem("Generate Update" ,  img.Resources.copy, new EventHandler(miUpdate_Click)),
            new ToolStripMenuItem("Generate Insert" ,  img.Resources.copy, new EventHandler(miInsert_Click)), 
            new ToolStripMenuItem("Generate Delete" ,  img.Resources.copy, new EventHandler(miDelete_Click)), 
            new ToolStripMenuItem("Save to SQLLITE database" ,  img.Resources.copy, new EventHandler(miSaveToSqlLite_Click))
            }
                );


            if (!(parent is DataGridView))
                cm.Items[1].Enabled = false;
            parent.ResumeLayout();
            return cm;

        }

        public ContextMenu GetContextMenu()
        {
            mk.Logic.simpleDebug.dump();
            MenuItem[] mi = new MenuItem[] { new MenuItem(miCopy.Text, new EventHandler(miCopy_Click)) };
            ContextMenu cm = new ContextMenu();
            cm.MenuItems.AddRange(new MenuItem[] {new MenuItem(miCopy.Text, new EventHandler(miCopy_Click))
            ,new MenuItem(miCopyWithHeaders.Text, new EventHandler(miCopyWithHeaders_Click))});

            return cm;
        }


        public void Undo()
        {
            mk.Logic.simpleDebug.dump();
            Control c = GetActiveControl();
            if (c is TextBoxBase) ((TextBoxBase)c).Undo();
        }

        /// <summary>
        /// Copy to clipboard
        /// </summary>
        public void Copy()
        {
            mk.Logic.simpleDebug.dump();
            Control c = GetActiveControl();
            if (c is TextBoxBase)
                ((TextBoxBase)c).Copy();
            if (c is DataGridView)
            {
                DataGridView ctrl = (DataGridView)c;
                // Add the selection to the clipboard.
                ctrl.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithAutoHeaderText;

                if (ctrl.GetClipboardContent() != null)
                    Clipboard.SetDataObject(
                        ctrl.GetClipboardContent());
            }
        }


        /// <summary>
        /// Copy to clipboard with headers
        /// </summary>
        public void CopyWithHeaders()
        {
            mk.Logic.simpleDebug.dump();
            Control c = GetActiveControl();
            if (c is TextBoxBase)
                return;
            if (c is DataGridView)
            {
                DataGridView ctrl = (DataGridView)c;
                // Add the selection to the clipboard.
                ctrl.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;

                if (ctrl.GetClipboardContent() != null)
                    Clipboard.SetDataObject(
                                       ctrl.GetClipboardContent());


            }


        }


        /// <summary>
        /// Update pomysl ataki aby mozna bylo update wykonac
        /// zrobimy update
        /// </summary>
        public void Update()
        {
            mk.Logic.simpleDebug.dump();
            Control c = GetActiveControl();
            if (c is TextBoxBase)
                return;
            if (c is DataGridView)
            {
                DataGridView ctrl = (DataGridView)c;
                StringBuilder output = new StringBuilder();
                string strsql = "/*Update for Row {0}*/\nupdate <table> set {1} \n where {2};\n";


                int row = -1;
                string values = "";
                int i = 0;


                /////////////sortowanie
                Dictionary<string, DataGridViewCell> selecteditemsDict = new Dictionary<string, DataGridViewCell>();
                foreach (DataGridViewCell cell in ctrl.SelectedCells)  //trzeba zrobic orde bo sa w kolejnosci zaznaczania 
                {
                    selecteditemsDict.Add(String.Format("{0:000000000}.{1:0000}", cell.RowIndex, cell.ColumnIndex), cell);
                }
                ArrayList aKeys = new ArrayList(selecteditemsDict.Keys); //< tutaj nazwa hashtable  
                aKeys.Sort();


                DataGridViewCell cellCurrent;
                foreach (string key in aKeys)
                {
                    selecteditemsDict.TryGetValue(key, out cellCurrent);
                    System.Diagnostics.Debug.Print(key);
                    if (i == 0)
                    {
                        row = cellCurrent.RowIndex;
                        i += 1; //tylko jednorazowo potrzbny zaznaczenie pierwszego przebiegu
                    }

                    if (row != cellCurrent.RowIndex)
                    {
                        output.AppendFormat(strsql, row, _RemoveLast(values, ","), getAllWhereValues(ctrl.Rows[row]));
                        values = ""; //reset
                    }

                    row = cellCurrent.RowIndex;
                    values += cellCurrent.OwningColumn.Name + "='" + cellCurrent.Value.ToString() + "' ,";

                }

                //dodanie ostatniego
                output.AppendFormat(strsql, row, _RemoveLast(values, ","), getAllWhereValues(ctrl.Rows[row]));
                InsertIntoSciDocument(output.ToString());

                //  Clipboard.SetText(output.ToString());
            }

        }




        /// <summary>
        /// Update pomysl taki aby mozna bylo delet wykonac
        /// </summary>
        /// <param name="command">command: select ,delete </param>
        public void Delete(string command)
        {
            mk.Logic.simpleDebug.dump();
            Control c = GetActiveControl();
            if (c is TextBoxBase)
                return;
            if (c is DataGridView)
            {
                DataGridView ctrl = (DataGridView)c;

                //   var table = ctrl.DataSource as DataTable;


                StringBuilder output = new StringBuilder();
                string strsql, separator = "";
                if (command == "delete")
                {
                    strsql = "/*Delete for Row {0}*/\n Delete  from <table> where {1};\n";
                    separator = ",";
                }
                else
                {
                    strsql = "/*select for Row {0}*/\n select *  from <table> where {1};\n";
                    separator = " and ";
                }


                int row = -1;
                string values = "";
                int i = 0;


                /////////////sortowanie
                Dictionary<string, DataGridViewCell> selecteditemsDict = new Dictionary<string, DataGridViewCell>();
                foreach (DataGridViewCell cell in ctrl.SelectedCells)  //trzeba zrobic orde bo sa w kolejnosci zaznaczania 
                {
                    selecteditemsDict.Add(String.Format("{0:000000000}.{1:0000}", cell.RowIndex, cell.ColumnIndex), cell);
                }
                ArrayList aKeys = new ArrayList(selecteditemsDict.Keys); //< tutaj nazwa hashtable  
                aKeys.Sort();


                DataGridViewCell cellCurrent;
                foreach (string key in aKeys)
                {
                    selecteditemsDict.TryGetValue(key, out cellCurrent);
                    System.Diagnostics.Debug.Print(key);
                    if (i == 0)
                    {
                        row = cellCurrent.RowIndex;
                        i += 1; //tylko jednorazowo potrzbny zaznaczenie pierwszego przebiegu
                    }

                    if (row != cellCurrent.RowIndex)
                    {
                        output.AppendFormat(strsql, row, _RemoveLast(values, separator));
                        values = ""; //reset
                    }

                    row = cellCurrent.RowIndex;
                    values += cellCurrent.OwningColumn.Name + "='" + cellCurrent.Value.ToString() + "' " + separator;

                }

                output.AppendFormat(strsql, row, _RemoveLast(values, separator));


                InsertIntoSciDocument(output.ToString());
                // Clipboard.SetText(output.ToString());
            }

        }


        /// <summary>
        /// Insert
        /// </summary>
        public void Insert()
        {
            mk.Logic.simpleDebug.dump();
            Control c = GetActiveControl();
            if (c is TextBoxBase)
                return;
            if (c is DataGridView)
            {
                DataGridView ctrl = (DataGridView)c;
                StringBuilder output = new StringBuilder();
                string strsql = "/*Row {0}*/ insert into  <table> ({1}) \n values ({2}); \nGO;";


                int row = -1;
                string columns = "";
                string values = "";
                int i = 0;

                /////////////sortowanie
                Dictionary<string, DataGridViewCell> selecteditemsDict = new Dictionary<string, DataGridViewCell>();
                foreach (DataGridViewCell cell in ctrl.SelectedCells)  //trzeba zrobic orde bo sa w kolejnosci zaznaczania 
                {
                    selecteditemsDict.Add(String.Format("{0:000000000}.{1:0000}", cell.RowIndex, cell.ColumnIndex), cell);
                }
                ArrayList aKeys = new ArrayList(selecteditemsDict.Keys); //< tutaj nazwa hashtable  
                aKeys.Sort();


                DataGridViewCell cellCurrent;
                foreach (string key in aKeys)
                {
                    selecteditemsDict.TryGetValue(key, out cellCurrent);
                    System.Diagnostics.Debug.Print(key);
                    if (i == 0)
                    {
                        row = cellCurrent.RowIndex;
                        i += 1; //tylko jednorazowo potrzbny zaznaczenie pierwszego przebiegu
                    }

                    if (row != cellCurrent.RowIndex)
                    {
                        output.AppendFormat(strsql, row, _RemoveLast(columns, ","), _RemoveLast(values, ","));
                        values = ""; //reset
                        columns = ""; values = ""; //reset
                    }
                    row = cellCurrent.RowIndex;
                    columns += cellCurrent.OwningColumn.Name + ",";
                    values += "'" + cellCurrent.Value.ToString() + "',";

                }





                output.AppendFormat(strsql, row, _RemoveLast(columns, ","), _RemoveLast(values, ","));
                InsertIntoSciDocument(output.ToString());
                //Clipboard.SetText(output.ToString());
            }

        }

        private void InsertIntoSciDocument(string text)
        {
            int selectionLength;
            selectionLength = document.Selection.Length;
            string query = selectionLength == 0 ? document.Text : document.Selection.Text;


            if (selectionLength == 0)
                document.InsertText("\n" + text);//Logic.mMisc.sqlhereWebservice(query);
            else
                document.Selection.Text = text;

        }


        public void SaveToSqlLite()
        {

            //Control c = GetActiveControl();
            //if (c is TextBoxBase)
            //    return;
            //if (c is DataGridView)
            //{


            //    DataGridView ctrl = (DataGridView)c;
            //    var table = ctrl.DataSource as DataTable;


            //    //wykonanie create table
            //    foreach (DataColumn column in table.Columns)
            //    {
            //    }

            //    string str = Environment.CurrentDirectory.ToString();


            //    foreach (DataRow row in table.Rows)
            //    {
            //        foreach (DataColumn column in table.Columns)
            //        {
            //            Console.WriteLine(row[column]);
            //        }
            //    }


            //}
        }


        string getAllWhereValues(DataGridViewRow row)
        {
            StringBuilder output = new StringBuilder();
            string strsql = " {0} = '{1}' and";

            foreach (DataGridViewCell cell in row.Cells)
            {
                output.AppendFormat(strsql, cell.OwningColumn.Name, cell.Value.ToString());

            }
            return _RemoveLast(output.ToString(), "and");

        }



        /// <summary>
        /// Przydatne w przypadku skladania komend
        /// usuwa tylko ostatnie wystapienie jezeli nic za nim nie ma 
        /// moga sbyc spacje (sa zawsze usuwane)
        /// </summary>
        /// <param name="lastStringToRemove">np [,] lub [and]</param>
        public static string _RemoveLast(string input, string lastStringToRemove)
        {
            //input = input.TrimEnd();
            int i = input.LastIndexOf(lastStringToRemove);
            int length = input.Length;

            if (i > 0 && length > 0 && length - lastStringToRemove.Length == i)
            {
                return input.Remove(i, lastStringToRemove.Length);
            }

            return input;

        }


        public void Cut()
        {
            Control c = GetActiveControl();
            if (c is TextBoxBase) ((TextBoxBase)c).Cut();
        }

        public void Paste()
        {
            Control c = GetActiveControl();
            if (c is TextBoxBase) ((TextBoxBase)c).Paste();
        }

        public void SelectAll()
        {
            Control c = GetActiveControl();

            if (c is TextBoxBase)
                ((TextBoxBase)c).SelectAll();
            if (c is DataGridView)
            {
                DataGridView ctrl = (DataGridView)c;
                // Add the selection to the clipboard.
                ctrl.SelectAll();
            }

        }

        protected Control GetActiveControl()
        {
            Form form = Form.ActiveForm;
            if (form != null && form.IsMdiContainer)
                form = form.ActiveMdiChild;
            return GetActiveControl(form);
        }

        protected Control GetActiveControl(Control ContainerControl)
        {
            if (ContainerControl == null || ContainerControl as ContainerControl == null)
                return ContainerControl;
            else
                return GetActiveControl(((ContainerControl)ContainerControl).ActiveControl);
        }

        protected void miEdit_Popup(object sender, EventArgs e)
        {
            EnableSubMenus();
        }

        private void EnableSubMenus()
        {
            bool canUndo, canCopy, canCopyWithHeaders, canCut, canPaste;
            System.Diagnostics.Debug.WriteLine("Popup");
            canUndo = canCopy = canCopyWithHeaders =
                      canCut = canPaste = false;
            Control c = GetActiveControl();
            if (c != null)
                CanEdit(c, ref canUndo, ref canCopy, ref canCopyWithHeaders, ref canCut, ref canPaste);
            if (miUndo != null) miUndo.Enabled = canUndo;
            if (miCopy != null) miCopy.Enabled = canCopy;
            if (miCopyWithHeaders != null) miCopyWithHeaders.Enabled = canCopyWithHeaders;
            if (miCut != null) miCut.Enabled = canCut;
            if (miPaste != null) miPaste.Enabled = canPaste;
        }

        protected void CanEdit(Control c, ref bool canUndo, ref bool canCopy, ref bool canCopyWithHeaders, ref bool canCut, ref bool canPaste)
        {
            if (c is TextBoxBase)
            {
                TextBoxBase t = (TextBoxBase)c;
                canUndo = t.CanUndo;
                canCopy = t.SelectionLength > 0;
                canCopyWithHeaders = false;
                canCut = t.SelectionLength > 0 && !t.ReadOnly;
                IDataObject iData = Clipboard.GetDataObject();
                canPaste = !t.ReadOnly && iData.GetDataPresent(DataFormats.Text); ;
            }
            else if (c is DataGridView)
            {
                DataGridView dgv = (DataGridView)c;
                canUndo = false;
                canCopy = dgv.RowCount > 0;
                canCopyWithHeaders = canCopy;
                canCut = false;
                canPaste = false;
            }
        }



        #region Handlery dynamicznie utworzonych przeyciskow


        protected void miSaveToSqlLite_Click(object sender, EventArgs e)
        {
            SaveToSqlLite();
        }
        protected void miUndo_Click(object sender, EventArgs e)
        {
            Undo();
        }


        protected void miCopy_Click(object sender, EventArgs e)
        {
            Copy();
        }



        protected void miCopyWithHeaders_Click(object sender, EventArgs e)
        {
            CopyWithHeaders();
        }
        protected void miUpdate_Click(object sender, EventArgs e)
        {
            Update();
        }
        protected void miInsert_Click(object sender, EventArgs e)
        {
            Insert();
        }
        protected void miDelete_Click(object sender, EventArgs e)
        {
            Delete("delete");
        }

        protected void miSelect_Click(object sender, EventArgs e)
        {
            Delete("select");
        }

        protected void miCut_Click(object sender, EventArgs e)
        {
            Cut();
        }
        protected void miPaste_Click(object sender, EventArgs e)
        {
            Paste();
        }
        protected void miSelectAll_Click(object sender, EventArgs e)
        {

            SelectAll();
        }
        #endregion


        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion
    }

}
