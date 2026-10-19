using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using EduPr.Data;
using System.Linq;

namespace EduPr;

public partial class HeadDepartments : UserControl
{
    public HeadDepartments()
    {
        InitializeComponent();
        LoadDepartments("");
    }

    private void LoadDepartments(string searchText)
    {
        using var context = new AppDbContext();
        var departments = context.Departments.ToList();
        if (!string.IsNullOrWhiteSpace(searchText))
            departments = departments.Where(d => d.NameDepartment.Contains(searchText, System.StringComparison.OrdinalIgnoreCase)).ToList();
        DataItems.ItemsSource = departments.OrderBy(d => d.NameDepartment).ToList();
    }

    private void SearchTC(object? sender, TextChangedEventArgs e)
    {
        LoadDepartments(searchDepartmentTb.Text ?? "");
    }

    private void SearchDepartment(object? sender, RoutedEventArgs e)
    {
        LoadDepartments(searchDepartmentTb.Text ?? "");
    }

    private async void TblDepartments_DoubleTapped(object? sender, TappedEventArgs e)
    {
        if (DataItems.SelectedItem is not Department department) return;
        var window = new AddEditDepartment(department.IdDepartment);
        await window.ShowDialog(this.VisualRoot as Window);
        LoadDepartments(searchDepartmentTb.Text ?? "");
    }

    private async void AddDepartmentBtn_Click(object? sender, RoutedEventArgs e)
    {
        var window = new AddEditDepartment(0);
        await window.ShowDialog(this.VisualRoot as Window);
        LoadDepartments(searchDepartmentTb.Text ?? "");
    }
}