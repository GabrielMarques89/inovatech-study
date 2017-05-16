using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.Models.DTO
{
    public class AvaliacaoDTO
    {
        public virtual long IdAvaliacao { get; set; }
        public virtual string Texto { get; set; }
        public virtual long IdAvaliador { get; set; }
        public virtual string NomeAvaliador { get; set; }
        public virtual string FotoAvaliador { get; set; }
        public virtual long IdAvaliado { get; set; }
        public virtual string NomeAvaliado { get; set; }
        public virtual long IdGrupo { get; set; }
        public virtual string NomeGrupo { get; set; }
        public virtual bool AvaliacaoPositiva { get; set; }
    }
}