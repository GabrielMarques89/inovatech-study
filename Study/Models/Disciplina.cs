using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.Models
{
    public class Disciplina : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Professor { get; set; }
        public virtual Curso Curso { get; set; }
    }
}