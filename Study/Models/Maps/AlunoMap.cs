using Study.Models;

namespace Study.Models.Maps
{
    public class AlunoMap : BaseClassMap<Aluno>
    {
        public AlunoMap()
        {
            Table("ALUNOS");

            Id(reg => reg.Id).Column("ID_ALUNO").GeneratedBy.Identity().Not.Nullable();

            Map(reg => reg.Matricula).Column("MATRICULA").Length(128).Not.Nullable();
            Map(reg => reg.Senha).Column("SENHA").Length(128).Not.Nullable();
            Map(reg => reg.Nome).Column("NOME").Length(128).Not.Nullable();
            Map(reg => reg.Email).Column("EMAIL").Length(128).Not.Nullable();
            Map(reg => reg.Telefone).Column("TELEFONE").Length(14);
            Map(reg => reg.Foto).Column("FOTO").CustomSqlType("MEDIUMTEXT"); ;
            Map(reg => reg.Token).Column("TOKEN").Length(256);
        }
    }
}