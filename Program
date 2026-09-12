using System;
using System.Collections.Generic;

namespace FinanceManagement
{
    // a. Define core model using records
    public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

    // b. Define an interface ITransactionProcessor
    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    // c. Create three concrete classes implementing this interface
    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Bank Transfer] Processing amount: {transaction.Amount:C} for category: {transaction.Category}");
        }
    }

    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Mobile Money] Processing amount: {transaction.Amount:C} for category: {transaction.Category}");
        }
    }

    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Crypto Wallet] Processing amount: {transaction.Amount:C} for category: {transaction.Category}");
        }
    }

    // d. Define a base class Account
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;
        }
    }

    // e. Define a sealed class SavingsAccount
    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance) : base(accountNumber, initialBalance) { }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount > Balance)
            {
                Console.WriteLine("Insufficient funds");
            }
            else
            {
                Balance -= transaction.Amount;
                Console.WriteLine($"Transaction applied successfully. Updated Balance: {Balance:C}");
            }
        }
    }

    // f. Create a class FinanceApp
    public class FinanceApp
    {
        private List<Transaction> _transactions = new List<Transaction>();

        public void Run()
        {
            // i. Instantiate a SavingsAccount
            SavingsAccount savings = new SavingsAccount("ACC-998877", 1000.00m);

            // ii. Create three Transaction records
            Transaction t1 = new Transaction(1, DateTime.Now, 150.00m, "Groceries");
            Transaction t2 = new Transaction(2, DateTime.Now, 300.00m, "Utilities");
            Transaction t3 = new Transaction(3, DateTime.Now, 200.00m, "Entertainment");

            // iii. Use the following processors
            ITransactionProcessor momoProcessor = new MobileMoneyProcessor();
            ITransactionProcessor bankProcessor = new BankTransferProcessor();
            ITransactionProcessor cryptoProcessor = new CryptoWalletProcessor();

            momoProcessor.Process(t1);
            bankProcessor.Process(t2);
            cryptoProcessor.Process(t3);

            // iv. Apply each transaction to the SavingsAccount
            Console.WriteLine("\nApplying Transaction 1:");
            savings.ApplyTransaction(t1);

            Console.WriteLine("\nApplying Transaction 2:");
            savings.ApplyTransaction(t2);

            Console.WriteLine("\nApplying Transaction 3:");
            savings.ApplyTransaction(t3);

            // v. Add all transactions to _transactions
            _transactions.Add(t1);
            _transactions.Add(t2);
            _transactions.Add(t3);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            FinanceApp app = new FinanceApp();
            app.Run();
        }
    }
}
