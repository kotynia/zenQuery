using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using mk.Logic;
using ScintillaNet;
using System.Collections;



namespace crycore
{
    /// <summary>
    /// Wszelkie helpery do stringow
    /// </summary>
    public  class STRING
    {


        //sprawdzenei czy slowo nie jest aliasem select * from test a
        //wylaczone na razei
     public static  String getRealObjectName(string word, ref Scintilla _document)
        {
            return word;

            simpleDebug.dump();
            string allText = _document.Text; 

            Hashtable h = new Hashtable();
            //znalezienie pozycji i transferowanie ich do hashtable
            GetPositions(allText + " ", " " + word + " ", ref h);
            GetPositions(allText, " " + word + Environment.NewLine, ref h);
            GetPositions(allText, " " + word + ",", ref h);
            GetPositions(allText, " " + word + "(", ref h);
            //znalezienie najblizej pozycji  5   25  29

            int carret = _document.Caret.Position - 1;


            int startword = 0;
            int startkey = 0; //gdzie sie zaczyna 
            IDictionaryEnumerator _enumerator = h.GetEnumerator();

            while (_enumerator.MoveNext())
            {
                if (startword == 0)
                {
                    startkey = Int32.Parse(_enumerator.Key.ToString());
                    startword = getWordBeforePosition(startkey - 1,allText);
                }
            }

            if (startword != 0)
            {
                string wordnew = _document.Text.Substring(startword, startword - startkey).ToLower();
                if (string.Compare(wordnew, "from", true) != 0)
                    return wordnew;
            }

            return word;
        }


     public  static int getWordBeforePosition(int position, string allText)
      {

          simpleDebug.dump();
          Hashtable h = new Hashtable();

          allText = allText.Replace("[", " ");
          allText = allText.Replace("]", " ");
          allText = allText.Replace(")", " ");
          allText = allText.Replace("(", " ");
          allText = allText.Replace(",", " ");

          GetPositions(allText, " ", ref h);

          ArrayList aKeys = new ArrayList(h.Keys); //< tutaj nazwa hashtable  
          aKeys.Sort();

          int _count = aKeys.Count;
          if (_count == 0) return 0; //zabezpieczenie gdyby nic nei bylo przed

          return Int32.Parse(aKeys[_count - 1].ToString()) + 1; //korekta dlatego zenQuery liczy spacje
      }


        /// <summary>
        /// Znajduje pozycje i eksportuje do hashtable
        /// </summary>
        /// <param name="input"></param>
        /// <param name="searchKey"></param>
        /// <param name="hash"></param>
     static void GetPositions(string input, string searchKey, ref Hashtable hash)
      {
          simpleDebug.dump();
          int searchKeylength = searchKey.Length;
          int idx = -1;       //biezaca pozycja szukania
          int strt = 0;       //pozycja znalezionego stringu

          if (searchKeylength < input.Length)  //zabezpieczenie na wypadek dodawani aznakow konca linii
              while (strt != -1)
              {

                  strt = input.IndexOf(searchKey, idx + searchKeylength); //dodanie dlugosci bo jesi zanleziony przeskakuje od razu pare do przdodu
                  if (strt != -1)
                      hash.Add(strt, searchKey);  //dodanie do tablicy 

                  idx = strt;

              }
      }

    }
}
