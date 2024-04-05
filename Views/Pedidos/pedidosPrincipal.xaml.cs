/*
 * Descripci�n:
 * Este c�digo define la l�gica de backend para la p�gina 'pedidosPrincipal' de la aplicaci�n Floristeria Margaritas.
 * Permite al usuario ver sus pedidos activos y su historial de pedidos, as� como acceder a otras funciones como notificaciones, productos y perfil.
 */

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Config;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using System.Collections.ObjectModel;
using System;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Pedidos;

public partial class pedidosPrincipal : ContentPage
{
    private ApiService _apiService = new ApiService();
    public ObservableCollection<pedidosViewModel> PedidosActivos { get; set; }
    public ObservableCollection<pedidosViewModel> Historial { get; set; }

    // Constructor
    public pedidosPrincipal()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;

        InitializeAsync();
    }

    // M�todo que se ejecuta cuando la p�gina se muestra
    protected async override void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // Verificar si existen notificaciones para el usuario y actualizar el �cono de notificaci�n
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
            // Manejar cualquier excepci�n que ocurra durante la verificaci�n de notificaciones
        }
    }

    // M�todo as�ncrono para inicializar la p�gina y cargar los pedidos activos e historial
    private async void InitializeAsync()
    {
        //CollectionView Pedidos Activos
        try
        {
            var pedidosActivos = await _apiService.PostDataAsync<pedidoModel[]>("obtenerPedidos.php", new { idcliente = Config.Config.activeUserId, tipo = 1 });
            PedidosActivos = new ObservableCollection<pedidosViewModel>();

            foreach (var pedido in pedidosActivos)
            {
                string? image = null;
                string? estado = null;
                Color? color = null;

                if(pedido.idestadopedido == 2 || pedido.idestadopedido == 1)
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

                string? horaFormateada = (pedido.fechaestimadaentrega).ToString("h:mm tt");
                string? fechaFormateada = (pedido.fechapedido).ToString("d/M/yyyy");


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

                PedidosActivos.Add(pedidosViewModel);
            }

            collectionViewPedidoActivo.ItemsSource = PedidosActivos;
        }
        catch (Exception ex)
        {
            //Agrega uno predeterminado para indicar que no hay un historial
            PedidosActivos = new ObservableCollection<pedidosViewModel>();

            pedidosViewModel pedidosViewModel = new pedidosViewModel
            {
                IdPedido = "�No Existe Ning�n Pedido Activo!",
                ImageSource = "Estados/empty.png",
                FrameBackgroundColor = Color.FromRgb(255, 0, 0),
                Visibilidad = false
            };

            PedidosActivos.Add(pedidosViewModel);

            collectionViewPedidoActivo.ItemsSource = PedidosActivos;
        }

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
                IdPedido = "�No Existe Ning�n Pedido!",
                ImageSource = "Estados/empty.png",
                FrameBackgroundColor = Color.FromRgb(255, 0, 0),
                Visibilidad = false
            };

            Historial.Add(pedidosViewModel);

            collectionViewHistorial.ItemsSource = Historial;
        }

    }

    // M�todo para manejar el comando Tapped cuando se selecciona un pedido
    private async void HandleTappedCommand(pedidoModel pedido)
    {
        await Navigation.PushAsync(new Views.Pedidos.detallePedido(pedido));
    }

    // M�todo para manejar el evento Clicked del bot�n de notificaci�n
    private async void btnNotification_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Notificaciones.notificacionesEstadoPedidos());
    }

    // M�todos para manejar eventos Clicked de los botones de navegaci�n y acciones del usuario
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
}