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
using Study.Models;

namespace Study.Data
{
    public class NHibernateSessionFactory
    {
        private static IDictionary<string, ISessionFactory> sessionFactorys = new Dictionary<string, ISessionFactory>();

        public static IStatelessSession OpenStatelessSession(string ConnectionString, IInterceptor localSessionInterceptor, params Assembly[] Assemblies)
        {
            return CurrentSessionFactory(ConnectionString, Assemblies).OpenStatelessSession();
        }

        public static ISession OpenSession(string ConnectionString, IInterceptor localSessionInterceptor, params Assembly[] Assemblies)
        {
            if (localSessionInterceptor != null)
            {
                return CurrentSessionFactory(ConnectionString, Assemblies).OpenSession(localSessionInterceptor);
            }
            ISession session = CurrentSessionFactory(ConnectionString, Assemblies).OpenSession();
            return session;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static ISessionFactory CurrentSessionFactory(string ConnectionString, params Assembly[] Assemblies)
        {
            ISessionFactory factory;
            if (!sessionFactorys.TryGetValue(ConnectionString, out factory))
            {
                factory = CreateSessionFactory(ConnectionString, Assemblies);
            }
            return factory;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static ISessionFactory CreateSessionFactory(string ConnectionString, params Assembly[] Assemblies)
        {
            FluentConfiguration fluentConfiguration = Fluently.Configure()

                .Database(MySQLConfiguration.Standard
                    .ConnectionString(c => c.FromConnectionStringWithKey(ConnectionString))
                    .Driver<NHibernate.Driver.MySqlDataDriver>());
            foreach (Assembly assembly in Assemblies)
            {
                fluentConfiguration.Mappings(m => m.FluentMappings.AddFromAssembly(assembly));
            }


            fluentConfiguration.Mappings(m => m.FluentMappings
                .Conventions.AddFromAssemblyOf<PrimaryKeyConvention>()
                .Conventions.AddFromAssemblyOf<NameConvention>());

            //fluentConfiguration.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(true, true));

            ISessionFactory factory = fluentConfiguration.BuildSessionFactory();
            sessionFactorys.Add(ConnectionString, factory);
            return factory;
        }

        public static void CloseAllSessionFactory()
        {
            foreach (ISessionFactory factory in sessionFactorys.Values)
            {
                if (factory.IsClosed == false)
                {
                    factory.Close();
                }
            }
        }
    }
}