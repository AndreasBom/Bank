using System;

namespace Bank.Core.Models.Events
{
    public class BankEvents
    {
        public enum Type
        {
            Payment,
            Transaction,
            CreditCardTransaction
        }
    }
}
