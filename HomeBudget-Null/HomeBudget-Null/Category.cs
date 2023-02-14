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
    /// <h4>Represents an individual category for a budget program</h4>
    /// 
    /// </summary>
    public class Category
    {
        // ====================================================================
        // Properties
        // ====================================================================
        /// <summary>
        /// The identification number of the Category
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The description of the Category
        /// </summary>
        public String Description { get; set; }
        /// <summary>
        /// The Category's budget type
        /// </summary>
        public CategoryType Type { get; set; }
        /// <summary>
        /// <h4>The type of budget in an account</h4>
        /// </summary>
        public enum CategoryType
        {
            /// <summary>Represents all that is gained</summary>
            Income,
            /// <summary>Represents all that is spent</summary>
            Expense,
            /// <summary>Represents all that is spent through credit</summary>
            Credit,
            /// <summary>Represents all that is being saved for a specific occasion</summary>
            Savings
        };

        // ====================================================================
        // Constructor
        // ====================================================================
        /// <summary>
        /// Initializes a new instance of the Category class with all properties filled in
        /// 
        /// <para>
            /// <example>
            /// Below is an example of how to use this constructor
            /// 
                /// <code>
                /// <![CDATA[
                /// int id = 1;
                /// string description = "This is the income category";
                /// CategoryType type = CategoryType.Income;
                /// 
                /// Category category = new Category(id, description, type);
                /// ]]>
                /// </code>
            /// Print the properties of the newly made Category object to the console:
                /// <code>
                /// <![CDATA[
                /// Console.WriteLine("Id: {0}\nDescription: {1}\nType: {2}", category.Id, category.Description, category.Type);
                /// ]]>
                /// </code>
            /// 
            /// Output:
                /// <code>
                /// Id: 1
                /// Description: This is the income category
                /// Type: Income
                /// </code>
            /// </example>
        /// </para>
        /// </summary>
        /// <param name="id">The identification number of the Category object</param>
        /// <param name="description">Tthe description of the Category object</param>
        /// <param name="type">The type of category the object will be. Is <b>CategoryType.Expense</b> by default</param>
        public Category(int id, String description, CategoryType type = CategoryType.Expense)
        {
            this.Id = id;
            this.Description = description;
            this.Type = type;
        }

        // ====================================================================
        // Copy Constructor
        // ====================================================================
        /// <summary>
        /// Initializes an instance of Category that has the same properties as another. Requires an existing Category object
        /// 
        /// <para>
            /// <example>
            /// For the example below, assume <i>category1</i> is an existing instance of the Category class with 1 as its Id, "Sample description" as its description, 
            /// and CategoryType.Expense as its Type
            ///         
                /// <code>
                /// <![CDATA[Category category2 = new Category(category1);]]>
                /// </code>
            /// Print the properties of the newly made Category object to the console:
                /// <code>
                /// <![CDATA[
                /// Console.WriteLine("Id: {0}\nDescription: {1}\nType: {2}", category.Id, category.Description, category.Type);
                /// ]]>
                /// </code>
            /// 
            /// Output:
                /// <code>
                /// Id: 1
                /// Description: Sample description
                /// Type: Expense
                /// </code>
            /// </example>
        /// </para>
        /// 
        /// </summary>
        /// <param name="category">Category object to be cloned</param>

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
        /// Returns the Description of the object
        /// 
        /// <para>
            /// <example>
            /// For the example below, assume <i>category</i> is an existing instance of the Category class with "Sample description" as the value of its Description property
            /// <code>
            /// <![CDATA[
            /// string desc = category.ToString();
            /// Console.WriteLine(desc);
            /// ]]>
            /// </code>
            /// Output:
            /// <code>
            /// Sample description
            /// </code>
            /// </example>
        /// </para>
        /// </summary>
        /// <returns>The Description property of the Category object</returns>
        public override string ToString()
        {
            return Description;
        }

    }
}

