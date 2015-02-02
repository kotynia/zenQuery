using System;
using System.Windows.Forms;
using zenQuery.Logic;
using zenQuery.Properties;
namespace zenQuery
{
    public partial class frmSearch : Form
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strsql">szukaj </param>
        /// <param name="name">czy wyszukiwac wsord nazw  </param>
        ///  <param name="name">czy wyszukiwac wsord tekstu  </param>
        public frmSearch(bool text,  string strsql, DbClient dbclient)
        {

            InitializeComponent();
            this.dbClient = dbclient;
            dgvRefresh(text,strsql);
        }

        private  DbClient dbClient;

     

        ///<summary>
        ///odswiez dgvsearch
        ///</summary>
        private void dgvRefresh( bool text, string strsql)
        {
           
            
            System.Data.DataSet  DS;
            string strsqlresult="";
           
           // dbClient.
            switch (dbClient.providerr)  
            {
            case "Oracle":
                  


                    //TODO: dla oracle
                    break;
            case "MSSQL":

                    if (text)
                    {
                        strsqlresult = mMisc.Replace(Settings.Default.mssqlSearchText, "[[objectname]]", strsql);
                    }
                    else
                    {
                        strsqlresult = mMisc.Replace(Settings.Default.mssqlSearchName, "[[objectname]]", strsql);
                    }
                   
                    break;
            }


            try
            {
      DS = dbClient.Execute(strsqlresult, 30);
            dgvSearch.DataSource = DS.Tables[0].DefaultView ;
            dgwidth();
            }
            catch (Exception)
            {
                
             
            }
      
        }


        /// <summary>
        /// Zmiana szerokosci kolumn
        /// </summary>
        private void dgwidth()
        {
            //Parametry
            DataGridView dg = dgvSearch;


            try
            {
                int fixedWidth = SystemInformation.VerticalScrollBarWidth + dg.RowHeadersWidth;
                int mul = (dg.Width - fixedWidth);
                dg.Columns[0].Width = mul;
            }
            catch { }
        }

        private void dgvSearch_SizeChanged(object sender, EventArgs e)
        {
            dgwidth();
        }

    }
}
