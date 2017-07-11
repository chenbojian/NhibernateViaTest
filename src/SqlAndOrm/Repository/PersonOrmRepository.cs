using System;
using System.Reflection;
using FluentNHibernate.Cfg;
using NHibernate;
using SqlAndOrm.Entity;

namespace SqlAndOrm.Repository
{
    public class PersonOrmRepository : IPersonRepository, IDisposable
    {
        readonly ISession session;

        public PersonOrmRepository()
        {
            session = OrmHeler.GetCurrentFactory().OpenSession();
        }

        public void Save(Person person)
        {
            WrapTransaction(() => session.Save(person));
        }

        public void Update(Person person)
        {
            WrapTransaction(() => session.Update(person));
        }

        public Person Get(long id)
        {
            return session.QueryOver<Person>().Where(p => p.Id == id).SingleOrDefault();
        }

        public void Delete(Person person)
        {
            WrapTransaction(() => session.Delete(person));
        }

        public void Dispose()
        {
            session.Close();
            session.Dispose();
        }


        void WrapTransaction(Action action)
        {
            using (ITransaction transaction = session.BeginTransaction())
            {
                action();
                transaction.Commit();
            }
        }
    }

    static class OrmHeler
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