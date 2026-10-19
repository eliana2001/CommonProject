using Avalonia.Controls;
using Avalonia.Interactivity;
using EduPr.Data;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EduPr;

public partial class AddEditDepartment : Window
{
    private readonly int _departmentId;

    public AddEditDepartment(int departmentId)
    {
        InitializeComponent();
        _departmentId = departmentId;
        if (_departmentId == 0)
        {
            Title = "Добавление отдела";
            titleTb.Text = "Добавление отдела";
            deleteBtn.IsVisible = false;
        }
        else
        {
            Title = "Редактирование отдела";
            titleTb.Text = "Редактирование отдела";
            using var context = new AppDbContext();
            var department = context.Departments.FirstOrDefault(d => d.IdDepartment == _departmentId);
            if (department != null)
                nameDepartmentTb.Text = department.NameDepartment;
                descriptionTb.Text = department.Description;
                addressTb.Text = department.Address;
                phoneTb.Text = department.Phone;
        }
    }

    private async void SaveDepartment_Click(object? sender, RoutedEventArgs e)
    {
        var name = nameDepartmentTb.Text?.Trim();
        var description = descriptionTb.Text?.Trim();
        var address = addressTb.Text?.Trim();
        var phone = phoneTb.Text?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            errorTb.Text = "Введите название отдела";
            return;
        }

        using var context = new AppDbContext();
        var exists = context.Departments.Any(d => d.NameDepartment == name && d.IdDepartment != _departmentId);
        if (exists)
        {
            errorTb.Text = "Отдел с таким названием уже существует";
            return;
        }

        if (_departmentId == 0)
            context.Departments.Add(new Department { NameDepartment = name, Description =  description, Address = address, Phone = phone});
        else
        {
            var department = context.Departments.FirstOrDefault(d => d.IdDepartment == _departmentId);
            if (department == null)
            {
                errorTb.Text = "Отдел не найден";
                return;
            }
            department.NameDepartment = name;
            department.Description = description;
            department.Address = address;
            department.Phone = phone;
        }

        await context.SaveChangesAsync();
        Close();
    }

    private async void DeleteDepartment_Click(object? sender, RoutedEventArgs e)
    {
        using var context = new AppDbContext();
        var department = context.Departments.FirstOrDefault(d => d.IdDepartment == _departmentId);
        if (department == null)
        {
            Close();
            return;
        }

        if (department.Doctors.Any() || department.Cabinets.Any())
        {
            errorTb.Text = "Нельзя удалить отдел, к которому привязаны врачи или кабинеты";
            return;
        }

        context.Departments.Remove(department);
        await context.SaveChangesAsync();
        Close();
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}