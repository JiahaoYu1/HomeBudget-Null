using Budget;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Budget;


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
            this.presenter = new Presenter(this);
            
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            Expense expense = new Expense(nameTextBox.Text, dateDatePicker.Text, categoryComboBox.SelectedItem.ToString(), double.Parse(amountTextBox.Text), descriptionTextBox.Text);

            presenter.AddExpense(expense);

            budgetLabel.Content = "Budget: $" + presenter.GetBudget().ToString("F2");
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
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog.Title = "Select a File";

            if (openFileDialog.ShowDialog() == true)
            {
                // TODO: Process the selected file
            }
        }
    }
}
