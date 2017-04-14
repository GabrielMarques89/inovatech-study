using FluentNHibernate.Mapping;
using Study.Data.Models;

namespace Study.Data.Maps
{
    public class BaseClassMap<T> : ClassMap<T> where T : BaseEntity
    {
        public BaseClassMap()
        {
            Map(x => x.Version);
        }
    }
}
