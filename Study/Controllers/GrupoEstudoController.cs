using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.WebPages;
using Study.Data;
using Study.Models;
using Study.Models.Enums;
using Study.Models.Views;

namespace Study.Controllers
{
    [RoutePrefix("api/grupo")]
    public class GrupoEstudoController : BaseApiController
    {
        private Repository<GrupoEstudo> _repositorioGrupoEstudo;
        private Repository<Aluno> _repositorioAluno;
        private Repository<Participacao> _repositorioParticipacao;
        private Repository<ViewAlunosGrupo> _repositorioViewAlunosGrupo;

        [HttpGet]
        [Route("buscar")]
        public HttpResponseMessage ListarGrupos([FromUri]string nomeGrupo, [FromUri]string nomeDisciplina
            , [FromUri]string ordenaPor)
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

            return MultipleResponse(HttpStatusCode.OK, result.ToList());
        }

        [HttpGet]
        [Route("detalhar")]
        public HttpResponseMessage GetGrupo([FromUri]long? idGrupo)
        {
            VerificaToken();
            if (Errors != null & HasError())
            {
                return SendErrorResponse(HttpStatusCode.Unauthorized);
            }

            _repositorioGrupoEstudo = new Repository<GrupoEstudo>(CurrentSession());
            GrupoEstudo result = null;
            if (idGrupo.HasValue)
            {
                result = _repositorioGrupoEstudo.Queryable().FirstOrDefault(x => x.Id == idGrupo.Value);
            }

            return MultipleResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("alunos")]
        public HttpResponseMessage GetAlunosGrupo()
        {
            VerificaToken();
            if (Errors != null & HasError())
            {
                return SendErrorResponse(HttpStatusCode.Unauthorized);
            }

            _repositorioViewAlunosGrupo = new Repository<ViewAlunosGrupo>(CurrentSession());
            var result = _repositorioViewAlunosGrupo.Queryable().ToList();

            return MultipleResponse(HttpStatusCode.OK, result);
        }


    }
}