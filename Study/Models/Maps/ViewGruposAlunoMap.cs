using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Study.Models.Views;

namespace Study.Models.Maps
{
    public class ViewGruposAlunoMap : BaseClassMap<ViewGruposAluno>
    {
        public ViewGruposAlunoMap()
        {
            Table("VW_GRUPOS_ALUNO");
            ReadOnly();

            Id(reg => reg.IdGrupo).Column("ID_GRUPO_ESTUDO").GeneratedBy.Identity();

            Map(reg => reg.NomeGrupo).Column("NOME");
            Map(reg => reg.DataEncontro).Column("DATA_ENCONTRO");
            Map(reg => reg.Local).Column("LOCAL");
            Map(reg => reg.Descricao).Column("DESCRICAO");
            Map(reg => reg.IdDisciplina).Column("ID_DISCIPLINA");
            Map(reg => reg.NomeDisciplina).Column("NOME_DISCIPLINA");
            Map(reg => reg.IdAluno).Column("ID_ALUNO");

        }
    }
}