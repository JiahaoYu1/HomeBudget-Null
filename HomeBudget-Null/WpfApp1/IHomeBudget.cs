using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public interface IHomeBudget
    {
        public void DisplayError(Exception e);

        public void GetFile(bool isCreatingNewFile);
    }
}
