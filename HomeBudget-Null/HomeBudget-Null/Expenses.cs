using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Data.Common;
using System.Data.SQLite;
using System.Globalization;
using System.Runtime.Intrinsics.Arm;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{
    // ====================================================================
    // CLASS: expenses
    //        - A collection of expense items,
    //        - Read / write to file
    //        - etc
    // ====================================================================
    /// <summary>
    /// <h4>Represents a list of expense items</h4>
    /// </summary>
    public class Expenses
    {
        SQLiteConnection _connection;

        internal Expenses()
        {
            _connection = Database.dbConnection;
        }
        internal Expenses(SQLiteConnection connection_)
        {
            _connection = connection_;
        }

        // ====================================================================
        // Add expense
        // ====================================================================

        /// <summary>
        /// Creates and adds a new Category object to the list
        /// 
        /// <para>
        /// Below is an example of how to use this method:
        /// <example>
        /// <code>
        /// string categoryDesc = "sample description";
        /// CategoryType categoryType = CategoryType.Expense;
        /// Categories cats = new Categories();
        /// 
        /// cats.Add(categoryDesc, categoryType);
        /// // A new Category object has been added
        /// </code>
        /// </example>
        /// </para>
        /// </summary>
        ///  <param name="date">The date that the expense was created</param>
        ///  <param name="category">The category number of the expense</param>
        ///  <param name="amount">The amount of money the expense uses</param>
        ///  <param name="description">The description of the expense</param>
        public void Add(DateTime date, int category, Double amount, String description)
        {
            try
            {
                using var cmd = new SQLiteCommand(_connection);
                cmd.CommandText = $@"INSERT INTO expenses (CategoryId, Date, Description, Amount) VALUES (@category, @date, @description, @amount)";

                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@amount", amount);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding expense: "+ ex.Message);
            }
        }

        // ====================================================================
        // Delete expense
        // ====================================================================
        /// <summary>
        /// Deletes a Category object of a specific Id from the list 
        /// 
        /// <para>
        /// Below is an example of how to use this method:
            /// <example>
                /// <code>
                /// Expenses expenses = new Expenses();
                /// int idToDelete = 4;
                /// 
                /// expenses.Delete(idToDelete);
                /// // The Expense with the id of 4 is no longer in the list
                /// </code>
            /// </example>
        /// </para>
        /// </summary>
        /// <param name="Id">The id of the Expense to remove</param>
        public void Delete(int Id)
        {
            try
            {
                using var cmd = new SQLiteCommand(_connection);
                cmd.CommandText = $@"DELETE FROM expenses WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting expense: "+ ex.Message);
            }
        }

        public void UpdateProperties(int id, int newCategory, DateTime newDate, string newDesc, double newAmount)
        {
            if (!IsCategoryTypeIdValid(newCategory))
                throw new ArgumentException($"new category ID must be less than {Categories.GetCategoryTypeArray().Length}", "newCategory");

            try
            {
                using var cmd = new SQLiteCommand(_connection);
                cmd.CommandText = $@"UPDATE expenses SET CategoryId = @newCategory, Description = @newDesc, Date = @newDate, Amount = @newAmount WHERE Id = @id";
                cmd.Parameters.AddWithValue("@newCategory", newCategory + 1);
                cmd.Parameters.AddWithValue("@newDesc", newDesc);
                cmd.Parameters.AddWithValue("@newDate", Database.ParseDateToSQLite(newDate));
                cmd.Parameters.AddWithValue("@newAmount", newAmount);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            { 
                throw new Exception("Error Updating Properties for expense: "+ex.Message);
            }
        }

        // ====================================================================
        // Return list of expenses
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================
        /// <summary>
        /// Returns a list of Expense objects 
        /// 
        /// <para>
            /// <example>
            /// For the example below, assume <i>expenses</i> is an existing Expenses object, and that the expense list contains the following:
                /// <code>
                /// Id     Date                     Amount      Description         Category
                /// 1       1/14/2023 12:00:00 AM   400         Entertainment       1
                /// 2       1/8/2023 12:00:00 AM    20          Medical Expenses    1
                /// 3       1/29/2023 12:00:00 AM   1400        Vacation            1
                /// </code>
            /// Get a list of expenses
                /// <code>
                /// <![CDATA[
                /// List<Expense> expenseList = expenses.List();
                /// 
                /// foreach(Expense expense in expenseList)
                ///     Console.WriteLine(expense.Description);
                /// ]]>
                /// </code>
            /// Output:
            /// <code>
            /// Entertainment
            /// Medical Expenses
            /// Vacation
            /// </code>
            /// </example>
        /// </para>
        /// </summary>
        /// <returns>A list of Expense objects</returns>
        public List<Expense> List()
        {
            List<Expense> tmpList = new List<Expense>();

            using var cmd = new SQLiteCommand(_connection);
            cmd.CommandText = $"SELECT Id, Date, Amount, Description, CategoryId FROM expenses";
            using SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                //string dateNew = reader.GetString(1);
                tmpList.Add(new Expense(reader.GetInt32(0), DateTime.ParseExact(reader.GetString(1), "yyyy-MM-dd", CultureInfo.InvariantCulture), reader.GetInt32(4), reader.GetDouble(2), reader.GetString(3)));
            }
            reader.Close();
            return tmpList;
        }

        private bool IsCategoryTypeIdValid(int typeId)
        {
            Category.CategoryType[] types = Categories.GetCategoryTypeArray();
            return typeId >= 0 && typeId < types.Length;
        }

    }
}

