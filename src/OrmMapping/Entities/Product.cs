using System;
using System.Collections.Generic;
using FluentNHibernate.Conventions.Inspections;
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
            Table("products");
            Id(p => p.Id).Column("id");
            Map(p => p.Name).Column("name");
            Map(p => p.Price).Column("price");

            #region many-to-many
            HasManyToMany(p => p.StoresStockedIn).Cascade.AllDeleteOrphan().Table("store_product").ChildKeyColumn("store_id");
            #endregion
        }
    }
}