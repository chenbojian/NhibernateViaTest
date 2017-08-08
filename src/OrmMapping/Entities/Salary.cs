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
            Table("salary");
            Id(s => s.EmployeeId).Column("employee_id").GeneratedBy.Foreign("Employee");
            Map(s => s.Fee).Column("fee");

            #region one-to-one
            HasOne(s => s.Employee).Cascade.All().Constrained();
            #endregion

        }
    }
}