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
        //private static String DefaultFileName = "budget.txt";
        //private List<Expense> _Expenses = new List<Expense>();
        //private string? _FileName;
        //private string? _DirName;

        // ====================================================================
        // Properties
        // ====================================================================
        /// <summary>
        /// The name of the file to read/write to. Is <b>"budget.txt"</b> by default
        /// </summary>
        //public String FileName { get { return _FileName; } }
        /// <summary>
        /// The name of the directory that holds the file to read/write to
        /// </summary>
        //public String DirName { get { return _DirName; } }
        SQLiteConnection _connection;

        public Expenses()
        {
            _connection = Database.dbConnection;
        }

        // ====================================================================
        // populate categories from a file
        // if filepath is not specified, read/save in AppData file
        // Throws System.IO.FileNotFoundException if file does not exist
        // Throws System.Exception if cannot read the file correctly (parsing XML)
        // ====================================================================
        /// <summary>
        /// Clears all Expense objects and reads from a given file path. If the file is an XML file and is in correct format, the list will get filled with Expense objects
        /// with properties that the XML file defines
        /// 
        /// <para>
        /// For the example below, assume that <i>./expenses.exp</i> is an existing XML file that defines 10 Expense objects and <i>expenses</i> is an existing 
        /// Expenses object whose list length is 8:
            /// <example>
                /// <code>
                /// string filePath = "./expenses.exp";
                /// Console.WriteLine("List length before reading file: " + expenses.List().Count);
                /// 
                /// cats.ReadFromFile(filePath);
                /// // The list is now filled with new Expense objects
                /// Console.WriteLine("List length after reading file: " + expenses.List().Count);
                /// </code>
                /// Output:
                /// <code>
                /// List length before reading file: 8
                /// List length after reading file: 10
                /// </code>
            /// </example>
        /// </para>
        /// </summary>
        /// <param name="filepath">The file path to be read. If null, it will be "./budget.txt"</param>
        //public void ReadFromFile(String? filepath = null)
        //{

        //    // ---------------------------------------------------------------
        //    // reading from file resets all the current expenses,
        //    // so clear out any old definitions
        //    // ---------------------------------------------------------------
        //    _Expenses.Clear();

        //    // ---------------------------------------------------------------
        //    // reset default dir/filename to null 
        //    // ... filepath may not be valid, 
        //    // ---------------------------------------------------------------
        //    _DirName = null;
        //    _FileName = null;

        //    // ---------------------------------------------------------------
        //    // get filepath name (throws exception if it doesn't exist)
        //    // ---------------------------------------------------------------
        //    filepath = BudgetFiles.VerifyReadFromFileName(filepath, DefaultFileName);

        //    // ---------------------------------------------------------------
        //    // read the expenses from the xml file
        //    // ---------------------------------------------------------------
        //    _ReadXMLFile(filepath);

        //    // ----------------------------------------------------------------
        //    // save filename info for later use?
        //    // ----------------------------------------------------------------
        //    _DirName = Path.GetDirectoryName(filepath);
        //    _FileName = Path.GetFileName(filepath);


        //}

        // ====================================================================
        // save to a file
        // if filepath is not specified, read/save in AppData file
        // ====================================================================
        /// <summary>
        /// Saves all Expense objects to an XML document file. Sets the FileName property to the name of the file, and the Directory property to that file's directory
        /// 
        /// <para>
        /// For the example below, assume that <i>expenses</i> is an existing Expenses object:
        /// <example>
        /// <code>
        /// string filePath = "./expenses.exp";
        /// cats.SaveToFile(filePath);
        /// </code>
        /// In the local directory, a new file named <i>expenses.exp</i> will appear
        /// </example>
        /// </para>
        /// </summary>
        /// <param name="filepath">The path of the file to be saved to. If null, it will be a path that contains the Categories object's FileName and DirName properties</param>
        //public void SaveToFile(String? filepath = null)
        //{
        //    // ---------------------------------------------------------------
        //    // if file path not specified, set to last read file
        //    // ---------------------------------------------------------------
        //    if (filepath == null && DirName != null && FileName != null)
        //    {
        //        filepath = DirName + "\\" + FileName;
        //    }
        //    // ---------------------------------------------------------------
        //    // reset path info if filepath is null
        //    // ---------------------------------------------------------------
        //    if (filepath == null)
        //    {
        //        _DirName = null;
        //        _FileName = null;
        //        throw new ArgumentNullException(nameof(filepath));
        //    }

        //    // ---------------------------------------------------------------
        //    // create the output directory if it doesn't exist
        //    // ---------------------------------------------------------------
        //    string directoryPath = Path.GetDirectoryName(filepath);
        //    if (!Directory.Exists(directoryPath))
        //    {
        //        Directory.CreateDirectory(directoryPath);
        //    }

        //    // ---------------------------------------------------------------
        //    // get filepath name (throws exception if it doesn't exist)
        //    // ---------------------------------------------------------------
        //    filepath = BudgetFiles.VerifyWriteToFileName(filepath, DefaultFileName);

        //    // ---------------------------------------------------------------
        //    // create the output file if it doesn't exist
        //    // ---------------------------------------------------------------
        //    if (!File.Exists(filepath))
        //    {
        //        File.Create(filepath).Dispose();
        //    }

        //    // ---------------------------------------------------------------
        //    // save as XML
        //    // ---------------------------------------------------------------
        //    _WriteXMLFile(filepath);

        //    // ----------------------------------------------------------------
        //    // save filename info for later use
        //    // ----------------------------------------------------------------
        //    _DirName = Path.GetDirectoryName(filepath);
        //    _FileName = Path.GetFileName(filepath);


        //}



        // ====================================================================
        // Add expense
        // ====================================================================
        /// <summary>
        /// Adds a new Expense object to the list 
        /// <para>
        /// Below is an example of how to use this method
            /// <example>
                /// <code>
                /// int expenseId = 1;
                /// string expenseDesc = "sample description";
                /// DateTime expenseDate = DateTime.Now();
                /// int category = 1;
                /// double amount = 500;
                /// 
                /// Expenses expenses = new Expenses();
                /// Expense expense = new Expense(expenseId, expenseDate, category, amount, expenseDesc);
                /// 
                /// expenses.Add(expense);
                /// // A new Expense object has been added
                /// </code>
            /// </example>
        /// </para>
        /// </summary>
        /// 
        /// <param name="exp">The Expense object to be added</param>
        private void Add(Expense exp)
        {
            Add(exp.Date, exp.Category, exp.Amount, exp.Description);
        }

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
            //int new_id = 1;

            //// if we already have expenses, set ID to max
            //if (_Expenses.Count > 0)
            //{
            //    new_id = (from e in _Expenses select e.Id).Max();
            //    new_id++;
            //}

            //_Expenses.Add(new Expense(new_id, date, category, amount, description));

            using var cmd = new SQLiteCommand(_connection);
            cmd.CommandText = $"INSERT INTO expenses (CategoryId, Date, Description, Amount) VALUES ({category}, {date}, {description}, {amount})";
            cmd.ExecuteNonQuery();
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
            //if (_Expenses.Exists(x => x.Id == Id))
            //{
            //    int i = _Expenses.FindIndex(x => x.Id == Id);
            //    _Expenses.RemoveAt(i);
            //}

            using var cmd = new SQLiteCommand(_connection);
            cmd.CommandText = $"DELETE FROM expenses WHERE Id = {Id}";
            cmd.ExecuteNonQuery();
        }

        public void UpdateProperties(int id, int newCategory, DateTime newDate, string newDesc, double newAmount)
        {
            if (!IsCategoryTypeIdValid(newCategory))
                throw new ArgumentException($"new category ID must be less than {Categories.GetCategoryTypeArray().Length}", "newCategory");


            using var cmd = new SQLiteCommand(_connection);
            cmd.CommandText = $"UPDATE expenses SET CategoryId = {newCategory + 1}, Description = '{newDesc}', Date = {_ParseDateToSQLite(newDate.ToString())}, Amount = {newAmount}  WHERE Id = {id}";
            cmd.ExecuteNonQuery();
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
            //List<Expense> newList = new List<Expense>();
            //foreach (Expense expense in _Expenses)
            //{
            //    newList.Add(new Expense(expense));
            //}
            //return newList;
            List<Expense> tmpList = new List<Expense>();

            using var cmd = new SQLiteCommand(_connection);
            cmd.CommandText = $"SELECT Id, CategoryId, Date, Description, Amount FROM expenses";
            using SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                tmpList.Add(new Expense(reader.GetInt32(0), DateTime.Parse(reader.GetString(1)), reader.GetInt32(2), reader.GetDouble(3), reader.GetString(4)));
            }
            reader.Close();

            return tmpList;
        }

        private bool IsCategoryTypeIdValid(int typeId)
        {
            Category.CategoryType[] types = Categories.GetCategoryTypeArray();
            return typeId >= 0 && typeId < types.Length;
        }

        private DateTime _ParseDateToSQLite(string date)
        {
            return DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        // ====================================================================
        // read from an XML file and add categories to our categories list
        // ====================================================================
        private void _ReadXMLFile(String filepath)
        {


            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);

                // Loop over each Expense
                foreach (XmlNode expense in doc.DocumentElement.ChildNodes)
                {
                    // set default expense parameters
                    int id = int.Parse((((XmlElement)expense).GetAttributeNode("ID")).InnerText);
                    String description = "";
                    DateTime date = DateTime.Parse("2000-01-01");
                    int category = 0;
                    Double amount = 0.0;

                    // get expense parameters
                    foreach (XmlNode info in expense.ChildNodes)
                    {
                        switch (info.Name)
                        {
                            case "Date":
                                date = DateTime.Parse(info.InnerText);
                                break;
                            case "Amount":
                                amount = Double.Parse(info.InnerText);
                                break;
                            case "Description":
                                description = info.InnerText;
                                break;
                            case "Category":
                                category = int.Parse(info.InnerText);
                                break;
                        }
                    }

                    // have all info for expense, so create new one
                    this.Add(new Expense(id, date, category, amount, description));

                }

            }
            catch (Exception e)
            {
                throw new Exception("ReadFromFileException: Reading XML " + e.Message);
            }
        }


        // ====================================================================
        // write to an XML file
        // if filepath is not specified, read/save in AppData file
        // ====================================================================
        private void _WriteXMLFile(String filepath)
        {
            // ---------------------------------------------------------------
            // loop over all categories and write them out as XML
            // ---------------------------------------------------------------
            try
            {
                // create top level element of expenses
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<Expenses></Expenses>");

                // foreach Category, create an new xml element
                foreach (Expense exp in _Expenses)
                {
                    // main element 'Expense' with attribute ID
                    XmlElement ele = doc.CreateElement("Expense");
                    XmlAttribute attr = doc.CreateAttribute("ID");
                    attr.Value = exp.Id.ToString();
                    ele.SetAttributeNode(attr);
                    doc.DocumentElement.AppendChild(ele);

                    // child attributes (date, description, amount, category)
                    XmlElement d = doc.CreateElement("Date");
                    XmlText dText = doc.CreateTextNode(exp.Date.ToString("yyyy-MM-dd"));
                    ele.AppendChild(d);
                    d.AppendChild(dText);

                    XmlElement de = doc.CreateElement("Description");
                    XmlText deText = doc.CreateTextNode(exp.Description);
                    ele.AppendChild(de);
                    de.AppendChild(deText);

                    XmlElement a = doc.CreateElement("Amount");
                    XmlText aText = doc.CreateTextNode(exp.Amount.ToString());
                    ele.AppendChild(a);
                    a.AppendChild(aText);

                    XmlElement c = doc.CreateElement("Category");
                    XmlText cText = doc.CreateTextNode(exp.Category.ToString());
                    ele.AppendChild(c);
                    c.AppendChild(cText);

                }

                // write the xml to FilePath
                doc.Save(filepath);

            }
            catch (Exception e)
            {
                throw new Exception("SaveToFileException: Reading XML " + e.Message);
            }
        }

    }
}

