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
        private Presenter presenter;
        private bool _isFileLoaded;

        public MainWindow()
        {
            InitializeComponent();
            // Set default start date and end date to today
            StartDatePicker.SelectedDate = DateTime.Today;
            EndDatePicker.SelectedDate = DateTime.Today;
        }

        public MainWindow(Presenter presenter)
        {
            InitializeComponent();
            this.presenter = presenter;
/*            ExpensesListView.ItemsSource = presenter.GetExpenses();*/
            FilterExpenses("");
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
                /*selectedFileLabel.Content = "Selected File: " + selectedFile;

                BlockingLabel.Visibility = Visibility.Hidden;*/
                presenter.LoadFile(selectedFile, isCreatingNewFile);

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
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Database files (*.db)|*.db";

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                // Create a new file at the specified location
                File.Create(filePath);

                // Optionally, open the file for editing
                Process.Start("notepad.exe", filePath);
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DB files (*.db)|*.db";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                // Open the selected file for editing
                Process.Start("notepad.exe", filePath);
            }
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
            AddExpenseWindow aew = new AddExpenseWindow(presenter);
            aew.ShowDialog();
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
            ICollectionView view = CollectionViewSource.GetDefaultView(ExpensesListView.ItemsSource);
            view.Filter = expenses =>
            {
                Expense item = expenses as Expense;
                return item.Description.Contains(searchedTerm);
            };
        }

        private void AboutUs_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Team name: Null\n Group members: Ryan Caden Kevin", "About Us", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ByMonthCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Set the group description for the ListView to group expenses by month
            ExpensesListView.GroupStyle.Clear();
            ExpensesListView.GroupStyle.Add(new GroupStyle
            {
                HeaderTemplate = (DataTemplate)this.Resources["MonthHeaderTemplate"],
                ContainerStyle = (Style)this.Resources["MonthContainerStyle"]
            });
        }

        private void ExpensesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ExpensesListView.SelectedItem != null)
            {
                // Enable the ContextMenu for the selected item
                ExpensesListView.ContextMenu.IsEnabled = true;
            }
            else
            {
                // Disable the ContextMenu if no item is selected
                ExpensesListView.ContextMenu.IsEnabled = false;
            }
        }

        private void ExpensesListView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            ListView listView = sender as ListView;
            if (listView != null)
            {
                listView.ContextMenu.PlacementTarget = listView;
                listView.ContextMenu.IsOpen = true;
            }
        }
    }
}
