using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.Models
{
    public class ErrorDTO
    {
        public string Message { get; set; }

        public ErrorDTO(string message)
        {
            this.Message = message;
        }
    }
}