using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos;

public partial class productos : ContentPage
{
    private ApiService _apiService = new ApiService();
    public ObservableCollection<FrameItem> Items { get; set; }
    public ObservableCollection<FiltroItem> Filtros { get; set; }
    private int currentIndex = 0; // Para saber el indice del carrusel

    //Para Filtrar los productos
    private FiltroItem _selectedFilter = null;
    public FiltroItem SelectedFilter
    {
        get { return _selectedFilter; }
        set
        {
            if (_selectedFilter != value)
            {
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                Console.WriteLine($"Selected Filter: {SelectedFilter?.LabelText}");
                UpdateFilteredItems();
            }
        }
    }

    //Para la busqueda de productos
    private string _searchQuery;
    public string SearchQuery
    {
        get { return _searchQuery; }
        set
        {
            if (_searchQuery != value)
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
                UpdateFilteredItems();
            }
        }
    }


    public productos()
    {
        InitializeComponent();

        Items = new ObservableCollection<FrameItem>();
        Filtros = new ObservableCollection<FiltroItem>();
        collectionViewProductos.ItemsSource = Items;
        carouselViewFiltros.ItemsSource = Filtros;

        AsyncTaskExec();
        
    }

    private async void AsyncTaskExec()
    {
        await LoadFiltrosDataAsync();
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var productos = await _apiService.GetDataAsync<ProductoModel[]>("obtenerProductosGeneral.php");

        Console.WriteLine($"Received {productos.Length} products from the server.");

        foreach (var producto in productos)
        {
            Items.Add(new FrameItem
            {
                ImageSource = producto.enlacefoto,
                LabelText = producto.nombreproducto,
                LabelPrecio = $"L {producto.precioventa}",
                Categoria = producto.categoria,
                TappedCommand = new Command(() => HandleItemTapped(producto))
            });
        }
        collectionViewProductos.ItemsSource = null;
        collectionViewProductos.ItemsSource = Items;
    }

    private async Task LoadFiltrosDataAsync()
    {
        var filtros = await _apiService.GetDataAsync<FiltroModel[]>("obtenerFiltros.php");

        Console.WriteLine($"Received {filtros.Length} filters from the server.");

        Filtros.Clear();
        Filtros.Add(new FiltroItem { LabelText = "Ninguno" });

        foreach (var filtro in filtros)
        {
            Filtros.Add(new FiltroItem
            {
                LabelText = filtro.categoria
            });
        }

        carouselViewFiltros.ItemsSource = Filtros;
    }

    //Para actualizar los filtros basado en el filtro seleccionado
    private void UpdateFilteredItems()
    {
        // If no filter is selected and no search query, show all items
        if ((SelectedFilter == null || SelectedFilter.LabelText == "Ninguno") && string.IsNullOrEmpty(SearchQuery))
        {
            collectionViewProductos.ItemsSource = Items;
        }
        else
        {
            // Filter items based on the selected category and search query
            var filteredItems = Items.Where(item =>
                (SelectedFilter == null || SelectedFilter.LabelText == "Ninguno" || item.Categoria == SelectedFilter.LabelText) &&
                (string.IsNullOrEmpty(SearchQuery) ||
                item.LabelText.ToLower().Contains(SearchQuery.ToLower()) ||
                item.Categoria.ToLower().Contains(SearchQuery.ToLower()))
            );

            collectionViewProductos.ItemsSource = new ObservableCollection<FrameItem>(filteredItems);
        }
    }

    public class FrameItem
    {
        public string? ImageSource { get; set; }
        public string? LabelText { get; set; }
        public string? LabelPrecio { get; set; }
        public string? Categoria { get; set; }
        public ICommand? TappedCommand { get; set; }
    }

    public class FiltroItem
    {
        public string? LabelText { get; set; }
    }

    private void HandleItemTapped(ProductoModel selectedProduct)
    {
        if (selectedProduct != null)
        {
            Navigation.PushAsync(new Views.Productos.productoDetalle(selectedProduct));
        }
    }

    private void btnNotification_Clicked(object sender, EventArgs e)
    {

    }

    private void searchBarProducots_SearchButtonPressed(object sender, EventArgs e)
    {
        SearchQuery = searchBarProductos.Text;
    }

    private void searchBarProducots_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            collectionViewProductos.ItemsSource = Items;
        }
        else
        {
            SearchQuery = e.NewTextValue;
        }
    }

    private void btnLogout_Clicked(object sender, EventArgs e)
    {

    }

    private void btnCarrito_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Productos.carritoCompras());
    }

    private void btnHome_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Home.homePageUser());
    }

    private void btnProductos_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Productos.productos());
    }

    private void btnPedidos_Clicked(object sender, EventArgs e)
    {

    }

    private void btnPerfil_Clicked(object sender, EventArgs e)
    {

    }

    private void TapGestureCarouselFiltros_Tapped(object sender, TappedEventArgs e)
    {

    }

    private void btnCancelarFiltro_Clicked(object sender, EventArgs e)
    {
        currentIndex = 0;
        carouselViewFiltros.Position = currentIndex;
        if (currentIndex >= 0 && currentIndex < Filtros.Count)
        {
            SelectedFilter = null;
            UpdateFilteredItems();
        }
    }

    private void btnSiguiente_Clicked(object sender, EventArgs e)
    {
        currentIndex = (currentIndex + 1) % Filtros.Count;
        carouselViewFiltros.Position = currentIndex;
        if (currentIndex >= 0 && currentIndex < Filtros.Count)
        {
            SelectedFilter = Filtros[currentIndex];
            UpdateFilteredItems();
        }
    }

    private void carouselViewFiltros_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
    {
        if (e.CurrentItem != null)
        {
            SelectedFilter = (FiltroItem)e.CurrentItem;
            currentIndex = Filtros.IndexOf(SelectedFilter);
            UpdateFilteredItems();
        }
    }
}