using System;
using System.Text;
using System.Collections.Generic;
using System.Dynamic;
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
    public class UnitTestBankServiceTimeSpan
    {
        private BankCustomer _customer1;
        private IBankAccount _bankAccount1;


        [TestInitialize]
        public void SetUp()
        {
            //Setting up custommer and 3 accounts and 1 creditCard
            _customer1 = new BankCustomer("Andreas");
            _bankAccount1 = new BankAccount(10000, false, _customer1);

            //Creating some transactions
            var event1 = new Payment { Amount = 200, Date = new DateTime(2016,08,01), Text = "Gym" };
            var event2 = new Transaction { Amount = 99, Date = new DateTime(2016,07, 23), Text= "Video Streaming" };
            var event3 = new Payment { Amount = 200, Date = new DateTime(2016, 07, 18), Text = "Gym" };
            var event4 = new Payment { Amount = 200, Date = new DateTime(2016, 07, 04), Text = "Gym" };
            var event5 = new Payment { Amount = 200, Date = new DateTime(2016, 06,28), Text = "Gym" };
            var event6 = new Transaction { Amount = 99, Date = new DateTime(2016, 06,22), Text = "Video Streaming" };
            var event7 = new Payment { Amount = 200, Date = new DateTime(2016, 06, 20), Text = "Gym" };
            var event8 = new Transaction { Amount = 99, Date = new DateTime(2016, 05,23), Text = "Video Streaming" };
            var event9 = new Transaction { Amount = 199, Date = new DateTime(2016, 05,23), Text = "fake" };
            var event10 = new Transaction { Amount = 199, Date = new DateTime(2016, 06,15), Text = "fake" };
            var event11 = new Transaction { Amount = 199, Date = new DateTime(2016, 06,16), Text = "fake" };
            

            //Do withdrawl
            _bankAccount1.Withdrawal(event1);
            _bankAccount1.Withdrawal(event2);
            _bankAccount1.Withdrawal(event3);
            _bankAccount1.Withdrawal(event4);
            _bankAccount1.Withdrawal(event5);
            _bankAccount1.Withdrawal(event6);
            _bankAccount1.Withdrawal(event7);
            _bankAccount1.Withdrawal(event8);
            _bankAccount1.Withdrawal(event9);
            _bankAccount1.Withdrawal(event10);
            _bankAccount1.Withdrawal(event11);
        }
  
        [TestMethod]
        public void Test_FindSubsequensial_transactions_For_Video_Streaming()
        {
            //Arrange
            var bankService = new BankService();
            var expectedSubscription = "Video Streaming"; //subscription
            var expectedTimeInterval = "31"; //interval of transactions

            //Act
            var subsequentials = bankService.GetTimeIntervalSubscriptions(_bankAccount1);
            

            //Assert
            Assert.AreEqual(expectedSubscription, subsequentials.FirstOrDefault().Key);
            Assert.AreEqual(expectedTimeInterval, subsequentials.FirstOrDefault().Value.ToString());
        }

        [TestMethod]
        public void Test_FindSubsequensial_transactions_For_Gym()
        {
            //Arrange
            var bankService = new BankService();
            var expectedSubscription = "Gym"; //subscription
            var expectedTimeInterval = "14"; //interval of payment

            //Act
            var subsequentials = bankService.GetTimeIntervalSubscriptions(_bankAccount1);
            var gymText = subsequentials.Where(g => g.Key == "Gym").Select(f=>f.Key).FirstOrDefault();
            var gymSeq = subsequentials.Where(g => g.Key == "Gym").Select(f => f.Value).FirstOrDefault();


            //Assert
            Assert.AreEqual(expectedSubscription, gymText);
            Assert.AreEqual(expectedTimeInterval, gymSeq.ToString());
        }
    }
}
