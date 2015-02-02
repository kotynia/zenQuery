using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using mk.Logic;
using ScintillaNet;
using System.Collections;
using System.Collections.Generic;
namespace crycore
{
   public class AUTOCOMPLETE
    {
 /// <summary>
 /// inicjalizuje autocomplete i przekazuej do okreslonego okna
 /// </summary>
 /// <returns></returns>
        public static  List<string[]> autoComplete(ref zenQuery.DbClient _dbClient)
       {

            if (string.Compare(_dbClient.providerr.ToString(),"mssql",true) != 0 ) 
                return null; 

           // Hashtable items =new Hashtable();
           DataSet ds = _dbClient.Execute("select COLUMN_NAME  ,table_name from INFORMATION_SCHEMA.COLUMNS where TABLE_CATALOG = db_name() order by table_name,column_name  ", _dbClient.Connection.ConnectionTimeout );
           if (ds == null || ds.Tables.Count == 0) return null;

           List<string[]> a = new List<string[]>();

           //Wypelnienie musi byc table_name, column_name
           foreach (DataRow row in ds.Tables[0].Rows)
           {
               string[] arr = new string[2];
               arr[0] = row["table_name"].ToString();
               arr[1] = row["column_name"].ToString();
               a.Add(arr);
           }
           return a;
       }


       /// <summary>
       /// Pobierz dane
       /// </summary>
      public static void getAutocomplete(ref Scintilla _document, ref List<string[]> _Autocomplete)
       {

           if (_Autocomplete == null) return;

           int type;

           _document.SuspendLayout();
           int carret;
           string word; //slowo przed kropka
           carret = _document.Caret.Position - 1; //odliczam kropke

           //46 krpopka
           if (carret > 0 && _document.CharAt(carret) == 46) //jezelie kropka
               type = 2;
           else
               type = 1;

           // jezeli kursor w polu szukaj wsrod nazw procedur,widokow,tabel
           //jezeli kropka : szukaj slowa przed kropka > i gdzie sie pojawia  > i slowo za nim 
           _document.AutoComplete.List.Clear();
           switch (type)
           {
               case 1:
                   string last = "#@$";
                   foreach (string[] f in _Autocomplete)
                   {
                       if (last != f[0])
                           _document.AutoComplete.List.Add(f[0]);
                       last = f[0];
                   }

                   break;
               case 2:
                   if (carret <= 0) return;

                   //			int startPosition = NativeInterface.WordStartPosition(position, true);
                   //int endPosition = NativeInterface.WordEndPosition(position, true);
                   //return GetRange(startPosition, endPosition).Text;

                   int test;
                   int startword = _document.NativeInterface.WordStartPosition(carret, true);
                   //tutaj mialem wlasna funkcje ale scintilla tez posiada wiec zobaczymy
                  // int startword = crycore.STRING.getWordBeforePosition(carret,_document.Text);
                   word =_document.Text.Substring(startword, carret - startword).ToLower();
                   //word = _document.GetWordFromPosition(carret - 1);
                   word = crycore.STRING.getRealObjectName(word, ref _document);
                   //szukaj wsyztskich alternatyw czyli tbldoc a zakladajac ze znaleziony ejst 

                   //string last = "#@$";
                   foreach (string[] f in _Autocomplete)
                   {
                       if (string.Compare(word, f[0].ToString(), true) == 0 && f[1] != "") //rowne tabeli 
                           _document.AutoComplete.List.Add(f[1]);
                       //  last = f[0];
                   }

                   _document.AutoComplete.Show();

                   break;

           }
           _document.ResumeLayout();
       }
    }
}
