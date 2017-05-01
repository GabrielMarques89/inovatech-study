using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.Models
{
    public class Avaliacao : BaseEntity
    {
        public virtual Aluno Avaliador { get; set; }
        public virtual Aluno Avaliado { get; set; }
        public virtual GrupoEstudo Grupo { get; set; }
        public virtual string Texto { get; set; }
        public virtual bool AvaliacaoPositiva { get; set; }
    }
}