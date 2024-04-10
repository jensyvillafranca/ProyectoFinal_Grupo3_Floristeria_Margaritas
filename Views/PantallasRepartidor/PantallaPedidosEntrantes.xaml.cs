using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.RestApi;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallasRepartidor;

public partial class PantallaPedidosEntrantes : ContentPage{
    private ApiService _apiService = new ApiService();
    public ObservableCollection<PedidoActivoViewModel> PedidoActivo { get; set; }
    public ObservableCollection<TableJoinQueries> Items{
        get; set;
    }

    public PantallaPedidosEntrantes(){
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        Items =new ObservableCollection<TableJoinQueries>();
    }

    protected override async void OnAppearing() {
        base.OnAppearing();
        Items.Clear();

        await load_data();
        this.BindingContext=this;

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
            // Manejar cualquier excepción que ocurra durante la obtención de productos del detalle del pedido
        }

        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        //CollectionView Pedido Activo
        try
        {
            var pedidosActivos = await _apiService.PostDataAsync<pedidoActivoModel[]>("obtenerPedidoActivo.php", new { idrepartidor = Config.Config.activeRepartidorId });
            PedidoActivo = new ObservableCollection<PedidoActivoViewModel>();

            foreach (var pedido in pedidosActivos)
            {
                string? image = null;
                string? estado = null;
                Color? color = null;


                PedidoActivoViewModel pedidoActivoViewModel = new PedidoActivoViewModel
                {
                    Direccion = pedido.dir_direccion,
                    NombreSucursal = pedido.suc_nombresucursal,
                    IdEstadoPedido = pedido.ped_idestadopedido,
                    EnlaceFoto = pedido.ped_enlacefoto,
                    NombreCompleto = pedido.cli_nombrecompleto,
                    IdPedido = pedido.ped_idpedido,
                    TappedCommand = new Command(() => HandleTappedCommand(pedido))
                };

                PedidoActivo.Add(pedidoActivoViewModel);
            }

            collectionViewPedidoActivo.ItemsSource = PedidoActivo;
        }
        catch (Exception ex)
        {
            //Agrega uno predeterminado para indicar que no hay un historial
        }
    }

    private async void HandleTappedCommand(pedidoActivoModel pedido)
    {
        if (pedido.ped_idestadopedido == 1)
        {
            await Navigation.PushAsync(new DetallePedido(pedido.ped_idpedido, 2));
        }
        else if ( pedido.ped_idestadopedido == 3)
        {
            await Navigation.PushAsync(new MapaEntregaCliente(pedido.ped_idpedido, Config.Config.sucursal));
        }
        
    }

    private async Task load_data() {
        try {
            var services_bd = await get_data();

            if(services_bd!=null) {
                foreach(var data in services_bd){
                    if(data.ped_idestadopedido!=3){
                        data.enlace_status="ImagenesRepartidor/check.png";
                    }else{
                        data.enlace_status="ImagenesRepartidor/cliente.png";
                    }

                    Items.Add(data);
                }
            }

        } catch(Exception ex) {
            await DisplayAlert("Advertencia","error: "+ex.ToString(),"OK");
        }
    }

    private async Task<List<TableJoinQueries>?> get_data(){
        TableJoinQueries data = new TableJoinQueries();
        data.ped_fk_idrepartidor=Config.Config.activeRepartidorId; 
        string response = "";

        try {
            response=await Task.Run(() => Methods.select_async(data,RestApiData.select_pedidos));

            if(!string.IsNullOrEmpty(response)) {
                List<TableJoinQueries> list = JsonSerializer.Deserialize<List<TableJoinQueries>>(response);

                return list;
            }
        } catch(Exception ex){
            await DisplayAlert("Advertencia",""+ex,"OK");
        }

        return null;
    }

    private async void get_specific_data(object sender,TappedEventArgs e) {
        if(PedidoActivo.Count > 0)
        {
            await DisplayAlert("Alerta", "Ya tiene un pedido activo, por favor complete el pedido antes de tomar otro.", "OK");
            return;
        }
        var stackLayout = (StackLayout) sender;
        var item = (TableJoinQueries) stackLayout.BindingContext;

        await Navigation.PushAsync(new DetallePedido(item.ped_idpedido, 1));
    }

    private async void btnInicioRepartidor_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Home.homePageRepartidor());
    }

    private async void btnPedidosRepartidor_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.PantallasRepartidor.PantallaPedidosEntrantes());
    }

    private async void btnIngresosRepartidor_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.PantallasRepartidor.IngresosRepartidor());
    }

    private async void btnHistorialPedidosRepartidor_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.PantallasRepartidor.historialEntregas());
    }

    private async void btnPerfilRepartidor_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.PantallasRepartidor.profilePageRepartidor());
    }

    private async void btnLogOutRepartidor_Clicked(object sender, EventArgs e)
    {
        bool isSuccess = await logout.PerformLogoutAsync(_apiService);

        if (isSuccess)
        {
            await Navigation.PushAsync(new Views.Login.login());
        }
    }

    private async void btnNotification_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Notificaciones.notificacionesRepartidor());
    }
}