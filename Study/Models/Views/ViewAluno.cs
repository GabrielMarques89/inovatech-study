using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.Models.Views
{
    public class ViewAluno : BaseEntity
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
        public virtual long Indicacoes { get; set; }
        public virtual long ContraIndicacoes { get; set; }

    }
}