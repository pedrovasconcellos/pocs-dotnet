using System;
using System.Collections.Generic;
using System.Text;

namespace TestBogus.UnitTest
{
    public class Customer
    {
        public Customer() => this.Address = new Address();
        public Guid Id { get; set; }
        public string Name { get; set; }
        //public string Cpf { get; set; }
        //public short Year { get; set; }
        //public DateTime Created { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        public Address() { }
        public Address(string zipeCode) => this.ZipeCode = zipeCode;
        //public string City { get; set; }
        //public string State { get; set; }
        //public string Country { get; set; }
        public string ZipeCode { get; private set; }
    }
}
