using FundTracker.Core.Contracts.Services;
using FundTracker.Core.Models;
using Microsoft.UI.Xaml.Controls;


namespace FundTracker.ContentDialogs;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AddFundContentDialog : ContentDialog
{
    private readonly IMorningStarService _morningStarService;

    private MorningStarFund? _selectedFund = null;

    private double _quantity = 0.0;

    private double _averagePurchasePrice = 0.0;

    public double Quantity => _quantity;

    public double AveragePurchasePrice => _averagePurchasePrice;

    public MorningStarFund? SelectedFund => _selectedFund;

    public AddFundContentDialog()
    {
        InitializeComponent();
        _morningStarService = App.GetService<IMorningStarService>();
    }

    private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        _ = double.TryParse(QuantityTextBox.Text, out _quantity);
        SetPrimaryEnabled();
    }

    private void APPTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        _= double.TryParse(APPTextBox.Text, out _averagePurchasePrice);
        SetPrimaryEnabled();
    }

    private async void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        async Task<bool> UserKeepsTyping()
        {
            var text = SearchTextBox.Text;
            await Task.Delay(500);
            return text != SearchTextBox.Text;
        }

        if (await UserKeepsTyping())
        {
            return;
        }

        ResultsListView.ItemsSource = await _morningStarService.FetchFunds(SearchTextBox.Text);
    }

    private void ResultsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selectedFund = ResultsListView.SelectedItem as MorningStarFund;
        SetPrimaryEnabled();
    }

    private void SetPrimaryEnabled()
    {
        IsPrimaryButtonEnabled = _selectedFund is not null && _quantity > 0 && _averagePurchasePrice > 0;
    }

}
