using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Study.Data;
using Study.Models;

namespace Study.Controllers
{
    [RoutePrefix("api/disciplina")]
    public class DisciplinaController : BaseApiController
    {
        private Repository<Disciplina> _repositorioDisciplina;
        private Repository<Aluno> _repositorioAluno;
        
        [HttpGet]
        [Route("buscar")]
        public HttpResponseMessage ListarDisciplinas([FromUri]string nomeDisciplina)
        {
            VerificaToken();
            if (Errors != null & HasError())
            {
                return SendErrorResponse(HttpStatusCode.Unauthorized);
            }

            _repositorioDisciplina = new Repository<Disciplina>(CurrentSession());

            var result = _repositorioDisciplina.Queryable();
            if (nomeDisciplina != null && nomeDisciplina.Length > 0)
            {
                result = result.Where(x => x.Nome.ToLower().Contains(nomeDisciplina.ToLower()));
            }
            return MultipleResponse(HttpStatusCode.OK, result.ToList());

        }

    }
}