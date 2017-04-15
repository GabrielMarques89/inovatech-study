
namespace Study.Models
{
    public class Aluno : BaseEntity
    {
        public virtual string Matricula { get; set; }
        public virtual string Senha { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Email { get; set; }
        public virtual string Telefone { get; set; }
        public virtual string FotoUrl { get; set; }
        public virtual string FotoThumbUrl { get; set; }
        public virtual int Periodo { get; set; }
        public virtual Curso Curso { get; set; }
        public virtual string Token { get; set; }
    }
}