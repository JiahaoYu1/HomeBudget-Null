using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public interface ViewInterface
    {
        public void AddCategory(string categoryName, string categoryType);
        public void GetFile();

        public void DisplayError(Exception errorToDisplay);
    }
}
