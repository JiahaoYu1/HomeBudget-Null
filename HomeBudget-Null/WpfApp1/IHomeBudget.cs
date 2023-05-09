using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Budget;

namespace WpfApp1
{
    public interface IHomeBudget
    {
        public void DisplayError(Exception e);

        public void GetFile(bool isCreatingNewFile);

        public void AddExpense();

        public void UpdateExpenses(List<BudgetItem> items);

        public void UpdateExpensesByMonth(List<BudgetItemsByMonth> items);

        public void UpdateExpensesByCategory(List<BudgetItemsByCategory> items);

        public void UpdateExpensesByMonthAndCategory(List<Dictionary<string, object>> items);

    }
}
