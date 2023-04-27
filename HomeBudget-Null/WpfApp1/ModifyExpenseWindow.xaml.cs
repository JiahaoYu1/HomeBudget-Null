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
        private bool _unsavedChanges;
        private bool _allFieldsFilledOut;

        public Expense Expense { get; private set; }
        public bool UnsavedChanges
        {
            get { return _unsavedChanges; }
            set
            {
                if (_unsavedChanges != value)
                {
                    _unsavedChanges = value;
                }
            }
        }

        public ModifyExpenseWindow(Presenter presenter)
        {
            InitializeComponent();
            this.presenter = presenter;
            // Initialize the controls with default values
            CategoryComboBox.ItemsSource = presenter.GetCategoryList();
            Datepicker.SelectedDate = DateTime.Today;
        }

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
            if (string.IsNullOrWhiteSpace(AmountTextBox.Text) ||
        string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ||
        CategoryComboBox.SelectedItem == null ||
        Datepicker.SelectedDate == null)
            {
                MessageBox.Show("Please fill out all fields");
                return;
            }

            if (!double.TryParse(AmountTextBox.Text, out double amount))
            {
                MessageBox.Show("Amount must be a number");
                return;
            }

            if (!DateTime.TryParse(Datepicker.Text, out DateTime date))
            {
                MessageBox.Show("Invalid date format");
                return;
            }

            presenter.UpdateExpense(expense.Id, date, expense.Category, amount, DescriptionTextBox.Text);
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Set the dialog result to false and close the window
            DialogResult = false;
            Close();
        }

        private void AmountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UnsavedChanges = true;
            ValidateAllFields();
        }

        private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UnsavedChanges = true;
            ValidateAllFields();
        }

        private void ModifyExpenseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (UnsavedChanges)
            {
                var result = MessageBox.Show("There are unsaved changes. Do you want to save them?", "Unsaved Changes", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    OkButton_Click(sender, null);
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void Datepicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateAllFields();
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateAllFields();
        }

        private void ValidateAllFields()
        {
            _allFieldsFilledOut =
                Datepicker.SelectedDate != null &&
                CategoryComboBox.SelectedItem != null &&
                !string.IsNullOrWhiteSpace(AmountTextBox.Text) &&
                !string.IsNullOrWhiteSpace(DescriptionTextBox.Text);
        }

    }
}
