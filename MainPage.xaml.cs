namespace ProyectoFinal_Grupo3_Floristeria_Margaritas;
using GeolocatorPlugin;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.CustomViews;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
    }

    private void CounterBtn_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Home.homePageUser());
    }

    private void btnHomeRepartidor_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Home.homePageRepartidor());
    }

    private void btnHomeAdmin_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Home.homePageAdmin());
    }

    private void btnProductos_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Productos.productos());
    }

    private void btnProductoDetalle_Clicked(object sender, EventArgs e)
    {
        
    }

    private void btnPedidosRepartidor_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.PantallasRepartidor.PantallaPedidosEntrantes());

    }

    private void btnCarrito_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Productos.carritoCompras());
    }

    private void btnlogin_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Login.login());
    }
    private void btnsignin_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Login.singin());

    }

    private void btnDireccion_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Productos.pagoDireccion());
    }

    private void btnPagoTarjeta_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Productos.PagoTarjeta());
    }

    private void btnFinalizar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Productos.confirmarOrden());
    }

    private async void btnPopupPago_Clicked(object sender, EventArgs e)
    {
        CustomPopupPagoRealizado customPopup = new CustomPopupPagoRealizado();
        await Navigation.PushModalAsync(new NavigationPage(customPopup));
    }


    private void btnCalificacionFinalizada_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Calificacion.CalificacionFinalizada());
    }

    private void btnPantallaCalificacion_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Calificacion.PantallaCalificacion());
    }

    private async void btnAplicarRepartidor_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.PantallasRepartidor.AplicarRepartidor());
    }

    private void btnCrearPromociones_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.PantallaAdministrador.CrearPromociones());
    }

    private void btnHistorialEntrega_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.PantallasRepartidor.historialEntregas());
    }

    private void btnNotificaciones_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.PantallaAdministrador.notificacionesAdministrador());
    }
    private void btnIngresosRepartidor_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.PantallasRepartidor.IngresosRepartidor());

    private void btnIngresosRepartidor_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.PantallasRepartidor.IngresosRepartidor());
    }


    private void btnDetallePedido_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.PantallasRepartidor.DetallePedido());
    }

    private void btnMapaSucursal_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.PantallasRepartidor.MapaSucursal());
    }

    //Permisos de geolocalización para los mapas de repartidores.

    //Solicitar permisos de geolocalización
    private async Task CheckAndRequestLocationPermissionAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }

        if (status == PermissionStatus.Granted)
        {
            //GetLocationAsync();
        }
        else if (status == PermissionStatus.Denied)
        {
            await DisplayAlert("Advertencia", "Esta aplicacion no puede funcionar si no tiene los permisos", "OK");
        }
    }
    public async Task CheckAndRequestPermissionAsync()
    {
        // Verificar si el permiso ya ha sido otorgado
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status != PermissionStatus.Granted)
        {
            // Solicitar el permiso
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }

        // Manejar el resultado del permiso
        if (status == PermissionStatus.Granted)
        {
            await DisplayAlert("Aviso", "Permiso otorgado", "OK");
        }
        else if (status == PermissionStatus.Denied)
        {
            await DisplayAlert("Aviso", "Permiso denegado", "OK");
        }
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();

        var connection = Connectivity.NetworkAccess;
        var local = CrossGeolocator.Current;

        if (connection == NetworkAccess.Internet)
        {
            if (local == null || !local.IsGeolocationAvailable || !local.IsGeolocationEnabled)
            {
                // Si la geolocalización no está disponible o no está habilitada
                CheckAndRequestLocationPermissionAsync();
            }
            else
            {
                //GetLocationAsync();
            }
        }
        else
        {
            await DisplayAlert("Sin Acceso a internet", "Por favor, revisa tu conexion a internet para continuar.", "OK");
        }
    }

    private void btnMapaCliente_Clicked_1(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.PantallasRepartidor.MapaEntregaCliente());
    }
}
