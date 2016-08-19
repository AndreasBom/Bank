using System;
using Bank.Core.Models.Abstract;
using Bank.Core.Models.Abstracts;

namespace Bank.Core.Models.Events
{
    public class Payment : IEventType
    {
        public BankEvents.Type BankEventType => BankEvents.Type.Payment;
        public string RecipientNumber { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public double Amount { get; set; }
    }
}
