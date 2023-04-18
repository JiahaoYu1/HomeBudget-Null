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
        private HomeBudget budget = null;
        private string dbFileName;

        /// <summary>
        /// Initializes a new instance of the Presenter class
        /// </summary>
        /// <param name="dbFile">The database file of the budget to use</param>
        /// <param name="newView">The view to use for displaying</param>
        public Presenter(ViewInterface newView)
        {
            view = newView;
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
            if (isFileLoaded())
            {
                try
                {
                    // Just for safety, make the type string have a capital letter at the start, and everything else lowercase
                    string parsableType = string.Format("{0}{1}", type.Substring(0, 1).ToUpper(), type.Substring(1, type.Length - 1).ToLower());

                    // Attempt to add the new category
                    budget.categories.Add(name, (Category.CategoryType)Enum.Parse(typeof(Category.CategoryType), parsableType));
                    view.AddCategory(name, parsableType);
                }
                catch (Exception e)
                {
                    view.DisplayError(e);
                }
            }
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
            if (isFileLoaded())
            {
                try
                {
                    budget.expenses.Add(date, categoryId, amount, desc);
                    view.AddExpense();
                }
                catch (Exception e)
                {
                    view.DisplayError(e);
                }
            }
            
        }

        /// <summary>
        /// Retrieves a database file and loads a HomeBudget object using it
        /// </summary>
        /// <param name="dbFile">The database file to load</param>
        public void LoadFile(string dbFile, bool isCreatingNewFile)
        {
            budget = new HomeBudget(dbFile, isCreatingNewFile);
        }
        /// <summary>
        /// Saves the budget to a database file
        /// </summary>
        //public void SaveToFile()
        //{
        //    budget.SaveToFile(dbFileName);
        //}

        private bool isFileLoaded()
        {
            bool isFileLoaded = budget is not null;

            if (!isFileLoaded)
                view.DisplayError(new Exception("A file must be created or loaded"));

            return isFileLoaded;
        }

        /// <summary>
        /// Returns a list of all expenses
        /// </summary>
        /// <returns>A list of all expenses</returns>
        public List<Expense> GetExpenseList()
        {
            return budget.expenses.List();
        }

        /// <summary>
        /// Returns a list of all categories
        /// </summary>
        /// <returns>A list of all categories</returns>
        public List<Category> GetCategoryList()
        {
            return budget.categories.List();
        }

        /// <summary>
        /// Deletes a category from the list
        /// </summary>
        /// <param name="id">The id of the category to delete</param>
        public void DeleteCategory(int id)
        {
            budget.categories.Delete(id);
        }

        /// <summary>
        /// Deletes an expense from the list
        /// </summary>
        /// <param name="id">The id of the expense to delete</param>
        public void DeleteExpense(int id)
        {
            budget.expenses.Delete(id);
        }
    }
}
