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
    // CLASS: Expense
    //        - An individual expens for budget program
    // ====================================================================
    /// <summary>
    /// <h4>Represents and individual expense for a budget program></h4>
    /// </summary>
    public class Expense
    {
        // ====================================================================
        // Properties
        // ====================================================================
        /// <summary>
        /// The identification number of the expense
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// The date that the expense was created
        /// </summary>
        public DateTime Date { get;  }
        /// <summary>
        /// The amount of money the expense uses
        /// </summary>
        public Double Amount { get; set; }
        /// <summary>
        /// The description of the expense
        /// </summary>
        public String Description { get; set; }
        /// <summary>
        /// The category Id of the expense
        /// </summary>
        public int Category { get; set; }

        // ====================================================================
        // Constructor
        //    NB: there is no verification the expense category exists in the
        //        categories object
        // ====================================================================
        /// <summary>
        /// Initalizes a new instance of the Expense class
        /// 
        /// <para>
        /// Below is an example of how to use this constructor:
        /// <example>
        /// <code>
        /// <![CDATA[
        /// int id = 1;
        /// DateTime date = new DateTime(2023, 1, 29);
        /// int category = 1;
        /// Double amount = 500;
        /// string description = "Sample description";
        /// 
        /// Expense expense = new Expense();
        /// Console.WriteLine("Id: {0}\nDate: {1}\nCategory: {2}\nAmount: {3}\nDescription: {4}", expense.Id, expense.Date, expense.Category, expense.Amount, expense.Description);
        /// ]]>
        /// </code>
        /// Output:
        /// <code>
        /// <![CDATA[
        /// Id: 1
        /// Date: 2023-01-29 12:00:00 AM
        /// Category: 1
        /// Amount: 500
        /// Description: Sample description
        /// ]]>
        /// </code>
        /// </example>
        /// </para>
        /// </summary>
        /// <param name="id">The identification number of the expense</param>
        /// <param name="date">The date that the expense was created</param>
        /// <param name="category">The category number of the expense</param>
        /// <param name="amount">The amount of money the expense uses</param>
        /// <param name="description">The description of the expense</param>
        public Expense(int id, DateTime date, int category, Double amount, String description)
        {
            this.Id = id;
            this.Date = date;
            this.Category = category;
            this.Amount = amount;
            this.Description = description;
        }

        // ====================================================================
        // Copy constructor - does a deep copy
        // ====================================================================
        /// <summary>
        /// Initalizes a clone of an instance of the Expense class 
        /// 
        /// <para>
        /// For the example below, assume that "expense1" is an already initalized expense object
            /// <example>
                /// <code>
                /// expense expense2 = new expense(expense1);
                /// 
                /// Console.WriteLine("expense1:\nId: {0}\nDate: {1}\nCategory: {2}\nAmount: {3}\nDescription: {4}", expense1.Id, expense1.Date, expense1.Category, expense1.Amount, expense1.Description);
                /// Console.WriteLine("\nexpense2:\nId: {0}\nDate: {1}\nCategory: {2}\nAmount: {3}\nDescription: {4}", expense2.Id, expense2.Date, expense2.Category, expense2.Amount, expense2.Description);
                /// </code>
            /// Output:
                /// <code>
                /// expense1:
                /// Id: 1
                /// Date: 2023-01-29 12:00:00 AM
                /// Category: 1
                /// Amount: 500
                /// Description: Sample description
                /// 
                /// expense2:
                /// Id: 1
                /// Date: 2023-01-29 12:00:00 AM
                /// Category: 1
                /// Amount: 500
                /// Description: Sample description
                /// </code>
            /// </example>
        /// </para>
        /// </summary>
        /// <param name="obj">The Expense object to clone</param>
        public Expense (Expense obj)
        {
            this.Id = obj.Id;
            this.Date = obj.Date;
            this.Category = obj.Category;
            this.Amount = obj.Amount;
            this.Description = obj.Description;
           
        }
    }
}
