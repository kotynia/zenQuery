
namespace zenQuery.Logic
{
    /// <summary>
    /// Konfiguracja simple db
    /// </summary>
    class s3dbconfig
    {
        public static string conn = @"CREATE TABLE [conn] (
[connectionName] VARCHAR(50)  UNIQUE NULL,
[server] VARCHAR(200)  NULL,
[database] VARCHAR(255)  NULL,
[trusted] BOOLEAN  NULL,
[login] VARCHAR(255)  NULL,
[password] VARCHAR(255)  NULL,
[provider] VARCHAR(255)  NULL,
[port] VARCHAR(10)  NULL,
[timeout] INTEGER DEFAULT '0' NULL,
[custom] VARCHAR(255)  NULL,
[tech] VARCHAR(255)  NULL
                            )";

        public static string sqlhist = @"CREATE VIRTUAL TABLE [sqlhist] USING FTS3(
                                        [tag] NVARCHAR(1000)  NULL,
                                        [DD] TIMESTAMP NULL ,
                                        [SQL] TEXT  NULL,
                                        [USERNAME] VARCHAR(255)  NULL,
                                        [LOGIN] VARCHAR(255)  NULL,
                                        [SERVER] NVARCHAR(255)  NULL,
                                        [DATABASE] NVARCHAR(255)  NULL,
                                        [TIME] INTEGER DEFAULT '0' NULL,
                                        [HOST] NVARCHAR(255)  NULL,
                                        [rows] INTEGER DEFAULT '0' NULL
                                        )";

        /// <summary>
        /// Browser snippets
        /// TYPE - 1 zwykle snippety
        /// TYPE - 2 browser snippets
        /// </summary>
        public static string tblsnipitem = @"CREATE TABLE [tblsnipitem] (
                                [snipitemID] INTEGER  NOT NULL PRIMARY KEY AUTOINCREMENT,
                                [DESCRIPTION] VARCHAR(20) UNIQUE  NULL,
                                [STRSQL] TEXT  NULL,
                                [TYPE] INT  NULL,
                                [DATABASE] NVARCHAR(255)  NULL,
                                [PROVIDER] VARCHAR(255)  NULL,
                                [OBJECTTYPE] VARCHAR(40)  NULL,
                                [OBJECTMASK] VARCHAR(40)  NULL,
                                [USERNAME] VARCHAR(255)  NULL,
                                [ADDdate] TIMESTAMP DEFAULT CURRENT_TIMESTAMP NULL
                                )";

        /// <summary>
        /// sample snippets
        /// </summary>

//        public static string snipitem1 = @"
//INSERT    into tblsnipitem (description, strsql, TYPE, provider)
//VALUES    ('SAMPLE_snippet_1',  'SELECT * INTO @DestinationTable@
//FROM @SourceTable@ WHERE (1 = 0)', 1, 'ALL')";
        

    }     
    
    
}
