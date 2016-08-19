using System;
using System.Collections.Generic;
using Bank.Core.Models.Accounts;

namespace Bank.Core.Models.Customer
{
    public class BankCustomer
    {
        public BankCustomer(string name)
        {
            Name = name;
            OwnerId = Guid.NewGuid();
        }
        public Guid OwnerId { get; set; }
        public string Name { get; set; }
        public IEnumerable<BankAccount> BankAccounts { get; set; }
    }
}
