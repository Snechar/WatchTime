using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace account_microservice.Models
{
    internal class Response
    {
        public Response(string status, string message, bool success)
        {
            Status = status;
            Message = message;
            Success = success;
        }
        public string Status { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }

    }
}
