using System;
using Bank.Core.Models.Events;

namespace Bank.Core.Models.Abstracts
{
    public interface IEventType
    {
        BankEvents.Type BankEventType { get; }
        DateTime Date { get; set; }
        string Text { get; set; }
        double Amount { get; set; }
    }
}
