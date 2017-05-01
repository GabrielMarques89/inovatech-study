using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.Models
{
    public class GrupoEstudo : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual int QuantidadeMaxAlunos { get; set; }
        public virtual DateTime DataInicio { get; set; }
        public virtual string Local { get; set; }
        public virtual string Descricao { get; set; }
        public virtual bool Privado { get; set; }
        public virtual Disciplina Disciplina { get; set; }
    }
}
