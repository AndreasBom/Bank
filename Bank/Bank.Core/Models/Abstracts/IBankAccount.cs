using System;
using System.Collections.Generic;
using Bank.Core.Models.Abstracts;
using Bank.Core.Models.Accounts;
using Bank.Core.Models.Events;

namespace Bank.Core.Models.Abstract
{
    public interface IBankAccount
    {
        Guid OwnerId { get; set; }
        double Amount { get; set; }
        //List<CreditCard> CreditCards { get; set; }
        List<Transaction> Transactions { get; set; }
        List<Payment> Payments { get; set; }
        bool Withdrawal(IEventType bankEvent);
        void Deposit(double amount);
    }
}