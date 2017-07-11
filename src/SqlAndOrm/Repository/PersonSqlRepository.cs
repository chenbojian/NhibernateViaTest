using System;
using System.Data;
using System.Data.SqlClient;
using SqlAndOrm.Entity;

namespace SqlAndOrm.Repository
{
    public class PersonSqlRepository : IPersonRepository
    {
        public void Save(Person person)
        {
            var sql = $"insert into Persons (LastName, FirstName, Address, City) values"
                + $"('{person.LastName}', '{person.FirstName}', '{person.Address}', '{person.City}');"
                + "select cast(scope_identity() as int)";

            person.Id = (int)SqlHelper.ExecuteScalar(sql);
        }

        public void Update(Person person)
        {
            var sql = $"update Persons set LastName='{person.LastName}', FirstName='{person.FirstName}'," +
                      $" Address='{person.Address}', City='{person.City}' where PersonID={person.Id}";
            SqlHelper.ExecuteNonQuery(sql);
        }

        public Person Get(long id)
        {
            var sql = $"select * from Persons where PersonID={id}";
            using (var reader = SqlHelper.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    return new Person
                    {
                        Id = (int)reader["PersonID"],
                        LastName = (string)reader["LastName"],
                        FirstName = (string)reader["FirstName"],
                        Address = (string)reader["Address"],
                        City = (string)reader["City"]
                    };
                }
            }

            return null;
        }

        public void Delete(Person person)
        {
            var sql = $"delete from Persons where PersonID={person.Id}";
            SqlHelper.ExecuteNonQuery(sql);
        }
    }

    static class SqlHelper
    {
        const string ConnectionString = @"Data Source=(local);Initial Catalog=TestDb;Integrated Security=True;";

        public static int ExecuteNonQuery(string commandText)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteScalar(string commandText)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static SqlDataReader ExecuteReader(string commandText)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);

            using (SqlCommand cmd = new SqlCommand(commandText, conn))
            {
                conn.Open();
                // When using CommandBehavior.CloseConnection, the connection will be closed when the 
                // IDataReader is closed.
                SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
        }

    }
}