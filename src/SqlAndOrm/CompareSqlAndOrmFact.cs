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

            repo.Save(superMan);

            superMan.Address = "220";

            repo.Update(superMan);

            repo.Get(superMan.Id);

            repo.Delete(superMan);
        }

        [Fact]
        public void should_do_CRUD_with_orm()
        {
            var repo = new PersonOrmRepository();
            
        }
    }
}
