using FundTracker.ContentDialogs;
using FundTracker.Core.Models;
using FundTracker.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace FundTracker.Views;

public sealed partial class PortfoliosPage : Page
{
    public PortfoliosViewModel ViewModel
    {
        get;
    }

    public PortfoliosPage()
    {
        ViewModel = App.GetService<PortfoliosViewModel>();
        InitializeComponent();
    }

    private async void TabView_AddTabButtonClick(TabView sender, object args)
    {
        var addContentDialog = new AddPortfolioContentDialog
        {
            XamlRoot = Content.XamlRoot
        };

        var result = await addContentDialog.ShowAsync(ContentDialogPlacement.Popup);

        if (result == ContentDialogResult.Primary)
        {
            var newItem = ViewModel.AddPortfolio(addContentDialog.PortfolioName);
            sender.TabItems.Add(CreateNewTab(newItem));
        }
    }

    private void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
    {
        var deletedTab = args.Tab;
        var item = deletedTab.Tag as PortfolioItem;
        if (item is not null)
        {
            ViewModel.RemovePortfolio(item);
        }
        sender.TabItems.Remove(deletedTab);
    }

    private void TabView_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        foreach (var  item in ViewModel.PortfoliosList)
        {
            (sender as TabView)?.TabItems.Add(CreateNewTab(item));
        }   
    }

    private TabViewItem CreateNewTab(PortfolioItem item)
    {
        TabViewItem newItem = new()
        {
            Header = item.Name,
            Tag = item,
            IconSource = new Microsoft.UI.Xaml.Controls.FontIconSource()
            {
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                Glyph = "\xE82d"
            },
        };

        Frame frame = new();
        frame.Navigate(typeof(FundsView),item);
        newItem.Content = frame;

        return newItem;
    }
}
