using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using Study.Data;
using Study.Data.Models;

namespace Study.Controllers
{
    public class CapuchoApiController : BaseApiController
    {

        public IEnumerable<string> Get()
        {
            var session = CurrentSession();
            var repositorio = new Repository<User>(session);
            return new string[] { "value1", "value2" };
        }

    }
}