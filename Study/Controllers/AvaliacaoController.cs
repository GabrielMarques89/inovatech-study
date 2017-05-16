using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Study.Data;
using Study.Models;
using Study.Models.Enums;
using Study.Models.DTO;

namespace Study.Controllers
{
    [RoutePrefix("api/avaliacao")]
    public class AvaliacaoController : BaseApiController
    {
        private Repository<GrupoEstudo> _repositorioGrupoEstudo;
        private Repository<Avaliacao> _repositorioAvaliacao;
        private Repository<Aluno> _repositorioAluno;
        private Repository<Participacao> _repositorioParticipacao;


        [HttpPost]
        [Route("avaliar")]
        public HttpResponseMessage Avaliar([FromBody] Avaliacao avaliacao)
        {
            VerificaToken();
            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.Unauthorized);
            }

            _repositorioGrupoEstudo = new Repository<GrupoEstudo>(CurrentSession());
            _repositorioAluno = new Repository<Aluno>(CurrentSession());
            GrupoEstudo grupo = null;
            if (avaliacao.Grupo != null && avaliacao.Grupo.Id > 0)
            {
                grupo = _repositorioGrupoEstudo.Queryable().FirstOrDefault(x => x.Id == avaliacao.Grupo.Id);
                if (grupo == null)
                {
                    AddError("O Grupo não foi encontrado.");
                }
            }else
            {
                AddError("Informe o grupo que você e o aluno avaliado participaram");
            }
            Aluno avaliado = null;
            Aluno avaliador = null;
            if (Request.Headers.Authorization != null)
            {
                avaliador = _repositorioAluno.Queryable()
                    .FirstOrDefault(x => x.Token == Request.Headers.Authorization.ToString());
                if (avaliador == null)
                {
                    AddError("O Aluno não foi encontrado.");
                }
            }
            if (avaliacao.Avaliado != null && avaliacao.Avaliado.Id > 0)
            {
                avaliado = _repositorioAluno.Queryable()
                    .FirstOrDefault(x => x.Id == avaliacao.Avaliado.Id);
                if (avaliado == null)
                {
                    AddError("O Aluno não foi encontrado.");
                }
            }
            else
            {
                AddError("Informe o aluno a ser avaliado.");
            }
            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }

            _repositorioAvaliacao = new Repository<Avaliacao>(CurrentSession());

            if (_repositorioAvaliacao.Queryable().Count(x => x.Avaliado.Id == avaliado.Id
                        && x.Avaliador.Id == avaliador.Id && x.Grupo.Id == grupo.Id) > 0)
            {
                AddError("Você já avaliou este aluno.");
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }
            if(avaliado.Id == avaliador.Id)
            {
                AddError("Não é possível avaliar a sí mesmo.");
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }

            Avaliacao aval = new Avaliacao
            {
                Avaliado = avaliado,
                Avaliador = avaliador,
                Grupo = grupo,
                AvaliacaoPositiva = avaliacao.AvaliacaoPositiva,
                Texto = avaliacao.Texto
            };
            try
            {
                _repositorioAvaliacao.Save(aval);
                _repositorioAvaliacao.Flush();
                return MultipleResponse(HttpStatusCode.OK, null);
            }
            catch (Exception)
            {
                AddError("Não foi possível avaliar o aluno.");
                return SendErrorResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [Route("listar/{id}")]
        public HttpResponseMessage ListarAvaliacoes([FromUri] long id)
        {
            VerificaToken();
            if (Errors != null && HasError())
            {
                return SendErrorResponse(HttpStatusCode.Unauthorized);
            }

            _repositorioAvaliacao = new Repository<Avaliacao>(CurrentSession());

            var query = _repositorioAvaliacao.Queryable().Where(x => x.Avaliado.Id == id);

            var result = query.ToList().Select(x => new AvaliacaoDTO
            {
                IdAvaliacao = x.Id,
                Texto = x.Texto,
                AvaliacaoPositiva = x.AvaliacaoPositiva,
                FotoAvaliador = x.Avaliador.Foto,
                IdAvaliador = x.Avaliador.Id,
                NomeAvaliador = x.Avaliador.Nome,
                IdAvaliado = x.Avaliado.Id,
                NomeAvaliado = x.Avaliado.Nome,
                IdGrupo = x.Grupo.Id,
                NomeGrupo = x.Grupo.Nome
            });

            return MultipleResponse(HttpStatusCode.OK, result);
        }


    }
}