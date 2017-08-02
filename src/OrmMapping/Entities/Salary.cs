using System;
using FluentNHibernate.Mapping;

namespace OrmMapping.Entities
{
    public class Salary
    {
        public virtual long Fee { get; set; }
        public virtual long EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
    }

    public class SalaryMap : ClassMap<Salary>
    {
        public SalaryMap()
        {
            throw new NotImplementedException();
        }
    }
}