using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.Models.Views
{
    public class ViewGruposAluno : BaseEntity
    {
        public virtual long IdGrupo { get; set; }
        public virtual string NomeGrupo { get; set; }
        public virtual DateTime DataEncontro { get; set; }
        public virtual string Local { get; set; }
        public virtual string Descricao { get; set; }
        public virtual long IdDisciplina { get; set; }
        public virtual string NomeDisciplina { get; set; }
        public virtual long IdAluno { get; set; }

    }
}