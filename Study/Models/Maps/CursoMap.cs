using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.Models.Maps
{
    public class CursoMap : BaseClassMap<Curso>
    {
        public CursoMap()
        {
            Table("CURSOS");

            Id(reg => reg.Id).Column("ID_CURSO").GeneratedBy.Identity().Not.Nullable();
            Map(reg => reg.Nome).Column("NOME").Length(128).Not.Nullable();
            Map(reg => reg.QuantidadePeriodos).Column("QUANTIDADE_PERIODOS").Not.Nullable();

            References(reg => reg.Instituicao).ForeignKey("FK_INSTITUICAO_X_CURSO").Column("ID_INSTITUICAO").Not.Nullable();
        }
    }
}