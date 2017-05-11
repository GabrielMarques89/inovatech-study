using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using FluentNHibernate.Conventions;
using Study.Data;
using Study.Models;
using Study.Models.Views;

namespace Study.Controllers
{
    [RoutePrefix("api/aluno")]
    public class AlunoController : BaseApiController
    {
        private Repository<Aluno> _repositorioAluno;
        private Repository<ViewAluno> _repositorioViewAluno;

        [HttpGet]
        public HttpResponseMessage GetAluno([FromUri]long? idAluno)
        {
            _repositorioViewAluno = new Repository<ViewAluno>(CurrentSession());
            ViewAluno result = null;
            if (idAluno.HasValue)
            {
                result = _repositorioViewAluno.Queryable().FirstOrDefault(x => x.Id == idAluno.Value);
            }
            return MultipleResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("cadastro")]
        public HttpResponseMessage CadastrarAluno([FromBody]Aluno aluno)
        {
            _repositorioAluno = new Repository<Aluno>(CurrentSession());

            ValidarCamposObrigatorios(aluno);
            VerificaUnicidade(aluno);

            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }
            if (aluno.Id <= 0 || aluno.Token == null)
            {
                aluno.Token = GeraToken(aluno);
            }
            
            _repositorioAluno.Save(aluno);
            _repositorioAluno.Flush();

            return MultipleResponse(HttpStatusCode.OK, aluno);
        }

        private void ValidarCamposObrigatorios(Aluno aluno)
        {
            if (string.IsNullOrEmpty(aluno.Matricula))
            {
                AddError("O campo [Matrícula] é obrigatório.");
            }
            if (string.IsNullOrEmpty(aluno.Senha))
            {
                AddError("O campo [Senha] é obrigatório.");
            }
            if (string.IsNullOrEmpty(aluno.Nome))
            {
                AddError("O campo [Nome] é obrigatório.");
            }
            if (string.IsNullOrEmpty(aluno.Email))
            {
                AddError("O campo [Email] é obrigatório.");
            }
            if (aluno.Periodo <= 0)
            {
                AddError("O campo [Período] é obrigatório.");
            }
        }

        private void VerificaUnicidade(Aluno aluno)
        {
            var alunoExistente = _repositorioAluno
                .Queryable().FirstOrDefault(x => (x.Email.ToLower().Equals(aluno.Email.ToLower())
                || x.Matricula.ToLower().Equals(aluno.Matricula.ToLower()))
                && x.Id != aluno.Id);
            if (alunoExistente != null)
            {
                AddError("Já existe um Aluno cadastrado com o Email e/ou Matrícula informados.");
            }
        }

        private string GeraToken(Aluno aluno)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            var bytes = Encoding.UTF8.GetBytes(aluno.Email + aluno.Matricula);
            var hash = sha1.ComputeHash(bytes);
            var sb = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
            {
                // can be "x2" if you want lowercase
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}