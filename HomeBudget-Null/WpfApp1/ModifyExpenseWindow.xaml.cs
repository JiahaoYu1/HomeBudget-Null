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
    /// Interaction logic for ModifyExpenseWindow.xaml
    /// </summary>
    public partial class ModifyExpenseWindow : Window
    {
        private Presenter presenter;
        private Expense expense;
        public Expense Expense { get; private set; }

        public ModifyExpenseWindow(Expense expense, Presenter presenter)
        {
            InitializeComponent();
            this.presenter = presenter;
            // Initialize the controls with the current expense data
            CategoryComboBox.ItemsSource = presenter.GetCategoryList();
            Datepicker.SelectedDate = expense.Date;
            CategoryComboBox.SelectedItem = expense.Category;
            AmountTextBox.Text = expense.Amount.ToString();

            Expense = expense;
            this.expense = expense;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            presenter.UpdateExpense(expense.Id, DateTime.Parse(Datepicker.Text),expense.Category, double.Parse(AmountTextBox.Text), DescriptionTextBox.Text);
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Set the dialog result to false and close the window
            DialogResult = false;
            Close();
        }
    }
}
