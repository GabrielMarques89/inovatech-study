using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Study.Models.Views;

namespace Study.Models.Maps
{
    public class ViewAlunoMap : BaseClassMap<ViewAluno>
    {
        public ViewAlunoMap()
        {
            Table("VW_ALUNO");
            ReadOnly();

            Id(reg => reg.Id).Column("ID_ALUNO").GeneratedBy.Identity();
            Map(reg => reg.Matricula).Column("MATRICULA");
            Map(reg => reg.Nome).Column("NOME");
            Map(reg => reg.Email).Column("EMAIL");
            Map(reg => reg.Telefone).Column("TELEFONE");
            Map(reg => reg.Foto).Column("FOTO");
            Map(reg => reg.Indicacoes).Column("INDICACOES");
            Map(reg => reg.ContraIndicacoes).Column("CONTRA_INDICACOES");

        }
    }
}