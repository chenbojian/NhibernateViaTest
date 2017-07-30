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
    }

    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            throw new NotImplementedException();
        }
    }
}
