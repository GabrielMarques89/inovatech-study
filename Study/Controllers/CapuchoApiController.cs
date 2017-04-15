using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NHibernate;
using Study.Data;
using Study.Models;

namespace Study.Controllers
{
    public class CapuchoApiController : BaseApiController
    {

        public string Get()
        {
            var session = CurrentSession();
            var repositorio = new Repository<Instituicao>(session);

            var instituicoes = repositorio.Queryable().ToList();
            string teste = "";
            foreach (var item in instituicoes)
            {
                teste = teste + item.Nome;
                teste = teste + "   -   ";
            }


            return teste;
        }

    }
}