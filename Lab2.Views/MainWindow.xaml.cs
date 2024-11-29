using System.Windows;
using System.Windows.Controls;

namespace Lab2.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        firstPointEntry.PointViewModel = mainViewModel.Point1VM;
        
        secondPointEntry.PointViewModel = mainViewModel.Point2VM;
        
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        mainViewModel.DistanceAxis = ((ListBoxItem) e.AddedItems [0]!).Content.ToString()!;

    }
}