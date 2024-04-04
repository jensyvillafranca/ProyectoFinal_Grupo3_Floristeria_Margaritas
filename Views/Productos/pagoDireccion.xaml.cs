/*
 * Descripción:
 * Este código define la lógica de backend para la página 'pagoDireccion' de la aplicación Floristeria Margaritas.
 * Permite al usuario seleccionar una dirección de entrega existente o agregar una nueva para proceder con el pago de la orden.
 */

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
    // Propiedad para almacenar las direcciones del usuario
    public ObservableCollection<DireccionesViewModel>? Direcciones { get; set; }
    private ApiService _apiService;
    private Modelos.DireccionModel SelectedDireccion { get; set; }

    // Variables para almacenar los datos de la orden
    private double TotalPrecio;
    private double ISV;
    private double Envio;
    private double Total;

    // Constructor
    public pagoDireccion()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
        _apiService = new ApiService();

        InitializeAsync();
    }

    // Método que se ejecuta al aparecer la página
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Si el contexto de enlace es un modelo de datos de pago y dirección combinados
        if (BindingContext is PagoDireccionTotalesModel data)
        {
            // Asigna los valores de la orden a las variables locales
            TotalPrecio = (data.TotalPrecio);
            ISV = (data.ISV);
            Envio = (data.Envio);
            Total = (data.Total);

            // Actualiza las etiquetas de la página con los valores correspondientes
            labelSubtotal.Text = $"L {Math.Round(data.TotalPrecio, 2):F2}";
            labelISV.Text = $"L {Math.Round(data.ISV, 2):F2}";
            labelEnvio.Text = $"L {Math.Round(data.Envio, 2):F2}";
            labelTotal.Text = $"L {Math.Round(data.Total, 2):F2}";
        }

        // Registra un observador para actualizar la colección de direcciones cuando sea necesario
        MessagingCenter.Subscribe<object>(this, "UpdateCollectionView", (sender) =>
        {
            InitializeAsync();
        });
    }

    // Método para inicializar la colección de direcciones
    private async void InitializeAsync()
    {
        try
        {
            // Obtiene las direcciones del usuario desde el servicio web
            var direcciones = await _apiService.PostDataAsync<DireccionModel[]>("obtenerDireccionesPorID.php", new { idcliente = Config.Config.activeUserId }); 
            Direcciones = new ObservableCollection<DireccionesViewModel>();

            // Itera sobre las direcciones obtenidas y las agrega a la colección observable
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

            // Actualiza la fuente de datos de la colección en la vista
            collectionViewDirecciones.ItemsSource = Direcciones;
        } catch(Exception ex) 
        {
            // Manejo de excepciones
        }
    }

    // Método para manejar el comando de tap en una dirección
    private void HandleTappedCommand(DireccionModel direccion)
    {
        SelectedDireccion = direccion;

        // Restablece el color de fondo para todas las direcciones
        foreach (var item in Direcciones)
        {
            item.FrameBackgroundColor = Color.FromRgb(255, 250, 240);
        }

        // Encuentra la dirección seleccionada y actualiza su color de fondo
        var selectedItem = Direcciones.FirstOrDefault(d => d.IdDireccion == direccion.iddireccion);
        if (selectedItem != null)
        {
            selectedItem.FrameBackgroundColor = Color.FromRgb(65, 185, 254); // Set your desired color
        }
    }

    // Método para manejar el evento de clic en el botón de retroceso
    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    // Método para manejar el evento de clic en el botón de cancelar
    private async void btnCancelar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Productos.carritoCompras());
    }

    // Método para manejar el evento de clic en el botón de realizar pago
    private async void btnRealizarPago_Clicked(object sender, EventArgs e)
    {
        // Verifica si se ha seleccionado una dirección
        if (SelectedDireccion == null)
        {
            await DisplayAlert("Aviso", "Por favor seleccione una dirección de entrega o agregue una nueva", "OK");
            return;
        }

        // Crea un objeto con los datos combinados de la orden y la dirección seleccionada
        var data = new PagoDireccionTotalesModel
        {
            TotalPrecio = TotalPrecio,
            ISV = ISV,
            Envio = Envio,
            Total = Total
        };

        // Navega a la página de pago con tarjeta, pasando los datos combinados como contexto de enlace
        var combinedData = new Tuple<PagoDireccionTotalesModel, DireccionModel>(data, SelectedDireccion);
        await Navigation.PushAsync(new Views.Productos.PagoTarjeta { BindingContext = combinedData });
    }

    // Método para manejar el evento de tap en la opción de agregar una nueva dirección
    private async void TapGestureNuevaDireccion_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameAgregarDireccion, Color.FromRgb(255, 250, 240), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new Views.DireccionesUsuario.AgregarDireccionNueva(2));
    }

    // Método para manejar el evento de tap en la opción de utilizar la ubicación actual como dirección
    private async void TapGestureUbicacionActual_Tapped(object sender, TappedEventArgs e)
    {
        // Realiza la animación de cambio de color del marco de la ubicación actual
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