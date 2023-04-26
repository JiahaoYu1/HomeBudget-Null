using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Budget;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for AddCategoryWindow.xaml
    /// </summary>
    public partial class AddCategoryWindow : Window
    {
        private Presenter _presenter;
        private string _categoryName;
        private string _categoryType;
        

        public string CategoryName { 
            get { return _categoryName; }
            private set { _categoryName = value; }
        }
        public string CategoryType
        {
            get { return _categoryType; }
            private set { _categoryType = value; }
        }

        public AddCategoryWindow()
        {
            InitializeComponent();
            categoryTypeComboBox.ItemsSource = Presenter.GetCategoryTypes();
            categoryTypeComboBox.SelectedIndex = 0;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(categoryNameTextBox.Text))
            {
                MessageBox.Show("Category name cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CategoryName = categoryNameTextBox.Text;
            CategoryType = categoryTypeComboBox.Text;
            DialogResult = true;
        }
    }
}
