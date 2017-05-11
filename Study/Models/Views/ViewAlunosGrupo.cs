using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Study.Models.Enums;

namespace Study.Models.Views
{
    public class ViewAlunosGrupo : BaseEntity
    {
        public virtual string Matricula { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Email { get; set; }
        public virtual string Telefone { get; set; }
        public virtual string FotoUrl { get; set; }
        public virtual string FotoThumbUrl { get; set; }
        public virtual int Periodo { get; set; }
        public virtual long IdCurso { get; set; }
        public virtual string NomeCurso { get; set; }
        public virtual long IdGrupo { get; set; }
        public virtual string NomeGrupo { get; set; }
        public virtual TipoParticipacao Tipo { get; set; }

    }
}