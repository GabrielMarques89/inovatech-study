using FluentNHibernate.Mapping;

namespace Study.Models.Maps
{
    public class BaseClassMap<T> : ClassMap<T> where T : BaseEntity
    {
        public BaseClassMap()
        {
            Map(x => x.Version).Column("VERSION");
        }
    }
}
