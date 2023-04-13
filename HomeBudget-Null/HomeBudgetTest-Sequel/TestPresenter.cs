using WpfApp1;
using Budget;

namespace HomeBudgetTest_Sequel
{
    public class TestPresenter: ViewInterface
    {
        private string DBFILE = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\HomeBudgetTest-Sequel\\testDBInput.db"));
        private Presenter presenter;

        public void AddCategory()
        {
            throw new NotImplementedException();
        }

        public void GetFile()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void TestAddCategory()
        {
            // Arrange
            presenter = new Presenter(DBFILE, this);

            // Act
            presenter.AddCategory("TestCategory");

            // Assert
            
        }
    }
}