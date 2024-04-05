using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using System.Collections.ObjectModel;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Profile;

public partial class HistorialCompras : ContentPage
{
    private ApiService _apiService = new ApiService();
    public ObservableCollection<pedidosViewModel> Historial { get; set; }
    public HistorialCompras()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;

        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        //CollectionView Historial
        try
        {
            var historialPedidos = await _apiService.PostDataAsync<pedidoModel[]>("obtenerPedidos.php", new { idcliente = Config.Config.activeUserId, tipo = 2 });
            Historial = new ObservableCollection<pedidosViewModel>();

            foreach (var pedido in historialPedidos)
            {
                string? image = null;
                string? estado = null;
                Color? color = null;

                if (pedido.idestadopedido == 2 || pedido.idestadopedido == 1)
                {
                    image = "Estados/procesando.png";
                    estado = "Procesando";
                    color = Color.FromRgb(204, 204, 0);
                }
                else if (pedido.idestadopedido == 3)
                {
                    image = "Estados/encamino.png";
                    estado = "En Camino";
                    color = Color.FromRgb(0, 191, 255);
                }
                else if (pedido.idestadopedido == 4)
                {
                    image = "Estados/delivery.jpg";
                    estado = "Entregado";
                    color = Color.FromRgb(0, 128, 0);
                }
                else if (pedido.idestadopedido == 5)
                {
                    image = "Estados/cancelado.jpg";
                    estado = "Cancelado";
                    color = Color.FromRgb(255, 0, 0);
                }

                string? horaFormateada;
                string? fechaFormateada = (pedido.fechapedido).ToString("d/M/yyyy");

                //convertir de string a formato hora
                if (pedido.fechaentregado != null)
                {
                    DateTime horaEntregado;

                    DateTime.TryParse(pedido.fechaentregado, out horaEntregado);

                    horaFormateada = horaEntregado.ToString("h:mm tt");
                }
                else
                {
                    horaFormateada = string.Empty;
                }

                pedidosViewModel pedidosViewModel = new pedidosViewModel
                {
                    IdPedido = $"Pedido: #PED-{pedido.idpedido}",
                    ImageSource = image,
                    EstadoPedido = $"Estado: {estado}",
                    DireccionEntrega = pedido.direccion,
                    HoraEntrega = horaFormateada,
                    FechaPedido = fechaFormateada,
                    TotalPedido = pedido.total,
                    FrameBackgroundColor = color,
                    TextColor = color,
                    Visibilidad = true,
                    TappedCommand = new Command(() => HandleTappedCommand(pedido))
                };

                Historial.Add(pedidosViewModel);
            }

            collectionViewHistorial.ItemsSource = Historial;
        }
        catch (Exception ex)
        {
            //Agrega uno predeterminado para indicar que no hay un historial
            Historial = new ObservableCollection<pedidosViewModel>();

            pedidosViewModel pedidosViewModel = new pedidosViewModel
            {
                IdPedido = "¡No Existe Ningún Pedido!",
                ImageSource = "Estados/empty.png",
                FrameBackgroundColor = Color.FromRgb(255, 0, 0),
                Visibilidad = false
            };

            Historial.Add(pedidosViewModel);

            collectionViewHistorial.ItemsSource = Historial;
        }

    }

    private async void HandleTappedCommand(pedidoModel pedido)
    {
        await Navigation.PushAsync(new Views.Pedidos.detallePedido(pedido));
    }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}