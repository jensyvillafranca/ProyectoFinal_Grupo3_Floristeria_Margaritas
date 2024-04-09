using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Utilities;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos;

public partial class productos : ContentPage
{
    // Variables de instancia y servicio de API
    private ApiService _apiService = new ApiService();
    public ObservableCollection<FrameItem> Items { get; set; }
    public ObservableCollection<FiltroItem> Filtros { get; set; }

    private int currentIndex = 0; // Para saber el indice del carrusel
    private double precioProducto = 0;
    private double discountPercentage = 0;
    private double discountedPrice = 0;

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

    // Constructor para la p�gina 'productos'
    public productos()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        Items = new ObservableCollection<FrameItem>();
        Filtros = new ObservableCollection<FiltroItem>();
        collectionViewProductos.ItemsSource = Items;
        carouselViewFiltros.ItemsSource = Filtros;

        AsyncTaskExec();

    }


    protected async override void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var resultado = await _apiService.PostDataAsync<existeUsuario>("revisarNotificaciones.php", new { idcliente = Config.Config.activeUserId });
            bool existe = resultado.existe;

            if (existe)
            {
                btnNotification.Source = "Iconos/notificacionn.png";
            }
            else
            {
                btnNotification.Source = "Iconos/notificacione.png";
            }
        }
        catch (Exception ex)
        {

        }
    }

    // M�todo para ejecutar las tareas as�ncronas de carga de filtros y productos
    private async void AsyncTaskExec()
    {
        await LoadFiltrosDataAsync();
        await LoadDataAsync();
    }

    // M�todo para cargar los datos de productos de manera as�ncrona
    private async Task LoadDataAsync()
    {
        var productos = await _apiService.GetDataAsync<ProductoModel[]>("obtenerProductosGeneral.php");

        Console.WriteLine($"Received {productos.Length} products from the server.");

        foreach (var producto in productos)
        {
            precioProducto = Double.Parse(producto.precioventa);
            discountPercentage = Double.Parse(producto.descuento) / 100.0;
            discountedPrice = Math.Round(precioProducto - (precioProducto * discountPercentage), 2);

            var frameItem = new FrameItem
            {
                ImageSource = producto.enlacefoto,
                LabelText = producto.nombreproducto,
                LabelPrecio = $"L {discountedPrice:N2}",
                Categoria = producto.categoria,
                Descripcion = producto.descripcion,
                LabelDescuento = producto.descuento,
                TappedCommand = new Command(() => HandleItemTapped(producto))
            };

            // Controla la visibilidad basado en el descuento
            frameItem.IsDescuentoVisible = int.Parse(producto.descuento) != 0;
            frameItem.IsDescuentoImageVisible = int.Parse(producto.descuento) != 0;
            if (int.Parse(producto.descuento) != 0)
            {
                frameItem.BorderColor = Color.FromHex("#F44336");
            }
            else
            {
                frameItem.BorderColor = Color.FromHex("#41B9FE");
            }

            Items.Add(frameItem);
        }

        collectionViewProductos.ItemsSource = null;
        collectionViewProductos.ItemsSource = Items;
    }

    // M�todo para cargar los datos de filtros de manera as�ncrona
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

    // M�todo para actualizar los elementos filtrados basados en los filtros seleccionados y la b�squeda
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
                item.Categoria.ToLower().Contains(SearchQuery.ToLower()) ||
                item.Descripcion.ToLower().Contains(SearchQuery.ToLower()))
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
        public string? Descripcion { get; set; }
        public string? LabelDescuento { get; set; }
        public ICommand? TappedCommand { get; set; }
        public bool IsDescuentoVisible { get; set; }
        public bool IsDescuentoImageVisible { get; set; }
        public Color BorderColor { get; set; }
    }

    public class FiltroItem
    {
        public string? LabelText { get; set; }
    }

    // M�todo para manejar el evento de pulsaci�n en un elemento de producto
    private async void HandleItemTapped(ProductoModel selectedProduct)
    {
        if (selectedProduct != null)
        {
            await Navigation.PushAsync(new Views.Productos.productoDetalle(selectedProduct));
        }
    }

    // M�todo para manejar el evento de clic en el bot�n de notificaci�n
    private async void btnNotification_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Notificaciones.notificacionesEstadoPedidos());
    }

    // M�todos para manejar el evento de presionar el bot�n de b�squeda y el cambio de texto en el cuadro de b�squeda
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

    // M�todos para manejar el evento de clic en los botones de navegaci�n
    private async void btnLogout_Clicked(object sender, EventArgs e)
    {
        bool isSuccess = await logout.PerformLogoutAsync(_apiService);

        if (isSuccess)
        {
            await Navigation.PushAsync(new Views.Login.login());
        }
    }

    private async void btnCarrito_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Productos.carritoCompras());
    }

    private async void btnHome_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Home.homePageUser());
    }

    private async void btnProductos_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Productos.productos());
    }

    private async void btnPedidos_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Pedidos.pedidosPrincipal());
    }

    private async void btnPerfil_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Profile.profilePageUser());
    }

    // M�todos para manejar el evento de clic en los botones de filtro
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

    // M�todo para manejar el cambio de elemento actual en el carrusel de filtros
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