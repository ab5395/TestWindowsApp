using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWindowsApp
{
    public class ErrorLog
    {
        public int ErrorId { get; set; }
        public string Message { get; set; }
        public string Stacktrace { get; set; }
        public string MethodName { get; set; }
        public DateTime CreateDate { get; set; }

        public ErrorLog(int _errorId,string _message, string _stacktrace, string _methodName, DateTime _createDate)
        {
            ErrorId = _errorId;
            Message = _message;
            Stacktrace = _stacktrace;
            MethodName = _methodName;
            CreateDate = _createDate;
        }
    }

    public class ServiceList
    {
        public List<ErrorLog> ErrorLog { get; set; }
    }
}
