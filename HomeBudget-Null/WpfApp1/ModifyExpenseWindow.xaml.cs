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
        public Expense Expense { get; private set; }

        public ModifyExpenseWindow(Expense expense)
        {
            InitializeComponent();

            // Initialize the controls with the current expense data
            Datepicker.SelectedDate = expense.Date;
            CategoryComboBox.SelectedItem = expense.Category;
            AmountTextBox.Text = expense.Amount.ToString();

            Expense = expense;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Set the dialog result to false and close the window
            DialogResult = false;
            Close();
        }
    }
}
