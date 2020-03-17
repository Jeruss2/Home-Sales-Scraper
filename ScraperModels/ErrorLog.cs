using System;

namespace ScraperModels
{
    public class ErrorLog
    {

        public string ExceptionMessage { get; set; }

        public DateTime? DateAdded { get; set; }

        public string StackTrace { get; set; }

        public string ApplicationName { get; set; }

    }

}
