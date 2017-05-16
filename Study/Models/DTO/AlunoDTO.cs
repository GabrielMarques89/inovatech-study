
namespace Study.Models.DTO
{
    public class AlunoDTO
    {
        public virtual long? Id { get; set; }
        public virtual long Version { get; set; }
        public virtual string Matricula { get; set; }
        public virtual string Senha { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Email { get; set; }
        public virtual string Telefone { get; set; }
        public virtual string Foto { get; set; }
        public virtual string Token { get; set; }
        public virtual bool? Autenticado { get; set; }
        public virtual bool? Avaliou { get; set; }
        public virtual long? AvaliacoesPositivas { get; set; }
        public virtual long? AvaliacoesNegativas { get; set; }
    }
}