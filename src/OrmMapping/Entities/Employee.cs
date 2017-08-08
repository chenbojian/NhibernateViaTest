using System;
using FluentNHibernate.Mapping;

namespace OrmMapping.Entities
{
    public class Employee
    {
        public virtual long Id { get; protected set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual Store Store { get; set; }
        public virtual Salary Salary { get; set; }
    }

    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Table("employees");
            Id(e => e.Id).Column("id");
            Map(e => e.FirstName).Column("first_name");
            Map(e => e.LastName).Column("last_name");

            #region many-to-one one to one
            HasOne(e => e.Salary).Cascade.All().PropertyRef("Employee");
            References(e => e.Store).Column("store_id");
            #endregion
        }
    }
}
