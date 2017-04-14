using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using NHibernate;
using Study.Data;

namespace Study.Controllers
{
    public class BaseApiController : ApiController
    {
        private ISession _session;
        private List<string> _errorMessages;
        private List<string> _successMessages;

        protected ISession CurrentSession()
        {
            if (_session == null || _session.IsOpen == false)
            {
                _session = NHibernateSession.CurrentSession();
            }
            return _session;
        }
    }
}