using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NHibernate;
using Study.Data;
using Study.Models;
using Study.Models.Views;

namespace Study.Controllers
{
    public class CapuchoApiController : BaseApiController
    {

        public string Get()
        {
            var session = CurrentSession();
            var repositorio = new Repository<ViewAluno>(session);

            var alunos = repositorio.Queryable().ToList();
            StringBuilder teste = new StringBuilder();
            foreach (var item in alunos)
            {
                teste.Append(item.Nome);
                teste.Append(" - ");
                teste.Append(item.Email);
                teste.Append(" - ");
                teste.Append(item.NomeCurso);
                teste.Append(" - ");
                teste.Append(item.Indicacoes);
                teste.Append(" - ");
                teste.Append(item.ContraIndicacoes);
            }
            
            return teste.ToString();
        }

    }
}