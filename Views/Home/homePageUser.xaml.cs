/*
 * Descripci�n:
 * Este c�digo define la l�gica de backend para la p�gina 'homePageUser' de la aplicaci�n Floristeria Margaritas, destinada a los usuarios clientes.
 * Incluye la carga din�mica de productos en descuento, gesti�n de notificaciones, navegaci�n entre p�ginas y funcionalidades relacionadas con el carrito de compras y el perfil del usuario.
 */

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Config;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Home;

public partial class homePageUser : ContentPage
{
    // Variables de instancia y servicio de API
    private ApiService _apiService = new ApiService();
    public ObservableCollection<CarouselItem> CarouselItems { get; set; }
    private int currentIndex = 0; // Para saber el indice del carrusel
    private double precioProducto = 0;
    private double discountPercentage = 0;
    private double discountedPrice = 0;

    // Constructor para la p�gina 'homePageUser'
    public homePageUser()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        CarouselItems = new ObservableCollection<CarouselItem>();
        carouselView.ItemsSource = CarouselItems;

        AsyncTaskExec();

        SizeChanged += OnSizeChanged;

        // Inicia un temporizador para cambiar las ofertas
        Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                MoveToNextItem();
            }
        });
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        if (Config.Config.activeUserId == -1)
        {
            await DisplayAlert("Alerta", "Su sesi�n no es v�lida, por favor ingrese de nuevo.", "OK");
            await Navigation.PushAsync(new Views.Login.login());
            return;
        }

        if (PreferencesManager.GetInt("tipoUsuario") != 1)
        {
            await DisplayAlert("Alerta", "Su sesi�n no es v�lida, por favor ingrese de nuevo.", "OK");
            await Navigation.PushAsync(new Views.Login.login());
            return;
        }

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

    // M�todo para cargar datos as�ncronamente
    private async void AsyncTaskExec()
    {
        await LoadDataAsync();
    }

    // M�todo para cargar los productos en descuento
    private async Task LoadDataAsync()
    {
        try
        {
            var productos = await _apiService.GetDataAsync<ProductoModel[]>("obtenerProductosDescuento.php");

            Console.WriteLine($"Received {productos.Length} products from the server.");

            foreach (var producto in productos)
            {
                precioProducto = Double.Parse(producto.precioventa);
                discountPercentage = Double.Parse(producto.descuento) / 100.0;
                discountedPrice = Math.Round(precioProducto - (precioProducto * discountPercentage), 2);

                var carouselItem = new CarouselItem
                {
                    ImageSource = producto.enlacefoto,
                    LabelText = producto.nombreproducto,
                    LabelPrecio = $"L {discountedPrice:N2}",
                    LabelDescuento = producto.descuento,
                    TappedCommand = new Command(() => HandleItemTapped(producto))
                };

                carouselItem.IsDescuentoImageVisible = true;

                CarouselItems.Add(carouselItem);
            }

            carouselView.ItemsSource = null;
            carouselView.ItemsSource = CarouselItems;
        }
        catch (Exception ex)
        {
            // Error, NO hay ningun producto en descuento
            Console.WriteLine($"Error loading data: {ex.Message}");

            // Create a default item
            var defaultItem = new CarouselItem
            {
                ImageSource = "logp.png",
                LabelText = "Floristeria Margaritas - Por el momento no hay ofertas",
                LabelPrecio = "", // Set the default price
                LabelDescuento = "",
                TappedCommand = new Command(() => HandleItemTapped(null))
            };

            defaultItem.IsDescuentoImageVisible = false;

            // Clear existing items and add the default one
            carouselView.ItemsSource = null;
            carouselView.ItemsSource = new List<CarouselItem> { defaultItem };
        }
    }

    // M�todo para avanzar al siguiente elemento del carrusel
    private void MoveToNextItem()
    {
        currentIndex = (currentIndex + 1) % CarouselItems.Count;
        carouselView.Position = currentIndex;
    }

    // Controlador de eventos para el cambio de tama�o de la pantalla
    private void OnSizeChanged(object sender, EventArgs e)
    {
        // Obtiene la altura de la pantalla
        double screenHeight = DeviceDisplay.MainDisplayInfo.Height;

        // Ajusta la altura del frame como un porcentaje de la altura de la pantalla
        double frameHeightPercentage = 0.08;

        frameProductos.HeightRequest = screenHeight * frameHeightPercentage;
        frameCarrito.HeightRequest = screenHeight * frameHeightPercentage;
        framePedidos.HeightRequest = screenHeight * frameHeightPercentage;
        framePerfil.HeightRequest = screenHeight * frameHeightPercentage;

        // Set CarouselView height as a percentage of the screen height
        //double carouselHeightPercentage = 0.09; // 50% of the screen height
        //carouselView.HeightRequest = screenHeight * carouselHeightPercentage;
    }

    // Clase para representar un elemento del carrusel
    public class CarouselItem
    {
        public string? ImageSource { get; set; }
        public string? LabelText { get; set; }
        public string? LabelPrecio { get; set; }
        public string? LabelDescuento { get; set; }
        public bool IsDescuentoImageVisible { get; set; }
        public ICommand? TappedCommand { get; set; }
    }

    // Controlador de eventos para el clic en un elemento del carrusel
    private void HandleItemTapped(ProductoModel selectedProduct)
    {
        if (selectedProduct != null)
        {
            Navigation.PushAsync(new Views.Productos.productoDetalle(selectedProduct));
        }
    }

    private async void btnLogout_Clicked(object sender, EventArgs e)
    {
        bool isSuccess = await logout.PerformLogoutAsync(_apiService);

        if (isSuccess)
        {
            await Navigation.PushAsync(new Views.Login.login());
        }
        
    }

    // M�todos para gestionar la navegaci�n a diferentes p�ginas
    private async void TapGestureProductos_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameProductos, Color.FromRgb(46, 117, 182), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new Views.Productos.productos());
    }

    private async void TapGestureCarrito_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameCarrito, Color.FromRgb(33, 52, 91), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new Views.Productos.carritoCompras());
    }

    private async void TapGesturePedidos_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(framePedidos, Color.FromRgb(33, 52, 91), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new Views.Pedidos.pedidosPrincipal());
    }

    private async void TapGesturePerfil_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(framePerfil, Color.FromRgb(0, 0, 0), Color.FromRgb(211, 211, 211), 250);
        await Navigation.PushAsync(new Profile.profilePageUser());
    }

    private async void TapGestureCarousel_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor((Frame)sender, Color.FromRgb(46, 117, 182), Color.FromRgb(65, 185, 254), 250);
    }

    private async void btnNotification_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Notificaciones.notificacionesEstadoPedidos());
    }
}