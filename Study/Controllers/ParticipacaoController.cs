using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Study.Data;
using Study.Models;
using Study.Models.Enums;

namespace Study.Controllers
{
    [RoutePrefix("api/participacao")]
    public class ParticipacaoController : BaseApiController
    {
        private Repository<GrupoEstudo> _repositorioGrupoEstudo;
        private Repository<Aluno> _repositorioAluno;
        private Repository<Participacao> _repositorioParticipacao;


        [HttpPost]
        [Route("solicitar")]
        public HttpResponseMessage SolicitarParticipacao([FromUri] long? idGrupo)
        {
            VerificaToken();
            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.Unauthorized);
            }

            _repositorioGrupoEstudo = new Repository<GrupoEstudo>(CurrentSession());
            _repositorioParticipacao = new Repository<Participacao>(CurrentSession());
            _repositorioAluno = new Repository<Aluno>(CurrentSession());
            GrupoEstudo grupo = null;
            if (idGrupo.HasValue)
            {
                grupo = _repositorioGrupoEstudo.Queryable().FirstOrDefault(x => x.Id == idGrupo.Value);
                if (grupo == null)
                {
                    AddError("O Grupo não foi encontrado.");
                }
            }
            Aluno aluno = null;
            if (Request.Headers.Authorization != null)
            {
                aluno = _repositorioAluno.Queryable()
                    .FirstOrDefault(x => x.Token == Request.Headers.Authorization.ToString());
                if (aluno == null)
                {
                    AddError("O Aluno não foi encontrado.");
                }
            }
            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }
            Participacao part = new Participacao
            {
                Aluno = aluno,
                Grupo = grupo,
                Tipo = TipoParticipacao.Membro,
                Participando = true
            };
            if (grupo != null && grupo.Privado)
            {
                part.Participando = null;
            }
            ParticipacaoDTO result = new ParticipacaoDTO
            {
                IdAluno = part.Aluno.Id,
                IdGrupo = part.Grupo.Id,
                NomeAluno = part.Aluno.Nome,
                NomeGrupo = part.Grupo.Nome,
                Participando = part.Participando,
                Recebendo = aluno != null && part.Aluno.Id == aluno.Id,
                Tipo = part.Tipo
            };
            try
            {
                _repositorioParticipacao.Save(part);
                _repositorioParticipacao.Flush();
                return MultipleResponse(HttpStatusCode.OK, result);
            }
            catch (Exception)
            {
                AddError("Não foi possível solicitar a participação no grupo.");
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [Route("pendentes")]
        public HttpResponseMessage ListarParticipacoesPendentes([FromUri] DateTime? data)
        {
            VerificaToken();
            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.Unauthorized);
            }

            _repositorioAluno = new Repository<Aluno>(CurrentSession());
            _repositorioParticipacao = new Repository<Participacao>(CurrentSession());
            var query = _repositorioParticipacao.Queryable().Where(x => x.Participando == null);
            Aluno aluno = null;
            if (Request.Headers.Authorization != null)
            {
                aluno = _repositorioAluno.Queryable()
                    .FirstOrDefault(x => x.Token == Request.Headers.Authorization.ToString());
            }
            if (data != null)
            {
                query = query.Where(x => x.Grupo.DataEncontro.Date > data.Value.Date);
            }
            if (aluno != null)
            {
                query = query.Where(x => (x.Aluno.Id != aluno.Id && x.Grupo.Lider.Id == aluno.Id) 
                                            || (x.Aluno.Id == aluno.Id));
            }
            var result = query.ToList().Select(x => new ParticipacaoDTO
            {
                IdParticipacao = x.Id,
                IdAluno = x.Aluno.Id,
                IdGrupo = x.Grupo.Id,
                NomeAluno = x.Aluno.Nome,
                NomeGrupo = x.Grupo.Nome,
                Participando = x.Participando,
                Recebendo = aluno != null && x.Aluno.Id == aluno.Id,
                Tipo = x.Tipo
            });

            return MultipleResponse(HttpStatusCode.OK, result);
        }

        [HttpPut]
        [Route("aprovar")]
        public HttpResponseMessage AprovarParticipacap([FromUri] long? idParticipacao, [FromUri] bool autoriza)
        {
            VerificaToken();
            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.Unauthorized);
            }

            _repositorioParticipacao = new Repository<Participacao>(CurrentSession());
            Participacao part = null;
            if (idParticipacao.HasValue)
            {
                part = _repositorioParticipacao.FindById(idParticipacao.Value);
            }
            if (part != null)
            {
                part.Participando = autoriza;
                try
                {
                    _repositorioParticipacao.Save(part);
                    _repositorioParticipacao.Flush();
                }
                catch
                {
                    AddError("Não foi possível salvar.");
                    return SendErrorResponse(HttpStatusCode.BadRequest);
                }
            }
            return MultipleResponse(HttpStatusCode.OK, part);
        }


    }
}