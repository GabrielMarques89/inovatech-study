using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.Models
{
    public class Instituicao : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Endereco { get; set; }
        public virtual string Telefone { get; set; }
    }
}