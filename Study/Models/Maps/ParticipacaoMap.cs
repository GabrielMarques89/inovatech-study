using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Type;
using Study.Models.Enums;

namespace Study.Models.Maps
{
    public class ParticipacaoMap : BaseClassMap<Participacao>
    {
        public ParticipacaoMap()
        {
            Table("PARTICIPACOES");

            Id(reg => reg.Id).Column("ID_PARTICIPACAO").GeneratedBy.Identity().Not.Nullable();

            Map(reg => reg.Tipo).Column("TIPO").CustomType<EnumType<TipoParticipacao>>().Not.Nullable();

            References(reg => reg.Aluno).ForeignKey("FK_ALUNO_X_PARTICIPACAO").Column("ID_ALUNO").Not.Nullable();
            References(reg => reg.Grupo).ForeignKey("FK_GRUPO_X_PARTICIPACAO").Column("ID_GRUPO_ESTUDO").Not.Nullable();

        }
    }
}