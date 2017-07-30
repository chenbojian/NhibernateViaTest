using System.Linq;
using NHibernate;
using OrmMapping.Entities;
using Xunit;

namespace OrmMapping
{
    public class MappingTest :TestBase
    {
        private readonly RepositoryBase<Store> storeRepository;
        private readonly RepositoryBase<Employee> employeeRepository;
        private readonly RepositoryBase<Product> productRepository;

        public MappingTest()
        {
            storeRepository = new RepositoryBase<Store>(Session);
            employeeRepository = new RepositoryBase<Employee>(Session);
            productRepository = new RepositoryBase<Product>(Session);
        }

        [Fact]
        void should_CRUD_employee()
        {
            var employee = new Employee
            {
                FirstName = "Wonder",
                LastName = "King"
            };
            Assert.Equal(default(long), employee.Id);

            //create
            employeeRepository.Create(employee);
            Assert.NotEqual(default(long), employee.Id);

            //retreive
            Employee retreived = employeeRepository.FindById(employee.Id);
            Assert.Equal(employee.FirstName, retreived.FirstName);
            Assert.Equal(employee.LastName, retreived.LastName);

            //update
            employee.LastName = "kingggg";
            employeeRepository.Update(employee);
            Assert.Equal("kingggg", employee.LastName);

            //delete
            employeeRepository.Delete(employee);
            Assert.Null(employeeRepository.FindById(employee.Id));
        }

        [Fact]
        void should_not_create_employee_when_associated_store_is_not_created()
        {
            var store = new Store
            {
                Name = "Wonder's Store",
            };

            var employee = new Employee
            {
                FirstName = "Wonder",
                LastName = "King",
                Store = store
            };

            store.AddEmployee(employee);
            Assert.Throws<TransientObjectException>(() => employeeRepository.Create(employee));
            Session.Clear();
        }

        [Fact]
        void should_not_delete_employee_when_associate_with_a_store()
        {
            var store = new Store
            {
                Name = "Wonder's Store",
            };

            var employee = new Employee
            {
                FirstName = "Wonder",
                LastName = "King",
                Store = store
            };

            store.AddEmployee(employee);
            storeRepository.Create(store);
            Assert.Throws<ObjectDeletedException>(() => employeeRepository.Delete(employee));
        }

        [Fact]
        void should_CRUD_employees_when_CRUD_a_store()
        {
            var store = new Store
            {
                Name = "Wonder's Store",
            };

            var employee = new Employee
            {
                FirstName = "Wonder",
                LastName = "King",
                Store = store
            };

            store.AddEmployee(employee);

            //create
            storeRepository.Create(store);

            //retreive from store
            Store retreivedStore = storeRepository.FindById(store.Id);
            Assert.Equal(store.Name, retreivedStore.Name);
            Assert.Equal(1, retreivedStore.Staff.Count);
            Assert.Equal(employee.FirstName, retreivedStore.Staff[0].FirstName);

            //retreive from employee
            Employee retreivedEmployee = employeeRepository.FindById(retreivedStore.Staff[0].Id);
            Assert.Equal(store.Name, retreivedEmployee.Store.Name);

            //update
            employee.LastName = "Kingggg";
            storeRepository.Update(store);
            Assert.Equal("Kingggg", store.Staff[0].LastName);

            //delete
            storeRepository.Delete(store);
            Assert.Null(employeeRepository.FindById(employee.Id));
        }

        [Fact]
        void should_CRUD_products_when_CRUD_a_store()
        {
            var store1 = new Store
            {
                Name = "Wonder's Store1"
            };

            var product1 = new Product
            {
                Name = "Wonder's Product1",
                Price = 5.2
            };

            store1.AddProduct(product1);

            //create
            storeRepository.Create(store1);

            //retreive from store
            Store retreivedStore1 = storeRepository.FindById(store1.Id);
            Assert.Equal(store1.Name, retreivedStore1.Name);
            Assert.Equal(1, retreivedStore1.Products.Count);
            Assert.Equal(product1.Name, retreivedStore1.Products[0].Name);
            Assert.Equal(product1.Price, retreivedStore1.Products[0].Price);

            //retreive from product
            Product retreivedProduct1 = productRepository.FindById(retreivedStore1.Products[0].Id);
            Assert.Equal(product1.Name, retreivedProduct1.Name);
            Assert.Equal(1, retreivedProduct1.StoresStockedIn.Count);
            Assert.Equal(store1.Name, retreivedProduct1.StoresStockedIn[0].Name);

            //update, add product2 to store1
            store1.Products[0].Price = 3.14;
            var product2 = new Product
            {
                Name = "Wonder's Product2",
                Price = 1000.01
            };
            store1.AddProduct(product2);
            storeRepository.Update(store1);

            Assert.Equal(2, retreivedStore1.Products.Count);
            retreivedProduct1 = retreivedStore1.Products.FirstOrDefault(p => p.Name == "Wonder's Product1");
            Assert.NotNull(retreivedProduct1);
            Assert.Equal(product1.Price, retreivedProduct1.Price);

            var retreivedProduct2 = retreivedStore1.Products.FirstOrDefault(p => p.Name == "Wonder's Product2");
            Assert.NotNull(retreivedProduct2);
            Assert.Equal(product2.Price, retreivedProduct2.Price);


            //create store2, add product2
            var store2 = new Store
            {
                Name = "Wonder's Store2"
            };
            store2.AddProduct(product2);
            storeRepository.Create(store2);

            retreivedProduct2 = productRepository.FindById(product2.Id);
            Assert.Equal(2, retreivedProduct2.StoresStockedIn.Count);
            Assert.True(retreivedProduct2.StoresStockedIn.AsEnumerable().Any(s => s.Name != store1.Name));
            Assert.True(retreivedProduct2.StoresStockedIn.AsEnumerable().Any(s => s.Name != store2.Name));


            //delete store2
            storeRepository.Delete(store2);
            retreivedProduct2 = productRepository.FindById(product2.Id);

        }

       
    }
}