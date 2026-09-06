using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

// Alias needed: MAUI has its own built-in "MenuItem" class (used for app menus),
// which collides with our own model of the same name. This line tells the
// compiler exactly which one "MenuItem" means, everywhere in this file.
using MenuItem = MNightWorks.Shared.Models.MenuItem;

namespace MNightWorks.Mobile;

public partial class MainPage : ContentPage
{
    // HttpClient sends web requests — same job fetch() does in JavaScript.
    // "static readonly" means one instance is reused for the app's whole lifetime,
    // instead of creating a new one every time — HttpClient is designed to be reused.
    private static readonly HttpClient _httpClient = new HttpClient
    {
        // Only correct for Windows. Android/iOS need a different address — we'll
        // cover why once you actually try running on Android.
        BaseAddress = new Uri("https://localhost:7188/")
    };

    public MainPage()
    {
        InitializeComponent();
    }

    // OnAppearing runs automatically every time this page becomes visible —
    // the MAUI equivalent of loadMenu() running as soon as script.js loaded.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadMenuAsync();
    }

    private async Task LoadMenuAsync()
    {
        // Make sure the local cache database file and its tables exist before we
        // try to read from or write to it. EnsureCreated() is the "no migrations
        // needed" version — safe to call every time, since it does nothing if the
        // database already exists.
        using var localDb = new LocalDbContext();
        localDb.Database.EnsureCreated();

        try
        {
            // Try the real server first — this is the "online" path.
            var items = await _httpClient.GetFromJsonAsync<List<MenuItem>>("api/MenuItems");
            MenuCollectionView.ItemsSource = items;

            // Got fresh data successfully — save a copy locally for next time there's
            // no connection. Clear the old cache first so it doesn't just accumulate
            // stale duplicates forever.
            localDb.MenuItems.RemoveRange(localDb.MenuItems);
            localDb.MenuItems.AddRange(items);
            await localDb.SaveChangesAsync();
        }
        catch (Exception)
        {
            // The server request failed — fall back to whatever was saved last time,
            // instead of showing nothing at all.
            var cachedItems = await localDb.MenuItems.ToListAsync();

            if (cachedItems.Count > 0)
            {
                MenuCollectionView.ItemsSource = cachedItems;
                await DisplayAlert("Offline", "Showing saved menu — couldn't reach the server.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Couldn't load the menu, and no saved copy exists yet.", "OK");
            }
        }
    }

}