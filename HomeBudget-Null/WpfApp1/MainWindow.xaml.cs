using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {
        private Presenter presenter;
        private const string dbFile = "../../../testDBInput.db";

        public MainWindow()
        {
            InitializeComponent();
            this.presenter = new Presenter(dbFile, this);

        }

        public void AddCategory()
        {
            // Create a new instance of the AddCategoryWindow
            AddCategoryWindow addCategoryWindow = new AddCategoryWindow();

            // Show the window as a modal dialog
            bool? userCreatedCategory = addCategoryWindow.ShowDialog();

            if (userCreatedCategory == true)
            {
                // User clicked the confirm button, so update the categoryComboBox
                string categoryName = addCategoryWindow.CategoryName;
                string categoryType = addCategoryWindow.CategoryType;

                //presenter.AddCategory(categoryName, categoryType);

                // Add the new category to the categoryComboBox
                ComboBoxItem newItem = new ComboBoxItem();
                newItem.Content = categoryName + " - " + categoryType;
                categoryComboBox.Items.Add(newItem);
                // Select the newly added category
                categoryComboBox.SelectedItem = newItem;
            }
            else
            {
                // User clicked the cancel button or closed the window, so do nothing
            }
        }

        public void GetFile()
        {
            string defaultDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Budgets";

            if (!Directory.Exists(defaultDir))
            {
                Directory.CreateDirectory(defaultDir);
            }

            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog.Title = "Select a File";
            openFileDialog.InitialDirectory = defaultDir;

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFile = openFileDialog.FileName;
                selectedFileLabel.Content = "Selected File: " + selectedFile;
            }
        }


        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            /* Expense expense = new Expense(nameTextBox.Text, dateDatePicker.Text, categoryComboBox.SelectedItem.ToString(), double.Parse(amountTextBox.Text), descriptionTextBox.Text);

             presenter.AddExpense(expense);

             budgetLabel.Content = "Budget: $" + presenter.GetBudget().ToString("F2");*/
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            nameTextBox.Text = "";
            amountTextBox.Text = "";
            dateDatePicker.Text = "";
            categoryComboBox.SelectedIndex = -1;
            //descriptionTextBox.Text = "";
        }

        private void chooseFile_Click(object sender, RoutedEventArgs e)
        {
            GetFile();
        }

        private void closeFile_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void addCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddCategory();
        }

        private void amountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            TextBox tb = (TextBox)sender;
            if (decimal.TryParse(tb.Text, out decimal value) && tb.Text != null)
            {
                budgetLabel.Content = "Budget: $" + tb.Text;
            }
            else
            {
                tb.Text = string.Empty;
                budgetLabel.Content = "Budget: $0.00";
            }
        }

        private void categoryTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}
