using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace SqlAndOrm
{
    public class TestBase
    {
        static TestBase()
        {
            CreateTestDb();
            CreateSchema();
        }
        public TestBase()
        {
            ExecuteQuery("truncate table Persons");
        }

        static void CreateSchema()
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
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        static void CreateTestDb()
        {
            var sql = @"
if exists (select * from sys.databases where name = 'TestDb')
    drop database TestDb;
create database TestDb;
";
            using (var connection = new SqlConnection("Data Source=(local);Initial Catalog=master;Integrated Security=True;"))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        protected string ExecuteQuery(string sql)
        {
            var results = new List<string>();
            using (var connection = new SqlConnection("Data Source=(local);Initial Catalog=TestDb;Integrated Security=True;"))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    results.Add(reader[0].ToString());
                }
                reader.Close();
            }
            return string.Join(",", results);
        }

    }
}