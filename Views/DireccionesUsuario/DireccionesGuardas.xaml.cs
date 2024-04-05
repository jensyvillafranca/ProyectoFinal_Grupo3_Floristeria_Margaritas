/*
 * Descripci�n:
 * Este c�digo define la l�gica de backend para la p�gina 'DireccionesGuardas' de la aplicaci�n Floristeria Margaritas.
 * Permite al usuario gestionar sus direcciones guardadas, incluyendo la visualizaci�n, eliminaci�n y adici�n de nuevas direcciones.
 */

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.DireccionesUsuario;

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Config;
using System.Collections.ObjectModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;

public partial class DireccionesGuardas : ContentPage
{
    //Api service y collection view
    public ObservableCollection<DireccionesViewModel>? Direcciones { get; set; }
    private ApiService _apiService;

    // Constructor
    public DireccionesGuardas()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
        _apiService = new ApiService();

        InitializeAsync();
    }

    // M�todo para inicializar la p�gina y cargar las direcciones guardadas
    private async void InitializeAsync()
    {
        try
        {
            // Obtener las direcciones guardadas del servidor
            var direcciones = await _apiService.PostDataAsync<DireccionModel[]>("obtenerDireccionesPorID.php", new { idcliente = Config.activeUserId });

            // Inicializar la colecci�n de direcciones
            Direcciones = new ObservableCollection<DireccionesViewModel>();

            // Iterar sobre las direcciones obtenidas y agregarlas a la colecci�n de direcciones
            foreach (var direccion in direcciones)
            {

                DireccionesViewModel direccionesViewModel = new DireccionesViewModel
                {
                    // Mapear los datos de la direcci�n al modelo de vista
                    IdDireccion = direccion.iddireccion,
                    Direccion = direccion.direccion,
                    Ciudad = direccion.ciudad,
                    Departamento = direccion.departamento,
                    IdCliente = direccion.fk_idcliente,
                    Descripcion = direccion.descripcion,
                    Longitud = direccion.longitud,
                    Latitud = direccion.latitude,
                    Referencia = direccion.referencia,
                    TappedCommand = new Command(() => HandleTappedCommand(direccion.iddireccion, direccion.fk_idcliente)),
                };

                // Agregar la direcci�n mapeada al modelo de vista a la colecci�n de direcciones
                Direcciones.Add(direccionesViewModel);
            }

            // Asignar la colecci�n de direcciones al origen de datos del CollectionView
            collectionViewDirecciones.ItemsSource = Direcciones;
        }
        catch (Exception ex)
        {
            // Manejar cualquier excepci�n que ocurra durante la inicializaci�n
        }

    }

    // M�todo para manejar el evento TappedCommand cuando se selecciona una direcci�n para eliminar
    private async void HandleTappedCommand(int idDireccion, int idCliente)
    {
        bool userConfirmed = await DisplayAlert("Confirmaci�n", "�Est� seguro de que desea eliminar esta direcci�n?", "Si", "No");

        if (userConfirmed)
        {
            var data = new
            {
                iddireccion = idDireccion,
                idcliente = idCliente
            };

            bool isSuccess = await _apiService.PostSuccessAsync("eliminarDireccion.php", data);

            if (isSuccess)
            {
                // Encontrar el �ndice del elemento a eliminar en la colecci�n de direcciones
                int indexToRemove = -1;

                for (int i = 0; i < Direcciones.Count; i++)
                {
                    if (Direcciones[i].IdDireccion == idDireccion)
                    {
                        indexToRemove = i;
                        break;
                    }
                }

                if (indexToRemove != -1)
                {
                    // Eliminar el elemento de la colecci�n de direcciones
                    Direcciones.RemoveAt(indexToRemove);

                    // Actualizar el origen de datos del CollectionView
                    collectionViewDirecciones.ItemsSource = null;
                    collectionViewDirecciones.ItemsSource = Direcciones;

                    await DisplayAlert("Alerta", "La direcci�n ha sido eliminada!", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "No se encontr� la direcci�n en la colecci�n.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Error al eliminar la direcci�n!", "OK");
            }
        }
    }

    // M�todos para manejar eventos de navegaci�n y acciones del usuario
    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void TapGestureNuevaDireccion_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameAgregarDireccion, Color.FromRgb(255, 250, 240), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new Views.DireccionesUsuario.AgregarDireccionNueva(1));
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

    private async void TapGestureUbicacionActual_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameUbicacionActual, Color.FromRgb(255, 250, 240), Color.FromRgb(65, 185, 254), 250);
        try
        {
            // Revisa si el permiso de ubicacion ha sido concedido
            var locationPermissionStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (locationPermissionStatus == PermissionStatus.Granted)
            {
                // Obtiene la ubicacion
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Default,
                    Timeout = TimeSpan.FromSeconds(10)
                });

                if (location != null)
                {
                    await Navigation.PushAsync(new Views.DireccionesUsuario.AgregarDireccionNueva(0));
                }
                else
                {
                    // Cuando la ubicacion es nula
                    await DisplayAlert("Alerta", "El GPS se encuentra desactivado. Porfavor active su GPS e intente de nuevo!", "Ok");
                }
            }
            else
            {
                // Cuando el permiso no es otorgado
                await DisplayAlert("Error", "Permiso de Ubicaci�n no otorgado. El Permiso es necesario para utilizar esta funci�n.", "OK");
            }
        }
        catch (FeatureNotEnabledException)
        {
            try
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "El GPS se encuentra desactivado. Porfavor active su GPS e intente de nuevo!", "Ok");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DisplayGpsNotEnabledAlert: {ex.Message}");
            }
        }
    }
}