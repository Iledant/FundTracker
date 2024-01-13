using System.Text;
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

    private async void TabView_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
    {
        var deletedTab = args.Tab;

        var dialog = new ContentDialog
        {
            XamlRoot = Content.XamlRoot,
            Content = "Supprimer définitivement le portefeuille ?",
            Title = "Confirmation de suppression",
            PrimaryButtonText = "Supprimer",
            SecondaryButtonText = "Annuler",
            DefaultButton = ContentDialogButton.Secondary
        };

        var result = await dialog.ShowAsync(ContentDialogPlacement.Popup);

        if (result == ContentDialogResult.Secondary)
        {
            return;
        }

        var item = deletedTab.Tag as PortfolioItem;
        if (item is not null)
        {
            ViewModel.RemovePortfolio(item);
        }
        sender.TabItems.Remove(deletedTab);
    }

    private void TabView_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        foreach (var item in ViewModel.PortfoliosList)
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
        newItem.Content = frame;
        frame.Navigate(typeof(FundsView), item);

        return newItem;
    }

    private async Task<string?> PortfolioNameDialog(string initialName)
    {
        var addContentDialog = new PortfolioNameDialog(initialName)
        {
            XamlRoot = Content.XamlRoot
        };

        var result = await addContentDialog.ShowAsync(ContentDialogPlacement.Popup);

        if (result == ContentDialogResult.Primary)
        {
            return addContentDialog.PortfolioName;
        }
        return null;
    }

    private async void AddTabCommand_ExecuteRequested(Microsoft.UI.Xaml.Input.XamlUICommand sender, Microsoft.UI.Xaml.Input.ExecuteRequestedEventArgs args)
    {
        var portfolioName = await PortfolioNameDialog("");
        
        if (portfolioName is not null)
        {
            var newItem = ViewModel.AddPortfolio(portfolioName);
            PortfolioTabView.TabItems.Add(CreateNewTab(newItem));
        }
    }

    private async void RenameTaddCommand_ExecuteRequested(Microsoft.UI.Xaml.Input.XamlUICommand sender, Microsoft.UI.Xaml.Input.ExecuteRequestedEventArgs args)
    {
        if ((PortfolioTabView.SelectedItem as TabViewItem)?.Tag is not PortfolioItem selectedPortfolio)
        {
            return;
        }
        var portfolioName = await PortfolioNameDialog(selectedPortfolio.Name);

        if (portfolioName is not null)
        {
            ViewModel.RenamePortfolio(selectedPortfolio, portfolioName);
            (PortfolioTabView.SelectedItem as TabViewItem).Header = portfolioName;
        }
    }
}
