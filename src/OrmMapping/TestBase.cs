using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using NHibernate;

namespace OrmMapping
{
    public class TestBase : IDisposable
    {
        public ISession Session { get; }

        static TestBase()
        {
            CreateTestDb();
            CreateSchema();
        }


        public TestBase()
        {
            Session = SessionHelper.GetCurrentFactory().OpenSession();
            ExecuteQuery("truncate table employees");
            ExecuteQuery("truncate table stores");
            ExecuteQuery("truncate table products");
            ExecuteQuery("truncate table store_product");
        }

        static void CreateSchema()
        {
            var sql = @"
CREATE TABLE employees (
    id bigint IDENTITY(1,1),
    first_name varchar(255),
    last_name varchar(255),
    store_id bigint
);

CREATE TABLE stores (
    id bigint IDENTITY(11,1),
    name varchar(255)
);

CREATE TABLE products (
    id bigint IDENTITY(101,1),
    name varchar(255),
    price dec(8,2)
);

CREATE TABLE store_product (
    id bigint IDENTITY(1001,1),
    store_id bigint,
    product_id_aaaaaa bigint
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

        public void Dispose()
        {
            Session.Close();
            Session.Dispose();
        }
    }
}