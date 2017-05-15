using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using NHibernate.Dialect;

namespace Study.Data
{
    public class MySQL5GuidFixDialect : MySQL5Dialect
    {
        public MySQL5GuidFixDialect()
        {
            RegisterColumnType(DbType.Guid, "BINARY(16)");
        }
    }
}