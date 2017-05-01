using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Study.Data.Conventions;

namespace Study.Data
{
    public class NHibernateSessionFactory
    {
        private static readonly IDictionary<string, ISessionFactory> SessionFactorys = new Dictionary<string, ISessionFactory>();

        public static IStatelessSession OpenStatelessSession(string connectionString, IInterceptor localSessionInterceptor, params Assembly[] Assemblies)
        {
            return CurrentSessionFactory(connectionString, Assemblies).OpenStatelessSession();
        }

        public static ISession OpenSession(string connectionString, IInterceptor localSessionInterceptor, params Assembly[] Assemblies)
        {
            if (localSessionInterceptor != null)
            {
                return CurrentSessionFactory(connectionString, Assemblies).OpenSession(localSessionInterceptor);
            }
            ISession session = CurrentSessionFactory(connectionString, Assemblies).OpenSession();
            return session;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static ISessionFactory CurrentSessionFactory(string connectionString, params Assembly[] assemblies)
        {
            ISessionFactory factory;
            if (!SessionFactorys.TryGetValue(connectionString, out factory))
            {
                factory = CreateSessionFactory(connectionString, assemblies);
            }
            return factory;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static ISessionFactory CreateSessionFactory(string connectionString, params Assembly[] assemblies)
        {
            FluentConfiguration fluentConfiguration = Fluently.Configure()

                .Database(MySQLConfiguration.Standard
                    .ConnectionString(c => c.FromConnectionStringWithKey(connectionString))
                    .Driver<NHibernate.Driver.MySqlDataDriver>());
            foreach (Assembly assembly in assemblies)
            {
                fluentConfiguration.Mappings(m => m.FluentMappings.AddFromAssembly(assembly));
            }


            fluentConfiguration.Mappings(m => m.FluentMappings
                .Conventions.AddFromAssemblyOf<PrimaryKeyConvention>()
                .Conventions.AddFromAssemblyOf<NameConvention>());

            //fluentConfiguration.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true));

            ISessionFactory factory = fluentConfiguration.BuildSessionFactory();
            SessionFactorys.Add(connectionString, factory);
            return factory;
        }

        public static void CloseAllSessionFactory()
        {
            foreach (ISessionFactory factory in SessionFactorys.Values)
            {
                if (factory.IsClosed == false)
                {
                    factory.Close();
                }
            }
        }
    }
}