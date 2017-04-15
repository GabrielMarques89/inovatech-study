using Study.Models;

namespace Study.Models.Maps
{
    public class AlunoMap : BaseClassMap<Aluno>
    {
        public AlunoMap()
        {
            Table("ALUNOS");

            Id(reg => reg.Id).Column("ID_ALUNO");

            Map(reg => reg.Matricula).Column("MATRICULA").Length(128).Not.Nullable();
            Map(reg => reg.Senha).Column("SENHA").Length(128).Not.Nullable();
            Map(reg => reg.Nome).Column("NOME").Length(128).Not.Nullable();
            Map(reg => reg.Email).Column("EMAIL").Length(128).Not.Nullable();
            Map(reg => reg.Telefone).Column("TELEFONE").Length(14);
            Map(reg => reg.FotoUrl).Column("FOTO_URL").Length(256);
            Map(reg => reg.FotoThumbUrl).Column("FOTO_THUMB_URL").Length(256);
            Map(reg => reg.Periodo).Column("PERIODO").Not.Nullable();
            Map(reg => reg.Token).Column("TOKEN").Length(256);

            References(reg => reg.Curso).ForeignKey("FK_CURSO_X_ALUNO").Column("ID_CURSO").Not.Nullable();
        }
    }
}