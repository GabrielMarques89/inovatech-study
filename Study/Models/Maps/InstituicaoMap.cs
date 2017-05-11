using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Study.Models.Maps
{
    public class InstituicaoMap : BaseClassMap<Instituicao>
    {
        public InstituicaoMap()
        {
            Table("INSTITUICOES");

            Id(reg => reg.Id).Column("ID_INSTITUICAO").GeneratedBy.Identity().Not.Nullable();

            Map(reg => reg.Nome).Column("NOME").Length(128).Not.Nullable();
            Map(reg => reg.Endereco).Column("ENDERECO").Length(256).Nullable();
            Map(reg => reg.Telefone).Column("TELEFONE").Length(14).Nullable();
        }
    }
}