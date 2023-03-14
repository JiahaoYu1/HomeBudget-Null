using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Dynamic;
using System.Data.SQLite;
using System.Globalization;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleToAttribute("HomeBudgetTest")]

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================


namespace Budget
{
    // ====================================================================
    // CLASS: HomeBudget
    //        - Combines categories Class and expenses Class
    //        - One File defines Category and Budget File
    //        - etc
    // ====================================================================
    /// <summary>
    /// <h4>Represents a combination between the Categories Class and Expenses Class. One file defines Categories and Budgets.</h4>
    /// <para><h5>
    /// See also:
    /// <see href="./Budget.Categories.html">Categories Class,</see>
    /// <see href="./Budget.Expenses.html">Expenses Class</see>
    /// </h5></para><br/>
        /// <example>
        /// Below is an example of the usage of this class (assuming <i>samplefile.budget</i> exists):
            /// <code>
            /// <![CDATA[
            /// HomeBudget hb = new HomeBudget();
            /// string filepath = "./samplefile.budget";
            /// 
            /// hb.ReadFromFile(filepath);
            /// List<BudgetItem> budgetItems = hb.GetBudgetItems(null, null, false, 0);
            /// List<BudgetItemsByMonth> budgetMonthItems = hb.GetBudgetItemsByMonth(null, null, false, 0);
            /// ]]>
            /// </code>
        /// </example>
    /// </summary>
    internal class HomeBudget
    {
        //private string _FileName;
        //private string _DirName;
        private Categories _categories;
        private Expenses _expenses;
        private SQLiteConnection _connection;

        // Properties (categories and expenses object)
        /// <summary>
        /// The list of categories
        /// </summary>
        public Categories categories { get { return _categories; } }
        /// <summary>
        /// The list of expenses
        /// </summary>
        public Expenses expenses { get { return _expenses; } }

        // -------------------------------------------------------------------
        // Constructor (new... default categories, no expenses)
        // -------------------------------------------------------------------
        /// <summary>
        /// Initalizes a new HomeBudget object with default categories and no expenses
        /// 
        /// <para>
        /// Below is an example of how to use this constructor:
            /// <example>
                /// <code>
                /// HomeBudget hb = new HomeBudget();
                /// </code>
            /// </example>
        /// </para>
        /// </summary>
        public HomeBudget()
        {
            _categories = new Categories();
            _expenses = new Expenses();
        }

        // -------------------------------------------------------------------
        // Constructor (existing budget ... must specify file)
        // -------------------------------------------------------------------
        /// <summary>
        /// Initalizes a new HomeBudget object with existing budget extracted from a file
        /// 
        /// <para>
        /// For the example below, assume that <i>budgets.budget</i> is an existing file:
            /// <example>
                /// <code>
                /// string budgetFileName = "./budgets.budget";
                /// HomeBudget hb = new HomeBudget(budgetFileName);
                /// </code>
            /// </example>
        /// </para>
        /// </summary>
        /// <param name="budgetFileName">The file to get the budget from</param>
        public HomeBudget(String budgetFileName)
        {
            _categories = new Categories();
            _expenses = new Expenses();
            //ReadFromFile(budgetFileName);
        }

        public HomeBudget(string dbFile, string budgetFile, bool dbExists)
        {
            if (dbExists)
                Database.existingDatabase(dbFile);
            else
                Database.newDatabase(dbFile);
            _connection = Database.dbConnection;

            _categories= new Categories();
            _expenses = new Expenses();
        }

        #region OpenNewAndSave
        // ---------------------------------------------------------------
        // Read
        // Throws Exception if any problem reading this file
        // ---------------------------------------------------------------
        /// <summary>
        /// Reads from a budget file, extracts expenses and categories from it, and saves the file path
        /// 
        /// <para>
        /// <example>
        /// For the example below, assume the budget file contains the
        /// following elements:
        /// <code>
        /// Category ID     Expense ID      Date                    Description                 Cost
        /// 10              1               1/10/2018 12:00:00 AM   Clothes hat (on credit)     10
        /// 9               2               1/11/2018 12:00:00 AM   Credit Card hat             -10
        /// 10              3               1/10/2019 12:00:00 AM   Clothes scarf(on credit)    15
        /// 9               4               1/10/2020 12:00:00 AM   Credit Card scarf           -15
        /// </code>
        /// Getting a list of ALL budget items:
        /// <code>
        /// <![CDATA[
        /// HomeBudget budget = new HomeBudget();
        /// budget.ReadFromFile(filename);
        ///
        /// // Get a list of all budget items
        /// var budgetItems = budget.GetBudgetItems(null, null, false, 0);
        ///
        /// // print important information
        /// foreach (var bi in budgetItems)
        /// {
        ///     Console.WriteLine (
        ///         String.Format("{0} {1,-20} {2,8:C} {3,12:C}",
        ///             bi.Date.ToString("yyyy/MMM/dd"),
        ///             bi.ShortDescription,
        ///             bi.Amount, bi.Balance)
        ///      );
        /// }
        /// ]]>
        /// </code>
        /// Output:
        /// <code>
        /// 2018/Jan/10 hat (on credit)         ($10.00) ($10.00)
        /// 2018/Jan/11 hat                     $10.00 $0.00
        /// 2019/Jan/10 scarf(on credit)        ($15.00) ($15.00)
        /// 2020/Jan/10 scarf                   $15.00 $0.00
        /// </code>
        /// </example>
        /// </para>
        /// </summary>
        /// <param name="budgetFileName">The name of the budget file to read from</param>
        /// <exception cref="Exception">Throws when any error with the reading process occurs</exception>
        //public void ReadFromFile(String budgetFileName)
        //{
        //    // ---------------------------------------------------------------
        //    // read the budget file and process
        //    // ---------------------------------------------------------------
        //    try
        //    {
        //        // get filepath name (throws exception if it doesn't exist)
        //        budgetFileName = BudgetFiles.VerifyReadFromFileName(budgetFileName, "");

        //        // If file exists, read it
        //        string[] filenames = System.IO.File.ReadAllLines(budgetFileName);

        //        // ----------------------------------------------------------------
        //        // Save information about budget file
        //        // ----------------------------------------------------------------
        //        string folder = Path.GetDirectoryName(budgetFileName);
        //        _FileName = Path.GetFileName(budgetFileName);

        //        // read the expenses and categories from their respective files
        //        _categories.ReadFromFile(folder + "\\" + filenames[0]);
        //        _expenses.ReadFromFile(folder + "\\" + filenames[1]);

        //        // Save information about budget file
        //        _DirName = Path.GetDirectoryName(budgetFileName);
        //        _FileName = Path.GetFileName(budgetFileName);

        //    }

        //    // ----------------------------------------------------------------
        //    // throw new exception if we cannot get the info that we need
        //    // ----------------------------------------------------------------
        //    catch (Exception e)
        //    {
        //        throw new Exception("Could not read budget info: \n" + e.Message);
        //    }

        //}

        // ====================================================================
        // save to a file
        // saves the following files:
        //  filepath_expenses.exps  # expenses file
        //  filepath_categories.cats # categories files
        //  filepath # a file containing the names of the expenses and categories files.
        //  Throws exception if we cannot write to that file (ex: invalid dir, wrong permissions)
        // ====================================================================
        /// <summary>
        /// Saves all categories and expenses to files
        /// <para> 
        /// <example>
        /// For the example below, assume <i>homebudget</i> is an existing HomeBudget object with the following budget items:
        /// <code>
        /// Category ID     Expense ID      Date                    Description                 Cost
        /// 10              1               1/10/2018 12:00:00 AM   Clothes hat (on credit)     10
        /// 9               2               1/11/2018 12:00:00 AM   Credit Card hat             -10
        /// 10              3               1/10/2019 12:00:00 AM   Clothes scarf(on credit)    15
        /// 9               4               1/10/2020 12:00:00 AM   Credit Card scarf           -15
        /// </code>
        /// <h5>Save all the budget files to an XML file</h5>
        /// <code>
        /// string filePath = "./filename";
        /// homebudget.SaveToFile();
        /// </code>
        /// After the code is ran, these files will appear in the path you specified (assume /path/ is an existing path):
        /// <code>
        /// expenses file: /path/filename_expenses.exps
        /// categories file: /path/filename_categories.cats
        /// </code>
        /// </example>
        /// </para>
        /// </summary>
        /// <param name="filepath">The path of the file to be created</param>
        //public void SaveToFile(String filepath)
        //{

        //    // ---------------------------------------------------------------
        //    // just in case filepath doesn't exist, reset path info
        //    // ---------------------------------------------------------------
        //    //_DirName = null;
        //    //_FileName = null;

        //    // ---------------------------------------------------------------
        //    // get filepath name (throws exception if we can't write to the file)
        //    // ---------------------------------------------------------------
        //    filepath = BudgetFiles.VerifyWriteToFileName(filepath, "");

        //    String path = Path.GetDirectoryName(Path.GetFullPath(filepath));
        //    String file = Path.GetFileNameWithoutExtension(filepath);
        //    String ext = Path.GetExtension(filepath);

        //    // ---------------------------------------------------------------
        //    // construct file names for expenses and categories
        //    // ---------------------------------------------------------------
        //    String expensepath = path + "\\" + file + "_expenses" + ".exps";
        //    String categorypath = path + "\\" + file + "_categories" + ".cats";

        //    // ---------------------------------------------------------------
        //    // save the expenses and categories into their own files
        //    // ---------------------------------------------------------------
        //    _expenses.SaveToFile(expensepath);
        //    _categories.SaveToFile(categorypath);

        //    // ---------------------------------------------------------------
        //    // save filenames of expenses and categories to budget file
        //    // ---------------------------------------------------------------
        //    string[] files = { Path.GetFileName(categorypath), Path.GetFileName(expensepath) };
        //    System.IO.File.WriteAllLines(filepath, files);

        //    // ----------------------------------------------------------------
        //    // save filename info for later use
        //    // ----------------------------------------------------------------
        //    //_DirName = path;
        //    //_FileName = Path.GetFileName(filepath);
        //}
        #endregion OpenNewAndSave

        #region GetList



        // ============================================================================
        // Get all expenses list
        // NOTE: VERY IMPORTANT... budget amount is the negative of the expense amount
        // Reasoning: an expense of $15 is -$15 from your bank account.
        // ============================================================================
        /// <summary>
        /// Returns a list of all expenses (Budget Items) within a specified timeframe. 
        /// <br/><u>Budget Amount is the negative of the expense amount;</u> an expense of $15 = -$15 from your bank acount
        /// 
        /// <para>
        /// <example>
        /// For all examples below, assume the budget file contains the
        /// following elements:
        /// <code>
        /// Category ID     Expense ID      Date                    Description                 Cost
        /// 10              1               1/10/2018 12:00:00 AM   Clothes hat (on credit)     10
        /// 9               2               1/11/2018 12:00:00 AM   Credit Card hat             -10
        /// 10              3               1/10/2019 12:00:00 AM   Clothes scarf(on credit)    15
        /// 9               4               1/10/2020 12:00:00 AM   Credit Card scarf           -15
        /// </code>
        /// <br/><h5>Getting a list of ALL budget items:</h5>
        /// <code>
        /// <![CDATA[
        /// HomeBudget budget = new HomeBudget();
        /// budget.ReadFromFile(filename);
        ///
        /// // Get a list of all budget items
        /// var budgetItems = budget.GetBudgetItems(null, null, false, 0);
        ///
        /// // print important information
        /// foreach (var bi in budgetItems)
        /// {
        ///     Console.WriteLine (
        ///         String.Format("{0} {1,-20} {2,8:C} {3,12:C}",
        ///             bi.Date.ToString("yyyy/MMM/dd"),
        ///             bi.ShortDescription,
        ///             bi.Amount, bi.Balance)
        ///      );
        /// }
        /// ]]>
        /// </code>
        /// Output:
        /// <code>
        /// 2018/Jan/10 hat (on credit)         ($10.00) ($10.00)
        /// 2018/Jan/11 hat                     $10.00 $0.00
        /// 2019/Jan/10 scarf(on credit)        ($15.00) ($15.00)
        /// 2020/Jan/10 scarf                   $15.00 $0.00
        /// </code>
        /// <br/><h5>Getting a list of only budget items with a category Id of 10:</h5>
        /// <code>
        /// <![CDATA[
        /// /// HomeBudget budget = new HomeBudget();
        /// budget.ReadFromFile(filename);
        ///
        /// // Get a list of all budget items with a Category Id of 10
        /// var budgetItems = budget.GetBudgetItems(null, null, true, 10);
        ///
        /// // print important information
        /// foreach (var bi in budgetItems)
        /// {
        ///     Console.WriteLine (
        ///         String.Format("{0} {1,-20} {2,8:C} {3,12:C}",
        ///             bi.Date.ToString("yyyy/MMM/dd"),
        ///             bi.ShortDescription,
        ///             bi.Amount, bi.Balance)
        ///      );
        /// }
        /// ]]>
        /// </code>
        /// Output: 
        /// <code>
        /// 2018/Jan/10 hat (on credit)         ($10.00) ($10.00)
        /// 2019/Jan/10 scarf(on credit)        ($15.00) ($15.00)
        /// </code>
        /// <br/><h5>Getting a list of only budget items between the years 2018 and 2019:</h5>
        /// <code>
        /// <![CDATA[
        /// /// HomeBudget budget = new HomeBudget();
        /// budget.ReadFromFile(filename);
        ///
        /// // Get a list of all budget items between the years 2018 and 2019
        /// var budgetItems = budget.GetBudgetItems(new DateTime(2018, 1, 1), new DateTime(2019, 12, 31), false, 0);
        ///
        /// // print important information
        /// foreach (var bi in budgetItems)
        /// {
        ///     Console.WriteLine (
        ///         String.Format("{0} {1,-20} {2,8:C} {3,12:C}",
        ///             bi.Date.ToString("yyyy/MMM/dd"),
        ///             bi.ShortDescription,
        ///             bi.Amount, bi.Balance)
        ///      );
        /// }
        /// ]]>
        /// </code>
        /// Output:
        /// <code>
        /// 2018/Jan/10 hat (on credit) ($10.00) ($10.00)
        /// 2018/Jan/11 hat $10.00 $0.00
        /// 2019/Jan/10 scarf(on credit) ($15.00) ($15.00)
        /// </code>
        /// </example>
        /// </para>
        /// </summary>
        /// <param name="Start">The start date and time of the budget timeframe. If null, it will be 1900/1/1 12:00:00 AM</param>
        /// <param name="End">The end date and time of the budget timeframe. If null, it will be 2500/1/1 12:00:00 AM</param>
        /// <param name="FilterFlag">If set to <b>true</b>, the method will exclude expenses and categories that do NOT have the same category Id as the <b>CategoryId</b> argument</param>
        /// <param name="CategoryID">The category Id to filter by (see FilterFlag)</param>
        /// <returns>A list of BudgetItems representing a query result</returns>
        public List<BudgetItem> GetBudgetItems(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // ------------------------------------------------------------------------
            // return joined list within time frame
            // ------------------------------------------------------------------------
            Start = Start ?? new DateTime(1900, 1, 1);
            End = End ?? new DateTime(2500, 1, 1);

            // This query gets information from the expense and category lists based on whether each expense's date is within the timeframe
            /* Information:
             *      - Category Id
             *      - Category Description
             *      - Expense Id
             *      - Expense Date
             *      - Expense Description
             *      - Expense Amount
             */
            var cmd = new SQLiteCommand(Database.dbConnection);
            cmd.CommandText = $@"SELECT c.Id, e.Id, e.Description, e.Date, e.Amount, c.Description FROM categories c, expenses e WHERE e.CategoryId = c.Id AND date(e.Date) <= date(@End) AND date(e.Date) >= date(@Start)";
            cmd.Parameters.AddWithValue("@Start", Start.Value.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@End", End.Value.Date.ToString("yyyy-MM-dd"));
            using SQLiteDataReader reader = cmd.ExecuteReader();


            // ------------------------------------------------------------------------
            // create a BudgetItem list with totals,
            // ------------------------------------------------------------------------
            List<BudgetItem> items = new List<BudgetItem>();
            Double total = 0;

            while (reader.Read())
            {
                int catId = reader.GetInt32(0);
                int expId = reader.GetInt32(1);
                string expDate = reader.GetString(2);
                string catDesc = reader.GetString(3);
                string expDesc = reader.GetString(4);
                double expAmount = reader.GetDouble(5);


                if (FilterFlag && CategoryID != catId)
                {
                    // keep track of running totals
                    // The total (balance) is all that was spent
                    total = total - expAmount;
                    items.Add(new BudgetItem
                    {
                        CategoryID = catId,
                        ExpenseID = expId,
                        ShortDescription = expDesc,
                        Date = DateTime.ParseExact(expDate.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Amount = -expAmount,
                        Category = catDesc,
                        Balance = total
                    });
                }
            }

            return items;
        }

        // ============================================================================
        // Group all expenses month by month (sorted by year/month)
        // returns a list of BudgetItemsByMonth which is 
        // "year/month", list of budget items, and total for that month
        // ============================================================================
        /// <summary>
        /// Returns a list of all expenses (Budget Items) grouped month by month, sorted by year/month.
        /// <br/>See also: <b>GetBudgetItems()</b>
        /// 
        /// <para>
        /// <example>
        /// For the example below, assume <i>budget.budget</i> is an existing file with the following contents:
        /// <code>
        /// Cat_ID      Expense_ID      Date                    Description                 Cost
        /// 10          1               1/10/2018 12:00:00 AM   Clothes hat (on credit)     10
        /// 9           2               16/10/2018 12:00:00 AM   Credit Card hat             -10
        /// 10          3               1/10/2019 12:00:00 AM   Clothes scarf(on credit)    15
        /// 9           4               1/6/2020 12:00:00 AM   Credit Card scarf           -15
        /// 14          5               1/6/2020 12:00:00 AM   Eating Out McDonalds        45
        /// 14          7               1/4/2020 12:00:00 AM   Eating Out Wendys           25
        /// 14          10              2/4/2020 12:00:00 AM    Eating Out Pizza            33.33
        /// </code>
        /// <h5>Get all items by month</h5>
        /// <code>
        /// <![CDATA[
        /// HomeBudget hb = new HomeBudget();
        /// hb.ReadFromFile("./budget.budget");
        /// 
        /// // Get all items sorted by month
        /// List<BudgetItemsByMonth> itemsByMonth = hb.GetBudgetItemsByMonth(null, null, false, 0);
        /// 
        /// // Print the months and totals to the console
        /// foreach(BudgetItemsByMonth monthBI in itemsByMonth)
        ///     Console.WriteLine("Month: {0}\nTotal: {1}\n", monthBI.Month, monthBI.Total);
        /// ]]>
        /// </code>
        /// Output:
        /// <code>
        /// Month: 2018/10
        /// Total: 0
        /// 
        /// Month: 2019/10
        /// Total: -15
        /// 
        /// Month: 2020/6
        /// Total: -30
        /// 
        /// Month: 2020/4
        /// Total: -58.33
        /// </code>
        /// </example>
        /// </para>
        /// </summary>
        /// <param name="Start">The start date and time of the budget timeframe. If null, it will be 1900/1/1 12:00:00 AM</param>
        /// <param name="End">The end date and time of the budget timeframe. If null, it will be 2500/1/1 12:00:00 AM</param>
        /// <param name="FilterFlag">If set to <b>true</b>, the method will exclude expenses and categories that do NOT have the same category Id as the <b>CategoryId</b> argument</param>
        /// <param name="CategoryID">The category Id to filter by (see FilterFlag)</param>
        /// <returns>A list of BudgetItemsByMonth</returns>
        public List<BudgetItemsByMonth> GetBudgetItemsByMonth(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items first
            // -----------------------------------------------------------------------
            List<BudgetItem> items = GetBudgetItems(Start, End, FilterFlag, CategoryID);

            // -----------------------------------------------------------------------
            // Group by year/month
            // -----------------------------------------------------------------------
            // group by year/month
            var GroupedByMonth = items.GroupBy(c => c.Date.Year.ToString("D4") + "/" + c.Date.Month.ToString("D2"));

            // -----------------------------------------------------------------------
            // create new list
            // -----------------------------------------------------------------------
            var summary = new List<BudgetItemsByMonth>();
            foreach (var MonthGroup in GroupedByMonth)
            {
                // calculate total for this month, and create list of details
                double total = 0;
                var details = new List<BudgetItem>();
                foreach (var item in MonthGroup)
                {
                    if (item.CategoryID == CategoryID) // filter by category
                    {
                        total = total + item.Amount;
                        details.Add(item);
                    }
                }

                // Add new BudgetItemsByMonth to our list if there are details
                if (details.Count > 0)
                {
                    summary.Add(new BudgetItemsByMonth
                    {
                        Month = MonthGroup.Key,
                        Details = details,
                        Total = total
                    });
                }
            }

            return summary;
        }

        // ============================================================================
        // Group all expenses by category (ordered by category name)
        // ============================================================================
        /// <summary>
        /// Returns a list of all expenses (Budget Items) ordered by category name
        /// <br/>See also: <b>GetBudgetItems()</b>
        /// 
        /// <para>
        /// <example>
        /// For the example below, assume the file <i>budget.budget</i> is an existing file with the following contents:
        /// <code>
        /// Cat_ID      Expense_ID      Date                     Description                 Cost
        /// 10          1               1/10/2018 12:00:00 AM    Clothes hat (on credit)     10
        /// 9           2               16/10/2018 12:00:00 AM   Credit Card hat             -10
        /// 10          3               1/10/2019 12:00:00 AM    Clothes scarf(on credit)    15
        /// 9           4               1/6/2020 12:00:00 AM     Credit Card scarf           -15
        /// 14          5               1/6/2020 12:00:00 AM     Eating Out McDonalds        45
        /// 14          7               1/4/2020 12:00:00 AM     Eating Out Wendys           25
        /// 14          10              2/4/2020 12:00:00 AM     Eating Out Pizza            33.33
        /// </code>
        /// <h5>Get all items by category</h5>
        /// <code>
        /// <![CDATA[
        /// HomeBudget hb = new HomeBudget();
        /// hb.ReadFromFile("./budget.budget");
        /// 
        /// // Get all items sorted by month
        /// List<GetBudgetItemsByCategory> itemsByCategory = hb.GetBudgetItemsByCategory(null, null, false, 0);
        /// 
        /// // Print the categories and totals to the console
        /// foreach(BudgetItemsByCategory catBI in itemsByCategory)
        ///     Console.WriteLine("Category: {0}\nTotal: {1}\n", catBI.Category, catBI.Total);
        /// ]]>
        /// </code>
        /// Output:
        /// <code>
        /// Category: Clothes
        /// Total: 25
        /// 
        /// Category: Credit Card
        /// Total: -25
        /// 
        /// Category: Eating Out
        /// Total: -103.33
        /// </code>
        /// </example>
        /// </para>
        /// </summary>
        /// <param name="Start">The start date and time of the budget timeframe. If null, it will be 1900/1/1 12:00:00 AM</param>
        /// <param name="End">The end date and time of the budget timeframe. If null, it will be 2500/1/1 12:00:00 AM</param>
        /// <param name="FilterFlag">If set to <b>true</b>, the method will exclude expenses and categories that do NOT have the same category Id as the <b>CategoryId</b> argument</param>
        /// <param name="CategoryID">The category Id to filter by (see FilterFlag)</param>
        /// <returns></returns>
        public List<BudgetItemsByCategory> GetBudgetItemsByCategory(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items first
            // -----------------------------------------------------------------------
            List<BudgetItem> items = GetBudgetItems(Start, End, FilterFlag, CategoryID);

            // -----------------------------------------------------------------------
            // Group by Category
            // -----------------------------------------------------------------------
            var GroupedByCategory = items.GroupBy(c => c.Category);

            // -----------------------------------------------------------------------
            // create new list
            // -----------------------------------------------------------------------
            var summary = new List<BudgetItemsByCategory>();
            foreach (var CategoryGroup in GroupedByCategory.OrderBy(g => g.Key))
            {
                // calculate total for this category, and create list of details
                double total = 0;
                var details = new List<BudgetItem>();
                foreach (var item in CategoryGroup)
                {
                    total = total + item.Amount;
                    details.Add(item);
                }

                // Add new BudgetItemsByCategory to our list
                summary.Add(new BudgetItemsByCategory
                {
                    Category = CategoryGroup.Key,
                    Details = details,
                    Total = total
                });
            }

            return summary;
        }


        // ============================================================================
        // Group all events by category and Month
        // creates a list of Dictionary objects (which are objects that contain key value pairs).
        // The list of Dictionary objects includes:
        //          one dictionary object per month with expenses,
        //          and one dictionary object for the category totals
        // 
        // Each per month dictionary object has the following key value pairs:
        //           "Month", <the year/month for that month as a string>
        //           "Total", <the total amount for that month as a double>
        //            and for each category for which there is an expense in the month:
        //             "items:category", a List<BudgetItem> of all items in that category for the month
        //             "category", the total amount for that category for this month
        //
        // The one dictionary for the category totals has the following key value pairs:
        //             "Month", the string "TOTALS"
        //             for each category for which there is an expense in ANY month:
        //             "category", the total for that category for all the months
        // ============================================================================
        /// <summary>
        /// Returns a list of dictionary objects containing budget items grouped by category and month.
        /// <br/>See also: <b>GetBudgetItems()</b>
        /// 
        /// <para>
            /// <example>
            /// For the example below, assume <i>budgets.budget</i> is an existing file with the following contents:
            /// <code>
                /// Cat_ID      Expense_ID      Date                     Description                 Cost
                /// 10          1               1/10/2018 12:00:00 AM    Clothes hat (on credit)     10
                /// 9           2               16/11/2018 12:00:00 AM   Credit Card hat             -10
                /// 10          3               1/10/2019 12:00:00 AM    Clothes scarf(on credit)    15
                /// 9           4               1/10/2020 12:00:00 AM     Credit Card scarf           -15
                /// 14          5               1/11/2020 12:00:00 AM     Eating Out McDonalds        45
                /// 14          7               1/12/2020 12:00:00 AM     Eating Out Wendys           25
                /// 14          10              2/1/2020 12:00:00 AM     Eating Out Pizza            33.33
            /// </code>
            /// <h5>Get all the files, ordered by month and category</h5>
                /// <code>
                /// <![CDATA[
                /// HomeBudget hb = new HomeBudget();
                /// hb.ReadFromFile("./");
                ///
                /// List<Dictionary<string, object>> budgetItems = hb.GetBudgetDictionaryByCategoryAndMonth(null, null, false, 0);
                ///
                /// // Print all the items
                /// for(int i = 0; i < budgetItems.Count; i++)
                /// {
                ///     Dictionary<string, object> budgetItem = budgetItems[i];
                ///     
                ///     foreach (KeyValuePair<string, object> pair in budgetItem)
                ///     {
                ///         if (pair.Key.IndexOf("details") == -1 && pair.Key != "Month" && pair.Key != "Total")
                ///             Console.WriteLine("{0, -2} {1, -20} {2, -20} {3, -20}", 
                ///                 budgetItem["Month"],
                ///                 pair.Key, 
                ///                 pair.Value, 
                ///                 budgetItem["Month"] != "TOTALS" ? budgetItem["Total"] : "");
                ///     }
                /// }
                /// ]]>
                /// </code>
            /// Output (month, category, amount, total):
                /// <code>
                /// 2018/01 Clothes              -10                  0
                /// 2018/01 Credit Card          10                   0
                /// 2019/01 Clothes              -15                  -15
                /// 2020/01 Credit Card          15                   -55
                /// 2020/01 Eating Out           -70                  -55
                /// TOTALS Credit Card           25
                /// TOTALS Clothes               -25
                /// TOTALS Eating Out            -70
                /// </code>     
            /// </example>
        /// </para>
        /// </summary>
        /// <param name="Start">The start date and time of the budget timeframe. If null, it will be 1900/1/1 12:00:00 AM</param>
        /// <param name="End">The end date and time of the budget timeframe. If null, it will be 2500/1/1 12:00:00 AM</param>
        /// <param name="FilterFlag">If set to <b>true</b>, the method will exclude expenses and categories that do NOT have the same category Id as the <b>CategoryId</b> argument</param>
        /// <param name="CategoryID">The category Id to filter by (see FilterFlag)</param>
        /// <returns>A list of dictionaries of varying object types</returns>
        public List<Dictionary<string,object>> GetBudgetDictionaryByCategoryAndMonth(DateTime? Start, DateTime? End, bool FilterFlag, int CategoryID)
        {
            // -----------------------------------------------------------------------
            // get all items by month 
            // -----------------------------------------------------------------------
            List<BudgetItemsByMonth> GroupedByMonth = GetBudgetItemsByMonth(Start, End, FilterFlag, CategoryID);

            // -----------------------------------------------------------------------
            // loop over each month
            // -----------------------------------------------------------------------
            var summary = new List<Dictionary<string, object>>();
            var totalsPerCategory = new Dictionary<String, Double>();

            foreach (var MonthGroup in GroupedByMonth)
            {
                // create record object for this month
                Dictionary<string, object> record = new Dictionary<string, object>();
                record["Month"] = MonthGroup.Month;
                record["Total"] = MonthGroup.Total;

                // break up the month details into categories
                var GroupedByCategory = MonthGroup.Details.GroupBy(c => c.Category);

                // -----------------------------------------------------------------------
                // loop over each category
                // -----------------------------------------------------------------------
                foreach (var CategoryGroup in GroupedByCategory.OrderBy(g => g.Key))
                {

                    // calculate totals for the cat/month, and create list of details
                    double total = 0;
                    var details = new List<BudgetItem>();

                    foreach (var item in CategoryGroup)
                    {
                        total = total + item.Amount;
                        details.Add(item);
                    }

                    // add new properties and values to our record object
                    record["details:" + CategoryGroup.Key] =  details;
                    record[CategoryGroup.Key] = total;

                    // keep track of totals for each category
                    if (totalsPerCategory.TryGetValue(CategoryGroup.Key, out Double CurrentCatTotal))
                    {
                        totalsPerCategory[CategoryGroup.Key] = CurrentCatTotal + total;
                    }
                    else
                    {
                        totalsPerCategory[CategoryGroup.Key] = total;
                    }
                }

                // add record to collection
                summary.Add(record);
            }
            // ---------------------------------------------------------------------------
            // add final record which is the totals for each category
            // ---------------------------------------------------------------------------
            Dictionary<string, object> totalsRecord = new Dictionary<string, object>();
            totalsRecord["Month"] = "TOTALS";

            foreach (var cat in categories.List())
            {
                try
                {
                    totalsRecord.Add(cat.Description, totalsPerCategory[cat.Description]);
                }
                catch {}
            }
            summary.Add(totalsRecord);


            return summary;
        }




        #endregion GetList
    }
}
