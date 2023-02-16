using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Data.SQLite;

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
        /// <summary>
        /// Backing field
        /// </summary>
        private static String DefaultFileName = "budgetCategories.txt";
        private List<Category> _Cats = new List<Category>();
        private string _FileName;
        private string _DirName;

        // ====================================================================
        // Properties
        // ====================================================================

        /// <summary>
        /// Readonly property where returns the name of the file from backing field where the datatype is string
        /// </summary>
        public String FileName { get { return _FileName; } }

        /// <summary>
        /// Readonly property where returns the name of the directory from backing field where the datatype is string
        /// </summary>
        public String DirName { get { return _DirName; } }

        // ====================================================================
        // Constructor
        // ====================================================================

        /// <summary>
        /// Creates a new instance of Categories and sets all category objects to a default value (see method 
        /// SetCategoriesToDefaults for more information)
        /// </summary>
        public Categories()
        {
            SetCategoriesToDefaults();
        }

        public Categories(SQLiteConnection connection, bool newDB)
        {
            using var cmd = new SQLiteCommand(connection);

            // Create the categoryType table
            cmd.CommandText = "CREATE TABLE categoryType (id INT PRIMARY KEY AUTOINCREMENT, Description VARCHAR(20))";
            cmd.ExecuteNonQuery();

 //           foreach(Category.CategoryType type in Category.CategoryType)
            {
 //               cmd.CommandText = string.Format("INSERT INTO categoryType VALUES ({0})", type.ToString());
  //              cmd.ExecuteNonQuery();
            }
            



            if (newDB)
            {
                SetCategoriesToDefaults();

                foreach(Category category in _Cats)
                {

                }
            }
            else
            {
                
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
            Category c = _Cats.Find(x => x.Id == i);
            if (c == null)
            {
                throw new Exception("Cannot find category with id " + i.ToString());
            }
            return c;
        }

        // ====================================================================
        // populate categories from a file
        // if filepath is not specified, read/save in AppData file
        // Throws System.IO.FileNotFoundException if file does not exist
        // Throws System.Exception if cannot read the file correctly (parsing XML)
        // ====================================================================

        /// <summary>
        /// Fill categories from a file, if no filepath is supplied, read/save in the AppData file.
        /// If the file does not exist, a System.IO.FileNotFoundException is thrown.
        /// If the file cannot be read correctly, a System.Exception will be thrown
        /// </summary>
        /// <param name="filepath">Represents the path of the file</param>
        /// <example>
        /// <b>To read categories from the respective files: </b>
        /// <code>
        /// <![CDATA[
        ///  _categories.ReadFromFile(folder + "\\" + filenames[0]);
        /// ]]>
        /// </code>
        /// </example>
        public void ReadFromFile(String filepath = null)
        {

            // ---------------------------------------------------------------
            // reading from file resets all the current categories,
            // ---------------------------------------------------------------
            _Cats.Clear();

            // ---------------------------------------------------------------
            // reset default dir/filename to null 
            // ... filepath may not be valid, 
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if it doesn't exist)
            // ---------------------------------------------------------------
            filepath = BudgetFiles.VerifyReadFromFileName(filepath, DefaultFileName);

            // ---------------------------------------------------------------
            // If file exists, read it
            // ---------------------------------------------------------------
            _ReadXMLFile(filepath);
            _DirName = Path.GetDirectoryName(filepath);
            _FileName = Path.GetFileName(filepath);
        }

        // ====================================================================
        // save to a file
        // if filepath is not specified, read/save in AppData file
        // ====================================================================
        /// <summary>
        /// Save to a file
        /// if filepath is not specified, read/save in AppData file
        /// </summary>
        /// <param name="filepath">Represents the path of the file</param>
        /// <example>
        /// <b>To save the categories into the own files: </b>
        /// <code>
        /// <![CDATA[
        /// _categories.SaveToFile(categorypath);
        /// ]]>
        /// </code>
        /// </example>
        public void SaveToFile(String filepath = null)
        {
            // ---------------------------------------------------------------
            // if file path not specified, set to last read file
            // ---------------------------------------------------------------
            if (filepath == null && DirName != null && FileName != null)
            {
                filepath = DirName + "\\" + FileName;
            }

            // ---------------------------------------------------------------
            // just in case filepath doesn't exist, reset path info
            // ---------------------------------------------------------------
            _DirName = null;
            _FileName = null;

            // ---------------------------------------------------------------
            // get filepath name (throws exception if it doesn't exist)
            // ---------------------------------------------------------------
            filepath = BudgetFiles.VerifyWriteToFileName(filepath, DefaultFileName);

            // ---------------------------------------------------------------
            // save as XML
            // ---------------------------------------------------------------
            _WriteXMLFile(filepath);

            // ----------------------------------------------------------------
            // save filename info for later use
            // ----------------------------------------------------------------
            _DirName = Path.GetDirectoryName(filepath);
            _FileName = Path.GetFileName(filepath);
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
            // ---------------------------------------------------------------
            // reset any current categories,
            // ---------------------------------------------------------------
            _Cats.Clear();

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

        // ====================================================================
        // Add category
        // ====================================================================
        /// <summary>
        /// Adds the category
        /// </summary>
        /// <param name="cat">A datatype of Category that represents the category</param>
        private void Add(Category cat)
        {
            _Cats.Add(cat);
        }

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
            int new_num = 1;
            if (_Cats.Count > 0)
            {
                new_num = (from c in _Cats select c.Id).Max();
                new_num++;
            }
            _Cats.Add(new Category(new_num, desc, type));
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
            if(_Cats.Exists(x => x.Id == Id))
            {
                int i = _Cats.FindIndex(x => x.Id == Id);
                _Cats.RemoveAt(i);
            }
            
        }

        public void UpdateProperties(int id, string newDesc, Category.CategoryType newType)
        {
            Category c = _Cats.Find(x => x.Id == id);
            if (c != null)
            {
         //       c.Description = newDesc;
          //      c.Type = newType;
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
            List<Category> newList = new List<Category>();
            foreach (Category category in _Cats)
            {
                newList.Add(new Category(category));
            }
            return newList;
        }

        // ====================================================================
        // read from an XML file and add categories to our categories list
        // ====================================================================
        private void _ReadXMLFile(String filepath)
        {

            // ---------------------------------------------------------------
            // read the categories from the xml file, and add to this instance
            // ---------------------------------------------------------------
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);

                foreach (XmlNode category in doc.DocumentElement.ChildNodes)
                {
                    String id = (((XmlElement)category).GetAttributeNode("ID")).InnerText;
                    String typestring = (((XmlElement)category).GetAttributeNode("type")).InnerText;
                    String desc = ((XmlElement)category).InnerText;

                    Category.CategoryType type;
                    switch (typestring.ToLower())
                    {
                        case "income":
                            type = Category.CategoryType.Income;
                            break;
                        case "expense":
                            type = Category.CategoryType.Expense;
                            break;
                        case "credit":
                            type = Category.CategoryType.Credit;
                            break;
                        default:
                            type = Category.CategoryType.Expense;
                            break;
                    }
                    this.Add(new Category(int.Parse(id), desc, type));
                }

            }
            catch (Exception e)
            {
                throw new Exception("ReadXMLFile: Reading XML " + e.Message);
            }

        }


        // ====================================================================
        // write all categories in our list to XML file
        // ====================================================================
        private void _WriteXMLFile(String filepath)
        {
            try
            {
                // create top level element of categories
                XmlDocument doc = new XmlDocument();
                doc.LoadXml("<Categories></Categories>");

                // foreach Category, create an new xml element
                foreach (Category cat in _Cats)
                {
                    XmlElement ele = doc.CreateElement("Category");
                    XmlAttribute attr = doc.CreateAttribute("ID");
                    attr.Value = cat.Id.ToString();
                    ele.SetAttributeNode(attr);
                    XmlAttribute type = doc.CreateAttribute("type");
                    type.Value = cat.Type.ToString();
                    ele.SetAttributeNode(type);

                    XmlText text = doc.CreateTextNode(cat.Description);
                    doc.DocumentElement.AppendChild(ele);
                    doc.DocumentElement.LastChild.AppendChild(text);

                }

                // write the xml to FilePath
                doc.Save(filepath);

            }
            catch (Exception e)
            {
                throw new Exception("_WriteXMLFile: Reading XML " + e.Message);
            }

        }

    }
}

