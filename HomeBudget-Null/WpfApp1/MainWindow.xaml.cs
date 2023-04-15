using Budget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ViewInterface
    {
        private Presenter presenter;
        private const string dbFile = "../../../testDBInput.db";
        private bool unsavedChanges = false;

        public MainWindow()
        {
            InitializeComponent();
            this.presenter = new Presenter(dbFile, this);
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
            /*// Create a new Expense object using the values entered by the user
            string name = nameTextBox.Text;
            DateTime date = dateDatePicker.SelectedDate.Value;
            string category = categoryComboBox.SelectedItem.ToString();
            double amount = double.Parse(amountTextBox.Text);
            string description = descriptionTextBox.Text;
            Expense expense = new Expense(id, date, category, amount, description);

            // Add the expense to the budget using the presenter
            presenter.AddExpense(expense);

            // Update the budget label
            budgetLabel.Content = "Budget: $" + presenter.GetBudget().ToString("F2");

            // Clear the form
            nameTextBox.Text = "";
            amountTextBox.Text = "";
            dateDatePicker.SelectedDate = null;
            categoryComboBox.SelectedIndex = -1;*/

            // Set unsavedChanges to true
            unsavedChanges = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            unsavedChanges = false;
            nameTextBox.Text = "";
            amountTextBox.Text = "";
            dateDatePicker.Text = "";
            categoryComboBox.SelectedIndex = -1;

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

        private void ThemeButton_Click(object sender, RoutedEventArgs e)
        {
           /* // Check if the IsDarkMode key exists in the resources dictionary
            if (Application.Current.Resources.Contains("IsDarkMode"))
            {
                // Check the current theme mode
                bool isDarkMode = false;
                if (Application.Current.Resources["IsDarkMode"] is bool value)
                {
                    isDarkMode = value;
                }

                // Toggle the theme mode
                isDarkMode = !isDarkMode;

                // Set the new theme mode
                Application.Current.Resources["IsDarkMode"] = isDarkMode;

                // Force a refresh of the UI
                var window = Application.Current.MainWindow;
                window.InvalidateMeasure();
                window.InvalidateArrange();
                window.UpdateLayout();

                // Set the UI element colors and styles based on the theme mode
                if (isDarkMode)
                {
                    Application.Current.Resources["BackgroundColor"] = Brushes.Black;
                    Application.Current.Resources["ForegroundColor"] = Brushes.White;
                    Application.Current.Resources["GroupBoxBackground"] = Brushes.DarkGray;
                    Application.Current.Resources["GroupBoxForeground"] = Brushes.White;
                    Application.Current.Resources["ButtonBackground"] = Brushes.DarkGray;
                    Application.Current.Resources["ButtonForeground"] = Brushes.White;
                }
                else
                {
                    Application.Current.Resources["BackgroundColor"] = Brushes.White;
                    Application.Current.Resources["ForegroundColor"] = Brushes.Black;
                    Application.Current.Resources["GroupBoxBackground"] = Brushes.LightGray;
                    Application.Current.Resources["GroupBoxForeground"] = Brushes.Black;
                    Application.Current.Resources["ButtonBackground"] = Brushes.LightGray;
                    Application.Current.Resources["ButtonForeground"] = Brushes.Black;
                }
            }*/
        }
    }
}
