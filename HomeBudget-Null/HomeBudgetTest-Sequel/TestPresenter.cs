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
        private string[][] randomCategories = new string[][]
        {
            new string[] { "TestExpense", "Expense" },
            new string[] { "TestSavings", "Savings" },
            new string[] { "TestIncome", "Income" },
            new string[] { "TestCredit", "Credit" },
        };

        #region ViewInterface Methods
        public void AddCategory(string categoryName, string categoryType)
        {
            categoriesAdded++;
        }

        public void AddExpense()
        {
            expensesAdded++;
        }

        public void GetFile()
        {
            throw new NotImplementedException();
        }

        public void DisplayError(Exception errorToDisplay)
        {
            throw errorToDisplay;
        }
        #endregion


        #region Public Test Methods
        [Fact]
        public void TestAddCategory_SuccessCase()
        {
            // Arrange
            presenter = new Presenter(DBFILE, this);
            int currentCatsAdded = categoriesAdded;
            string[] category = GetRandomCategory();

            // Act
            BeforeAll();
            presenter.AddCategory(category[0], category[1]);

            // Assert
            Assert.Equal(currentCatsAdded + 1, categoriesAdded);
        }

        [Fact]
        public void TestAddCategory_FailureCase()
        {
            // Arrange
            presenter = new Presenter(DBFILE, this);
            int currentCatsAdded = categoriesAdded;
            string name = "TestFailure", type = "What type is this?";

            // Act
            BeforeAll();
            try { presenter.AddCategory(name, type); } catch(Exception e) { }

            // Assert
            Assert.Equal(currentCatsAdded, categoriesAdded);
        }

        
        #endregion

        private string[] GetRandomCategory()
        {
            Random random = new Random();
            int randomNum = random.Next(0, randomCategories.Length);

            return randomCategories[randomNum];
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