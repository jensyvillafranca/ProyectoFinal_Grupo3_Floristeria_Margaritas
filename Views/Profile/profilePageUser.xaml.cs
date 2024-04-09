namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Profile;

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Utilities;
using System.Collections.ObjectModel;

public partial class profilePageUser : ContentPage
{
    private string? imagenFilePath = null;
    private string? imgURL = null;
    private ApiService _apiService = new ApiService();

    public profilePageUser()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        //Obtiene los detalles del cliente
        try
        {
            // Obtener los productos del detalle del pedido
            var clientDetails = await _apiService.PostDataAsync<ClienteModel>("obtenerDetalleCliente.php", new { idcliente = Config.Config.activeUserId });

            labelNombre.Text = clientDetails.nombrecompleto;
            labelUsuario.Text = clientDetails.usuario;
            labelCorreoElectronico.Text = clientDetails.correo;
            labelNumeroTelefono.Text = clientDetails.telefono;
            imgURL = clientDetails.enlacefoto;
            imgPerfil.Source = imgURL;
        }
        catch (Exception ex)
        {
            // Manejar cualquier excepción que ocurra durante la obtención de productos del detalle del pedido
        }

    private async void TapGestureHistorial_Tapped(object sender, TappedEventArgs e)
    {

        await AnimationUtilities.ChangeFrameColor(frameHistorial, Color.FromRgb(255, 255, 255), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new HistorialCompras());
        //Revisa si hay notificaciones
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
            // Manejar cualquier excepción que ocurra durante la obtención de productos del detalle del pedido
        }
    }

    private async void TapGestureCambiarcontra_Tapped(object sender, TappedEventArgs e)
    {
        //await AnimationUtilities.ChangeFrameColor(frameCambiarcontrasenia, Color.FromRgb(255, 255, 255), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new CambiarContrasenia());
    }

    private void btnEditarPefil_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Profile.editPageUserProfile());
    }

    private async void btnNotification_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Notificaciones.notificacionesEstadoPedidos());
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

    private async void btnLogout_Clicked(object sender, EventArgs e)
    {
        bool isSuccess = await logout.PerformLogoutAsync(_apiService);

        if (isSuccess)
        {
            await Navigation.PushAsync(new Views.Login.login());
        }
    }

    //Metodo para seleccionar una imagen y cambiarla en la base de datos
    private async void btnCambiarImagen_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Abre la galería de medios para que el usuario seleccione una foto.
            var galeria = await MediaPicker.PickPhotoAsync();

            // Verifica si se ha seleccionado una foto desde la galería.
            if (galeria != null)
            {
                // Abre un flujo de memoria para la foto seleccionada.
                var memoriaStream = await galeria.OpenReadAsync();

                // Establece la imagen seleccionada como origen de la imagen en la interfaz de usuario.
                imgPerfil.Source = ImageSource.FromFile(galeria.FullPath);
                imagenFilePath = galeria.FullPath;

                try
                {
                    var data = new
                    {
                        idcliente = Config.Config.activeUserId,
                        enlacefoto = photoHelper.ImageToBase64(imagenFilePath),
                        urlanterior = imgURL
                    };

                    bool isSuccess = await _apiService.PostSuccessAsync("updateFotoPerfil.php", data);

                    if (isSuccess) 
                    {
                        await DisplayAlert("Alerta", "Su imagen de perfil se ha actualizado", "OK");
                    }
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                Console.WriteLine("Photo selection cancelled or failed."); // Mensaje en caso de que se cancele o falle la selección de la foto.
            }
        }
        catch (FeatureNotSupportedException)
        {
            Console.WriteLine("Photo picking is not supported on this device."); // Mensaje en caso de que la selección de fotos no esté soportada en el dispositivo.
        }
        catch (PermissionException)
        {
            Console.WriteLine("Gallery permission denied."); // Mensaje en caso de que se deniegue el permiso para acceder a la galería.
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error picking photo: {ex.Message}"); // Mensaje en caso de que ocurra un error al seleccionar la foto.
        }
    }

    private async void TapGestureDirecciones_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameDirecciones, Color.FromRgb(255, 250, 240), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new Views.DireccionesUsuario.DireccionesGuardas());
    }

    private async void TapGestureHistorial_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameHistorialCompras, Color.FromRgb(255, 250, 240), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new Views.Profile.HistorialCompras());
    }

    private async void TapGestureContrasenia_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameContrasenia, Color.FromRgb(255, 250, 240), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new Views.Profile.CambiarContrasenia());
    }

    private async void TapGestureCorreo_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameCorreo, Color.FromRgb(255, 250, 240), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new Views.Profile.cambiarCorreo());
    }

    private async void TapGestureTelefono_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameTelefono, Color.FromRgb(255, 250, 240), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new Views.Profile.cambiarTelefono());
    }
}