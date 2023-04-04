using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget;

namespace WpfApp1
{
    public class Presenter
    {
        private ViewInterface view;
        private HomeBudget budget;

        public Presenter(string dbFile, ViewInterface newView)
        {
            view = newView;
            budget = new HomeBudget(dbFile);
        }

        public void AddExpense(DateTime date, int categoryId, double amount, string desc)
        {
            budget.expenses.Add(date, categoryId, amount, desc);
        }

        //public double GetBudget()
        //{
        //    return budget.GetBudget();
        //}
    }
}
