using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NHibernate.Util;

namespace Study.Data.Conventions
{
    public class NameConvention : IClassConvention, IHasManyConvention
    {
        public void Apply(IClassInstance instance)
        {
            instance.Table(instance.EntityType.Name.ToLower());
        }

        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Key.ForeignKey($"{instance.Member.Name}_{instance.EntityType.Name}_FK");
            instance.Key.Column(instance.EntityType.Name + "Id");
        }
    }
}