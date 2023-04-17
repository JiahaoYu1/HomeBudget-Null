using WpfApp1;
using Budget;

namespace HomeBudgetTest_Sequel
{
    public class TestPresenter: ViewInterface
    {
        private string DBFILE = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\HomeBudgetTest-Sequel\\testDBInput.db"));
        private Presenter presenter;
        private bool beforeAllActivated = false;
        private int categoriesAdded = 0;
        private int expensesAdded = 0;
        private string[,] randomCategories = new string[,]
        {
            { "TestGroceries", "Expense" },
            { "TestSavings", "Savings" },
            { "TestIncome", "Income" },
            { "TestCredit", "Credit" },
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
        public void TestAddCategory_BestCase()
        {
            BeforeAll();

            // Arrange
            presenter = new Presenter(DBFILE, this);

            // Act
            presenter.AddCategory("TestCategory");

            // Assert
            Assert.StrictEqual(1, categoriesAdded);
        }

        
        #endregion

        public void BeforeAll()
        {
            if (!beforeAllActivated)
            {
                beforeAllActivated = true;
            }
        }
    }
}