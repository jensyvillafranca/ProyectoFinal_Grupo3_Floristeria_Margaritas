/*
 * Descripción:
 * Este código define la lógica de backend para la página 'PagoTarjeta' de la aplicación Floristeria Margaritas.
 * Permite al usuario seleccionar una tarjeta de pago existente o agregar una nueva para proceder con el pago de la orden.
 */

using System.Collections.ObjectModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Config;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos;

public partial class PagoTarjeta : ContentPage
{
    // Propiedad para almacenar las tarjetas del usuario
    public ObservableCollection<TarjetasViewModel> Tarjetas { get; set; }
    private ApiService _apiService;

    // Variables para almacenar los datos de la orden y la dirección seleccionada
    private double TotalPrecio;
    private double ISV;
    private double Envio;
    private double Total;
    private Modelos.DireccionModel SelectedDireccion { get; set; }

    private Modelos.TarjetaModel SelectedTarjeta { get; set; }

    // Constructor
    public PagoTarjeta()
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

        // Si el contexto de enlace es un par de datos combinados
        if (BindingContext is Tuple<PagoDireccionTotalesModel, DireccionModel> combinedData)
        {
            // Obtiene los datos combinados
            PagoDireccionTotalesModel data = combinedData.Item1;
            DireccionModel selectedDireccion = combinedData.Item2;

            // Asigna los valores de la orden y la dirección seleccionada a las variables locales
            TotalPrecio = (data.TotalPrecio);
            ISV = (data.ISV);
            Envio = (data.Envio);
            Total = (data.Total);
            SelectedDireccion = selectedDireccion;

            // Actualiza las etiquetas de la página con los valores correspondientes
            labelSubtotal.Text = $"L {Math.Round(data.TotalPrecio, 2):F2}";
            labelISV.Text = $"L {Math.Round(data.ISV, 2):F2}";
            labelEnvio.Text = $"L {Math.Round(data.Envio, 2):F2}";
            labelTotal.Text = $"L {Math.Round(data.Total, 2):F2}";

            // Registra un observador para actualizar la colección de tarjetas cuando sea necesario
            MessagingCenter.Subscribe<object>(this, "UpdateCollectionView", (sender) =>
            {
                InitializeAsync();
            });
        }
    }

    // Método para inicializar la colección de tarjetas
    private async void InitializeAsync()
    {
        try
        {
            // Obtiene las tarjetas del usuario desde el servicio web
            var tarjetas = await _apiService.PostDataAsync<TarjetaModel[]>("obtenerTarjetaPorID.php", new { idcliente = Config.Config.activeUserId });

            // Crea una nueva colección observable de tarjetas
            Tarjetas = new ObservableCollection<TarjetasViewModel>();

            // Itera sobre las tarjetas obtenidas y las agrega a la colección observable
            foreach (var tarjeta in tarjetas)
            {

                TarjetasViewModel tarjetasViewModel = new TarjetasViewModel
                {
                    IdTarjeta = tarjeta.idtarjeta,
                    NumeroTarjeta = tarjeta.numerotarjeta,
                    FechaVencimiento = tarjeta.fechavencimiento,
                    IdCliente = tarjeta.fk_idcliente,
                    Nombre = tarjeta.nombre,
                    Descripcion = tarjeta.descripcion,
                    FrameBackgroundColor = Color.FromRgb(255, 250, 240),
                    TappedCommand = new Command(() => HandleTappedCommand(tarjeta))
                };

                Tarjetas.Add(tarjetasViewModel);
            }

            // Actualiza la fuente de datos de la colección en la vista
            collectionViewTarjetas.ItemsSource = Tarjetas;

        }catch (Exception ex)
        {
            // Manejo de excepciones
        }
    }

    // Método para manejar el comando de tap en una tarjeta
    private void HandleTappedCommand(TarjetaModel tarjeta)
    {
        SelectedTarjeta = tarjeta;

        // Restablece el color de fondo de todas las tarjetas
        foreach (var item in Tarjetas)
        {
            item.FrameBackgroundColor = Color.FromRgb(255, 250, 240);
        }

        // Resalta la tarjeta seleccionada
        var selectedItem = Tarjetas.FirstOrDefault(d => d.IdTarjeta == tarjeta.idtarjeta);
        if (selectedItem != null)
        {
            selectedItem.FrameBackgroundColor = Color.FromRgb(65, 185, 254);
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
    
    // Método para manejar el evento de clic en el botón de siguiente
    private async void btnSiguiente_Clicked(object sender, EventArgs e)
    {
        // Verifica si se ha seleccionado una tarjeta
        if (SelectedTarjeta == null)
        {
            await DisplayAlert("Aviso", "Por favor seleccione una tarjeta de pago o agregue una nueva", "OK");
            return;
        }

        // Crea un objeto con los datos combinados de la orden, la dirección y la tarjeta seleccionada
        var data = new PagoDireccionTotalesModel
        {
            TotalPrecio = TotalPrecio,
            ISV = ISV,
            Envio = Envio,
            Total = Total
        };

        // Navega a la página de confirmación de orden, pasando los datos combinados como contexto de enlace
        var combinedData = new Tuple<PagoDireccionTotalesModel, DireccionModel, TarjetaModel>(data, SelectedDireccion, SelectedTarjeta);
        await Navigation.PushAsync(new Views.Productos.confirmarOrden { BindingContext = combinedData });
    }

    // Método para manejar el evento de tap en la opción de nueva tarjeta
    private void TapGestureNuevaTarjeta_Tapped(object sender, TappedEventArgs e)
    {
         Navigation.PushAsync(new AgregarTarjetas(1));

    }
}