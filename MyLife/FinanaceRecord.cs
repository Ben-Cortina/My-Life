using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyLife
{
    public class Finance
    {
        int bankBalance;

        public int BankBalance
        {
            get { return bankBalance; }
            set { bankBalance = value; }
        }

        int cash;

        public int Cash
        {
            get { return cash; }
            set { cash = value; }
        }

        int creditCardBalance;

        /// <summary>
        /// total credit balance
        /// </summary>
        public int CreditCardBalance
        {
            get { return creditCardBalance; }
            set { creditCardBalance = value; }
        }

        int creditCardMonthBalance;

        /// <summary>
        /// credit balance from this month
        /// </summary>
        public int CreditCardMonthBalance
        {
            get { return creditCardMonthBalance; }
            set { creditCardMonthBalance = value; }
        }

        int creditCardLimit;

        /// <summary>
        /// credit limit
        /// </summary>
        public int CreditCardLimit
        {
            get { return creditCardLimit; }
            set { creditCardLimit = value; }
        }

        List<Transaction> transactions;

        public List<Transaction> Transactions
        {
            get { return transactions; }
            set { transactions = value; }
        }

        public Finance(int p_b, int p_c, int p_cc, int p_ccl)
        {
            bankBalance = p_b;
            cash = p_c;
            creditCardMonthBalance = 0;
            creditCardBalance = p_cc;
            creditCardLimit = p_ccl;
            transactions = new List<Transaction>();
        }
    }

    public class Transaction
    {
        String name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        int amount;

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        int cash;

        public int Cash
        {
            get { return cash; }
            set { cash = value; }
        }

        int bank;

        public int Bank
        {
            get { return bank; }
            set { bank = value; }
        }

        int credit;

        public int Credit
        {
            get { return credit; }
            set { credit = value; }
        }

        public Transaction(String p_name, int p_amount)
        {
            name = p_name;
            amount = p_amount;
            date = GlobalVariables.Date;
            cash = GlobalVariables.Finance.Cash;
            bank = GlobalVariables.Finance.BankBalance;
            credit = GlobalVariables.Finance.CreditCardBalance + GlobalVariables.Finance.CreditCardMonthBalance;

        }
    }
}
