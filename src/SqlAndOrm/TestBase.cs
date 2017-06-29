using System.Data.SqlClient;

namespace SqlAndOrm
{
    public class TestBase
    {

        public TestBase()
        {
            CreateTestDb();
            CreateSchema();
        }

        void CreateSchema()
        {
            var sql = @"
CREATE TABLE Persons (
    PersonID int,
    LastName varchar(255),
    FirstName varchar(255),
    Address varchar(255),
    City varchar(255) 
);
";
            using (var connection = new SqlConnection("Data Source=(local);Initial Catalog=TestDb;Integrated Security=True;"))
            {
                connection.Open();
                var command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }

        private void CreateTestDb()
        {
            var sql = @"
if exists (select * from sys.databases where name = 'TestDb')
    drop database TestDb;
create database TestDb;
";
            using (var connection = new SqlConnection("Data Source=(local);Initial Catalog=master;Integrated Security=True;"))
            {
                connection.Open();
                var command = new SqlCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }

    }
}