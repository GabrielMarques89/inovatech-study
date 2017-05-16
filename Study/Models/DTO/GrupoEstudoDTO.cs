using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.Models.DTO
{
    public class GrupoEstudoDTO
    {
        public virtual long Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual int QuantidadeMaxAlunos { get; set; }
        public virtual DateTimeOffset DataEncontro { get; set; }
        public virtual string Local { get; set; }
        public virtual string Descricao { get; set; }
        public virtual bool Privado { get; set; }
        public virtual long IdDisciplina { get; set; }
        public virtual string NomeDisciplina { get; set; }
        public virtual long IdLider { get; set; }
        public virtual string NomeLider { get; set; }
        public virtual string FotoLider { get; set; }
        public virtual bool IsLider { get; set; }
        public virtual bool Participando { get; set; }
    }
}