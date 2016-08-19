using System;
using System.Collections.Generic;
using System.Linq;
using Bank.Core.Models.Abstract;
using Bank.Core.Models.Accounts;
using Bank.Core.Models.Customer;
using Bank.Core.Models.Events;
using Bank.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bank.UnitTest
{
    [TestClass]
    public class UnitTestBankService
    {
        private BankCustomer _customer1;
        private IBankAccount _bankAccount1;
        private IBankAccount _bankAccount2;
        private IBankAccount _bankAccount3;
        private CreditCard _creditCard;

        [TestInitialize]
        public void SetUp()
        {
            //Setting up custommer and 3 accounts and 1 creditCard
            _customer1 = new BankCustomer("Andreas");
            _bankAccount1 = new BankAccount(1000, false, _customer1) as BankAccount;
            _bankAccount2 = new BankAccount(5000, false, _customer1);
            _bankAccount3 = new BankAccount(3000, false, _customer1);
            _creditCard = new CreditCard(_bankAccount1);
            


            //Doing some transactions, payments and credit card transactions
            var transaction1 = new Transaction {Amount = 1, Date = DateTime.Now, Text = "Transaction 1"};
            var transaction2 = new Transaction {Amount = 10, Date = DateTime.Now, Text = "Transaction 10"};
            var transaction3 = new Transaction {Amount = 100, Date = DateTime.Now, Text = "Transaction 100"};
            var payment1 = new Payment {Amount = 10, Date = DateTime.Now, Text = "Transaction 10"};
            var payment2 = new Payment {Amount = 20, Date = DateTime.Now, Text = "Transaction 20"};
            var payment3 = new Payment {Amount = 30, Date = DateTime.Now, Text = "Transaction 30"};
            var transactionCC1 = new Transaction {Amount = 10, Date = DateTime.Now, Text = "Transaction 10"};
            var transactionCC2 = new Transaction {Amount = 100, Date = DateTime.Now, Text = "Transaction 100"};


            //Events on bankAccount 1, total withdrowl: 151
            _bankAccount1.Withdrawal(transaction1);
            _bankAccount1.Withdrawal(transaction2);
            _bankAccount1.Withdrawal(payment1);
            _bankAccount1.Withdrawal(payment2);
            var b1 = _bankAccount1 as BankAccount;
            b1.PayWithCreditCard(_creditCard, transactionCC1);
            b1.PayWithCreditCard(_creditCard, transactionCC2);

            //Events on bankaccount 2, total withdrowl 152
            _bankAccount2.Withdrawal(transaction1);
            _bankAccount2.Withdrawal(transaction1);
            _bankAccount2.Withdrawal(transaction3);
            _bankAccount2.Withdrawal(payment2);
            _bankAccount2.Withdrawal(payment3);

            //Events on bankaccount 3 ONLY bank transactions!, total withdrawl 111
            _bankAccount3.Withdrawal(transaction1);
            _bankAccount3.Withdrawal(transaction2);
            _bankAccount3.Withdrawal(transaction3);

        }

        [TestMethod]
        public void Test_WithDrawl_is_reducing_Account1_Amount()
        {
            var B1 = 1000;

            var expectedB1 = B1 - 151;

            Assert.AreEqual(expectedB1, _bankAccount1.Amount);

        }

        [TestMethod]
        public void Test_WithDrawl_is_reducing_Account2_Amount()
        {
            var B2 = 5000;

            var expectedB2 = B2 - 152;

            Assert.AreEqual(expectedB2, _bankAccount2.Amount);
        }

        [TestMethod]
        public void Test_WithDrawl_is_reducing_Account3_Amount()
        {
            var B3 = 3000;
            var expectedB3 = B3 - 111;

            Assert.AreEqual(expectedB3, _bankAccount3.Amount);
        }

        [TestMethod]
        public void Test_BankService_Remove_AllBank_Transactions_Expected_Two_Accounts_left()
        {
            //arrange
            var bankService = new BankService();
            var listWithBankAcounts = new List<BankAccount>();
            listWithBankAcounts.Add(_bankAccount1 as BankAccount);
            listWithBankAcounts.Add(_bankAccount2 as BankAccount);
            listWithBankAcounts.Add(_bankAccount3 as BankAccount);

            //act
            var manipulatedList = bankService.RemoveAllBankAccountTransactions(listWithBankAcounts);

            //assert
            var expects = 2;
            Assert.AreEqual(expects, manipulatedList.Count());
        }

        [TestMethod]
        public void Test_GetAmount()
        {
            //Arrange
            var B3 = 3000;
            var expectedBalance = B3 - 111;
            var bankService = new BankService();
            
            //Act
            var amount = bankService.GetBalance(_bankAccount3);



            Assert.AreEqual(expectedBalance, amount);
        }

        [TestMethod]
        public void Test_Group_Text_And_Get_TimeSpan()
        {
            var bankService = new BankService();
            bankService.GetTimeIntervalSubscriptions(_bankAccount1);
        }
    }
}
