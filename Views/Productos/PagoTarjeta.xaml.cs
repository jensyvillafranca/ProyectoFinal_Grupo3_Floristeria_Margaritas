using System.Collections.ObjectModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Config;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos;

public partial class PagoTarjeta : ContentPage
{
    public ObservableCollection<TarjetasViewModel> Tarjetas { get; set; }
    private ApiService _apiService;

    //data recibida de la pantalla anterior
    private double TotalPrecio;
    private double ISV;
    private double Envio;
    private double Total;
    private Modelos.DireccionModel SelectedDireccion { get; set; }

    private Modelos.TarjetaModel SelectedTarjeta { get; set; }
    public PagoTarjeta()
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

        if (BindingContext is Tuple<PagoDireccionTotalesModel, DireccionModel> combinedData)
        {
            PagoDireccionTotalesModel data = combinedData.Item1;
            DireccionModel selectedDireccion = combinedData.Item2;

            TotalPrecio = (data.TotalPrecio);
            ISV = (data.ISV);
            Envio = (data.Envio);
            Total = (data.Total);
            SelectedDireccion = selectedDireccion;

            labelSubtotal.Text = $"L {Math.Round(data.TotalPrecio, 2):F2}";
            labelISV.Text = $"L {Math.Round(data.ISV, 2):F2}";
            labelEnvio.Text = $"L {Math.Round(data.Envio, 2):F2}";
            labelTotal.Text = $"L {Math.Round(data.Total, 2):F2}";

            MessagingCenter.Subscribe<object>(this, "UpdateCollectionView", (sender) =>
            {
                InitializeAsync();
            });
        }
    }

    private async void InitializeAsync()
    {
        var tarjetas = await _apiService.PostDataAsync<TarjetaModel[]>("obtenerTarjetaPorID.php", new { idcliente = Config.Config.activeUserId });

        Tarjetas = new ObservableCollection<TarjetasViewModel>();

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

        collectionViewTarjetas.ItemsSource = Tarjetas;
    }

    private void HandleTappedCommand(TarjetaModel tarjeta)
    {
        SelectedTarjeta = tarjeta;

        foreach (var item in Tarjetas)
        {
            item.FrameBackgroundColor = Color.FromRgb(255, 250, 240);
        }

        var selectedItem = Tarjetas.FirstOrDefault(d => d.IdTarjeta == tarjeta.idtarjeta);
        if (selectedItem != null)
        {
            selectedItem.FrameBackgroundColor = Color.FromRgb(65, 185, 254);
        }
    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private void btnCancelar_Clicked(object sender, EventArgs e)
    {
        Navigation.PopToRootAsync();
    }

    private async void btnSiguiente_Clicked(object sender, EventArgs e)
    {
        if(SelectedTarjeta == null)
        {
            await DisplayAlert("Aviso", "Por favor seleccione una tarjeta de pago o agregue una nueva", "OK");
            return;
        }

        var data = new PagoDireccionTotalesModel
        {
            TotalPrecio = TotalPrecio,
            ISV = ISV,
            Envio = Envio,
            Total = Total
        };

        var combinedData = new Tuple<PagoDireccionTotalesModel, DireccionModel, TarjetaModel>(data, SelectedDireccion, SelectedTarjeta);
        await Navigation.PushAsync(new Views.Productos.confirmarOrden { BindingContext = combinedData });
    }

    private void TapGestureNuevaTarjeta_Tapped(object sender, TappedEventArgs e)
    {

    }
}