using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using FundTracker.Contracts.ViewModels;
using FundTracker.Core.Contracts.Services;
using FundTracker.Core.Models;

namespace FundTracker.ViewModels;

public partial class PortfoliosViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;

    public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

    public PortfoliosViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _sampleDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
