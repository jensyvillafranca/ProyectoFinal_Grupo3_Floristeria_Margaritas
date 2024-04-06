namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.CreacionProductos;
using System.Collections.ObjectModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Config;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using System.Collections.ObjectModel;

using System.Windows.Input;
using System.ComponentModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos;
public partial class HistorialProductosAgregados : ContentPage
{

    public ObservableCollection<FrameItem>? HistorialProductos { get; set; }

    /*
     * Colección observable que contiene los productos.
     */
    public ObservableCollection<FrameOrden>? Producto { get; set; }

    /*
     * Instancia del servicio de API utilizado para realizar solicitudes HTTP.
     */
    private ApiService _apiService;

    /*
     * Almacena el precio del producto.
     */
    private double precioProducto = 0;

    /*
     * Almacena el porcentaje de descuento aplicado al producto.
     */
    private double discountPercentage = 0;

    /*
     * Almacena el precio descontado del producto.
     */
    private double discountedPrice = 0;

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

    public HistorialProductosAgregados()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        _apiService = new ApiService();
        HistorialProductos = new ObservableCollection<FrameItem>();
        collectionViewHistorialAgregados.ItemsSource = HistorialProductos;
        AsyncTaskExec();
    }

    private async void AsyncTaskExec()
    {
        VisualizacionProductos();
    }

    private async void VisualizacionProductos()
    {
        // Obtiene los datos de los productos desde el servidor.
        var visualizacionesProductos = await _apiService.GetDataAsync<ProductoModel[]>("obtenerProductosGeneral.php");

        // Imprime en la consola la cantidad de productos recibidos del servidor.
        Console.WriteLine($"Received {visualizacionesProductos.Length} products from the server.");

        foreach (var historialproducto in visualizacionesProductos)
        {
            // Convierte el precio y el descuento de string a double.
            precioProducto = Double.Parse(historialproducto.precioventa);
            discountPercentage = Double.Parse(historialproducto.descuento) / 100.0;

            // Calcula el precio descontado del producto.
            discountedPrice = Math.Round(precioProducto - (precioProducto * discountPercentage), 2);

            // Crea un nuevo objeto FrameItem para representar el producto.
            var DetalleProductoViewModel = new FrameItem
            {
                IdProducto = historialproducto.idproducto,
                ImageSource = historialproducto.enlacefoto,
                LabelText = historialproducto.nombreproducto,
                LabelPrecio = $"L {discountedPrice:N2}",
                Categoria = historialproducto.categoria,
                Descripcion = historialproducto.descripcion,
                LabelDescuento = historialproducto.descuento,
                LabelStock = historialproducto.stock,
                // Asigna un comando para manejar el evento de toque en el producto.
                TappedCommand = new Command(() => HandleTappedCommand(historialproducto)),
                DeleteCommand = new Command(() => DeleteCommand(historialproducto.idproducto))
            };

            // Controla la visibilidad de la imagen de descuento basado en el descuento aplicado.
            DetalleProductoViewModel.IsDescuentoImageVisible = int.Parse(historialproducto.descuento) != 0;

            // Asigna un color de borde basado en si hay un descuento aplicado al producto.
            DetalleProductoViewModel.BorderColor = int.Parse(historialproducto.descuento) != 0 ? Color.FromHex("#F44336") : Color.FromHex("#41B9FE");

            // Agrega el producto a la colección de historial de productos.
            HistorialProductos.Add(DetalleProductoViewModel);
        }

        // Establece la colección de historial de productos como origen de datos para la vista de colección.
        collectionViewHistorialAgregados.ItemsSource = HistorialProductos;
    }

    // Método para actualizar los elementos filtrados basados en los filtros seleccionados y la búsqueda
    private void UpdateFilteredItems()
    {
        // If no filter is selected and no search query, show all items
        if (string.IsNullOrEmpty(SearchQuery))
        {
            collectionViewHistorialAgregados.ItemsSource = HistorialProductos;
        }
        else
        {
            // Filter items based on the search query
            var filteredItems = HistorialProductos.Where(item =>
                (string.IsNullOrEmpty(SearchQuery) ||
                item.LabelText.ToLower().Contains(SearchQuery.ToLower()) ||
                item.Categoria.ToLower().Contains(SearchQuery.ToLower()) ||
                item.Descripcion.ToLower().Contains(SearchQuery.ToLower()))
            );

            collectionViewHistorialAgregados.ItemsSource = new ObservableCollection<FrameItem>(filteredItems);
        }
    }

    private async void HandleTappedCommand(ProductoModel producto)
    {
        // Muestra un cuadro de diálogo de confirmación para que el usuario confirme la eliminación del producto.
        await Navigation.PushAsync(new Views.CreacionProductos.ActualizarProductos(1, producto));
     }


    //Boton para eliminar los productos agregados a la lista
    private async void DeleteCommand(int idProducto)
    {
        var tappedItem = HistorialProductos.FirstOrDefault(item => item.IdProducto == idProducto);

        if (tappedItem != null)
        {
            // Muestra un cuadro de diálogo de confirmación para que el usuario confirme la eliminación del producto.
            bool userConfirmed = await DisplayAlert("Confirmación", "¿Está seguro de que desea eliminar este producto?", "Si", "No");

            // Verifica si el usuario ha confirmado la eliminación del producto.
            if (userConfirmed)
            {
                // Crea un objeto de datos con el ID del producto a eliminar.
                var data = new
                {
                    idproducto = idProducto
                };

                // Realiza una solicitud POST al servidor para eliminar el producto.
                bool isSuccess = await _apiService.PostSuccessAsync("eliminarProductos.php", data);

                // Verifica si la solicitud fue exitosa.
                if (isSuccess)
                {
                    HistorialProductos.Remove(tappedItem);
                    collectionViewHistorialAgregados.ItemsSource = HistorialProductos;
                    await DisplayAlert("Aviso", "Producto Eliminado", "OK");
                }
            }

        }
    }

    public class FrameItem
    {
        // Campo privado que almacena el ID del producto.
        private int _idProducto;

        /*
         * Propiedad pública que proporciona acceso al ID del producto.
         */
        public int IdProducto
        {
            get { return _idProducto; } // Obtiene el ID del producto.
            set
            {
                // Verifica si el ID del producto ha cambiado.
                if (_idProducto != value)
                {
                    _idProducto = value; // Establece el nuevo valor del ID del producto.
                                         // OnPropertyChanged(nameof(IdProducto)); // (Comentario: este código podría estar asociado a una notificación de cambio de propiedad)
                }
            }
        }


        public string? ImageSource { get; set; }
        public string? LabelText { get; set; }
        public string? LabelPrecio { get; set; }
        public string? Categoria { get; set; }
        public string? Descripcion { get; set; }
        public string? LabelDescuento { get; set; }
        public int? LabelStock { get; set; }
        public bool IsDescuentoVisible { get; set; }
        public bool IsDescuentoImageVisible { get; set; }
        public Color BorderColor { get; set; }

        public ICommand? TappedCommand { get; set; }
        public ICommand? DeleteCommand { get; set; }
    }
    public class FiltroItem
    {
        public string? LabelText { get; set; }
    }



    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async void btnHome_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Home.homePageAdmin());
    }

    private async void btnStock_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.CreacionProductos.HistorialProductosAgregados());
    }

    private void btnEstadisticas_Clicked(object sender, EventArgs e)
    {

    }

    private void btnAnuncios_Clicked(object sender, EventArgs e)
    {

    }

    private void btnPerfil_Clicked(object sender, EventArgs e)
    {

    }

    private void btnLogout_Clicked(object sender, EventArgs e)
    {

    }

    private async void btnAgregarProducto_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.CreacionProductos.AgregarProducto(1));
    }

    private void searchBarProductos_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            collectionViewHistorialAgregados.ItemsSource = HistorialProductos;
        }
        else
        {
            SearchQuery = e.NewTextValue;
        }
    }

    private void searchBarProductos_SearchButtonPressed(object sender, EventArgs e)
    {
        SearchQuery = searchBarProductos.Text;
    }
}