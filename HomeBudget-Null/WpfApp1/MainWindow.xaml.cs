﻿using Budget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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
        const int DATAGRID_DATE_COLUMN = 2;

        private Presenter presenter;
        private bool _isFileLoaded;
        private bool _fileSelected = false;

        private List<Expense> _expensesList = new List<Expense>();

        public MainWindow()
        {
            InitializeComponent();
            // Set default start date and end date to today
            //StartDatePicker.SelectedDate = DateTime.Today;
            //EndDatePicker.SelectedDate = DateTime.Today;

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

                BlockingLabel.Visibility = Visibility.Hidden;
                presenter.LoadFile(selectedFile, isCreatingNewFile);

                _isFileLoaded = true;

                FilterByCategoryCheckBox.IsEnabled = true;
                ByCategoryCheckBox.IsEnabled = true;
                ByMonthCheckBox.IsEnabled = true;

                FillDataGrid();
                CategoryComboBox.ItemsSource = presenter.GetCategoryList();
                CategoryComboBox.SelectedIndex = 0;

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

            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;
            int categoryId = CategoryComboBox.SelectedIndex;



            if (ByMonthCheckBox.IsChecked == true && ByCategoryCheckBox.IsChecked == false)
                ExpensesDataGrid.ItemsSource = presenter.GetBudgetItemsByMonth(startDate, endDate, (bool)FilterByCategoryCheckBox.IsChecked, categoryId);
            else if (ByMonthCheckBox.IsChecked == false && ByCategoryCheckBox.IsChecked == true)
                ExpensesDataGrid.ItemsSource = presenter.GetBudgetItemsByCategory(startDate, endDate, (bool)FilterByCategoryCheckBox.IsChecked, categoryId);
            else if (ByMonthCheckBox.IsChecked == true && ByCategoryCheckBox.IsChecked == true)
            {
                List<Dictionary<string, object>> dictionaries = presenter.GetBudgetDictionaryByMonthAndCategory(startDate, endDate, (bool)FilterByCategoryCheckBox.IsChecked, categoryId); ;
                List<BudgetItem> budgetItems = new List<BudgetItem>();

                foreach(Dictionary<string, object> dictionary in dictionaries)
                {
                    foreach (Category category in presenter.GetCategoryList())
                    {

                    }
                }
                
                
                ExpensesDataGrid.ItemsSource = budgetItems;
            }
            else
                ExpensesDataGrid.ItemsSource = presenter.GetBudgetItems(startDate, endDate, (bool)FilterByCategoryCheckBox.IsChecked, categoryId);

           // ((DataGridTextColumn)ExpensesDataGrid.Columns[DATAGRID_DATE_COLUMN]).Binding.StringFormat = "d";
        }



        private void FilterByCategoryCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CategoryComboBox.IsEnabled = true;
            FillDataGrid();
        }

        private void FilterByCategoryCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CategoryComboBox.IsEnabled = false;
            FillDataGrid();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            // Set _fileSelected to true
            _fileSelected = true;

            GetFile(true);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            // Set _fileSelected to true
            _fileSelected = true;

            GetFile(false);
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            _fileSelected = true;

            GetFile(true);
        }

        private void AddExpenseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isFileLoaded)
            {
                AddExpenseWindow aew = new AddExpenseWindow(presenter);
                aew.ShowDialog();
                int selectedIndex = CategoryComboBox.SelectedIndex;

                CategoryComboBox.ItemsSource = presenter.GetCategoryList();
                ExpensesDataGrid.ItemsSource = presenter.GetExpenseList();
                CategoryComboBox.SelectedIndex = selectedIndex >= CategoryComboBox.Items.Count ? 0 : selectedIndex;
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
            view.Filter = budgetItems =>
            {
                BudgetItem item = budgetItems as BudgetItem;
                return item.ShortDescription.Contains(searchedTerm);
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
            try
            {
                BudgetItem selectedItem = (BudgetItem)ExpensesDataGrid.SelectedItem;

                if (selectedItem != null)
                {
                    // Show a message box to confirm the deletion
                    var messageBoxResult = MessageBox.Show("Are you sure you want to delete this?", "Confirm Deletion", MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        presenter.DeleteExpense(selectedItem.ExpenseID);
                        FillDataGrid();
                    }
                }
            }
            catch(Exception exception) 
            { 
                DisplayError(exception);
            }
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected item in the data grid
            BudgetItem selectedItem = (BudgetItem)ExpensesDataGrid.SelectedItem;

            if (selectedItem != null)
            {
                Expense expense = presenter.GetExpenseById(selectedItem.ExpenseID);

                // Show a dialog or window to allow the user to modify the item
                var dialog = new ModifyExpenseWindow(expense, presenter);

                // Show the dialog and wait for the user's response
                bool? result = dialog.ShowDialog();

                if (result == true)
                {
                    // Reset the data grid by updating the ItemsSource property
                    FillDataGrid();
                }
            }
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillDataGrid();
        }
    }

}
