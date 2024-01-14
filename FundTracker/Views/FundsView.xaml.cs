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
using CommunityToolkit.WinUI.UI.Controls;


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
        DataGrid.Visibility = isProgressVisible ? Visibility.Collapsed : Visibility.Visible;
        AppBarStackPanel.Visibility = isProgressVisible ? Visibility.Collapsed : Visibility.Visible;
    }

    private CartesianChart CreateCartesianChart()
    {
        return new CartesianChart
        {
            XAxes = new Axis[] {
                new DateTimeAxis(TimeSpan.FromDays(1), date => date.ToString("dd/MM/yy"))
                {
                    CrosshairLabelsBackground = SKColors.DarkOrange.AsLvcColor(),
                    CrosshairLabelsPaint = new SolidColorPaint(SKColors.DarkRed, 1),
                    CrosshairPaint = new SolidColorPaint(SKColors.DarkOrange, 1),
                    Labeler = value => new DateTime((long)value).ToString("dd/MM/yy")
                }
            },
            YAxes = new Axis[]
            {
                new() {
                    CrosshairLabelsBackground = SKColors.DarkOrange.AsLvcColor(),
                    CrosshairLabelsPaint = new SolidColorPaint(SKColors.DarkRed, 1),
                    CrosshairPaint = new SolidColorPaint(SKColors.DarkOrange, 1),
                    Labeler = value => value.ToString("F2")
                }
            },
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
            ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X,
            Margin = new Thickness { Left = 12, Top=0, Right=0, Bottom=0 },
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            //MinHeight = 300,
            //MinWidth = 500
        };
    }

    private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.Selected = (Core.Models.PortfolioLine?)DataGrid.SelectedItem;
        if (_chart is not null)
        {
            MainGrid.Children.Remove(_chart);
        }
        _chart = CreateCartesianChart();
        _chart.VerticalAlignment = VerticalAlignment.Stretch;
        _chart.HorizontalAlignment = HorizontalAlignment.Stretch;
        MainGrid.Children.Add(_chart);
        Grid.SetColumn(_chart, 1);
    }

    private void DataGrid_Sorting(object sender, DataGridColumnEventArgs e)
    {
        if (e.Column.Header.ToString() is not string sortingColumnName)
        {
            return;
        }
        
        var sortDirection = e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending ?
            DataGridSortDirection.Ascending : DataGridSortDirection.Descending;
        
        ViewModel.SortLines(sortingColumnName, sortDirection);
        
        foreach (var col in DataGrid.Columns)
        {
            col.SortDirection =  col.Header.ToString() != sortingColumnName ? null : sortDirection;
        }
    }
}
