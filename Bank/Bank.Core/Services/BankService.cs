using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Bank.Core.Models.Abstract;
using Bank.Core.Models.Accounts;
using Bank.Core.Models.Events;

namespace Bank.Core.Services
{
    public class BankService
    {
        public IEnumerable<IBankAccount> RemoveAllBankAccountTransactions (List<BankAccount> bankAccounts)
        {
            //Cast to List
            var baList = bankAccounts.ToList();

            //Remove all Bank transactions
            foreach (var account in baList)
            {
                account.Transactions.Clear();
            }

            var accountsWithEvents = new List<IBankAccount>();

            //Accounts with other events are added to a new list
            foreach (var account in baList)
            {
                if (account.Payments.Any())
                {
                    accountsWithEvents.Add(account);
                }
                if (account.CreditCards != null)
                {
                    accountsWithEvents.AddRange((from creditCard in account.CreditCards
                                                 where creditCard.Transactions.Any()
                                                 select account).Cast<IBankAccount>());
                }
            }

            //Returning a list with accounts that has other transactions than Bank Transactions
            return accountsWithEvents;
        }

        public double GetBalance(IBankAccount bankAccount)
        {
            return bankAccount.Amount;
        }
        
        //returns subscription and payment interval in a Dictionary
        public Dictionary<string, int> GetTimeIntervalSubscriptions(IBankAccount bankAccount)
        {
            var subsequentials = new Dictionary<string, int>();
            var transactions = bankAccount.Transactions;
            var payments = bankAccount.Payments;

            var groupedTransactions = (from t in transactions
                                       .GroupBy(g => g.Text)
                                        select t).ToList();

            var groupedPayments = (from t in payments
                                       .GroupBy(g => g.Text)
                                       select t).ToList();

            //Transactions
            foreach (var tGroup in groupedTransactions)
            {
                var tempList = transactions.Where(t => t.Text == tGroup.Key).ToList();
                
                //If there is more than 2 transactions that contains same text
                if (tempList.Count > 2)
                {
                    DateTime previousTime = tempList.Select(t => t.Date).FirstOrDefault();
                    DateTime previousPreviousTime = previousTime;
                    var count = 0;

                    foreach (var item in tempList)
                    {
                        count += 1;
                        TimeSpan timeSpan1;
                        TimeSpan timeSpan2;

                        timeSpan1 = item.Date.Subtract(previousTime);
                        timeSpan2 = item.Date.Subtract(previousPreviousTime);

                        previousPreviousTime = previousTime;
                        previousTime = item.Date;

                        
                        if (timeSpan1.Days == 0 || count < 2) continue;

                        //If timespan2 is double +- 2 days from timespan1 indicates it is periodical
                        if (timeSpan2.Days >= timeSpan1.Days*2 - 2 && timeSpan2.Days <= timeSpan1.Days + 2)
                        {
                            if (!subsequentials.ContainsKey(item.Text))
                            {
                                //Add text and interval of payment/transaction
                                subsequentials.Add(item.Text, Math.Abs(timeSpan1.Days));
                            }
                        }

                    }
                }
            }

            //Payments
            foreach (var tGroup in groupedPayments)
            {
                var tempList = payments.Where(t => t.Text == tGroup.Key).ToList();

                //If there is more than 2 transactions that contains same text
                if (tempList.Count > 2)
                {
                    DateTime previousTime = tempList.Select(t => t.Date).FirstOrDefault();
                    DateTime previousPreviousTime = previousTime;
                    var count = 0;

                    foreach (var item in tempList)
                    {
                        count += 1;
                        TimeSpan timeSpan1;
                        TimeSpan timeSpan2;

                        timeSpan1 = item.Date.Subtract(previousTime);
                        timeSpan2 = item.Date.Subtract(previousPreviousTime);

                        previousPreviousTime = previousTime;
                        previousTime = item.Date;


                        if (timeSpan1.Days == 0 || count < 2) continue;

                        //If timespan2 is double +- 2 days from timespan1 indicates it is periodical
                        if (timeSpan2.Days >= timeSpan1.Days * 2 - 2 && timeSpan2.Days <= timeSpan1.Days + 2)
                        {
                            if (!subsequentials.ContainsKey(item.Text))
                            {
                                //Add text and interval of payment/transaction
                                subsequentials.Add(item.Text, Math.Abs(timeSpan1.Days));
                            }
                        }

                    }
                }
            }

            return subsequentials;
        }

    }
}
