using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;

namespace zenQuery
{
    /// <summary>
    /// An implementation of IBrowser for MS SQL Server.
    /// </summary>
    public class SqlBrowser : IBrowser
    {

        class SqlNode : TreeNode
        {
            internal string type = "";
            internal string name, owner, safeName, dragText;
            public SqlNode(string text) : base(text) { }
        }

        const int timeout = 10;
        DbClient dbClient;

        /// <summary>
        /// Pobranie akcji         
        /// </summary>
        /// <param name="type">"simple"-proste ;"actions"-akcje ze snippetow, "history"-historia dla obiektu </param>
        /// <returns></returns>

        public StringCollection GetActionList(TreeNode node, string type)
        {

            mk.Logic.simpleDebug.dump();
            // if (!(node is SqlNode)) return null;

            SqlNode sn = (SqlNode)node;
            StringCollection output = new StringCollection();

            switch (type)
            {
                case "simple":

                    //Procedures (P) 
                    //Scalar Funtions (FN) 
                    //Scalar Funtions (IF) 
                    //Table-valued Function (TF) 
                    //Tables (U) 
                    //Triggers (TR) 
                    //Views (V) 

                    if (sn.type == "V" || sn.type == "P" || sn.type == "FN" || sn.type == "IF" || sn.type == "TF" || sn.type == "TR")
                        output.Add("Alter");

                    //if (sn.type == "CO" && ((SqlNode)sn.Parent).type == "U")
                    //    output.Add("Alter column...");

                    if (sn.type == "U" || sn.type == "S" || sn.type == "V")
                    {
                        output.Add("select top 100  * from " + sn.safeName + " order by 1 desc");
                        output.Add((string.Format("sp_spaceused '{0}' ", sn.safeName)));
                        //output.Add("(insert all fields)");
                        //output.Add("(insert all fields, table prefixed)");
                    }

                    break;

                case "actions":
                    //Pobranie akcji dla node snippetow

                    //IDictionaryEnumerator _enumerator = zenQuery.Logic.actions.ht.GetEnumerator();
                    //while (_enumerator.MoveNext())
                    // {
                    //  _string += _enumerator.Key + " ";
                    //  _string += _enumerator.Value + "\n";
                    // }

                    SQLiteDataReader dr;
                    dr = mk.msqllite.GetDataReader("select description from tblsnipitem where type =2 and (provider = 'MSSQL' or provider = '') and objecttype like '%[" + sn.type + "]%' and (lower(objectmask) = '" + sn.safeName.ToLower() + "' or objectmask='')  and ( lower(database) = '" + dbClient.Database.ToLower() + "' or database='')   order by description ");
                    string temp;
                    if (dr.HasRows)
                        while (dr.Read())
                        {
                            temp = dr["description"].ToString();
                            output.Add(temp);
                        }
                    dr.Close();
                    break;

                case "history":
                    //Pobranie info dla node
                    SQLiteDataReader dr1;
                    dr1 = mk.msqllite.GetDataReader("SELECT rowid,dd,substr(sql,1,25) sql  FROM sqlhist where TAG MATCH '" + sn.name + "' ORDER BY  rowid desc");
                    if (dr1.HasRows)
                        while (dr1.Read())
                        {
                            output.Add(string.Format("[{0}] {1}  {2}...", dr1["rowid"].ToString(), dr1["dd"].ToString(), dr1["sql"].ToString()));
                        }
                    dr1.Close();
                    break;

            }

            return output.Count == 0 ? null : output;

        }




        /// <summary>
        /// TODO: trzeb ato ladniej zrobic
        /// Pobiera text dla pozycji z menu kontekstowego
        /// </summary>
        public string GetActionText(TreeNode node, string action)
        {


            mk.Logic.simpleDebug.dump();


            if (!(node is SqlNode)) return null;

            SqlNode sn = (SqlNode)node;
            string temp = "";


            //SqlNode sn = (SqlNode)node;
            ///////////////////////tutaj pracuje

            //if (action.StartsWith("select * from ") || action.StartsWith("sp_"))
            //    return action;

            if (action.StartsWith("(insert all fields"))
            {
                StringBuilder sb = new StringBuilder();
                // If the table-prefixed option has been selected, add the table name to all the fields
                string prefix = action == "(insert all fields)" ? "" : sn.safeName + ".";


                return "";//allFieldsCommaSeparated.Length == 0 ? null : allFieldsCommaSeparated;
            }

            if (action == "Alter")
            {
                DataSet ds = this.dbClient.Execute(string.Format("sp_helptext '{0}' ", sn.safeName), dbClient.Connection.ConnectionTimeout);
                if (ds == null || ds.Tables.Count == 0) return null;

                StringBuilder sb = new StringBuilder();
                bool altered = false;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string line = row[0].ToString();
                    if (!altered && line.Trim().ToUpper().StartsWith("CREATE"))
                    {
                        sb.Append("ALTER" + line.Trim().Substring(6, line.Trim().Length - 6) + "\r\n");
                        altered = true;
                    }
                    else
                        sb.Append(line);
                }
                return sb.ToString().Trim();
            }


            //SQLiteDataReader dr;





            //if (action == "Alter column...")
            //    return "alter table " + ((SqlNode)sn.Parent).dragText + " alter column " + sn.safeName + " ";

            //jelsi nic z powyzszych wtedy zworc text

            return action.ToString();

            //return null;
        }




        public SqlBrowser(DbClient dbClient)
        {
            this.dbClient = dbClient;
        }

        public DbClient DbClient
        {
            get { return dbClient; }
        }

        public TreeNode[] GetObjectHierarchy(string filter) //przygotowany keidys pod filtrowanie
        {
            mk.Logic.simpleDebug.dump();
            TreeNode[] top = new TreeNode[]
			{
				new TreeNode ("User Tables"),
				//new TreeNode ("System Tables"),
				new TreeNode ("Views"),
				new TreeNode ("User Stored Procs"),
				//new TreeNode ("MS Stored Procs"),
				new TreeNode ("Scalar Functions"),
                new TreeNode ("Table Functions"),
                	new TreeNode ("Triggers")
			};

            string version = dbClient.ExecuteScalar("select SERVERPROPERTY('productversion')", timeout) as string;
            string schemaFunc = version != null && (version[0] == '9' || version[0] == '1') ? "schema_name" : "user_name";

            DataSet ds = dbClient.Execute(
                        @"select 
	                    type,
	                    ObjectProperty (id, N'IsMSShipped') shipped, 
	                    object_name(id) object, 
	                    " + schemaFunc + @"(uid) owner 
                        from sysobjects 
                        where type in (N'U', N'S', N'V', N'P', N'FN', N'IF', N'TF', N'TR')   order by  owner,object", timeout); // and   object_name(id) like  '%" + filter + "%'
            if (ds == null || ds.Tables.Count == 0) return null;

           // string owner = "#$";

            foreach (DataRow row in ds.Tables[0].Rows)
            {

                string type = row["type"].ToString().Substring(0, 2).Trim();

                int position;
                if (type == "U") position = 0;
                else if (type == "V") position = 1;
                else if (type == "FN") position = 3;
                else if (type == "IF") position = 3;
                else if (type == "TF") position = 4;
                else if (type == "TR") position = 5;
                else if (type == "P") position = 2;
                // else if ((int)row["shipped"] == 0) position = 2;			
                else continue;    //position = 2;										

                //Procedures (P) 
                //Scalar Funtions (FN) 
                //Scalar Funtions (IF) 
                //Table-valued Function (TF) 
                //Tables (U) 
                //Triggers (TR) 
                //Views (V) 




                string prefix = row["owner"].ToString() == "dbo" ? "" : row["owner"].ToString() + ".";
                SqlNode node = new SqlNode(prefix + row["object"].ToString());
                node.type = type;
                node.name = row["object"].ToString();
                node.owner = row["owner"].ToString();


                // If the object name contains a space, wrap the "safe name" in square brackets.
                if (node.owner.IndexOf(' ') >= 0 || node.name.IndexOf(' ') >= 0)
                {
                    node.safeName = "[" + node.name + "]";
                    node.dragText = "[" + node.owner + "].[" + node.name + "]";
                }
                else
                {
                    node.safeName = node.name;
                    node.dragText = node.owner + "." + node.name;
                }
                if (node.owner != "" && node.owner.ToLower() != "dbo")
                    node.safeName = node.dragText;

                switch (type)
                {

                    case "U":// user table
                        //node.ImageIndex = 1;
                        break;
                    case "S":// system table
                        //node.ImageIndex = 1;
                        break;
                    case "V":// view
                        //node.ImageIndex = 1;
                        break;
                    case "P":// user stored proc
                        //node.ImageIndex = 1;

                        break;

                    case "FN":// function
                        //node.ImageIndex = 1;

                        break;
                }
                top[position].Nodes.Add(node);

                // Add a dummy sub-node to user tables and views so they'll have a clickable expand sign
                // allowing us to have GetSubObjectHierarchy called so the user can view the columns
                if (type == "U" || type == "V") node.Nodes.Add(new TreeNode());
            }
            return top;
        }
        /// <summary>
        /// Get columns to view and table
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public TreeNode[] GetSubObjectHierarchy(TreeNode node)
        {
            mk.Logic.simpleDebug.dump();
            // Show the column breakdown for the selected table
            if (node is SqlNode)
            {
                SqlNode sn = (SqlNode)node;
                if (sn.type == "U" || sn.type == "V")					// break down columns for user tables and views
                {
                    DataSet ds = dbClient.Execute("select COLUMN_NAME name, DATA_TYPE type, CHARACTER_MAXIMUM_LENGTH clength, NUMERIC_PRECISION nprecision, NUMERIC_SCALE nscale, IS_NULLABLE nullable, COLUMN_DEFAULT  from INFORMATION_SCHEMA.COLUMNS where TABLE_CATALOG = db_name() and TABLE_SCHEMA = '"
                        + sn.owner + "' and TABLE_NAME = '" + sn.name + "' order by ORDINAL_POSITION", timeout);
                    if (ds == null || ds.Tables.Count == 0) return null;


                    TreeNode[] tn = new SqlNode[ds.Tables[0].Rows.Count];
                    int count = 0;

                    string nCOLUMN_DEFAULT;

                    //_autoComplete.Add(sn.name,sn.name); //dodanie tabeli  

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        //   _autoComplete.Add(sn.name +'.'+ row["name"].ToString(), sn.name); //dodanie pola

                        string length;
                        if (row["clength"].ToString() != "")
                            length = "(" + row["clength"].ToString() + ")";
                        else if (row["nprecision"].ToString() != "")
                            length = "(" + row["nprecision"].ToString() + "," + row["nscale"].ToString() + ")";
                        else length = "";

                        string nullable = row["nullable"].ToString().StartsWith("Y") ? "null" : "not null";

                        nCOLUMN_DEFAULT = row["COLUMN_DEFAULT"].ToString();
                        SqlNode column;

                        if (string.IsNullOrEmpty(nCOLUMN_DEFAULT))
                            column = new SqlNode(string.Format("{0}  [{1}] [{2}]", row["name"].ToString(), row["type"].ToString() + length, nullable));
                        else
                            column = new SqlNode(string.Format("{0}  [{1}] [{2}] [{3}]", row["name"].ToString(), row["type"].ToString() + length, nullable, nCOLUMN_DEFAULT));



                        column.type = "CO";			// column
                        column.dragText = row["name"].ToString();
                        if (column.dragText.IndexOf(' ') >= 0)
                            column.dragText = "[" + column.dragText + "]";
                        column.safeName = column.dragText;
                        tn[count++] = column;
                    }
                    return tn;
                }
            }
            return null;
        }

        public string GetDragText(TreeNode node)
        {
            mk.Logic.simpleDebug.dump();
            if (node is SqlNode)
                return ((SqlNode)node).dragText;
            else
                return "";
        }




        public string[] GetDatabases()
        {
            mk.Logic.simpleDebug.dump();
            // cool, but only supported in SQL Server 2000+
            DataSet ds = dbClient.Execute("dbo.sp_MShasdbaccess", timeout);
            // works in SQL Server 7...
            if (ds == null || ds.Tables.Count == 0)
                ds = dbClient.Execute("select name from master.dbo.sysdatabases order by name", timeout);
            if (ds == null || ds.Tables.Count == 0) return null;
            string[] sa = new string[ds.Tables[0].Rows.Count];
            int count = 0;
            foreach (DataRow row in ds.Tables[0].Rows)
                sa[count++] = row[0].ToString().Trim();
            return sa;
        }

        public IBrowser Clone(DbClient newDbClient)
        {
            SqlBrowser sb = new SqlBrowser(newDbClient);
            return sb;
        }
    }
}
