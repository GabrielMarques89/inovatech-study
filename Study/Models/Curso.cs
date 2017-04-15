using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.Models
{
    public class Curso : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual int QuantidadePeriodos { get; set; }
        public virtual Instituicao Instituicao { get; set; }
    }
}