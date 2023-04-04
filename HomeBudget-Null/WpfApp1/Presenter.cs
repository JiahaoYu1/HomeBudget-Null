using System;
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

        public Presenter(string dbFile, ViewInterface newView)
        {
            view = newView;
            dbFileName = dbFile;
            budget = new HomeBudget(dbFileName);
        }

        public void AddExpense(DateTime date, int categoryId, double amount, string desc)
        {
            budget.expenses.Add(date, categoryId, amount, desc);
        }

        public void AddCategory(string name, Budget.Category.CategoryType type = Budget.Category.CategoryType.Expense)
        {
            budget.categories.Add(name, type);
        }

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
