using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.Models.Maps
{
    public class AvaliacaoMap : BaseClassMap<Avaliacao>
    {
        public AvaliacaoMap()
        {
            Table("AVALIACOES");

            Id(reg => reg.Id).Column("ID_AVALIACAO").GeneratedBy.Identity().Not.Nullable();

            Map(reg => reg.Texto).Column("TEXTO").Length(256).Not.Nullable();
            Map(reg => reg.AvaliacaoPositiva).Column("AVALIACAO_POSITIVA").Not.Nullable();

            References(reg => reg.Avaliador).ForeignKey("FK_ALUNO_X_AVALIACAO_01").Column("ID_AVALIADOR").Not.Nullable();
            References(reg => reg.Avaliado).ForeignKey("FK_ALUNO_X_AVALIACAO_02").Column("ID_AVALIADO").Not.Nullable();
            References(reg => reg.Grupo).ForeignKey("FK_GRUPO_X_AVALIACAO_01").Column("ID_GRUPO_ESTUDO").Not.Nullable();

        }
    }
}