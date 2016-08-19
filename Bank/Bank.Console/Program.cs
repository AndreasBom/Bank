using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Core.Models;
using Bank.Core.Models.Accounts;
using Bank.Core.Models.Customer;
using Bank.Core.Models.Events;

namespace Bank.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var customer = new BankCustomer("Andreas");
            System.Console.WriteLine(customer.OwnerId);
            System.Console.WriteLine(customer.Name);

            var bankAccount = new BankAccount(customer);
            var creditCard = new CreditCard(bankAccount);

            //Deposit 100
            bankAccount.Deposit(100);
            
            //new transaction
            var transaction1 = new Transaction
            {
                Amount = 20,
                Date = DateTime.Now,
                Text = "Getting 20 out of the bank"
            };

            var transactionCreditCard = new Transaction
            {
                Amount = 30,
                Date = DateTime.Now,
                Text = "Getting 20 out from my credit card"
            };

            var payment1 = new Payment
            {
                Amount = 25,
                Date = DateTime.Now,
                Text = "Paying 25 from my account"
            };

            bankAccount.PayWithCreditCard(creditCard, transactionCreditCard);
            bankAccount.Withdrawal(transaction1);
            bankAccount.Withdrawal(payment1);

            System.Console.WriteLine(bankAccount.Amount);


            System.Console.ReadKey();

        }
    }
}
