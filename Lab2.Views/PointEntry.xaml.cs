using System.Windows.Controls;
using Lab2.ViewModels;

namespace Lab2.Views;

/// <summary>
/// Interaction logic for PointEntry.xaml
/// </summary>
public partial class PointEntry : UserControl
{
    public PointEntry()
    {
        InitializeComponent();
    }

    public ProjectionPointViewModel PointViewModel
    {
        get => (ProjectionPointViewModel) DataContext; set
        {
            DataContext = value;
        }
    }
}