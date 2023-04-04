using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Presenter presenter;
        public MainWindow()
        {
            InitializeComponent();
            /*this.presenter = new Presenter(this);*/

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
            descriptionTextBox.Text = "";
        }

        private void chooseFile_Click(object sender, RoutedEventArgs e)
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

        private void closeFile_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void addCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            string newCategory = Microsoft.VisualBasic.Interaction.InputBox("Enter a new category name:", "Add Category", "");

            if (string.IsNullOrEmpty(newCategory) && newCategory == null)
            {
                MessageBox.Show("The new category cannot be empty.", "Error");
                return;
            }
            else if (newCategory == null)
            {
                // Handle the case where user closed the InputBox dialog without clicking any button
                MessageBox.Show("The operation was canceled.", "Information");
                return;
            }

            ComboBoxItem newItem = new ComboBoxItem();
            newItem.Content = newCategory;
            categoryComboBox.Items.Add(newItem);

            categoryComboBox.SelectedItem = newItem;
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
    }
}
