using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Config;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Reflection;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos;

public partial class pagoDireccion : ContentPage
{
    public ObservableCollection<DireccionesViewModel>? Direcciones { get; set; }
    private ApiService _apiService;
    private Modelos.DireccionModel SelectedDireccion { get; set; }

    private double TotalPrecio;
    private double ISV;
    private double Envio;
    private double Total;

    public pagoDireccion()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
        _apiService = new ApiService();

        InitializeAsync();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is PagoDireccionTotalesModel data)
        {
            TotalPrecio = (data.TotalPrecio);
            ISV = (data.ISV);
            Envio = (data.Envio);
            Total = (data.Total);

            labelSubtotal.Text = $"L {Math.Round(data.TotalPrecio, 2):F2}";
            labelISV.Text = $"L {Math.Round(data.ISV, 2):F2}";
            labelEnvio.Text = $"L {Math.Round(data.Envio, 2):F2}";
            labelTotal.Text = $"L {Math.Round(data.Total, 2):F2}";
        }

        MessagingCenter.Subscribe<object>(this, "UpdateCollectionView", (sender) =>
        {
            InitializeAsync();
        });
    }

    private async void InitializeAsync()
    {
        try
        {
            var direcciones = await _apiService.PostDataAsync<DireccionModel[]>("obtenerDireccionesPorID.php", new { idcliente = Config.Config.activeUserId }); 
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
                    FrameBackgroundColor = Color.FromRgb(255, 250, 240),
                    Referencia = direccion.referencia,
                TappedCommand = new Command(() => HandleTappedCommand(direccion))
                };

                Direcciones.Add(direccionesViewModel);
            }

            collectionViewDirecciones.ItemsSource = Direcciones;
        } catch(Exception ex) 
        { 

        }   
    }

    private void HandleTappedCommand(DireccionModel direccion)
    {
        SelectedDireccion = direccion;

        // Update the FrameBackgroundColor property for all items
        foreach (var item in Direcciones)
        {
            item.FrameBackgroundColor = Color.FromRgb(255, 250, 240);
        }

        // Find the selected item and update its FrameBackgroundColor
        var selectedItem = Direcciones.FirstOrDefault(d => d.IdDireccion == direccion.iddireccion);
        if (selectedItem != null)
        {
            selectedItem.FrameBackgroundColor = Color.FromRgb(65, 185, 254); // Set your desired color
        }
    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void btnCancelar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Productos.carritoCompras());
    }

    private async void btnRealizarPago_Clicked(object sender, EventArgs e)
    {
        if (SelectedDireccion == null)
        {
            await DisplayAlert("Aviso", "Por favor seleccione una dirección de entrega o agregue una nueva", "OK");
            return;
        }

        var data = new PagoDireccionTotalesModel
        {
            TotalPrecio = TotalPrecio,
            ISV = ISV,
            Envio = Envio,
            Total = Total
        };

        var combinedData = new Tuple<PagoDireccionTotalesModel, DireccionModel>(data, SelectedDireccion);
        await Navigation.PushAsync(new Views.Productos.PagoTarjeta { BindingContext = combinedData });
    }

    private async void TapGestureNuevaDireccion_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameAgregarDireccion, Color.FromRgb(255, 250, 240), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new Views.DireccionesUsuario.AgregarDireccionNueva(2));
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