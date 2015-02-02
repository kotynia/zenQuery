using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Text;
using System.Text.RegularExpressions;
namespace zenQuery
{
    public partial class frmHistory : DockContent
    {

        private void HistoryForm_Load(object sender, EventArgs e)
        {
            DGrefresh();
            dgwidth();
        }

        public frmHistory()
        {
            InitializeComponent();
        }
        ///<summary>
        ///odswiez dgsqlhist
        ///</summary>
        private void DGrefresh()
        {
            //dgCONN.SelectionChanged += new System.Windows.Forms.(this.dgCONN_CellContentClick);

            System.Data.DataTable DT;
            DT = mk.msqllite.GetDataTable("SELECT dd [DateTime],server,database,sql,TIME,rows,login,username,HOST FROM sqlhist ORDER BY dd desc");
            dgsqlhist.DataSource = DT;
            DT.Dispose();

        }
        private void DGrefreshWithParam(string param)
        {
            //dgCONN.SelectionChanged += new System.Windows.Forms.(this.dgCONN_CellContentClick);

            System.Data.DataTable DT;
            DT = mk.msqllite.GetDataTable("SELECT dd [DateTime],server,database,sql,TIME,rows,login,username,HOST FROM sqlhist where TAG MATCH '" + param + "' ORDER BY dd desc");
            dgsqlhist.DataSource = DT;
            DT.Dispose();
        }


        private void dgsqlhist_SelectionChanged(object sender, EventArgs e)
        {

            try
            {
                rtsqlhist.Text = Convert.ToString(dgsqlhist.CurrentRow.Cells[3].Value);
            }
            catch { }

        }


        /// <summary>
        /// zmiana szerokosci kolumn
        /// </summary>
        private void dgwidth()
        {
            try
            {
                int fixedWidth = SystemInformation.VerticalScrollBarWidth + dgsqlhist.RowHeadersWidth + 2;

                int mul = (dgsqlhist.Width - fixedWidth);
                int widthallcolumns = dgsqlhist.Columns[0].Width + dgsqlhist.Columns[1].Width + dgsqlhist.Columns[2].Width + dgsqlhist.Columns[4].Width + dgsqlhist.Columns[5].Width + dgsqlhist.Columns[6].Width;
                dgsqlhist.Columns[3].Width = mul - widthallcolumns;
            }
            catch { }
        }

        private void dgsqlhist_SizeChanged(object sender, EventArgs e)
        {

            dgwidth();
        }




        private void btnrefresh_Click(object sender, EventArgs e)
        {
            DGrefresh();
            dgwidth();
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            mk.msqllite.ExecuteNonQuery("delete from sqlhist;");
            DGrefresh();
        }

        private void frmHistory_FormClosed(object sender, FormClosedEventArgs e)
        {
            // isLoaded = false;
            //frmHistoryLoaded = false;
        }


        /// <summary>
        /// Zamienia entery na spacje 
        /// </summary>
        private void dgsqlhist_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
                e.Value = zenQuery.Logic.mMisc.Replace(e.Value.ToString(), "\r\n", " ");

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //where ingredients match 'onions cheese'
            DGrefreshWithParam(txtSearch.Text);
        }

        private void frmHistory_Activated(object sender, EventArgs e)
        {
            DGrefresh();
        }

    }
}
