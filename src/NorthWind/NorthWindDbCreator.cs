using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NorthWind
{
    public class NorthWindDbCreator
    {
        const string ConnectionString = "Data Source=(local);Initial Catalog=master;Integrated Security=True;";
        const string NorthWindConnectionString = "Data Source=(local);Initial Catalog=NorthWind;Integrated Security=True;";

        private void DropDataBase()
        {
            var dropDbQuery = @"
if exists (select * from sys.databases where name = 'NorthWind')
    drop database NorthWind;";

            using(var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand(dropDbQuery, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private void CreateDataBase()
        {
            var createDbQuery = @"
if not exists (select * from sys.databases where name = 'NorthWind')
    create database NorthWind;";

            using(var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand(createDbQuery, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Create()
        {
            DropDataBase();
            CreateDataBase();

            var sqlFile = this.GetType().Assembly.GetManifestResourceStream("NorthWind.Scripts.northwind.sql");
            string[] sqlStrings;
            using (var sr = new StreamReader(sqlFile))
            {
                sqlStrings = new Regex("^GO", RegexOptions.Multiline).Split(sr.ReadToEnd());
            }

            using (var connection = new SqlConnection(NorthWindConnectionString))
            {
                connection.Open();
                foreach(var s in sqlStrings)
                {
                    var command = new SqlCommand(s, connection);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
