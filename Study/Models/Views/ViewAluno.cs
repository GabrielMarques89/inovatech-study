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
        public virtual string FotoB64 { get; set; }
        public virtual byte[] Foto { get; set; }
        public virtual string Token { get; set; }
        public virtual long Indicacoes { get; set; }
        public virtual long ContraIndicacoes { get; set; }

    }
}