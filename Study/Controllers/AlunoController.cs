using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Study.Data;
using Study.Models;
using Study.Models.Views;
using System;
using System.Net.Mail;
using System.Web.Helpers;
using System.Drawing;
using System.IO;
using System.Web;
using Study.Models.DTO;

namespace Study.Controllers
{
    [RoutePrefix("api/aluno")]
    public class AlunoController : BaseApiController
    {
        private Repository<Aluno> _repositorioAluno;
        private Repository<ViewAluno> _repositorioViewAluno;
        private Repository<ViewGruposAluno> _repositorioViewGrupos;

        [HttpPost]
        [Route("login")]
        public HttpResponseMessage LogarAluno([FromBody] Aluno aluno)
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
            var senha = Crypto.SHA1(aluno.Senha);
            var alunoEncontrado = _repositorioAluno.Queryable()
                .SingleOrDefault(x => x.Matricula.Equals(aluno.Matricula) && x.Senha.Equals(senha));

            if (alunoEncontrado == null)
            {
                AddError("A Matrícula e/ou a Senha informados são inválidos.");
            }

            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }

            return MultipleResponse(HttpStatusCode.OK, new Aluno
            {
                Token = alunoEncontrado.Token
            });
        }

        [HttpGet]
        [Route("simplesDetalhamento")]
        public HttpResponseMessage GetAluno()
        {
            _repositorioViewAluno = new Repository<ViewAluno>(CurrentSession());
            ViewAluno result = null;
            string val = Request.Headers.Authorization.Scheme;
                result = _repositorioViewAluno.Queryable()
                    .FirstOrDefault(x => x.Token.Equals(Request.Headers.Authorization.Scheme));

            if (result != null)
            {
                result = new ViewAluno
                {
                    Id = result.Id,
                    Nome = result.Nome,
                    Matricula = result.Matricula
                };
            }

            return MultipleResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("detalhar")]
        public HttpResponseMessage GetAluno([FromUri] long? idAluno)
        {
            _repositorioViewAluno = new Repository<ViewAluno>(CurrentSession());
            ViewAluno result = null;
            if (idAluno.HasValue && idAluno >= 0)
            {
                result = _repositorioViewAluno.Queryable().FirstOrDefault(x => x.Id == idAluno.Value);
            }
            else if (Request.Headers.Authorization != null)
            {
                string val = Request.Headers.Authorization.Scheme;
                result = _repositorioViewAluno.Queryable()
                    .FirstOrDefault(x => x.Token.Equals(Request.Headers.Authorization.Scheme));
            }

            if (result != null)
            {
                result.Token = "";
                result.Foto = "";
            }

            return MultipleResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("detalhar/foto")]
        public HttpResponseMessage GetPhoto(long? idAluno)
        {
            _repositorioViewAluno = new Repository<ViewAluno>(CurrentSession());
            String result = null;
            if (idAluno.HasValue && idAluno >= 0)
            {
                result = _repositorioViewAluno.Queryable().Where(x => x.Id == idAluno.Value)
                    .Select(x => x.Foto).FirstOrDefault();
            }
            else if (Request.Headers.Authorization != null)
            {
                string val = Request.Headers.Authorization.Scheme;
                result = _repositorioViewAluno.Queryable()
                    .Where(x => x.Token.Equals(Request.Headers.Authorization.Scheme))
                    .Select(x => x.Foto).FirstOrDefault();
            }

            if (result != null && result.Length > 0)
            {
                Aluno aluno = new Aluno
                {
                    Foto = result
                };
                return MultipleResponse(HttpStatusCode.OK, aluno);
            }

            return MultipleResponse(HttpStatusCode.NoContent, null);
        }

        [HttpGet]
        [Route("logado")]
        public HttpResponseMessage AlunoLogado()
        {
            _repositorioAluno = new Repository<Aluno>(CurrentSession());
            Aluno result = null;
            if (Request.Headers.Authorization != null)
            {
                result = _repositorioAluno.Queryable()
                    .FirstOrDefault(x => x.Token == Request.Headers.Authorization.Scheme.ToString());
            }

            return MultipleResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("cadastro")]
        public HttpResponseMessage CadastrarAluno([FromBody] Aluno aluno)
        {
            _repositorioAluno = new Repository<Aluno>(CurrentSession());

            ValidarCamposObrigatorios(aluno);
            if (Errors == null || !HasError())
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
            aluno.Senha = Crypto.SHA1(aluno.Senha);

            try
            {
                _repositorioAluno.Save(aluno);
                _repositorioAluno.Flush();

                return MultipleResponse(HttpStatusCode.OK, aluno);
            }
            catch (Exception e)
            {
                AddError("Não foi possível salvar.");
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [Route("atualizar")]
        public HttpResponseMessage AtualizarAluno([FromBody] Aluno aluno)
        {
            VerificaToken();
            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.Unauthorized);
            }

            _repositorioAluno = new Repository<Aluno>(CurrentSession());

            ValidarCamposObrigatorios(aluno);
            VerificaUnicidade(aluno);
            var temp = _repositorioAluno.FindById(aluno.Id);
            if (temp != null && temp.Matricula != aluno.Matricula)
            {
                AddError("A Matrícula não pode ser alterada.");
            }
            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }
            if (aluno.Id <= 0 || aluno.Token == null)
            {
                aluno.Token = GeraToken(aluno);
            }
            aluno.Senha = Crypto.SHA1(aluno.Senha);
            try
            {
                _repositorioAluno.Save(aluno);
                _repositorioAluno.Flush();

                return MultipleResponse(HttpStatusCode.OK, aluno);
            }
            catch (Exception e)
            {
                AddError("Não foi possível salvar.");
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }

        }

        [HttpPut]
        [Route("esqueciSenha")]
        public HttpResponseMessage EsqueciSenha([FromBody] string email)
        {
            _repositorioAluno = new Repository<Aluno>(CurrentSession());
            Aluno aluno = null;
            if (email != null && email.Length > 0)
            {
                aluno = _repositorioAluno.Queryable().FirstOrDefault(x => x.Email.ToLower().Equals(email.ToLower()));
            }
            if (aluno != null)
            {
                aluno.Senha = Crypto.SHA1("senhapadrao123");
                aluno.Token = "";
                try
                {
                    _repositorioAluno.Save(aluno);
                    _repositorioAluno.Flush();
                }
                catch (Exception)
                {
                    AddError("Não foi possível resetar a senha do aluno.");
                    return SendErrorResponse(HttpStatusCode.BadRequest);
                }
                MailMessage mail = new MailMessage("hcapuchop@gmail.com", email);
                mail.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("hcapuchop@gmail.com", "1103Bianca");
                client.Host = "smtp.gmail.com";
                mail.Subject = "Esquecimento de Senha - ClickStudy";
                mail.Body = "Favor acessar o aplicativo com a senha: senhapadrao123";
                client.Send(mail);
                return MultipleResponse(HttpStatusCode.OK, null);
            }
            
            AddError("Não existe cadastro para o email informado");
            return SendErrorResponse(HttpStatusCode.BadRequest);

        }

        [HttpGet]
        [Route("grupos")]
        public HttpResponseMessage ListarGrupos([FromUri] bool ativo)
        {
            VerificaToken();
            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.Unauthorized);
            }
            _repositorioViewGrupos = new Repository<ViewGruposAluno>(CurrentSession());

            var grupos = _repositorioViewGrupos.Queryable();
            if (ativo)
            {
                var a = DateTime.Today;
                grupos = grupos.Where(x => x.DataEncontro >= DateTime.Today);
            }
            else
            {
                grupos = grupos.Where(x => x.DataEncontro < DateTime.Today);
            }
            _repositorioAluno = new Repository<Aluno>(CurrentSession());
            Aluno alunoLogado = null;
            if (Request.Headers.Authorization != null)
            {
                alunoLogado = _repositorioAluno.Queryable()
                    .FirstOrDefault(x => x.Token == Request.Headers.Authorization.ToString());
            }
            if (alunoLogado != null)
            {
                grupos = grupos.Where(x => x.IdAluno == alunoLogado.Id);
            }
            var repositorioParticipacao = new Repository<Participacao>(CurrentSession());
            return MultipleResponse(HttpStatusCode.OK, grupos.Select(x => new GrupoEstudoDTO
            {
                Id = x.IdGrupo,
                DataEncontro = new DateTimeOffset(x.DataEncontro),
                Nome = x.NomeGrupo,
                NomeDisciplina = x.NomeDisciplina,
                IdDisciplina = x.IdDisciplina,
                Participando = repositorioParticipacao.Queryable()
                    .Count(y => y.Aluno.Id == (alunoLogado == null ? 0 : alunoLogado.Id)
                        && y.Grupo.Id == x.IdGrupo) >0
            }).ToList());
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