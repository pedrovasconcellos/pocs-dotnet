using Bogus;
using System;
using System.Linq;
using Xunit;

namespace TestBogus.UnitTest
{
    public class UnitTest1
    {
        private static Func<string, string> funcZipeCode =
            str => str.Replace("-", "").PadRight(8, '0').Insert(5, "-");

        [Fact]
        public void Test1()
        {
            var fakerAddress = new Faker<Address>("pt_BR").StrictMode(true)
                .RuleFor(a => a.ZipeCode, f => funcZipeCode(f.Address.ZipCode()));

            var fakerCustomer = new Faker<Customer>("pt_BR").StrictMode(true)
                .RuleFor(p => p.Id, f => f.Random.Guid())
                .RuleFor(p => p.Name, f => f.Person.FullName)
                .RuleFor(p => p.Address, () => fakerAddress);

            var customer = fakerCustomer.Generate(5);

            var address = new Faker<Address>("pt_BR").StrictMode(true)
                .RuleFor(a => a.ZipeCode, f => funcZipeCode(f.Address.ZipCode()))
                .Generate(10);

            var fakerCustomer2 = new Faker<Customer>("pt_BR")
                .StrictMode(false)
                .Rules((f, o) =>
                {
                    o.Id = f.Random.Guid();
                    o.Name = f.Person.FullName;
                    o.Address = fakerAddress.Generate(1).FirstOrDefault();
                });

            var customer2 = fakerCustomer2.Generate(3);
        }

        public T GetInstance<T>()
        {
            Type t = typeof(T);
            T s = (T)Activator.CreateInstance(t);
            return s;
        }
    }
}
