using System;
using System.Collections.Generic;
using Bank.Core.Models.Abstracts;
using Bank.Core.Models.Accounts;
using Bank.Core.Models.Events;

namespace Bank.Core.Models.Abstract
{
    public abstract class BankAccountBase : IBankAccount
    {
        public Guid OwnerId { get; set; }
        public double Amount { get; set; }
        public abstract bool AllowCredit { get; protected set; }
        public abstract List<Transaction> Transactions { get; set; }
        public abstract List<Payment> Payments { get; set; }

        //Transactions and Payments
        public bool Withdrawal(IEventType bankEvent)
        {
            if (AllowCredit == false && Amount < 0)
            {
                return false;
            }

            //Register Transactions and Payments
            RegisterBankEvent(bankEvent);
            Amount -= bankEvent.Amount;

            return true;
        }

        public void Deposit(double amount)
        {
            Amount += amount;
        }

        protected void RegisterBankEvent(IEventType eventType)
        {
            switch (eventType.BankEventType)
            {
                case BankEvents.Type.Transaction:
                    Transactions.Add(eventType as Transaction);
                    break;
                case BankEvents.Type.Payment:
                    Payments.Add(eventType as Payment);
                    break;
                default:
                    throw new NotImplementedException("Type is not implemented");
            }
        }
    }
}
