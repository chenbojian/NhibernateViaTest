using SqlAndOrm.Entity;
using SqlAndOrm.Repository;
using Xunit;

namespace SqlAndOrm
{
    public class CompareSqlAndOrmFact : TestBase
    {
        [Fact]
        public void should_do_CRUD_with_sql()
        {
            var repo = new PersonSqlRepository();
            var superMan = new Person
            {
                FirstName = "Super",
                LastName = "Man",
                Address = "110",
                City = "New York",
            };

            Assert.Equal("0", ExecuteQuery("select count(1) from Persons"));

            repo.Save(superMan);
            Assert.Equal("1", ExecuteQuery("select count(1) from Persons"));

            superMan.Address = "220";
            repo.Update(superMan);
            Assert.Equal("220", ExecuteQuery("select Address from Persons"));

            Person person = repo.Get(superMan.Id);

            Assert.Equal("Super", person.FirstName);

            repo.Delete(superMan);
            Assert.Equal("0", ExecuteQuery("select count(1) from Persons"));

        }

        [Fact]
        public void should_do_CRUD_with_orm()
        {
            var repo = new PersonOrmRepository();
            
            var superMan = new Person
            {
                FirstName = "Super",
                LastName = "Man",
                Address = "110",
                City = "New York",
            };

            Assert.Equal("0", ExecuteQuery("select count(1) from Persons"));

            repo.Save(superMan);
            Assert.Equal("1", ExecuteQuery("select count(1) from Persons"));

            superMan.Address = "220";
            repo.Update(superMan);
            Assert.Equal("220", ExecuteQuery("select Address from Persons"));

            Person person = repo.Get(superMan.Id);

            Assert.Equal("Super", person.FirstName);

            repo.Delete(superMan);
            Assert.Equal("0", ExecuteQuery("select count(1) from Persons"));
        }
    }
}
