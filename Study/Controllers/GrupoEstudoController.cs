using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.WebPages;
using Study.Data;
using Study.Models;
using Study.Models.DTO;
using Study.Models.Enums;
using Study.Models.Views;

namespace Study.Controllers
{
    [RoutePrefix("api/grupo")]
    public class GrupoEstudoController : BaseApiController
    {
        private Repository<GrupoEstudo> _repositorioGrupoEstudo;
        private Repository<Aluno> _repositorioAluno;
        private Repository<Disciplina> _repositorioDisciplina;
        private Repository<Participacao> _repositorioParticipacao;
        private Repository<ViewAlunosGrupo> _repositorioViewAlunosGrupo;

        [HttpGet]
        [Route("buscar")]
        public HttpResponseMessage ListarGrupos([FromUri]string nomeGrupo, [FromUri]string nomeDisciplina
            , [FromUri]string ordenaPor, [FromUri]DateTime data)
        {
            VerificaToken();
            if (Errors != null & HasError())
            {
                return SendErrorResponse(HttpStatusCode.Unauthorized);
            }

            _repositorioGrupoEstudo = new Repository<GrupoEstudo>(CurrentSession());
            
            var result = _repositorioGrupoEstudo.Queryable();
            if (!nomeGrupo.IsEmpty())
            {
                result = result.Where(x => x.Nome.ToLower().Contains(nomeGrupo.ToLower()));
            }
            if (!nomeDisciplina.IsEmpty())
            {
                result = result.Where(x => x.Disciplina.Nome.ToLower().Contains(nomeDisciplina.ToLower()));
            }
            if (data != null)
            {
                result = result.Where(x => x.DataEncontro >= data.Date);
            }

            var grupos = result.ToList().Select(x => new GrupoEstudoDTO
            {
                DataEncontro = x.DataEncontro,
                Descricao = x.Descricao,
                IdDisciplina = x.Disciplina.Id,
                IdLider = x.Lider.Id,
                Local = x.Local,
                Nome = x.Nome,
                NomeDisciplina = x.Disciplina.Nome,
                NomeLider = x.Lider.Nome,
                Privado = x.Privado,
                QuantidadeMaxAlunos = x.QuantidadeMaxAlunos
            });
            
            return MultipleResponse(HttpStatusCode.OK, grupos);
        }

        [HttpGet]
        [Route("detalhar")]
        public HttpResponseMessage GetGrupo([FromUri]long? idGrupo)
        {
            VerificaToken();
            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.Unauthorized);
            }

            _repositorioGrupoEstudo = new Repository<GrupoEstudo>(CurrentSession());
            GrupoEstudo result = null;
            if (idGrupo.HasValue)
            {
                result = _repositorioGrupoEstudo.Queryable().FirstOrDefault(x => x.Id == idGrupo.Value);
            }
            GrupoEstudoDTO dto = new GrupoEstudoDTO();
            if (result != null)
            {
                dto.Nome = result.Nome;
                dto.DataEncontro = result.DataEncontro;
                dto.Descricao = result.Descricao;
                dto.Local = result.Local;
                dto.Privado = result.Privado;
                dto.QuantidadeMaxAlunos = result.QuantidadeMaxAlunos;
                dto.IdDisciplina = result.Disciplina.Id;
                dto.NomeDisciplina = result.Disciplina.Nome;
                dto.IdLider = result.Lider.Id;
                dto.NomeLider = result.Lider.Nome;
            }
            return MultipleResponse(HttpStatusCode.OK, dto);
        }

        [HttpGet]
        [Route("alunos")]
        public HttpResponseMessage GetAlunosGrupo()
        {
            VerificaToken();
            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.Unauthorized);
            }

            _repositorioViewAlunosGrupo = new Repository<ViewAlunosGrupo>(CurrentSession());
            var result = _repositorioViewAlunosGrupo.Queryable().ToList();

            return MultipleResponse(HttpStatusCode.OK, result);
        }


        [HttpPost]
        [Route("criar")]
        public HttpResponseMessage CriarGrupo([FromBody]GrupoEstudo grupo)
        {
            VerificaToken();
            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.Unauthorized);
            }

            _repositorioGrupoEstudo = new Repository<GrupoEstudo>(CurrentSession());
            _repositorioDisciplina = new Repository<Disciplina>(CurrentSession());
            _repositorioAluno = new Repository<Aluno>(CurrentSession());
            
            ValidarCamposObrigatorios(grupo);
            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }
            grupo.Disciplina = _repositorioDisciplina.FindById(1);
            grupo.Lider = _repositorioAluno.FindById(1);
            try
            {
                _repositorioGrupoEstudo.Save(grupo);
                _repositorioGrupoEstudo.Flush();
                grupo.Disciplina = null;
                grupo.Lider = null;
                return MultipleResponse(HttpStatusCode.OK, grupo);
            }
            catch (Exception e)
            {
                AddError(e.Message);
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }
        }

        private void ValidarCamposObrigatorios(GrupoEstudo grupo)
        {
            if (string.IsNullOrEmpty(grupo.Nome))
            {
                AddError("O campo [Nome] é obrigatório.");
            }
            if (string.IsNullOrEmpty(grupo.Local))
            {
                AddError("O campo [Local] é obrigatório.");
            }
            if (string.IsNullOrEmpty(grupo.Descricao))
            {
                AddError("O campo [Descricao] é obrigatório.");
            }
            if (grupo.QuantidadeMaxAlunos == 0)
            {
                AddError("O campo [Quantida Máxima de Alunos] é obrigatório.");
            }
            if (grupo.DataEncontro == DateTime.MinValue)
            {
                AddError("O campo [Data Encontro] é obrigatório.");
            }

        }

    }
}