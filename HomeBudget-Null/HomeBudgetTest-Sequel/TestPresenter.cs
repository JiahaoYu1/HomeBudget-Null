using WpfApp1;
using Budget;

namespace HomeBudgetTest_Sequel
{
    public class TestPresenter: ViewInterface
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
        [Fact]
        public void TestAddCategory_SuccessCase()
        {
            ///// Arrange
            presenter = new Presenter(this);
            int currentCatsAdded = categoriesAdded;

            // Category info
            string name = "TestSavings", type = "Savings";


            ///// Act
            BeforeAll();
            presenter.AddCategory(name, type);

            ///// Assert
            Assert.Equal(currentCatsAdded + 1, categoriesAdded);
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
            Assert.Equal(currentCatsAdded, categoriesAdded);
            Assert.Equal(currentErrors + 1, errorsDisplayed);
        }


        [Fact]
        public void TestAddExpense_SuccessCase()
        {
            ///// Arrange
            presenter = new Presenter(this);
            int currentExpsAdded = expensesAdded;

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
            Assert.Equal(currentExpsAdded + 1, expensesAdded);
        }

        [Fact]
        public void TestAddExpense_FailureCase()
        {
            ///// Arrange
            presenter = new Presenter(this);
            int currentExpsAdded = expensesAdded;
            int currentErrors = errorsDisplayed;

            // Expense info (The category with the id of 50 should not exist in the database)
            int categoryId = 50;
            DateTime date = DateTime.Now;
            double amount = 20;
            string expenseName = "TestFailure";


            ////// Act
            BeforeAll();
            try { presenter.AddExpense(date, categoryId, amount, expenseName); } catch (Exception e) { }

            ////// Assert
            Assert.Equal(currentExpsAdded, expensesAdded);
            Assert.Equal(currentErrors + 1, errorsDisplayed);
        }
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

        private void BeforeAll()
        {
            if (!beforeAllActivated)
            {
                beforeAllActivated = true;
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