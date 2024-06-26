using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Home;

public partial class homePageRepartidor : ContentPage
{
    private ApiService _apiService = new ApiService();

    public homePageRepartidor()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        labelBienvenido.Text = $"�Bienvenido {PreferencesManager.GetString("usuario")}!";
        //SizeChanged += OnSizeChanged;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        //Revisa si hay notificaciones
        try
        {
            var resultado = await _apiService.PostDataAsync<existeUsuario>("revisarNotificacionesRepartidor.php", new { idrepartidor = Config.Config.activeRepartidorId });
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
            // Manejar cualquier excepci�n que ocurra durante la obtenci�n de productos del detalle del pedido
        }
    }

    private void OnSizeChanged(object sender, EventArgs e)
    {
        // Obtiene la altura de la pantalla
        double screenHeight = DeviceDisplay.MainDisplayInfo.Height;

        // Ajusta la altura del frame como un porcentaje de la altura de la pantalla
        double frameHeightPercentage = 0.08;

        frameIngresos.HeightRequest = screenHeight * frameHeightPercentage;
        frameHistorial.HeightRequest = screenHeight * frameHeightPercentage;
        framePedidos.HeightRequest = screenHeight * frameHeightPercentage;
        framePerfil.HeightRequest = screenHeight * frameHeightPercentage;
    }

    private async void btnNotification_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Notificaciones.notificacionesRepartidor());
    }

    private async void TapGesturePedidos_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(framePedidos, Color.FromRgb(46, 117, 182), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new Views.PantallasRepartidor.PantallaPedidosEntrantes());
    }

    private async void TapGestureIngresos_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameIngresos, Color.FromRgb(33, 52, 91), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new Views.PantallasRepartidor.IngresosRepartidor());
    }

    private async void TapGestureHistorial_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameHistorial, Color.FromRgb(33, 52, 91), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new Views.PantallasRepartidor.historialEntregas());
    }

    private async void TapGesturePerfil_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(framePerfil, Color.FromRgb(46, 117, 182), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new Views.PantallasRepartidor.profilePageRepartidor());
    }

    private async void btnLogout_Clicked(object sender, EventArgs e)
    {
        bool isSuccess = await logout.PerformLogoutAsync(_apiService);

        if (isSuccess)
        {
            await Navigation.PushAsync(new Views.Login.login());
        }
    }
}