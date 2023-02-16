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
    // CLASS: Category
    //        - An individual category for budget program
    //        - Valid category types: Income, Expense, Credit, Saving
    // ====================================================================
    /// <summary>
    /// An individual category for budget program,
    /// valid category types: Income, Expense, Credit, Saving
    /// </summary>
    public class Category
    {
        // ====================================================================
        // Properties
        // ====================================================================

        /// <summary>
        /// 
        /// Takes in a unique int identifier, data type of int, for example category ID 
        /// and returns the value
        /// 
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Takes in a data type of String of description to the budget
        /// and returns the value
        /// </summary>
        
        public String Description { get; }

        /// <summary>
        /// Takes in a data type of CategoryType of Type which stores valid category types(Income, Expense, Credit and Savings)
        /// and returns the value
        /// </summary>
        public CategoryType Type { get; }

        /// <summary>
        /// Ways to define different types of category
        /// (Income, Expense, Credit and Savings) where we can use as a datatype for Type
        /// </summary>
        public enum CategoryType
        {
            /// <summary>
            /// Valid category type represents the money received
            /// </summary>
            Income,
            /// <summary>
            /// Valid category type represents the money received
            /// </summary>
            Expense,
            /// <summary>
            /// Valid category type represents the trust of a customer
            /// </summary>
            Credit,
            /// <summary>
            /// Valid category type represents the reduction in money
            /// </summary>
            Savings

        };

        // ====================================================================
        // Constructor
        // ====================================================================

        /// <summary>
        /// Constructor that takes in three parameters, where we set id, description and type into default values
        /// </summary>
        /// <param name="id">int datatype, unique identifier where it represents the budgets</param>
        /// <param name="description">String datatype, a description to the budget</param>
        /// <param name="type">CategoryType datatype, which stores valid category types(Income, Expense, Credit and Savings), 
        /// if no type provided, will be expense by default </param>
        public Category(int id, String description, CategoryType type = CategoryType.Expense)
        {
            Id = id;
            Description = description;
            Type = type;
        }

        // ====================================================================
        // Copy Constructor
        // ====================================================================
        /// <summary>
        /// Copy Constructor: Constructor that takes in one parameter, where we set id, description and type as the object under the parameter "category"
        /// </summary>
        /// <param name="category">datatype of Category, which is used to set id, description and type</param>
        public Category(Category category)
        {
            this.Id = category.Id;;
            this.Description = category.Description;
            this.Type = category.Type;
        }
        // ====================================================================
        // String version of object
        // ====================================================================
        /// <summary>
        /// Method that returns a string that represents the description
        /// </summary>
        /// <returns>Returns the Description</returns>
        /// <example>
        /// <b>To return a string that represents the description (group by year/month)</b>
        /// <code>
        /// <![CDATA[
        /// var GroupedByMonth = items.GroupBy(c => c.Date.Year.ToString("D4") + "/" + c.Date.Month.ToString("D2"));
        /// ]]>
        /// </code>
        /// </example>
        public override string ToString()
        {
            return Description;
        }

    }
}

