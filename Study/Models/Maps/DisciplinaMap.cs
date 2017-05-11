using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.Models.Maps
{
    public class DisciplinaMap : BaseClassMap<Disciplina>
    {
        public DisciplinaMap()
        {
            Table("DISCIPLINAS");

            Id(reg => reg.Id).Column("ID_DISCIPLINA").GeneratedBy.Identity().Not.Nullable(); 

            Map(reg => reg.Nome).Column("NOME").Length(128).Not.Nullable();
            Map(reg => reg.Professor).Column("PROFESSOR").Length(128).Not.Nullable();

            References(reg => reg.Curso).ForeignKey("FK_CURSO_X_DISCIPLINA").Column("ID_CURSO").Not.Nullable();
        }
    }
}