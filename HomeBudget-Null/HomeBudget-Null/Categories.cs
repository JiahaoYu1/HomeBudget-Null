using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Data.SQLite;
using System.Drawing.Printing;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{
    // ====================================================================
    // CLASS: categories
    //        - A collection of category items,
    //        - Read / write to file
    //        - etc
    // ====================================================================
    /// <summary>
    /// A collection of category items, Read/write to file and etc
    /// </summary>
    public class Categories
    {
        SQLiteConnection _connection;

        // ====================================================================
        // Constructor
        // ====================================================================

        /// <summary>
        /// Creates a new instance of Categories and sets all category objects to a default value (see method 
        /// SetCategoriesToDefaults for more information)
        /// </summary>
        public Categories()
        {
            _connection = Database.dbConnection;
            _FillCategoryTypesTable();
            SetCategoriesToDefaults();
        }

        /// <summary>
        /// Initializes a new instance of the Categories class, as well as creating several SQLite tables 
        /// </summary>
        /// <param name="connection">An existing SQLite connection</param>
        /// <param name="newDB">Represents whether the Categories tables are going to use new (default) values or not</param>
        public Categories(SQLiteConnection connection, bool newDB)
        {
            
            if (connection is not null)
            {
                using var cmd = new SQLiteCommand(connection);
                Category.CategoryType[] types = (Category.CategoryType[])Enum.GetValues(typeof(Category.CategoryType));
                _connection = connection;
                _FillCategoryTypesTable();
                if (newDB)
                {
                    SetCategoriesToDefaults();
                }
            }

        }

        // ====================================================================
        // get a specific category from the list where the id is the one specified
        // ====================================================================

        /// <summary>
        /// Second constructor where takes in a parameter of int.
        /// Declares a value called c with a datatype of Category.
        /// Loops through all the ids inside the categories and finds the id where equals to i and returns it if c is null throw a new exception
        /// </summary>
        /// <param name="i">the unique identifier of the category</param>
        /// <returns>returns the category where the parameter is equal to the id of the category</returns>
        /// <exception cref="Exception">Throw an exception if the category is null</exception>
        /// <example>
        /// <b>To get the category from id: </b>
        /// <code>
        /// <![CDATA[
        /// GetCategoryFromId(i);
        /// ]]>
        /// </code>
        /// </example>
        public Category GetCategoryFromId(int i)
        {
            Category.CategoryType[] types = GetCategoryTypeArray();
            Category categoryGot = null;
            try
            {
                using var cmd = new SQLiteCommand(_connection);
                cmd.CommandText = $@"SELECT Id, Description, TypeId FROM categories WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", i);
                using SQLiteDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int id = reader.GetInt32(0);

                    if (id == i)
                    {
                        categoryGot = new Category(id, reader.GetString(1), types[reader.GetInt32(2) - 1]);
                    }
                }
                reader.Close();

                if (categoryGot is null)
                    throw new Exception("Cannot find category with id " + i.ToString());

                return categoryGot;
            }
            catch(Exception e)
            {
                throw new Exception("Error getting category from id: "+e.Message);
            }
        }

        // ====================================================================
        // set categories to default
        // ====================================================================
        /// <summary>
        /// set categories to default
        /// </summary>
        /// <example>
        /// <b>To set categories to default: </b>
        /// <code>
        /// <![CDATA[
        /// SetCategoriesToDefaults();
        /// ]]>
        /// </code>
        /// </example>
        public void SetCategoriesToDefaults()
        {
            try
            {
                // ---------------------------------------------------------------
                // reset any current categories,
                // ---------------------------------------------------------------
                using var cmd = new SQLiteCommand(_connection);
                cmd.CommandText = $"DELETE FROM Categories";
                cmd.ExecuteNonQuery();
                // ---------------------------------------------------------------
                // Add Defaults
                // ---------------------------------------------------------------
                Add("Utilities", Category.CategoryType.Expense);
                Add("Rent", Category.CategoryType.Expense);
                Add("Food", Category.CategoryType.Expense);
                Add("Entertainment", Category.CategoryType.Expense);
                Add("Education", Category.CategoryType.Expense);
                Add("Miscellaneous", Category.CategoryType.Expense);
                Add("Medical Expenses", Category.CategoryType.Expense);
                Add("Vacation", Category.CategoryType.Expense);
                Add("Credit Card", Category.CategoryType.Credit);
                Add("Clothes", Category.CategoryType.Expense);
                Add("Gifts", Category.CategoryType.Expense);
                Add("Insurance", Category.CategoryType.Expense);
                Add("Transportation", Category.CategoryType.Expense);
                Add("Eating Out", Category.CategoryType.Expense);
                Add("Savings", Category.CategoryType.Savings);
                Add("Income", Category.CategoryType.Income);
            }
            catch (Exception e)
            {
                throw new Exception("Error setting categories to default values: " + e.Message);
            }

        }

        // ====================================================================
        // Add category
        // ====================================================================

        /// <summary>
        /// Adds the category
        /// if the number of category from the backing field is less than zero, select the max id in category and increment it once a time
        /// </summary>
        /// <param name="desc">The name of the stuff</param>
        /// <param name="type">Type of the category</param>
        /// <example>
        /// <b>To add default:  </b>
        /// <code>
        /// <![CDATA[
        /// Add("Utilities", Category.CategoryType.Expense);
        /// ]]>
        /// </code>
        /// </example>
        public void Add(String desc, Category.CategoryType type)
        {
            try
            {
                _FillCategoryTypesTable();

                using var cmd = new SQLiteCommand(_connection);
                cmd.CommandText = "INSERT INTO categories (Description, TypeId) VALUES (@desc, @typeId)";
                cmd.Parameters.AddWithValue("@desc", desc);
                cmd.Parameters.AddWithValue("@typeId", _GetCategoryTypeId(type));
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("Error adding category: "+e.Message);
            }
        }

        // ====================================================================
        // Delete category
        // ====================================================================
        /// <summary>
        /// Deletes the category,  
        /// the method will find the index inside id and remove the index category
        /// </summary>
        /// <param name="Id">The unique identity of the category</param>
        /// <example>
        /// <b>To delete the index category:</b>
        /// <code>
        /// <![CDATA[
        /// Delete(id);
        /// ]]>
        /// </code>
        /// </example>
        public void Delete(int Id)
        {
            try
            {
                using var cmd = new SQLiteCommand(_connection);
                cmd.CommandText = "DELETE FROM categories WHERE Id = @id";
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                throw new Exception("Error deleting category: "+e.Message);
            }
        }

        public void UpdateProperties(int id, string newDesc, Category.CategoryType newType)
        {
            try
            {
                int typeId = (int)newType + 1;
                using var cmd = new SQLiteCommand(_connection);
                cmd.CommandText = "UPDATE categories SET Description = @newDesc, TypeId = @typeId WHERE Id = @id";
                cmd.Parameters.AddWithValue("@newDesc", newDesc);
                cmd.Parameters.AddWithValue("@typeId", typeId);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch(Exception e )
            {
                throw new Exception("Error updating category properties: "+e.Message);
            }
        }

        // ====================================================================
        // Return list of categories
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================
        /// <summary>
        /// Returns the list of categories
        /// the method will make new copy of list, so user cannot modify what is part of this instance
        /// </summary>
        /// <returns>Returns a new list of categories</returns>
        /// <example>
        /// <b>To use the list method: </b>
        /// <code>
        /// <![CDATA[
        /// Categories categories = new Categories();
        /// categories.Add(DateTime.Now, (int) Category.CategoryType.categories,
        /// 23.45, "textbook" );
        /// List<categories> list = categories.List();
        /// foreach (Categories categories in list)
        /// Console.WriteLine(categories.Description);
        /// ]]>
        /// </code>
        /// </example>
        public List<Category> List()
        {
            Category.CategoryType[] types = GetCategoryTypeArray();
            List<Category> newList = new List<Category>();

            using var cmd = new SQLiteCommand(_connection);
            cmd.CommandText = "SELECT * FROM categories";
            using SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                newList.Add(new Category(reader.GetInt32(0), reader.GetString(1), types[reader.GetInt32(2) - 1]));
            }
            reader.Close();
            
            return newList;
        }

        // ====================================================================
        // Get the categoryType enum values as an array
        // ====================================================================
        public static Category.CategoryType[] GetCategoryTypeArray()
        {
            return (Category.CategoryType[])Enum.GetValues(typeof(Category.CategoryType));
        }

        // ====================================================================
        // Fill the category types table if it's empty
        // ====================================================================
        private void _FillCategoryTypesTable()
        {
            Category.CategoryType[] types = GetCategoryTypeArray();

            using var queryCmd = new SQLiteCommand(_connection);
            queryCmd.CommandText = $"SELECT id FROM categoryTypes";
            using SQLiteDataReader reader = queryCmd.ExecuteReader();


            if (!reader.HasRows)
            {
                using var insertCmd = new SQLiteCommand(_connection);

                foreach (Category.CategoryType type in types)
                {
                    insertCmd.CommandText = $"INSERT INTO categoryTypes (Id, Description) VALUES ({_GetCategoryTypeId(type)}, '{type}')";
                    insertCmd.ExecuteNonQuery();
                }
            }
        }

        // ====================================================================
        // Get a categoryType enum Id
        // ====================================================================
        private int _GetCategoryTypeId(Category.CategoryType type)
        {
            Category.CategoryType[] types = (Category.CategoryType[])Enum.GetValues(typeof(Category.CategoryType));

            for (int i = 0; i < types.Length; i++)
            {
                if (types[i] == type)
                    return i + 1;
            }

            return -1;
        }

    }
}

