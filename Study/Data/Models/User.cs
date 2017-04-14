using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.Data.Models
{
    public class User : BaseEntity
    {
        public virtual string Login { get; set; }
        public virtual string Password { get; set; }
    }
}