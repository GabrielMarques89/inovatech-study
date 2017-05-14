using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using FluentNHibernate.Conventions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NHibernate;
using Study.Data;
using Study.Models;

namespace Study.Controllers
{
    public class BaseApiController : ApiController
    {
        private ISession _session;
        private List<string> _errorMessages;
        private List<string> _successMessages;
        private Repository<Aluno> _repositorioAluno;

        protected ISession CurrentSession()
        {
            if (_session == null || _session.IsOpen == false)
            {
                _session = NHibernateSession.CurrentSession();
            }
            return _session;
        }

        public string ToJSON(object obj)
        {
            return JsonConvert.SerializeObject(obj,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    NullValueHandling = NullValueHandling.Ignore
                });
        }

        public HttpResponseMessage Response(object content)
        {
            return Response(HttpStatusCode.OK, ToJSON(content));
        }

        protected HttpResponseMessage SingleResponse(HttpStatusCode status, object errors)
        {
            return MultipleResponse(status, new { errors });
        }

        public HttpResponseMessage MultipleResponse(HttpStatusCode status, object errors)
        {
            return Response(status, ToJSON(errors));
        }

        protected HttpResponseMessage Response(HttpStatusCode status, string content)
        {
            var response = new HttpResponseMessage(status)
            {
                Content = new StringContent(content)
            };
            return response;
        }


        #region Error Handler

        protected IList<ErrorDTO> Errors { get; set; }

        public void AddError(string message)
        {
            if (Errors == null)
                Errors = new List<ErrorDTO>();
            Errors.Add(new ErrorDTO(message));
        }

        public void AddError(ICollection<string> messages)
        {
            if (Errors == null)
                Errors = new List<ErrorDTO>();
            foreach (var message in messages)
                Errors.Add(new ErrorDTO(message));
        }

        public bool HasError()
        {
            return Errors.IsEmpty() == false;
        }

        public HttpResponseMessage SendErrorResponse(HttpStatusCode status)
        {
            return Response(status, ToJSON(new { errors = Errors }));
        }

        public HttpResponseMessage SendErrorResponse(HttpStatusCode status, ICollection<string> errors)
        {
            AddError(errors);
            return SendErrorResponse(status);
        }

        #endregion


        public void VerificaToken()
        {
            _repositorioAluno = new Repository<Aluno>(CurrentSession());
            Aluno alunoLogado = null;
            if (Request.Headers.Authorization != null)
            {
                alunoLogado = _repositorioAluno.Queryable()
                    .FirstOrDefault(x => x.Token == Request.Headers.Authorization.ToString());
            }
            if (alunoLogado == null || alunoLogado.Id <= 0)
            {
                AddError("Aluno não autenticado");
            }
        }

    }
}