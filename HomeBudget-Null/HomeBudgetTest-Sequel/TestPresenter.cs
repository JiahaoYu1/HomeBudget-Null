using WpfApp1;
using Budget;

namespace HomeBudgetTest_Sequel
{
    public class TestPresenter: IExpense
    {
        private string DBFILE = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\testDBInput.db"));
        private Presenter presenter;
        private bool beforeAllActivated = false;
        private int categoriesAdded = 0;
        private int expensesAdded = 0;
        private int errorsDisplayed = 0;


        #region ViewInterface Methods
        public void AddCategory(string categoryName, string categoryType)
        {
            categoriesAdded++;
        }

        public void AddExpense()
        {
            expensesAdded++;
        }

        public void GetFile(bool isCreatingNewFile)
        {
            throw new NotImplementedException();
        }

        public void DisplayError(Exception errorToDisplay)
        {
            errorsDisplayed++;
            throw errorToDisplay;
        }
        #endregion


        #region Public Test Methods
        #region READ Tests
        [Fact] 
        // The database file must NOT be empty for this test to pass
        public void GetCategoryList_SuccessCase()
        {
            ///// Arrange
            List<Category> categoryList;
            presenter = new Presenter(this);

            ///// Act
            BeforeAll();
            categoryList = presenter.GetCategoryList();

            ///// Assert
            Assert.NotEmpty(categoryList);
        }

        [Fact]
        // The database file must NOT be empty for this test to pass
        public void GetExpenseList_SuccessCase()
        {
            ///// Arrange
            List<Expense> expenseList;
            presenter = new Presenter(this);

            ///// Act
            BeforeAll();
            expenseList = presenter.GetExpenseList();

            ///// Assert
            Assert.NotEmpty(expenseList);
        }
        #endregion

        #region CREATE Tests
        [Fact]
        public void TestAddCategory_SuccessCase()
        {
            ///// Arrange
            presenter = new Presenter(this);

            // Category info
            string name = "TestSavings", type = "Savings";


            ///// Act
            BeforeAll();
            presenter.AddCategory(name, type);

            ///// Assert
            Assert.NotEqual(-1, GetCategoryId(name));
        }

        [Fact]
        public void TestAddCategory_FailureCase()
        {
            ///// Arrange
            presenter = new Presenter(this);
            int currentCatsAdded = categoriesAdded;
            int currentErrors = errorsDisplayed;

            // Category info
            string name = "TestFailure", type = "What type is this?";


            ///// Act
            BeforeAll();
            try { presenter.AddCategory(name, type); } catch(Exception e) { }

            ///// Assert
            Assert.Equal(-1, GetCategoryId(name));
        }


        [Fact]
        public void TestAddExpense_SuccessCase()
        {
            ///// Arrange
            presenter = new Presenter(this);

            // Expense info
            string catName = "TestIncome", catType = "Income";
            DateTime date = DateTime.Now;
            double amount = 20;
            string expenseName = catName;


            ///// Act
            BeforeAll();
            presenter.AddCategory(catName, catType);
            presenter.AddExpense(date, GetCategoryId(catName), amount, expenseName);

            ///// Assert
            Assert.NotEqual(-1, GetCategoryId(catName));
            Assert.NotEqual(-1, GetExpenseId(expenseName));
        }

        [Fact]
        public void TestAddExpense_FailureCase()
        {
            ///// Arrange
            presenter = new Presenter(this);

            // Expense info (The category with the id of 50 should not exist in the database)
            int categoryId = 50;
            DateTime date = DateTime.Now;
            double amount = 20;
            string expenseName = "TestFailure";


            ////// Act
            BeforeAll();
            try { presenter.AddExpense(date, categoryId, amount, expenseName); } catch (Exception e) { }

            ////// Assert
            Assert.Equal(-1, GetExpenseId(expenseName));
        }
        #endregion

        #region DELETE Tests
        [Fact]
        public void DeleteCategory_SuccessCase()
        {
            ///// Arrange
            string categoryName = "TestVacation", categoryType = "Expense";
            presenter = new Presenter(this);

            ///// Act
            BeforeAll();


            ///// Assert
        }
        #endregion
        #endregion



        private int GetCategoryId(string categoryName)
        {
            List<Category> categories = presenter.GetCategoryList();

            foreach(Category cat in categories)
            {
                if (cat.Description == categoryName)
                    return cat.Id;
            }

            return -1;
        }

        private int GetExpenseId(string expenseName)
        {
            List<Expense> expenses = presenter.GetExpenseList();

            foreach (Expense exp in expenses)
            {
                if (exp.Description == expenseName)
                    return exp.Id;
            }

            return -1;
        }

        private void BeforeAll()
        {
            if (!beforeAllActivated)
            {
                beforeAllActivated = true;
                presenter.LoadFile(DBFILE, false);

                List<Expense> expenses = presenter.GetExpenseList();
                List<Category> categories = presenter.GetCategoryList();

                
                foreach(Expense expense in expenses)
                {
                    if (expense.Description.IndexOf("Test") != -1)
                        presenter.DeleteExpense(expense.Id);
                }

                foreach (Category category in categories)
                {
                    if (category.Description.IndexOf("Test") != -1)
                        presenter.DeleteCategory(category.Id);
                }
            }
        }
    }
}