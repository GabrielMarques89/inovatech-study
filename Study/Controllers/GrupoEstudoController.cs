using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.WebPages;
using Study.Data;
using Study.Models;
using Study.Models.Views;

namespace Study.Controllers
{
    [RoutePrefix("api/grupo")]
    public class GrupoEstudoController : BaseApiController
    {
        private Repository<GrupoEstudo> _repositorioGrupoEstudo;
        private Repository<ViewAlunosGrupo> _repositorioViewAlunosGrupo;

        [HttpGet]
        public HttpResponseMessage GetGrupoEstudo([FromUri]string nomeGrupo, [FromUri]string nomeDisciplina
            , [FromUri]string ordenaPor)
        {
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
        [Route("alunos")]
        public HttpResponseMessage GetAlunosGrupo()
        {
            _repositorioViewAlunosGrupo = new Repository<ViewAlunosGrupo>(CurrentSession());
            var result = _repositorioViewAlunosGrupo.Queryable().ToList();

            return MultipleResponse(HttpStatusCode.OK, result);
        }

    }
}