using FluentNHibernate.Mapping;

namespace SqlAndOrm.Entity
{
    public class Person
    {
        public virtual long Id { get; set; }
        public virtual string LastName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
    }

    public class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Table("Persons");
            Id(x => x.Id).Column("PersonID");
            Map(x => x.LastName);
            Map(x => x.FirstName);
            Map(x => x.Address);
            Map(x => x.City);
        }
    }
}