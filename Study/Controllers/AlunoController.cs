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
using System;

namespace Study.Controllers
{
    [RoutePrefix("api/aluno")]
    public class AlunoController : BaseApiController
    {
        private Repository<Aluno> _repositorioAluno;
        private Repository<ViewAluno> _repositorioViewAluno;

        [HttpPost]
        [Route("login")]
        public HttpResponseMessage LogarAluno([FromBody]Aluno aluno)
        {
            _repositorioAluno = new Repository<Aluno>(CurrentSession());
            if (aluno == null)
            {
                AddError("Informe os dados para autenticar.");
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }
            ValidarCamposObrigatoriosLogin(aluno.Matricula, aluno.Senha);

            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }

            var alunoEncontrado = _repositorioAluno.Queryable()
                .Where(x => x.Matricula.Equals(aluno.Matricula) && x.Senha.Equals(aluno.Senha))
                .SingleOrDefault();

            if (alunoEncontrado == null)
            {
                AddError("A [Matrícula] e/ou a [Senha] informados são inválidos.");
            }

            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }

            return MultipleResponse(HttpStatusCode.OK, alunoEncontrado);
        }

        [HttpGet]
        [Route("auth")]
        public HttpResponseMessage Autenticar()
        {
            var encontrado = _repositorioAluno.Queryable().FirstOrDefault(x => x.Token.Equals(Request.Headers.Authorization)) != null;
            if (!encontrado)
            {
                AddError("O aluno não está mais autenticado");
                return SendErrorResponse(HttpStatusCode.Unauthorized);
            }
            return MultipleResponse(HttpStatusCode.OK, null);
        }

        [HttpGet]
        [Route("detalhar")]
        public HttpResponseMessage GetAluno([FromUri]long? idAluno)
        {
            _repositorioViewAluno = new Repository<ViewAluno>(CurrentSession());
            ViewAluno result = null;
            if (idAluno.HasValue)
            {
                result = _repositorioViewAluno.Queryable().FirstOrDefault(x => x.Id == idAluno.Value);
            }else if(Request.Headers.Authorization != null)
            {
                result = _repositorioViewAluno.Queryable()
                    .FirstOrDefault(x => x.Token.Equals(Request.Headers.Authorization));
            }

            if (result != null)
            {
                result.Token = "";
                if(result.Foto != null && result.Foto.Length > 0)
                {
                    result.FotoB64 = "data:image/jpeg;base64," + Convert.ToBase64String(result.Foto);
                }
            }

            return MultipleResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("cadastro")]
        public HttpResponseMessage CadastrarAluno([FromBody]Aluno aluno)
        {
            _repositorioAluno = new Repository<Aluno>(CurrentSession());

            ValidarCamposObrigatorios(aluno);
            if(Errors == null || !HasError())
            {
                VerificaUnicidade(aluno);
            }

            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }
            if (aluno.Id <= 0 || aluno.Token == null)
            {
                aluno.Token = GeraToken(aluno);
            }
            
            if(aluno.FotoB64 != null && aluno.FotoB64.Length > 0)
            {
                var foto = aluno.FotoB64.Substring(aluno.FotoB64.IndexOf(",")+1);
                aluno.Foto = Convert.FromBase64String(foto);
            }

            try
            {
                _repositorioAluno.Save(aluno);
                _repositorioAluno.Flush();

                return MultipleResponse(HttpStatusCode.OK, aluno);
            }catch(Exception e)
            {
                AddError("Não foi possível salvar.");
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }
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

        private void ValidarCamposObrigatoriosLogin(string matricula, string senha)
        {
            if (string.IsNullOrEmpty(matricula))
            {
                AddError("O campo [Matrícula] é obrigatório.");
            }
            if (string.IsNullOrEmpty(senha))
            {
                AddError("O campo [Senha] é obrigatório.");
            }
        }
    }
}