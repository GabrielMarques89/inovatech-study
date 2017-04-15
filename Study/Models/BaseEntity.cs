using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.Data.Models
{
    public class BaseEntity
    {
        public virtual long Id { get; set; }
        public virtual long Version { get; set; }
    }
}