using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using NHibernate;

namespace Study.Data
{
    public class NHibernateSession
    {
        public static string CsMaster = "CS_STUDY";

        public static IStatelessSession CurrentStatelessSession()
        {
            return NHibernateSessionFactory.OpenStatelessSession(CsMaster, null);
        }

        public static ISession CurrentSession()
        {
            return NHibernateSessionFactory.OpenSession(CsMaster, null, Assembly.GetExecutingAssembly());
        }

        public static ISession CurrentSession(IInterceptor localSessionInterceptor)
        {
            return NHibernateSessionFactory.OpenSession(CsMaster, localSessionInterceptor, Assembly.GetExecutingAssembly());
        }

        public static ISession NestedScopeSession()
        {
            var nestedScope = AutofacDependencyResolver.Current.RequestLifetimeScope.BeginLifetimeScope(builder =>
                builder.Register(x => NHibernateSession.CurrentSession())
                    .As<ISession>());
            return nestedScope.Resolve<ISession>();
        }

        public static Assembly GetAssemby()
        {
            return Assembly.GetExecutingAssembly();
        }

    }
}