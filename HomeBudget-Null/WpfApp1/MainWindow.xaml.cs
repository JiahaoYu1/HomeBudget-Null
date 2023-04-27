using Budget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro;
using ControlzEx.Theming;
using System.Globalization;
using Budget;
using Microsoft.Win32;
using System.Diagnostics;
using WpfApp1;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IHomeBudget
    {
        private readonly string DEFAULT_DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Budgets";
        private readonly string APPDATA_DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        const string FILEDIALOG_FILTER = "Database Files (*.db)|*.db";
        const string WINDOW_TITLE = "Home Budget";
        const int DATAGRID_DATE_COLUMN = 1;

        private Presenter presenter;
        private bool _isFileLoaded;
        private bool _fileSelected = false;

        private List<Expense> _expensesList = new List<Expense>();

        public MainWindow()
        {
            InitializeComponent();
            // Set default start date and end date to today
            StartDatePicker.SelectedDate = DateTime.Today;
            EndDatePicker.SelectedDate = DateTime.Today;

            presenter = new Presenter(this);
        }

        public void GetFile(bool isCreatingNewFile)
        {
            // Get the file that holds the path to the last directory used to save a file in this app
            string lastDirFile = System.IO.Path.Combine(APPDATA_DIRECTORY, "LastBudgetDirectory.txt");
            string defaultDir = File.Exists(lastDirFile) ? File.ReadAllText(lastDirFile) : DEFAULT_DIRECTORY;

            if (!Directory.Exists(defaultDir))
            {
                Directory.CreateDirectory(defaultDir);
            }

            Microsoft.Win32.FileDialog fileDialog = isCreatingNewFile ? new Microsoft.Win32.SaveFileDialog() : new Microsoft.Win32.OpenFileDialog();
            fileDialog.Title = isCreatingNewFile ? "Create a File" : "Select a File";
            fileDialog.Filter = FILEDIALOG_FILTER;
            fileDialog.InitialDirectory = defaultDir;


            if (fileDialog.ShowDialog() == true)
            {
                string selectedFile = fileDialog.FileName;
                Title = WINDOW_TITLE + " - " + Path.GetFileName(selectedFile);
                /*selectedFileLabel.Content = "Selected File: " + selectedFile;*/

                BlockingLabel.Visibility = Visibility.Hidden;
                presenter.LoadFile(selectedFile, isCreatingNewFile);

                _isFileLoaded = true;

                FilterByCategoryCheckBox.IsEnabled = true;
                ByCategoryCheckBox.IsEnabled = true;
                ByMonthCheckBox.IsEnabled = true;

                FillDataGrid();
                CategoryComboBox.SelectedIndex = 0;

                //ExpensesDataGrid.ItemsSource = presenter.GetExpenseList();

                // Save the last directory used for the budget file
                File.WriteAllText(lastDirFile, System.IO.Path.GetDirectoryName(selectedFile));
            }
        }

        public void DisplayError(Exception e)
        {
            DisplayError(e.Message);
        }

        public void DisplayError(string errorToDisplay)
        {
            MessageBox.Show(errorToDisplay, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }


        private void FillDataGrid()
        {
            if (!_isFileLoaded)
            {
                DisplayError("Create or select a file");
                return;
            }

            object expenseList = null;
            DateTime startDate = (DateTime)StartDatePicker.SelectedDate;
            DateTime endDate = (DateTime)EndDatePicker.SelectedDate;
            int categoryId = CategoryComboBox.SelectedIndex;

            if (ByMonthCheckBox.IsChecked == true && ByCategoryCheckBox.IsChecked == false)
            {
                List<Expense> expenses = new List<Expense>();
                List<BudgetItemsByMonth> budgetItems = presenter.GetExpenseMonthFilter(startDate, endDate, categoryId);

                foreach(BudgetItemsByMonth bgitem in budgetItems)
                {
                    //expenses.Add();
                }

                ExpensesDataGrid.ItemsSource = expenses;
            }
                

            else if (ByMonthCheckBox.IsChecked == false && ByCategoryCheckBox.IsChecked == true)
                ExpensesDataGrid.ItemsSource = presenter.GetExpenseDateFilter(startDate, endDate, categoryId);

            else
                ExpensesDataGrid.ItemsSource = presenter.GetExpenseList();


            CategoryComboBox.ItemsSource = presenter.GetCategoryList();

            ((DataGridTextColumn)ExpensesDataGrid.Columns[DATAGRID_DATE_COLUMN]).Binding.StringFormat = "d";
        }

        private void SortDataGrid()
        {
            if (_isFileLoaded)
            {

            }
        }



        private void FilterByCategoryCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CategoryComboBox.IsEnabled = true;
        }

        private void FilterByCategoryCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CategoryComboBox.IsEnabled = false;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            // Set _fileSelected to true
            _fileSelected = true;

            // Set the Visibility property of the DataGridTextColumn elements to Visible
            //DateColumn.Visibility = Visibility.Visible;
            //CategoryColumn.Visibility = Visibility.Visible;
            //DescriptionColumn.Visibility = Visibility.Visible;
            //AmountColumn.Visibility = Visibility.Visible;
            //BalanceColumn.Visibility = Visibility.Visible;

            GetFile(true);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            // Set _fileSelected to true
            _fileSelected = true;

            // Set the Visibility property of the DataGridTextColumn elements to Visible
            //DateColumn.Visibility = Visibility.Visible;
            //CategoryColumn.Visibility = Visibility.Visible;
            //DescriptionColumn.Visibility = Visibility.Visible;
            //AmountColumn.Visibility = Visibility.Visible;
            //BalanceColumn.Visibility = Visibility.Visible;

            GetFile(false);
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Database files (*.db)|*.db";

            if (saveFileDialog.ShowDialog() == true)
            {
                // Code to save the file to the selected location goes here
            }
        }

        private void AddExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isFileLoaded)
            {
                AddExpenseWindow aew = new AddExpenseWindow(presenter);
                aew.ShowDialog();

                ExpensesDataGrid.ItemsSource = presenter.GetExpenseList();
            }
            else
                MessageBox.Show("Select or create a file", "Add Expense", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchedTerm = SearchTextBox.Text;
            FilterExpenses(searchedTerm);
        }

        private void FilterExpenses(string searchedTerm)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(ExpensesDataGrid.ItemsSource);
            view.Filter = expenses =>
            {
                Expense item = expenses as Expense;
                return item.Description.Contains(searchedTerm);
            };
        }

        private void AboutUs_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Team name: Null\nGroup members: Ryan Caden Kevin", "About Us", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ByMonthCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (_isFileLoaded)
            {
                FillDataGrid();
            }
        }

        private void ByCategoryCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (_isFileLoaded)
            {
                FillDataGrid();
            }
        }

        private void ByMonthCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
        }

        private void ByCategoryCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
        }

        private void ExpensesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(!_fileSelected){
                ExpensesDataGrid.UnselectAll();
            }
             if (ExpensesDataGrid.SelectedItem != null)
            {
                // Enable the ContextMenu for the selected item
                ExpensesDataGrid.ContextMenu.IsEnabled = true;
            }
            else
            {
                // Disable the ContextMenu if no item is selected
                ExpensesDataGrid.ContextMenu.IsEnabled = false;
            }
        }

        private void ExpensesDataGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGrid expenseGrid = sender as DataGrid;
            if (expenseGrid != null)
            {
                expenseGrid.ContextMenu.PlacementTarget = expenseGrid;
                expenseGrid.ContextMenu.IsOpen = true;
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected item in the data grid
            var selectedItem = ExpensesDataGrid.SelectedItem;

            if (selectedItem != null)
            {
                // Show a message box to confirm the deletion
                var messageBoxResult = MessageBox.Show("Are you sure you want to delete this?", "Confirm Deletion", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    // Remove the selected item from the data source
                    _expensesList.Remove((Expense)selectedItem);

                    // Reset the data grid by updating the ItemsSource property
                    ExpensesDataGrid.ItemsSource = _expensesList;
                }
            }
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected item in the data grid
            var selectedItem = ExpensesDataGrid.SelectedItem;

            if (selectedItem != null)
            {
                // Show a dialog or window to allow the user to modify the item
                var dialog = new ModifyExpenseWindow((Expense)selectedItem);

                // Show the dialog and wait for the user's response
                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    // Update the data source with the modified item
                    var modifiedExpense = dialog.Expense;
                    var index = _expensesList.IndexOf((Expense)selectedItem);
                    _expensesList[index] = modifiedExpense;

                    // Reset the data grid by updating the ItemsSource property
                    ExpensesDataGrid.ItemsSource = _expensesList;
                }
            }
        }
    }

}
