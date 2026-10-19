using Avalonia.Controls;
using Avalonia.Interactivity;
using EduPr.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EduPr;

public partial class EquipmentList : UserControl
{
    private AppDbContext db = new();

    public EquipmentList()
    {
        InitializeComponent(); 
        Load(); 
    }

    private void Load()
    {
        var s = tbSearch.Text?.ToLower() ?? "";
        lbEquipment.ItemsSource = db.Equipments.AsNoTracking().Include(e => e.Cabinet)
            .Where(e => e.NameEquipment.ToLower().Contains(s) || e.InventoryNumber.ToLower().Contains(s))
            .ToList();
    }

    private void TbSearch_TextChanged(object? s, TextChangedEventArgs e) => Load();

    private async void BtnAdd_Click(object? s, RoutedEventArgs e)
    {
        await new EquipmentEditWindow(null).ShowDialog((Window)VisualRoot!);
        Load();   
    }

    private async void BtnEdit_Click(object? s, RoutedEventArgs e)
    {
        if ((s as Button)?.DataContext is not Equipment q) return;
        await new EquipmentEditWindow(q.IdEquipment).ShowDialog((Window)VisualRoot!);
        Load();
    }

    private void BtnDelete_Click(object? s, RoutedEventArgs e)
    {
        if ((s as Button)?.DataContext is not Equipment q) return; 
        db.Equipments.Remove(q);
        db.SaveChanges();
        Load();  
    }
}