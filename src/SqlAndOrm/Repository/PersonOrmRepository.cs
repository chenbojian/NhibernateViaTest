using FluentNHibernate.Cfg;
using NHibernate;
using SqlAndOrm.Entity;

namespace SqlAndOrm.Repository
{
    public class PersonOrmRepository : IPersonRepository
    {
        public void Save(Person person)
        {
            var sessionFactory = OrmHeler.GetCurrentFactory();
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(person);
                    transaction.Commit();
                }
            }
        }

        public void Update(Person person)
        {
            var sessionFactory = OrmHeler.GetCurrentFactory();
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(person);
                    transaction.Commit();
                }
            }
        }

        public Person Get(long id)
        {
            Person person = null;
            var sessionFactory = OrmHeler.GetCurrentFactory();
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    person = session.QueryOver<Person>().Where(p => p.Id == id).SingleOrDefault();
                    transaction.Commit();
                }
            }

            return person;
        }

        public void Delete(Person people)
        {
            var sessionFactory = OrmHeler.GetCurrentFactory();
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Delete(people);
                    transaction.Commit();
                }
            }
        }
    }

    static class OrmHeler
    {
        public static ISessionFactory GetCurrentFactory()
        {
            if (sessionFactory == null)
            {
                sessionFactory = CreateSessionFactory();
            }
            return sessionFactory;
        }

        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(
                    FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2008
                        .ConnectionString("Data Source=(local);Initial Catalog=TestDb;Integrated Security=True;")
                ).Mappings(o => o.FluentMappings.Add(typeof(PersonMap)))
                .BuildSessionFactory();
        }

        private static ISessionFactory sessionFactory { get; set; }
    }
}