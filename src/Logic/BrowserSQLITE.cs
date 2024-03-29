﻿using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SQLite;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;

namespace zenQuery
{
    #region Simple SQLite Browser
    /// <summary>
        /// A simple implementation of IBrowser for Oracle.  No support for SPs, packages, etc
        /// </summary>
        public class SQLite : IBrowser
        {

            private List<string[]> _autoComplete; //

            public List<string[]> autoComplete()
            {
                return _autoComplete;
            }

            class SqlNode : TreeNode
            {
                internal string type = "";
                internal string name, owner, safeName, dragText;
                public SqlNode(string text) : base(text) { }
            }

            const int timeout = 8;
            DbClient dbClient;

            public SQLite(DbClient dbClient)
            {
                this.dbClient = dbClient;
            }

            public DbClient DbClient
            {
                get { return dbClient; }
            }

            public TreeNode[] GetObjectHierarchy(string filter)
            {
                mk.Logic.simpleDebug.dump();
                TreeNode[] top = new TreeNode[]
			{
				new TreeNode ("Tables"),
				new TreeNode ("Views"),
			};

                //new TreeNode ("User Tables"),
                DataSet ds = dbClient.Execute("SELECT name FROM sqlite_master WHERE type='table' ORDER BY name", timeout);
                if (ds == null || ds.Tables.Count == 0) return null;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SqlNode node = new SqlNode(row[0].ToString());
                    node.type = "T";
                    node.dragText = node.Text;
                    top[0].Nodes.Add(node);
                    // Add a dummy sub-node to user tables and views so they'll have a clickable expand sign
                    // allowing us to have GetSubObjectHierarchy called so the user can view the columns
                    node.Nodes.Add(new TreeNode());
                }

                //new TreeNode ("User Temp Tables"),
                ds = dbClient.Execute("SELECT name FROM sqlite_master WHERE type='view'", timeout);
                if (ds == null || ds.Tables.Count == 0) return top;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    SqlNode node = new SqlNode(row[0].ToString());
                    node.type = "V";
                    node.dragText = node.Text;
                    top[1].Nodes.Add(node);
                    // Add a dummy sub-node to user tables and views so they'll have a clickable expand sign
                    // allowing us to have GetSubObjectHierarchy called so the user can view the columns
                    node.Nodes.Add(new TreeNode());
                }

                return top;
            }




            public TreeNode[] GetSubObjectHierarchy(TreeNode node)
            {
                mk.Logic.simpleDebug.dump();
                // Show the column breakdown for the selected table
               // return null;
                if (node is SqlNode)
                {
                    SqlNode on = (SqlNode)node;
                    if (on.type == "T" || on.type == "V")
                    {
                        DataSet ds = dbClient.Execute("PRAGMA table_info("+ on.Text + ");", timeout);
                        if (ds == null || ds.Tables.Count == 0) return null;

                        TreeNode[] tn = new SqlNode[ds.Tables[0].Rows.Count];
                        int count = 0;

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            
                            string nullable = row["notnull"].ToString().StartsWith("0") ? "" : "not null";

                            SqlNode column = new SqlNode(row["name"].ToString() + " ("
                                + row["type"].ToString()  + ", " + nullable + ")");

                            column.dragText = row["name"].ToString();

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
                    return null;
            }

            public StringCollection GetActionList(TreeNode node, string type)
            {
                mk.Logic.simpleDebug.dump();
                if (!(node is SqlNode)) return null;

                SqlNode on = (SqlNode)node;
                StringCollection output = new StringCollection();

                if (on.type == "T" || on.type == "V")
                {
                    output.Add("select * from " + on.dragText);
                }

                return output.Count == 0 ? null : output;
            }

            public string GetActionText(TreeNode node, string action)
            {
                mk.Logic.simpleDebug.dump();
                if (!(node is SqlNode)) return null;
                SqlNode on = (SqlNode)node;
                if (action.StartsWith("select * from "))
                    return action;
                else
                    return null;
            }

            public string[] GetDatabases()
            {
                mk.Logic.simpleDebug.dump();
                return new String[] { dbClient.Database };
            }

            public IBrowser Clone(DbClient newDbClient)
            {
                mk.Logic.simpleDebug.dump();
                OracleBrowser ob = new OracleBrowser(newDbClient);
                return ob;
            }
        }
        #endregion
   
}
