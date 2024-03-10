namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.DireccionesUsuario;

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Config;
using System.Collections.ObjectModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;

public partial class DireccionesGuardas : ContentPage
{
    public ObservableCollection<DireccionesViewModel>? Direcciones { get; set; }
    private ApiService _apiService;

    public DireccionesGuardas()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
        _apiService = new ApiService();

        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        var direcciones = await _apiService.PostDataAsync<DireccionModel[]>("obtenerDireccionesPorID.php", new { idcliente = Config.activeUserId });

        Direcciones = new ObservableCollection<DireccionesViewModel>();

        foreach (var direccion in direcciones)
        {

            DireccionesViewModel direccionesViewModel = new DireccionesViewModel
            {
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

            Direcciones.Add(direccionesViewModel);
        }

        collectionViewDirecciones.ItemsSource = Direcciones;
    }

    private async void HandleTappedCommand(int idDireccion, int idCliente)
    {
        bool userConfirmed = await DisplayAlert("Confirmación", "¿Está seguro de que desea eliminar esta dirección?", "Si", "No");

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
                // Find the index of the item to be removed
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
                    // Remove the item from the ObservableCollection
                    Direcciones.RemoveAt(indexToRemove);

                    // Update the ItemsSource of the CollectionView
                    collectionViewDirecciones.ItemsSource = null;
                    collectionViewDirecciones.ItemsSource = Direcciones;

                    await DisplayAlert("Alerta", "La dirección ha sido eliminada!", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "No se encontró la dirección en la colección.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Error al eliminar la dirección!", "OK");
            }
        }
    }


    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void TapGestureNuevaDireccion_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameAgregarDireccion, Color.FromRgb(255, 250, 240), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new Views.DireccionesUsuario.AgregarDireccionNueva(1));
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

    private void btnLogout_Clicked(object sender, EventArgs e)
    {

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
                await DisplayAlert("Error", "Permiso de Ubicación no otorgado. El Permiso es necesario para utilizar esta función.", "OK");
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