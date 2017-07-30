using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace OrmMapping.Entities
{
    public class Product
    {
        public virtual long Id { get; protected set; }
        public virtual string Name { get; set; }
        public virtual double Price { get; set; }
        public virtual IList<Store> StoresStockedIn { get; protected set; }

        public Product()
        {
            StoresStockedIn = new List<Store>();
        }
    }

    public class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            throw new NotImplementedException();
        }
    }
}