using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace OrmMapping.Entities
{
    public class Store
    {
        public virtual long Id { get; protected set; }
        public virtual string Name { get; set; }
        public virtual IList<Product> Products { get; set; }
        public virtual IList<Employee> Staff { get; set; }

        public Store()
        {
            Products = new List<Product>();
            Staff = new List<Employee>();
        }

        public virtual void AddProduct(Product product)
        {
            product.StoresStockedIn.Add(this);
            Products.Add(product);
        }

        public virtual void AddEmployee(Employee employee)
        {
            employee.Store = this;
            Staff.Add(employee);
        }
    }

    public class StoreMap : ClassMap<Store>
    {
        public StoreMap()
        {
            Table("stores");
            Id(s => s.Id).Column("id");
            Map(s => s.Name).Column("name");

            #region one-to-many many-to-many
            #endregion

        }
    }
}