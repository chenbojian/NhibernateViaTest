using SqlAndOrm.Entity;

namespace SqlAndOrm.Repository
{
    public interface IPersonRepository
    {
        void Save(Person person);
        void Update(Person people);
        Person Get(long id);
        void Delete(Person people);
    }
}