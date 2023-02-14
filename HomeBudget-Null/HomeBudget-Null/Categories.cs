using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

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
    /// <h4>Represents a collection of category items</h4>
    /// </summary>
    public class Categories
    {
        private static String DefaultFileName = "budgetCategories.txt";
        private List<Category> _Cats = new List<Category>();
        private string _FileName;
        private string _DirName;

        // ====================================================================
        // Properties
        // ====================================================================
        /// <summary>
        /// The name of the file to read/write to. Is <b>"budgetCategories.txt"</b> by default
        /// </summary>
        public String FileName { get { return _FileName; } }
        /// <summary>
        /// The name of the directory that holds the file to read/write to
        /// </summary>
        public String DirName { get { return _DirName; } }

        // ====================================================================
        // Constructor
        // ====================================================================
        /// <summary>
        /// Initializes a new instance of the Categories class, with every Category object being set to default values
        /// 
        /// <example>
        /// <para>Below is an example of how to use this constructor:</para>
        /// 
        /// <code>
        /// Categories cats = new Categories();
        /// </code>
        /// </example>
        /// </summary>
        public Categories()
        {
            SetCategoriesToDefaults();
        }

        // ====================================================================
        // get a specific category from the list where the id is the one specified
        // ====================================================================
        /// <summary>
        /// Returns a Category object whose id is the same as the argument
        /// 
        /// <para>
        ///     For the example below, assume there exists a Category object in the list with its Id set to 1:
            /// <example>
                /// <code>
                /// int idToFind = 1;
                /// 
                /// Categories cats = new Categories();
                /// Category categoryWithIdOfOne = cats.GetCategoryFromId(idToFind);
                /// 
                /// // Print the id to the console
                /// Console.WriteLine(categoryWithIdOfOne.Id);
                /// </code>
            /// Output:
                /// <code>
                /// 1
                /// </code>
            /// </example>
        /// </para>
        /// </summary>
        /// 
        /// <param name="i">The id to be found</param>
        /// <returns>A Category object with the same id as the argument</returns>
        /// <exception cref="Exception">Throws when no Category with the given id was found</exception>
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
        /// Resets all Categories and reads from a given file path. If the file is an XML file and is in correct format, the list will get filled with Category objects
        /// with properties that the XML file defines
        /// 
        /// <para>
        /// For the example below, assume that <i>./categories.cats</i> is an existing XML file that defines 10 Category objects and <i>cats</i> is an existing 
        /// Categories object whose list length is 8:
            /// <example>
                /// <code>
                /// string filePath = "./categories.cats";
                /// Console.WriteLine("List length before reading file: " + cats.List().Count);
                /// 
                /// cats.ReadFromFile(filePath);
                /// // The list is now filled with new Category objects
                /// Console.WriteLine("List length after reading file: " + cats.List().Count);
                /// </code>
            /// Output:
                /// <code>
                /// List length before reading file: 8
                /// List length after reading file: 10
                /// </code>
            /// </example>
        /// </para>
        /// </summary>
        /// <param name="filepath">The file path to be read. If null, it will be "./budgetCategories.txt"</param>
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
        /// Saves all Category objects to an XML document file. Sets the FileName property to the name of the file, and the Directory property to that file's directory
        /// 
        /// <para>
        /// For the example below, assume that <i>cats</i> is an existing Categories object:
            /// <example>
                /// <code>
                /// string filePath = "./categories.cats";
                /// cats.SaveToFile(filePath);
                /// </code>
            /// In the local directory, a new file named <i>categories.cats</i> will appear
            /// </example>
        /// </para>
        /// </summary>
        /// <param name="filepath">The path of the file to be saved to. If null, it will be a path that contains the Categories object's FileName and DirName properties</param>
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
        /// Sets all categories to their default values
        /// 
        /// <para>
        /// Below is an example of how to use this method: 
            /// <example>
                /// <code>
                /// Categories cats = new Categories();
                /// cats.SetCategoriesToDefaults();
                /// </code>
            /// Run through the <i>cats</i> object's list and print all of the Category descriptions:
                /// <code>
                /// foreach(Category cat in cats)
                ///     Console.WriteLine(cat.Description);
                /// </code>
            /// Output:
                /// <code>
                /// Utilities
                /// Rent
                /// Food
                /// Entertainment
                /// Education
                /// Miscellaneous
                /// Medical Expenses
                /// Vacation
                /// Credit Card
                /// ...
                /// </code>
            /// </example>
        /// </para>
        /// </summary>
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
        /// Adds a new Category object to the list 
        /// <para>
        /// Below is an example of how to use this method
            /// <example>
                /// <code>
                /// int categoryId = 1;
                /// string categoryDesc = "sample description";
                /// CategoryType categoryType = CategoryType.Expense;
                /// 
                /// Categories cats = new Categories();
                /// Category cat = new Category(categoryId, categoryDesc, categoryType);
                /// 
                /// cats.Add(cat);
                /// // A new Category object has been added
                /// </code>
            /// </example>
        /// </para>
        /// </summary>
        /// 
        /// <param name="cat">The Category object to be added</param>
        private void Add(Category cat)
        {
            _Cats.Add(cat);
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
        /// <param name="desc">The description of the new Category</param>
        /// <param name="type">The type of the new Category</param>
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
        /// Deletes a Category object of a specific Id from the list 
        /// 
        /// <para>
        /// Below is an example of how to use this method:
            /// <example>
                /// <code>
                /// Categories cats = new Categories();
                /// int idToDelete = 4;
                /// 
                /// cats.Delete(idToDelete);
                /// </code>
            /// </example>
        /// </para>
        /// </summary>
        /// <param name="Id">The id of the Category to remove</param>
        public void Delete(int Id)
        {
            if (_Cats.Exists(x => x.Id == Id))
            {
                int i = _Cats.FindIndex(x => x.Id == Id);
                _Cats.RemoveAt(i);
            } 
        }

        // ====================================================================
        // Return list of categories
        // Note:  make new copy of list, so user cannot modify what is part of
        //        this instance
        // ====================================================================
        /// <summary>
        /// Returns a list of Categories
        /// 
        /// <para>
        /// Below is an example of how to use this method:
            /// <example>
                /// <code>
                /// <![CDATA[
                /// Categories cats = new Categories();
                /// List<Category> listOfCategories = cats.List();
                /// 
                /// // Print all the descriptions of the objects to the console
                /// foreach(Category cat in listOfCategories)
                ///     Console.WriteLine(cat.Description);
                /// ]]>
                /// </code>
            /// Output:
            /// <code>
                /// Utilities
                /// Rent
                /// Food
                /// Entertainment
                /// Education
                /// Miscellaneous
                /// Medical Expenses
                /// Vacation
                /// Credit Card
                /// ...
            /// </code>
            /// </example>
        /// </para>
        /// </summary>
        /// <returns>A list of Category objects</returns>
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
                        case "savings":
                            type = Category.CategoryType.Savings;
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

