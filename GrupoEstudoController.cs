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

        [HttpPost]
        [Route("criar")]
        public HttpResponseMessage CriarGrupo([FromBody]GrupoEstudo grupo)
        {
            _repositorioViewAlunosGrupo = new Repository<ViewAlunosGrupo>(CurrentSession());

            if(grupo == null)
            {
                AddError("Os campos devem ser preenchidos.");
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }

            ValidarCamposObrigatorios(grupo);

            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                _repositorioGrupoEstudo.Save(grupo);
                _repositorioGrupoEstudo.Flush();
                return MultipleResponse(HttpStatusCode.OK,grupo);
            }
            catch (Exception e)
            {
                AddError("Não foi possível criar o grupo.");
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
            if(grupo.QuantidadeMaxAlunos == 0)
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