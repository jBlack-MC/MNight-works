using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Linq;

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

    // Hardcoded for now — there's no restaurant-selection screen yet, so the
    // app always shows this one restaurant's menu. This becomes a real setting
    // once you build that screen, which is future scope, not today.
    private const int RestaurantId = 2;

    // OnAppearing runs automatically every time this page becomes visible —
    // the MAUI equivalent of loadMenu() running as soon as script.js loaded.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadMenuAsync();
    }

    private async Task LoadMenuAsync()
    {
        using var localDb = new LocalDbContext();
        localDb.Database.EnsureCreated();

        // If this cache file was created before RestaurantId existed on MenuItem,
        // its schema won't match anymore, and any query against it will throw.
        // Since this is disposable cache data — not the real source of truth,
        // which lives on the server — the simplest fix is to wipe and recreate
        // it, not patch its schema in place.
        try
        {
            _ = localDb.MenuItems.Any();
        }
        catch
        {
            localDb.Database.EnsureDeleted();
            localDb.Database.EnsureCreated();
        }

        try
        {
            var items = await _httpClient.GetFromJsonAsync<List<MenuItem>>(
                $"api/restaurants/{RestaurantId}/menuitems") ?? new List<MenuItem>();

            MenuCollectionView.ItemsSource = items;

            localDb.MenuItems.RemoveRange(localDb.MenuItems);
            localDb.MenuItems.AddRange(items);
            await localDb.SaveChangesAsync();
        }
        catch (Exception)
        {
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