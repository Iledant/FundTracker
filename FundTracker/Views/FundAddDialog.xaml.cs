using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Dispatching;
using FundTracker.Core.Contracts.Services;
using FundTracker.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using Microsoft.UI.Xaml;
using System.Runtime.CompilerServices;
namespace FundTracker.Views;
/// <summary>
/// Le contenu du dialogue permettant de rechercher un fond dans la base Morningstar et de renvoyer les informations
/// </summary>
public sealed partial class FundAddDialog : Page
{
    private readonly IMorningStarService _morningStarService;

    private MorningStarFund? _selectedFund = null;

    private double _quantity = 0.0;

    private double _averagePurchasePrice = 0.0;

    public delegate void OnChangedDelegate(bool isValid);

    public OnChangedDelegate? OnChanged = null;

    public double Quantity => _quantity;

    public double AveragePurchasePrice => _averagePurchasePrice;

    public MorningStarFund? SelectedFund => _selectedFund;

    public FundAddDialog()
    {
        InitializeComponent();
        _morningStarService = App.GetService<IMorningStarService>();
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
        RaiseOnChanged();
    }

    private void QuantityTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        Double.TryParse(QuantityTextBox.Text, out _quantity);
        RaiseOnChanged();
    }

    private void APPTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        Double.TryParse(APPTextBox.Text, out _averagePurchasePrice);
        RaiseOnChanged();
    }

    private void RaiseOnChanged()
    {
        var isValid = _selectedFund is not null && _quantity > 0 && _averagePurchasePrice > 0;
        OnChanged?.Invoke(isValid);
    }
}
