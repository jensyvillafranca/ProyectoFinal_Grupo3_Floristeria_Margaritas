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

/* Cambio no fusionado mediante combinaci�n del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-windows10.0.19041.0)'
Antes:
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
public partial class HistorialProductosAgregados : ContentPage
Despu�s:
public partial class HistorialProductosAgregados : ContentPage
*/

/* Cambio no fusionado mediante combinaci�n del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-ios)'
Antes:
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
public partial class HistorialProductosAgregados : ContentPage
Despu�s:
public partial class HistorialProductosAgregados : ContentPage
*/

/* Cambio no fusionado mediante combinaci�n del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-maccatalyst)'
Antes:
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
public partial class HistorialProductosAgregados : ContentPage
Despu�s:
public partial class HistorialProductosAgregados : ContentPage
*/
public partial class HistorialProductosAgregados : ContentPage
{

    public ObservableCollection<FrameItem>? HistorialProductos { get; set; }

    /*
     * Colecci�n observable que contiene los productos.
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
    public HistorialProductosAgregados()
    {
        InitializeComponent();
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
                ImageSource = historialproducto.enlacefoto,
                LabelText = historialproducto.nombreproducto,
                LabelPrecio = $"L {discountedPrice:N2}",
                Categoria = historialproducto.categoria,
                Descripcion = historialproducto.descripcion,
                LabelDescuento = historialproducto.descuento,
                LabelStock = historialproducto.stock,
                // Asigna un comando para manejar el evento de toque en el producto.
                TappedCommand = new Command(() => HandleTappedCommand(historialproducto.idproducto)),
            };

            // Controla la visibilidad de la imagen de descuento basado en el descuento aplicado.
            DetalleProductoViewModel.IsDescuentoImageVisible = int.Parse(historialproducto.descuento) != 0;

            // Asigna un color de borde basado en si hay un descuento aplicado al producto.
            DetalleProductoViewModel.BorderColor = int.Parse(historialproducto.descuento) != 0 ? Color.FromHex("#F44336") : Color.FromHex("#41B9FE");

            // Agrega el producto a la colecci�n de historial de productos.
            HistorialProductos.Add(DetalleProductoViewModel);
        }

        // Establece la colecci�n de historial de productos como origen de datos para la vista de colecci�n.
        collectionViewHistorialAgregados.ItemsSource = HistorialProductos;
    }

    //Boton para eliminar los productos agregados a la lista
    private async void HandleTappedCommand(int idProducto)
    {
        // Muestra un cuadro de di�logo de confirmaci�n para que el usuario confirme la eliminaci�n del producto.
        bool userConfirmed = await DisplayAlert("Confirmaci�n", "�Est� seguro de que desea eliminar este producto?", "Si", "No");

        // Verifica si el usuario ha confirmado la eliminaci�n del producto.
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
                // Encuentra el �ndice del elemento a eliminar en la colecci�n.
                int indexToRemove = -1;

                for (int i = 0; i < HistorialProductos.Count; i++)
                {
                    if (HistorialProductos[i].IdProducto == idProducto)
                    {
                        indexToRemove = i;
                        break;
                    }
                }

                // Verifica si se encontr� el �ndice del elemento a eliminar.
                if (indexToRemove != -1)
                {
                    // Elimina el elemento de la ObservableCollection.
                    HistorialProductos.RemoveAt(indexToRemove);

                    // Actualiza el origen de datos de la vista de colecci�n.
                    collectionViewHistorialAgregados.ItemsSource = null;
                    collectionViewHistorialAgregados.ItemsSource = HistorialProductos;

                    // Muestra un mensaje de �xito.
                    await DisplayAlert("Alerta", "El producto ha sido eliminado!", "OK");

                    // Recarga la p�gina navegando a ella de nuevo.
                    await Navigation.PopAsync();
                    await Navigation.PushAsync(new HistorialProductosAgregados());
                }
                else
                {
                    // Muestra un mensaje de error si no se encontr� el producto en la colecci�n.
                    await DisplayAlert("Error", "No se encontr� ning�n producto en la colecci�n.", "OK");
                }
            }
        }
    }

    public class FrameItem
    {
        // Campo privado que almacena el ID del producto.
        private int _idProducto;

        /*
         * Propiedad p�blica que proporciona acceso al ID del producto.
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
                                         // OnPropertyChanged(nameof(IdProducto)); // (Comentario: este c�digo podr�a estar asociado a una notificaci�n de cambio de propiedad)
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
    }
    public class FiltroItem
    {
        public string? LabelText { get; set; }
    }



    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private void btnActualizarProducto_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ActualizarProductos(1));
    }

    private void btnHome_Clicked(object sender, EventArgs e)
    {

    }

    private void BtnHistorialProductos_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new HistorialProductosAgregados());
    }



    private void btnAnuncios_Clicked(object sender, EventArgs e)
    {

    }

    private void btnPerfil_Clicked(object sender, EventArgs e)
    {

    }

    private void btnVerVencidos_Clicked(object sender, EventArgs e)
    {

    }

    private void btnLogout_Clicked(object sender, EventArgs e)
    {

    }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


}