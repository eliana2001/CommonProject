using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using EduPr.Models;
using EduPr.Data;

namespace EduPr
{
    public partial class SupplierControl : UserControl
    {
        private AppDbContext _context = new AppDbContext();
        private Supplier _selectedSupplier;

        public SupplierControl()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            SuppliersDataGrid.ItemsSource = _context.Suppliers.ToList();
            if (StatusMessage != null) StatusMessage.Text = "";
        }

        private void ClearFields()
        {
            OrgNameTxt.Text = "";
            ContactTxt.Text = "";
            PhoneTxt.Text = "";
            AddressTxt.Text = "";
            EmailTxt.Text = "";
            _selectedSupplier = null;
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            ClearFields();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OrgNameTxt.Text))
            {
                StatusMessage.Text = "Введите название организации!";
                return;
            }

            var newSupplier = new Supplier
            {
                OrganizationName = OrgNameTxt.Text,
                ContactPerson = ContactTxt.Text,
                Phone = PhoneTxt.Text,
                Address = AddressTxt.Text,
                Email = EmailTxt.Text
            };

            _context.Suppliers.Add(newSupplier);
            _context.SaveChanges();
            LoadData();
            ClearFields();
            StatusMessage.Text = "Поставщик успешно добавлен!";
        }

        private void SuppliersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedSupplier = SuppliersDataGrid.SelectedItem as Supplier;
            if (_selectedSupplier != null)
            {
                OrgNameTxt.Text = _selectedSupplier.OrganizationName;
                ContactTxt.Text = _selectedSupplier.ContactPerson;
                PhoneTxt.Text = _selectedSupplier.Phone;
                AddressTxt.Text = _selectedSupplier.Address;
                EmailTxt.Text = _selectedSupplier.Email;
            }
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedSupplier == null)
            {
                StatusMessage.Text = "Выберите поставщика из таблицы!";
                return;
            }

            _selectedSupplier.OrganizationName = OrgNameTxt.Text;
            _selectedSupplier.ContactPerson = ContactTxt.Text;
            _selectedSupplier.Phone = PhoneTxt.Text;
            _selectedSupplier.Address = AddressTxt.Text;
            _selectedSupplier.Email = EmailTxt.Text;

            _context.SaveChanges();
            LoadData();
            ClearFields();
            StatusMessage.Text = "Данные обновлены!";
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedSupplier == null)
            {
                StatusMessage.Text = "Выберите поставщика для удаления!";
                return;
            }

            _context.Suppliers.Remove(_selectedSupplier);
            _context.SaveChanges();
            LoadData();
            ClearFields();
            StatusMessage.Text = "Поставщик удален!";
        }
    }
}