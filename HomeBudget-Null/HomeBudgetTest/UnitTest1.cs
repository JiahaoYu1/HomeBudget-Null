using Budget;

namespace HomebudgetTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // Arrage

            // Act
            HomeBudget budget = new HomeBudget();

            // Assert
            Assert.IsType(typeof(HomeBudget), budget);
        }
    }
}