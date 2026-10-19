using Avalonia.Controls;
using Avalonia.Interactivity;
using EduPr.Data;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Xml.Linq;

namespace EduPr;

public partial class EquipmentEditWindow : Window
{
    private AppDbContext db = new();
    private Equipment q;
    private bool isNew;
    public EquipmentEditWindow(int? id)
    {
        InitializeComponent();
        isNew = id == null; q = isNew ? new Equipment() : db.Equipments.Find(id)!;
        cbStatus.ItemsSource = new[] { "Исправно", "На обслуживании", "В ремонте", "Списано" };
        cbCabinet.ItemsSource = db.Cabinets.ToList(); 
        tbName.Text = q.NameEquipment; tbInv.Text = q.InventoryNumber; 
        cbCabinet.SelectedItem = db.Cabinets.Local.FirstOrDefault(c => c.IdCabinet == q.IdCabinet);
        cbStatus.SelectedItem = q.StatusEquipment;
        dpLast.SelectedDate = q.LastServiceDate?.ToDateTime(TimeOnly.MinValue);
        dpNext.SelectedDate = q.NextServiceDate?.ToDateTime(TimeOnly.MinValue);
    }

    private void BtnSave_Click(object? s, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(tbName.Text) || string.IsNullOrWhiteSpace(tbInv.Text)) { lblError.Text = "Заполните название и номер"; return; } 
        q.NameEquipment = tbName.Text; q.InventoryNumber = tbInv.Text;
        q.IdCabinet = (cbCabinet.SelectedItem as Cabinet)?.IdCabinet;
        q.StatusEquipment = cbStatus.SelectedItem as string ?? "Исправно";
        q.LastServiceDate = dpLast.SelectedDate is { } a ? DateOnly.FromDateTime(a) : null; 
        q.NextServiceDate = dpNext.SelectedDate is { } b ? DateOnly.FromDateTime(b) : null;
        if (isNew) db.Equipments.Add(q);   
        try { db.SaveChanges(); Close(); } 
        catch { lblError.Text = "Не сохранено: номер уже занят?"; }
    }



}