using System;
using System.Diagnostics;
using System.Reflection;
namespace mk.Logic
{
    class simpleDebug
    {
        private static  int i=1;
       private static  string last="#$";

        /// <summary>Zwraca na Output Window nazwe biezacej metody</summary>
        /// <example> 
        /// using mk.Logic; 
        /// simpleDebug.dump();
        /// </example>
        [Conditional("DEBUG")]
    public  static void  dump()
        {

            //String dbgLine = new String();
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();
            if (last == methodBase.Name)
            {
                i += 1; 
                return;
            }
            
            Debug.WriteLine(String.Format(" Method Before:{0} TIME:{1} ms:{2} {3}",i, System.DateTime.Now.ToString() , System.DateTime.Now.Millisecond, methodBase.Name));
            i = 1;
            last = methodBase.Name;
        }
    }
}
