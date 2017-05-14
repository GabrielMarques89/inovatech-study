using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Study.Models.Enums;

namespace Study.Models
{
    public class Participacao : BaseEntity
    {
        public virtual TipoParticipacao Tipo { get; set; }
        public virtual Aluno Aluno { get; set; }
        public virtual GrupoEstudo Grupo { get; set; }
        public virtual Boolean? Participando { get; set; }
    }
}