using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.Models.Maps
{
    public class GrupoEstudoMap : BaseClassMap<GrupoEstudo>
    {
        public GrupoEstudoMap()
        {
            Table("GRUPOS_ESTUDO");

            Id(reg => reg.Id).Column("ID_GRUPO_ESTUDO").GeneratedBy.Identity().Not.Nullable();

            Map(reg => reg.Nome).Column("NOME").Length(128).Not.Nullable();
            Map(reg => reg.QuantidadeMaxAlunos).Column("QUANTIDADE_MAX_ALUNOS").Not.Nullable();
            Map(reg => reg.Privado).Column("PRIVADO").Not.Nullable();

            References(reg => reg.Disciplina).ForeignKey("FK_DISCIPLINA_X_GRUPO_ESTUDO").Column("ID_DISCIPLINA").Not.Nullable();
        }
    }
}