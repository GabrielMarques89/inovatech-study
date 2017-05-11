using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Type;
using Study.Models.Enums;
using Study.Models.Views;

namespace Study.Models.Maps
{
    public class ViewAlunosGrupoMap : BaseClassMap<ViewAlunosGrupo>
    {
        public ViewAlunosGrupoMap()
        {
            Table("VW_ALUNOS_GRUPO");
            ReadOnly();

            Id(reg => reg.Id).Column("ID_ALUNO").GeneratedBy.Identity();
            Map(reg => reg.Matricula).Column("MATRICULA");
            Map(reg => reg.Nome).Column("NOME");
            Map(reg => reg.Email).Column("EMAIL");
            Map(reg => reg.Telefone).Column("TELEFONE");
            Map(reg => reg.FotoUrl).Column("FOTO_URL");
            Map(reg => reg.FotoThumbUrl).Column("FOTO_THUMB_URL");
            Map(reg => reg.Periodo).Column("PERIODO");
            Map(reg => reg.IdCurso).Column("ID_CURSO");
            Map(reg => reg.NomeCurso).Column("NOME_CURSO");
            Map(reg => reg.IdGrupo).Column("ID_GRUPO_ESTUDO");
            Map(reg => reg.NomeGrupo).Column("NOME_GRUPO");
            Map(reg => reg.Tipo).Column("TIPO").CustomType<EnumType<TipoParticipacao>>();
            
        }
    }
}