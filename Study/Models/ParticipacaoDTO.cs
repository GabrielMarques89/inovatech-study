using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Study.Models.Enums;

namespace Study.Models
{
    public class ParticipacaoDTO
    {
        public virtual long IdParticipacao { get; set; }
        public virtual TipoParticipacao Tipo { get; set; }
        public virtual long IdAluno { get; set; }
        public virtual string NomeAluno { get; set; }
        public virtual long IdGrupo { get; set; }
        public virtual string NomeGrupo { get; set; }
        public virtual bool? Participando { get; set; }
        public virtual bool? Recebendo { get; set; }


    }
}