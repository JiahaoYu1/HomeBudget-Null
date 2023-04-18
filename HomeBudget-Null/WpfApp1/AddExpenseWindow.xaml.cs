﻿using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for AddExpenseWindow.xaml
    /// </summary>
    public partial class AddExpenseWindow : Window, IExpense
    {
        private readonly string DEFAULT_DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Budgets";
        private readonly string APPDATA_DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        const string FILEDIALOG_FILTER = "Database Files (*.db)|*.db";

        private Presenter presenter;
        private bool unsavedChanges = false;
        private bool isDarkTheme = false;

        public AddExpenseWindow()
        {
            InitializeComponent();
            this.presenter = new Presenter(this);
            dateDatePicker.SelectedDate = DateTime.Today;
        }

        // Define color themes
        public enum ColorTheme
        {
            Light,
            Dark,
            Blue,
            Green
        }

        public void AddCategory(string categoryName, string categoryType)
        {
            // User clicked the confirm button, so update the categoryComboBox
            //presenter.AddCategory(categoryName, categoryType);

            categoryComboBox.ItemsSource = presenter.GetCategoryList();
            // Add the new category to the categoryComboBox
            //ComboBoxItem newItem = new ComboBoxItem();
            //newItem.Content = categoryName + " - " + categoryType;
            //categoryComboBox.Items.Add(newItem);
            //// Select the newly added category
            //categoryComboBox.SelectedItem = newItem;
        }

        public void AddExpense()
        {
            // Update the budget label
            budgetLabel.Content = "Budget: $0.00";

            // Clear the form
            nameTextBox.Text = "";
            amountTextBox.Text = "";
            // dateDatePicker.SelectedDate = null;
            //categoryComboBox.SelectedIndex = -1;

            // Set unsavedChanges to true
            //MessageBox.Show("Expense Added", "Expense Status");
            unsavedChanges = true;
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
                selectedFileLabel.Content = "Selected File: " + selectedFile;

                BlockingLabel.Visibility = Visibility.Hidden;
                presenter.LoadFile(selectedFile, isCreatingNewFile);

                // Save the last directory used for the budget file
                File.WriteAllText(lastDirFile, System.IO.Path.GetDirectoryName(selectedFile));

                categoryComboBox.ItemsSource = presenter.GetCategoryList();
            }
        }


        public void DisplayError(Exception errorToDisplay)
        {
            DisplayError(errorToDisplay.Message);
        }

        private void DisplayError(string errorToDisplay)
        {
            MessageBox.Show(errorToDisplay, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }


        private bool AreInputsFilledOut()
        {

            if (string.IsNullOrEmpty(nameTextBox.Text))
            {
                DisplayError("Please provide a name for the Expense.");
                return false;
            }
            if (string.IsNullOrEmpty(amountTextBox.Text))
            {
                DisplayError("Please provide an amount for the Expense");
                return false;
            }

            DateTime? date = dateDatePicker.SelectedDate;
            if (date is null)
            {
                DisplayError("Please provide a valid date for the Expense\nFormat: yyyy-mm-dd");
                return false;
            }

            //if (!DateTime.TryParseExact(dateDatePicker.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            //{
            //    MessageBox.Show("Please provide a valid date for the Expense\nFormat: yyyy-mm-dd", "Expense Date");
            //    return false;
            //}

            if (categoryComboBox.SelectedIndex == -1)
            {
                DisplayError("Please select a Category in which the Expense falls under.");
                return false;
            }
            /*if(selectedFileLabel.Content.ToString() == "Selected File: ")
            {
                MessageBox.Show("Please select a file for the Expense to be stored in.", "Expense File");
                return false;
            }*/

            return true;
        }


        #region Events
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            //Validate that all fields are filled
            if (!AreInputsFilledOut())
                return;

            // Add the expense to the budget using the presenter
            DateTime? date = dateDatePicker.SelectedDate;//DateTime.ParseExact(dateDatePicker.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            double amount = double.Parse(amountTextBox.Text.ToString());
            int index = categoryComboBox.SelectedIndex;
            presenter.AddExpense((DateTime)date, index + 1, amount, nameTextBox.Text);
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            unsavedChanges = false;
            nameTextBox.Text = "";
            amountTextBox.Text = "";
            dateDatePicker.SelectedDate = DateTime.Now;
            categoryComboBox.SelectedIndex = -1;
        }

        private void chooseFile_Click(object sender, RoutedEventArgs e)
        {
            GetFile(false);
        }

        private void closeFile_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void createFile_Click(object sender, RoutedEventArgs e)
        {
            GetFile(true);
        }

        private void addCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of the AddCategoryWindow
            AddCategoryWindow addCategoryWindow = new AddCategoryWindow();

            // Show the window as a modal dialog
            bool? userCreatedCategory = addCategoryWindow.ShowDialog();

            if (userCreatedCategory == true)
            {
                string categoryName = addCategoryWindow.CategoryName;
                string categoryType = addCategoryWindow.CategoryType;

                presenter.AddCategory(categoryName, categoryType);
            }
        }

        private void amountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            unsavedChanges = true;
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

        private void nameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            unsavedChanges = true;
        }

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            unsavedChanges = true;
        }


        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (unsavedChanges)
            {
                MessageBoxResult result = MessageBox.Show("There are unsaved changes. Are you sure you want to close the application? ", "Confirm Close", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                    e.Cancel = true;
            }
        }

        private void descriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            unsavedChanges = true;
        }
        #endregion
    }
}
