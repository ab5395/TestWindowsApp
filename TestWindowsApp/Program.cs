using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Newtonsoft.Json;

//Added


namespace TestWindowsApp
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Add handler to handle the exception raised by main threads
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

            // Add handler to handle the exception raised by additional threads
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            // Stop the applicatin and all the threads in suspended state.
            Environment.Exit(-1);
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {// All exceptions thrown by the main thread is handled over this method

            ShowExceptionDetails(e.Exception);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {// All exceptions thrown by additional threads are handled in this method

            ShowExceptionDetails(e.ExceptionObject as Exception);

            // Suspend the current thread for now to stop the exception from throwing.
            Thread.CurrentThread.Suspend();
        }

        static void ShowExceptionDetails(Exception Ex)
        {
            string exMessage = string.Empty, exStatckTrace = string.Empty, methodName = String.Empty, formName = String.Empty;
            int ErrorId = 0;

            exMessage = Ex.Message;
            exStatckTrace = Ex.StackTrace;
            methodName = Ex.TargetSite.Name;


            string directory = Environment.CurrentDirectory;
            string exePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            string filePath = exePath + @"\ErrorLog.json";
            string oldJson = File.ReadAllText(filePath);
            var oldresult = JsonConvert.DeserializeObject<ServiceList>(oldJson);

            List<ErrorLog> errorLogslist = new List<ErrorLog>();
            errorLogslist.Clear();
            if (oldresult.ErrorLog != null && oldresult.ErrorLog.ToString() != "")
            {
                foreach (var item in oldresult.ErrorLog)
                {
                    errorLogslist.Add(new ErrorLog(item.ErrorId, item.Message, item.Stacktrace, item.MethodName, item.CreateDate));
                    ErrorId = item.ErrorId;
                }
            }
            ErrorId = ErrorId + 1;

            errorLogslist.Add(new ErrorLog(ErrorId, exMessage, exStatckTrace, methodName, DateTime.Now));

            string jsonnewdata = JsonConvert.SerializeObject(new { ErrorLog = errorLogslist });
            File.WriteAllText(filePath, jsonnewdata);

            // Do logging of exception details
            MessageBox.Show(Ex.Message, Ex.TargetSite.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
