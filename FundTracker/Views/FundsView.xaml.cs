using FundTracker.ContentDialogs;
using FundTracker.Core.Contracts.Services;
using FundTracker.Core.Models;
using FundTracker.ViewModels;
using LiveChartsCore.Defaults;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting;


namespace FundTracker.Views;
/// <summary>
/// Page affichant les lignes d'un portefeuille
/// </summary>
public sealed partial class FundsView : Page
{
    private readonly FundsViewModel ViewModel;
    private CartesianChart? _chart = null;

    public FundsView()
    {
        ViewModel = App.GetService<FundsViewModel>();
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) => ViewModel.OnNavigatedTo(e.Parameter);

    private async void AddFundButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var addContentDialog = new AddFundContentDialog
        {
            XamlRoot = Content.XamlRoot
        };

        var result = await addContentDialog.ShowAsync(ContentDialogPlacement.Popup);

        if (result == ContentDialogResult.Primary && addContentDialog.SelectedFund is not null)
        {
            ToggleProgressVisibility(true);
            ViewModel.AddFund(addContentDialog.SelectedFund, addContentDialog.Quantity, addContentDialog.AveragePurchasePrice);
            ToggleProgressVisibility(false);
        }
    }

    private void DeleteFundButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        ViewModel.RemoveFund(DataGrid.SelectedItem as PortfolioLine);
    }

    private void ToggleProgressVisibility(bool isProgressVisible)
    {
        ProgressStackPanel.Visibility = isProgressVisible ? Visibility.Visible : Visibility.Collapsed;
        FundGrid.Visibility = isProgressVisible ? Visibility.Collapsed : Visibility.Visible;
        AppBarStackPanel.Visibility = isProgressVisible ? Visibility.Collapsed : Visibility.Visible;
    }

    private CartesianChart CreateCartesianChart()
    {
        return new CartesianChart
        {
            XAxes = new Axis[] { new DateTimeAxis(TimeSpan.FromDays(1), date => date.ToString("dd/MM/yy")) },
            Series = new ISeries[] {
                new LineSeries<DateTimePoint> {
                    Values = ViewModel.ChartValues,
                    Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 1 },
                    Fill = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    LineSmoothness = 0
                }
            },
            ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.ZoomX | LiveChartsCore.Measure.ZoomAndPanMode.PanX,
            MinHeight = 300,
            MinWidth = 500
        };
    }

    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.Selected = (Core.Models.PortfolioLine?)DataGrid.SelectedItem;
        if (_chart is not null)
        {
            FundGrid.Children.Remove(_chart);
        }
        _chart = CreateCartesianChart();
        _chart.VerticalAlignment = VerticalAlignment.Stretch;
        _chart.HorizontalAlignment= HorizontalAlignment.Stretch;
        FundGrid.Children.Add(_chart);
        Grid.SetColumn(_chart, 1);
    }
}
