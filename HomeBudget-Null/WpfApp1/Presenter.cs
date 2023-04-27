using Budget;
using System;
using System.Collections.Generic;

namespace WpfApp1
{
    public class Presenter
    {
        private IExpense expenseView = null;
        private IHomeBudget homeBudgetView = null;
        private HomeBudget budget = null;
        private string dbFileName = null;

        /// <summary>
        /// Initializes a new instance of the Presenter class that uses a view that uses the IExpense interface
        /// </summary>
        /// <param name="newView">The IExpense view to use for displaying</param>
        public Presenter(IExpense newView, HomeBudget newBudget = null)
        {
            expenseView = newView;
            budget = newBudget;
        }

        /// <summary>
        /// Initializes a new instance of the Presenter class that uses a view that uses the IHomeBudget interface
        /// </summary>
        /// <param name="newView">The IHomeBudget view to use for displaying</param>
        public Presenter(IHomeBudget newView)
        {
            homeBudgetView = newView;
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
                        //expenseView.AddCategory(name, parsableType);
                }
                catch (Exception e)
                {
                    expenseView.DisplayError(e);
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
                }
                catch (Exception e)
                {
                    expenseView.DisplayError(e);
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
                expenseView.DisplayError(new Exception("A file must be created or loaded"));

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

        /// <summary>
        /// Updates an expenses details provided the new values
        /// </summary>
        /// <param name="expenseId">Id of expense to be updated</param>
        /// <param name="date">New date value</param>
        /// <param name="categoryId">New category value</param>
        /// <param name="amount">New amount for expense</param>
        /// <param name="description">Updated description of expense</param>
        public void UpdateExpense(int expenseId, DateTime date, int categoryId, double amount, string description)
        {
            budget.expenses.UpdateProperties(expenseId, date, categoryId, amount, description);
        }

        /// <summary>
        /// Retrieves Expenses based on date and or category type
        /// </summary>
        /// <param name="from">Expenses from a starting date</param>
        /// <param name="to">Expenses to an ending date</param>
        /// <param name="categoryId">The id of the category wanted</param>
        /// <returns>A list of BudgetItemsByCategory</returns>
        public List<BudgetItem> GetExpenseDateFilter(DateTime? from, DateTime? to, bool filterFlag, int categoryId)
        {
            return budget.GetBudgetItems(to, from, filterFlag, categoryId);
        }

        /// <summary>
        /// Retrieves Expenses based on a month given
        /// </summary>
        /// <param name="from">Expenses from a starting date</param>
        /// <param name="to">Expenses to an ending date</param>
        /// <param name="categoryId">The id of the category wanted</param>
        /// <returns>A list of BudgetItemsByMonth</returns>
        public List<BudgetItemsByMonth> GetExpensesByMonth(DateTime? from, DateTime? to, bool filterFlag, int categoryId)
        {
            return budget.GetBudgetItemsByMonth(to, from, filterFlag, categoryId);
        }

        public List<BudgetItemsByCategory> GetExpensesByCategory(DateTime? from, DateTime? to, bool filterFlag, int categoryId)
        {
            return budget.GetBudgetItemsByCategory(to, from, filterFlag, categoryId);
        }

        public List<Dictionary<string, object>> GetExpenseDictionaryByMonthAndCategory(DateTime? from, DateTime? to, bool filterFlag, int categoryId)
        {
            return budget.GetBudgetDictionaryByCategoryAndMonth(to, from, filterFlag, categoryId);
        }


        //Need documentation
        public Category GetCatergoryById(int id)
        {
            return budget.categories.GetCategoryFromId(id);
        }
        //Need documentation

        public Expense GetExpenseById(int id)
        {
            List<Expense> expenses = budget.expenses.List();
            
            foreach(Expense expense in expenses)
            {
                if (expense.Id == id)
                    return expense;
            }

            throw new Exception("No Id found");
        }
    }
}
