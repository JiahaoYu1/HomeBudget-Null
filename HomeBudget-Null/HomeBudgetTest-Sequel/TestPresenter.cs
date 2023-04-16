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

        public void AddCategory(string categoryName, string categoryType)
        {
            categoriesAdded++;
        }

        public void GetFile()
        {
            throw new NotImplementedException();
        }


        #region Public Test Methods
        [Fact]
        public void TestAddCategory()
        {
            // Arrange
            presenter = new Presenter(DBFILE, this);

            // Act
            presenter.AddCategory("TestCategory");

            // Assert
            
        }
        #endregion

        private void BeforeAll()
        {
            beforeAllActivated = true;
        }

        private void BeforeEach()
        {
            BeforeAll();
        }
    }
}