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
        /// Returns all the category types as strings
        /// </summary>
        /// <returns>All the category types</returns>
        public static string[] GetCategoryTypes()
        {
            List<string> types = new List<string>();

            foreach (Category.CategoryType categoryType in Enum.GetValues(typeof(Category.CategoryType)))
                types.Add(categoryType.ToString());

            return types.ToArray();
        }

        /// <summary>
        /// Adds a new expense category to the budget
        /// </summary>
        /// <param name="name">The name of the category</param>
        /// <param name="type">The type of category (Expense, Savings, Income, Credit)</param>
        public void AddCategory(string name, string type = "Expense")
        {
            try
            {
                // Just for safety, make the type string have a capital letter at the start, and everything else lowercase
                string parsableType = string.Format("{0}{1}", type.Substring(0, 1).ToUpper(), type.Substring(1, type.Length).ToLower());

                // Attempt to add the new category
                budget.categories.Add(name, (Category.CategoryType)Enum.Parse(typeof(Category.CategoryType), parsableType));
                view.AddCategory(name, parsableType);
            }
            catch(Exception e)
            {
                view.DisplayError(e);
            }
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
