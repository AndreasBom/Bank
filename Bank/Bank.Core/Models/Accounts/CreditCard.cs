using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Core.Models.Abstract;
using Bank.Core.Models.Abstracts;
using Bank.Core.Models.Events;

namespace Bank.Core.Models.Accounts
{
    public sealed class CreditCard
    {
        
        public CreditCard(IBankAccount bankAccount)
        {
            BankAccount = bankAccount;
            CreditCardNumber = Guid.NewGuid();
            Transactions = new List<Transaction>();
        }

        public BankEvents.Type BankEventType => BankEvents.Type.CreditCardTransaction;
        public IBankAccount BankAccount { get; private set; }
        public Guid CreditCardNumber { get; private set; }
        public string NameOnCard { get; set; }
        public List<Transaction> Transactions { get; set; }

    }
}
