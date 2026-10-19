using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;


namespace EduPr;

public partial class AdminWindow : Window
{
    public AdminWindow()
    {
        InitializeComponent();
    }
    private void LogButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var mainW = new MainWindow();
        mainW.Show();
        this.Close();
    }

    private void BtnPatients_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainContentControl.Content = new PatientList();
    }
    private void BtnAppoints_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainContentControl.Content = new AppointmentList();

    }
    private void BtnPayments_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainContentControl.Content = new PaymentsList();
    }
    private void BtnReviews_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainContentControl.Content = new ReviewList();
    }

    private void BtnEquipment_Click(object? sender, RoutedEventArgs e)
    {
        MainContentControl.Content = new EquipmentList();
    }
}