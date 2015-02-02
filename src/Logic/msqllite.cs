using System;
using System.Data;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
    namespace  mk
    {
        public class msqllite
        {
            string success="Done";

           // private static string _sqlliteDSN = "Data Source=" + Directory.GetCurrentDirectory() + "\\config.f";
            private static string _sqlliteDSN = "Data Source=config.cfg;UTF8Encoding=True;Version=3";

            public static System.Data.SQLite.SQLiteDataReader GetDataReader(string sql)
            {

               
                try
                {
                    SQLiteConnection cnn = new SQLiteConnection(_sqlliteDSN);
                    cnn.Open();
                    SQLiteCommand mycommand = new SQLiteCommand(cnn);
                    mycommand.CommandText = sql;
                    SQLiteDataReader reader = mycommand.ExecuteReader(CommandBehavior.CloseConnection);
                     return reader;
                    //dt.Load(reader);
                    //reader.Close();
                    //cnn.Close();
                }
                catch
                {
                }
                return null;
            }
         
            public static System.Data.DataTable  GetDataTable(string sql)
            {
                
                DataTable dt = new DataTable();
                try
                {
                    SQLiteConnection cnn = new SQLiteConnection(_sqlliteDSN);
                    cnn.Open();
                    SQLiteCommand mycommand = new SQLiteCommand(cnn);
                    mycommand.CommandText = sql;
                    SQLiteDataReader reader = mycommand.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();
                    cnn.Close();
                }
                catch
                {
                }
                return dt;
            }
            public static int ExecuteNonQuery(string sql)
            {
                SQLiteConnection cnn = new SQLiteConnection(_sqlliteDSN);
                cnn.Open();
                SQLiteCommand mycommand = new SQLiteCommand(cnn);
                mycommand.CommandText = sql;

                int rowsUpdated = mycommand.ExecuteNonQuery();
                cnn.Close();
                return rowsUpdated;
            }


            public static string mSQLLiteExecute( bool storedProcedure, string strsql, List<SQLiteParameter> paramss, string _dsn)
            {
                SQLiteConnection dbconn;
                SQLiteCommand dbCMD;

                if ((_dsn == null))
                    _dsn = _sqlliteDSN;

                dbconn = new SQLiteConnection(_dsn);
                dbconn.Open();

                try
                {
                    dbCMD = new SQLiteCommand(strsql, dbconn);
                    if ((storedProcedure))
                        dbCMD.CommandType = CommandType.StoredProcedure;
                    else
                        dbCMD.CommandType = CommandType.Text;

                    //Dodanie Parametrow
                    if (paramss != null)
                        foreach (SQLiteParameter param in paramss)  //skrot jesli 1 linijka nie trzeba {}
                        {
                            dbCMD.Parameters.Add(param);
                        }

                    string test = CommandParametersToSQLSimple(strsql, dbCMD.Parameters, dbCMD.CommandType.ToString());
                    System.Diagnostics.Debug.Write(test);


                    int countrecords = dbCMD.ExecuteNonQuery();

                  

                    //szukanie czy jest jakis parametr zwrotny 
                    string retval = "";
                    foreach (SQLiteParameter param in dbCMD.Parameters)
                    {
                        if (param.Direction == System.Data.ParameterDirection.Output)
                        {
                            retval += param.Value.ToString();
                        }
                    }


                    if (retval.Length > 0)
                        return retval;
                    else
                        return Convert.ToString(countrecords);
                }

                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
                finally
                {
                    dbconn.Close();
                }
            }

            public static string ExecuteScalar(string sql)
            {
#if DEBUG
                StackTrace st = new StackTrace();
                StackFrame sf = st.GetFrame(2);
                MethodBase mb = sf.GetMethod();
             

                Debug.WriteLine( mb.Name );
#endif

                SQLiteConnection cnn = new SQLiteConnection(_sqlliteDSN);
                cnn.Open();
                SQLiteCommand mycommand = new SQLiteCommand(cnn);
                mycommand.CommandText = sql;
                object value = mycommand.ExecuteScalar();
                cnn.Close();
                if (value != null)
                {
                    return value.ToString();
                }
                return "";
            }
            /// <summary>
            /// Check if sql return value , TRUE
            /// </summary>
            /// <param name="sql"></param>
            /// <returns></returns>
            public static bool ifExist (string sql)
            {
                string _value;
                _value = ExecuteScalar(sql);
                if (_value != "")
                    return true;
                else
                    return false ;

            }



  #region DEBUGOWANIE
        ///<summary>
        ///Convert SQL command with Parameters to STRSQL
        ///Wywolanie
        ///</summary>
        ///<example>
        /// crycore.sqldebug.CommandParametersToSQL(e.Command.CommandText , e.Command.Parameters, e.Command.CommandType.ToString());
        ///</example>

        public static string CommandParametersToSQLSimple(string selectCommand, System.Data.Common.DbParameterCollection parameters, string commandType)
        {

            //Convert parameters to STRSQL
           

            if (parameters != null)
            {
                int count = parameters.Count;
                for (int i = 0; i < count; i++)
                {selectCommand = selectCommand.Replace(parameters[i].ParameterName, SqlString(parameters[i]));
                }
            }

            return selectCommand;
            //konie iteracja wsrod parametrow  
        }


        /// <summary>
        /// Otrzymujemy typ z wartosci ale na razie idziemy na latwizne
        /// </summary>
        /// <param name="parameterValue"></param>
        /// <returns></returns>
        private static string SqlType(System.Data.Common.DbParameter param)
        {
            if (param == null) //zabezpieczenie
                return "nvarchar(max)";
            if (param.Value == null)
                return "nvarchar(max)";

            return "nvarchar(" + param.Value.ToString().Length + ")";
        }

        //Dodanie literki n jelsi inne niz liczba
        private static string SqlString(System.Data.Common.DbParameter param)
        {
            if (param == null) //zabezpieczenie
                return "null";

            if (param.Value == null)
                return "null";

            if (IsNumeric(param.Value.ToString()))
            {
                return param.Value.ToString();
            }
            else
            {
                return "'" + param.Value.ToString() + "'";
            }
        }

        //czy tekst jest liczba
        private static bool IsNumeric(object Expression)
        {
            bool isNum;
            double retNum;
            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        #endregion
    }


        }
    


