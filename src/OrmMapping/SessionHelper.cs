using System.Reflection;
using FluentNHibernate.Cfg;
using NHibernate;

namespace OrmMapping
{
    static class SessionHelper
    {
        public static ISessionFactory GetCurrentFactory()
        {
            return sessionFactory ?? (sessionFactory = CreateSessionFactory());
        }

        static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(
                    FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2008
                        .ConnectionString("Data Source=(local);Initial Catalog=TestDb;Integrated Security=True;")
                )
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .BuildSessionFactory();
        }

        static ISessionFactory sessionFactory { get; set; }
    }
}