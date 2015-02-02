using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using WeifenLuo.WinFormsUI.Docking;

namespace zenQuery
{
    public partial class frmaction : DockContent
    {
        public frmaction()
        {
            InitializeComponent();
            this.Load += new EventHandler(frmaction_Load);
            dgCONN.SelectionChanged += new EventHandler(dgCONN_SelectionChanged);
            btnsave.Click += new EventHandler(btnsave_Click); //zapisanie
            btnnew.Click += new EventHandler(btnnew_Click); //dodanie
            btndelete.Click += new EventHandler(btndelete_Click); //usuniecie
            dgCONN.CellContentDoubleClick += new DataGridViewCellEventHandler(dgCONN_CellContentDoubleClick);
        }

        void dgCONN_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            tab.SelectedIndex = 1; //podglad szczegolow
        }

        void btndelete_Click(object sender, EventArgs e)
        {
            txtinfo.Text = mk.msqllite.mSQLLiteExecute(false, "delete from tblsnipitem where snipitemid = " + txtsnipitemid.Text + "", null, null);
            fillDG();
        }

        void btnnew_Click(object sender, EventArgs e)
        {
            List<SQLiteParameter> paramss = new List<SQLiteParameter>();
            paramss.Add(new SQLiteParameter("@Description", txtdescription.Text));
            paramss.Add(new SQLiteParameter("@strsql", txtstrsql.Text));
            paramss.Add(new SQLiteParameter("@type", txttype.Text));
            paramss.Add(new SQLiteParameter("@database", txtdatabase.Text));
            paramss.Add(new SQLiteParameter("@provider", txtprovider.Text));
            paramss.Add(new SQLiteParameter("@objecttype", txtobjecttype.Text));
            paramss.Add(new SQLiteParameter("@objectmask", txtobjectmask.Text));
            txtinfo.Text = mk.msqllite.mSQLLiteExecute(false, "insert into tblsnipitem  (Description,strsql,type,database,provider,objecttype,objectmask ) values (@Description,@strsql,@type,@database,@provider,@objecttype,@objectmask)", paramss, null);
            fillDG();

        }

        void btnsave_Click(object sender, EventArgs e)
        {
            List<SQLiteParameter> paramss = new List<SQLiteParameter>();
            paramss.Add(new SQLiteParameter("@snipitemid", txtsnipitemid.Text));//poprzednai wartosc tu trzymana
            paramss.Add(new SQLiteParameter("@Description", txtdescription.Text));
            paramss.Add(new SQLiteParameter("@strsql", txtstrsql.Text));
            paramss.Add(new SQLiteParameter("@type", txttype.Text));
            paramss.Add(new SQLiteParameter("@database", txtdatabase.Text));
            paramss.Add(new SQLiteParameter("@provider", txtprovider.Text));
            paramss.Add(new SQLiteParameter("@objecttype", txtobjecttype.Text));
            paramss.Add(new SQLiteParameter("@objectmask", txtobjectmask.Text));

            //RUNsnipitemID,Description,strsql,type,database,provider,objecttype,objectmask
            txtinfo.Text = mk.msqllite.mSQLLiteExecute(false, "update tblsnipitem  set description = @description,strsql=@strsql,type=@type,database=@database,provider=@provider,objecttype=@objecttype,objectmask=@objectmask where snipitemID = @snipitemID", paramss, null);



            fillDG();

        }

        void frmaction_Load(object sender, EventArgs e)
        {
            fillDG();

        }







        #region ObslugaDatagridview



        /// <summary>
        /// odswiezenei datagrid
        /// </summary>
        void fillDG()
        {
            string strsql = "select snipitemID,Description,strsql,type,database,provider,objecttype,objectmask from tblsnipitem order by snipitemID desc";

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
            dgRefresh();
        }


        /// <summary>
        /// Odswiezenei datagridview
        /// </summary>
        void dgRefresh()
        {
            //inicjalize value
            if (dgCONN.CurrentRow != null)
            {
                txtsnipitemid.Text = Convert.ToString(dgCONN.CurrentRow.Cells[0].Value);
                txtdescription.Text = Convert.ToString(dgCONN.CurrentRow.Cells[1].Value);
                txtstrsql.Text = Convert.ToString(dgCONN.CurrentRow.Cells[2].Value);
                txttype.Text = Convert.ToString(dgCONN.CurrentRow.Cells[3].Value);
                txtdatabase.Text = Convert.ToString(dgCONN.CurrentRow.Cells[4].Value);
                txtprovider.Text = Convert.ToString(dgCONN.CurrentRow.Cells[5].Value);
                txtobjecttype.Text = Convert.ToString(dgCONN.CurrentRow.Cells[6].Value);
                txtobjectmask.Text = Convert.ToString(dgCONN.CurrentRow.Cells[7].Value);

            }

        }
        #endregion

        private void btnopenActions_Click(object sender, EventArgs e)
        {
            try
            {


                string appPath = System.IO.Directory.GetCurrentDirectory() + "\\Actions";
                System.Diagnostics.Process.Start(appPath);
            }
            catch (Exception)
            {

                MessageBox.Show(this, "Problem", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }

        }

        private void btnRefreshActions_Click(object sender, EventArgs e)
        {
            try
            {

               crycore.actions.fillActions();
                fillDG();
            }
            catch (Exception)
            {

                MessageBox.Show(this, "Problem", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
        }


    }
}
