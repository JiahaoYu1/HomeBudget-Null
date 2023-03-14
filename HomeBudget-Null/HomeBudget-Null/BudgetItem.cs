using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{
    // ====================================================================
    // CLASS: BudgetItem
    //        A single budget item, includes Category and Expense
    // ====================================================================
    /// <summary>
    /// <h4>Represents a single budget item. Includes Category and Expense</h4>
    /// </summary>
    public class BudgetItem
    {
        /// <summary>
        /// The unique identification number of a Category
        /// </summary>
        public int CategoryID { get; set; }
        /// <summary>
        /// The unique identification number of an Expense 
        /// </summary>
        public int ExpenseID { get; set; }
        /// <summary>
        /// The date of the Budget Item
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// The name of the Category
        /// </summary>
        public String Category { get; set; }
        /// <summary>
        /// A short description describing the budget
        /// </summary>
        public String ShortDescription { get; set; }
        /// <summary>
        /// The amount of money the budget has
        /// </summary>
        public Double Amount { get; set; }
        /// <summary>
        /// The amount left over from income minus expenses
        /// </summary>
        public Double Balance { get; set; }

        internal BudgetItem() { }
    }


    /// <summary>
    /// <h4>Represents a list of all BudgetItems created within a specific month</h4>
    /// </summary>
    /// 
    public class BudgetItemsByMonth
    {
        /// <summary>
        /// The name of the month
        /// </summary>
        public String Month { get; set; }
        /// <summary>
        /// The list of BudgetItems for this specific month
        /// </summary>
        public List<BudgetItem> Details { get; set; }
        /// <summary>
        /// The total amount of money from each budget item
        /// </summary>
        public Double Total { get; set; }

        internal BudgetItemsByMonth() { }
    }

    /// <summary>
    /// <h4>Represents a list of all BudgetItems that share a specific Category</h4>
    /// </summary>
    public class BudgetItemsByCategory
    {
        /// <summary>
        /// The name of the Category
        /// </summary>
        public String Category { get; set; }
        /// <summary>
        /// The list of BudgetItems that all share the same Category
        /// </summary>
        public List<BudgetItem> Details { get; set; }
        /// <summary>
        /// The total amount of money from each budget item
        /// </summary>
        public Double Total { get; set; }

        internal BudgetItemsByCategory() { }
    }


}
