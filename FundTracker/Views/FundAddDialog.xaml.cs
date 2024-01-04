using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Dispatching;
using FundTracker.Core.Contracts.Services;
using FundTracker.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
namespace FundTracker.Views;
/// <summary>
/// Le contenu du dialogue permettant de rechercher un fond dans la base Morningstar et de renvoyer les informations
/// </summary>
public sealed partial class FundAddDialog : Page
{
    private readonly IMorningStarService _morningStarService;

    private MorningStarFund? _selectedFund = null;

    public delegate void SelectionChangedDelegate(bool isSelected);

    public SelectionChangedDelegate? SelectionChangedCallback = null;

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
        SelectionChangedCallback?.Invoke(_selectedFund is not null);
    }
}
