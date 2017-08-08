using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Mapping;

namespace OrmMapping
{
    public class TestBase : IDisposable
    {
        public ISession Session { get; }
        private readonly List<string> tables = new List<string>
        {
            "salary",
            "employees",
            "products",
            "stores",
            "store_product"
        };
        public TestBase()
        {
            Session = SessionHelper.GetCurrentFactory().OpenSession();
            tables.ForEach(ClearTable);
        }

        private void ClearTable(string table)
        {
            var sql = table == "employees" ? $"delete from {table}"  : $"truncate table {table}";
            var query = Session.CreateSQLQuery(sql);
            query.ExecuteUpdate();
        }

        public void Dispose()
        {
            Session.Close();
            Session.Dispose();
        }
    }
}