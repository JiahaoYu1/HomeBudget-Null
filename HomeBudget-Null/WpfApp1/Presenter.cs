﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Budget;

namespace WpfApp1
{
    public class Presenter
    {
        private ViewInterface view;
        private HomeBudget budget;
        private string dbFileName;

        /// <summary>
        /// Initializes a new instance of the Presenter class
        /// </summary>
        /// <param name="dbFile">The database file of the budget to use</param>
        /// <param name="newView">The view to use for displaying</param>
        public Presenter(string dbFile, ViewInterface newView)
        {
            view = newView;
            dbFileName = dbFile;
            budget = new HomeBudget(dbFileName);
        }

        /// <summary>
        /// Adds a new expense to the budget
        /// </summary>
        /// <param name="date">The date of the expense</param>
        /// <param name="categoryId">The ID number of the specific category associated with the expense</param>
        /// <param name="amount">The amount of money expended or gained</param>
        /// <param name="desc">The name of the expense</param>
        public void AddExpense(DateTime date, int categoryId, double amount, string desc)
        {
            budget.expenses.Add(date, categoryId, amount, desc);
        }

        /// <summary>
        /// Adds a new expense category to the budget
        /// </summary>
        /// <param name="name">The name of the category</param>
        /// <param name="type">The type of category (Expense, Savings, Income, Credit)</param>
        public void AddCategory(string name, Budget.Category.CategoryType type = Budget.Category.CategoryType.Expense)
        {
            budget.categories.Add(name, type);
        }

        /// <summary>
        /// Saves the budget to a database file
        /// </summary>
        public void SaveToFile()
        {
            budget.SaveToFile(dbFileName);
        }

        //public double GetBudget()
        //{
        //    return budget.GetBudget();
        //}
    }
}
