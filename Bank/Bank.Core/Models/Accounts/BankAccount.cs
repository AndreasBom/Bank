using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Bank.Core.Models.Abstract;
using Bank.Core.Models.Abstracts;
using Bank.Core.Models.Customer;
using Bank.Core.Models.Events;

namespace Bank.Core.Models.Accounts
{
    public sealed class BankAccount : BankAccountBase
    {
        //Constructors
        public BankAccount(BankCustomer customer)
        {
            OwnerId = customer.OwnerId;
            Amount = 0;
            AllowCredit = false;
            Transactions = new List<Transaction>();
            Payments = new List<Payment>();
        }

        public BankAccount(double amount, bool allowCredit, BankCustomer customer)
        {
            OwnerId = customer.OwnerId;
            Amount = amount;
            AllowCredit = allowCredit;
            Transactions = new List<Transaction>();
            Payments = new List<Payment>();
        }

        //Properties
        public override bool AllowCredit { get; protected set; }
        public List<CreditCard> CreditCards { get; set; }
        public override List<Transaction> Transactions { get; set; }
        public override List<Payment> Payments { get; set; }


        //Methods
        public void AddCreditCard(CreditCard creditCard)
        {
            if (CreditCards == null)
            {
                CreditCards = new List<CreditCard>();
            }

            CreditCards.Add(creditCard);
        }

        public void RemoveCreditCard(CreditCard creditCard)
        {
            CreditCards.Remove(creditCard);
        }

        public bool PayWithCreditCard(CreditCard creditCard, Transaction transaction)
        {
            if (AllowCredit == false && Amount < 0)
            {
                return false;
            }

            //Register credit card transaction
            creditCard.Transactions.Add(transaction as Transaction);
            Amount -= transaction.Amount;

            return true;
        }

        


    }
}
