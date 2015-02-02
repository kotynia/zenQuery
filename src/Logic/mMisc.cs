using System;
using System.Data.SQLite;
using System.Text;
using System.Text.RegularExpressions;
namespace zenQuery.Logic
{
       class mMisc
    {

           public static Regex regexTags = new Regex("\\b\\w{3,}\\b", RegexOptions.Singleline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

           public static string appVersion = "zenQuery v0.9";
        private static string _sqlliteDSN = "Data Source=config.cfg;UTF8Encoding=True;Version=3";


        public static void writeSqlhist(string _sql, string _server, string _login, string _database, int _time,int _rows)
        {
            
            string strsql;
            strsql = "insert into sqlhist (sql,username,login,server, database,time,host,tag,rows,dd) values   (@sql,@username,@login,@server,@database,@time,@host,@tag,@rows,@dd);";

            string tag = "";
            //niepowtarzajace sie slowa z snippet,header

            MatchCollection mc;
            mc = regexTags.Matches(_sql);
            foreach (Match m in mc)
            {
                tag += m.Groups[0].Value + " ";
            }

            SQLiteConnection cnn = new SQLiteConnection(_sqlliteDSN);
           // cnn.ConnectionTimeout = 3; //sekundy
            cnn.Open();
            SQLiteCommand mycommand = new SQLiteCommand(strsql,cnn);
            mycommand.Parameters.AddWithValue("@sql", _sql);
            mycommand.Parameters.AddWithValue("@username", Environment.UserName );
            mycommand.Parameters.AddWithValue("@login", _login);
            mycommand.Parameters.AddWithValue("@server", _server);
            mycommand.Parameters.AddWithValue("@database", _database);
            mycommand.Parameters.AddWithValue("@time", _time);
            mycommand.Parameters.AddWithValue("@host", Environment.MachineName );
            mycommand.Parameters.AddWithValue("@tag", tag);
            mycommand.Parameters.AddWithValue("@rows", _rows);
            mycommand.Parameters.AddWithValue("@dd", System.DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss"));
            int rowsUpdated = mycommand.ExecuteNonQuery();
            cnn.Close();
            
           
            //return rowsUpdated;

            //MK.msqllite.ExecuteNonQuery(strsql);
  
        }

           /// <summary>
           /// Beauty CODE
           /// </summary>
           /// <returns>Formatted text</returns>
     
        //Replace string function.
        public static String Replace(String oText, String oFind, String oReplace)
        {
            int iPos = oText.IndexOf(oFind);
            String strReturn = "";
            while (iPos != -1)
            {
                strReturn += oText.Substring(0, iPos) + oReplace;
                oText = oText.Substring(iPos + oFind.Length);
                iPos = oText.IndexOf(oFind);
            }
            if (oText.Length > 0)
                strReturn += oText;
            return strReturn;
        }

        public static string Left(string param, int length)
        {
            //we start at 0 since we want to get the characters starting from the
            //left and with the specified lenght and assign it to a variable
            string result = param.Substring(0, length);
            //return the result of the operation
            return result;
        } 
    }
}
