using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace EduPr;

public partial class HeadMainWindow : Window
{
    public HeadMainWindow()
    {
        InitializeComponent();
    }

    private void BtnAnalytics_Click(object? sender, RoutedEventArgs e)
    {
        MainContentControl.Content = new HeadAnalytics();
    }

    private void BtnDoctors_Click(object? sender, RoutedEventArgs e)
    {
        MainContentControl.Content = new HeadDoctors();
    }
    private void BtnHeadAdvancedAnalytics_Click(object? sender, RoutedEventArgs e)
    {
        MainContentControl.Content = new HeadAdvancedAnalytics();
    }
    private void BtnHeadDepartments_Click(object? sender, RoutedEventArgs e)
    {
        MainContentControl.Content = new HeadDepartments();
    }

    private void BtnAppeals_Click(object? sender, RoutedEventArgs e)
    {
        MainContentControl.Content = new PatientAppealsControl();
    }

    private void BtnLogout_Click(object? sender, RoutedEventArgs e)
    {
        var mainW = new MainWindow();
        mainW.Show();
        this.Close();
    }

    private void BtnEquipment_Click(object? sender, RoutedEventArgs e)
    {
        MainContentControl.Content = new EquipmentList();
    }
}