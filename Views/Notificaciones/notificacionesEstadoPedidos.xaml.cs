using Org.Apache.Http.Cookies;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using System.Collections.ObjectModel;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Notificaciones;

public partial class notificacionesEstadoPedidos : ContentPage
{
    private ApiService _apiService = new ApiService();
    public ObservableCollection<notificacionEstadoViewModel> Notificaciones { get; set; }
    public notificacionesEstadoPedidos()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var notificaciones = await _apiService.PostDataAsync<notificacionEstadoModel[]>("obtenerNotificacionesEstado.php", new { idcliente = Config.Config.activeUserId});
            Notificaciones = new ObservableCollection<notificacionEstadoViewModel>();

            foreach (var notificacion in notificaciones)
            {
                string? image = null;
                string? estado = null;
                Color? color = null;

                if (notificacion.idestadopedido == 2 || notificacion.idestadopedido == 1)
                {
                    image = "Estados/procesando.png";
                }
                else if (notificacion.idestadopedido == 3)
                {
                    image = "Estados/encamino.png";                    
                }
                else if (notificacion.idestadopedido == 4)
                {
                    image = "Estados/delivery.jpg";
                }
                else if (notificacion.idestadopedido == 5)
                {
                    image = "Estados/cancelado.jpg";
                }

                if(notificacion.lectura == 0)
                {
                    estado = "Nueva Notificación";
                    color = Color.FromRgb(255, 0, 0);
                }
                else
                {
                    estado = "Leída";
                    color = Color.FromRgb(0, 191, 255);
                }

                string? fechaFormateada = (notificacion.created).ToString("d/M/yyyy h:mm tt");


                notificacionEstadoViewModel notificacionEstadoViewModel = new notificacionEstadoViewModel
                {
                    IdItem = notificacion.idnotificacionpedido,
                    ImageSource = image,
                    Titulo = notificacion.titulo,
                    Cuerpo = notificacion.cuerpo,
                    Fecha = fechaFormateada,
                    Lectura = estado,
                    FrameBackgroundColor = color,
                    TextColor = color,
                    TappedCommand = new Command(() => HandleTappedCommand(notificacion)),
                    EliminarCommand = new Command(() => HandleEliminarCommand(notificacion))
                };

                Notificaciones.Add(notificacionEstadoViewModel);

                if (notificacion.lectura == 0)
                {
                    try
                    {
                        var resultado = await _apiService.PostDataAsync<existeUsuario>("updateNotificacionLecturaEstado.php", new { idnotificacion = notificacion.idnotificacionpedido });
                        bool existe = resultado.existe;
                        if (!existe) 
                        {
                            Console.WriteLine("Error de update en Notificacion");
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            collectionViewNotificaciones.ItemsSource = Notificaciones;
        }
        catch (Exception ex)
        {

        }
    }

    private async void HandleTappedCommand(notificacionEstadoModel notificacion)
    {
        await Navigation.PushAsync(new Views.Pedidos.pedidosPrincipal());
    }

    private async void HandleEliminarCommand(notificacionEstadoModel notificacion)
    {
        var tappedItem = Notificaciones.FirstOrDefault(item => item.IdItem == notificacion.idnotificacionpedido);

        bool userConfirmed = await DisplayAlert("Confirmación", "¿Está seguro de que desea eliminar esta notificación?", "Si", "No");

        if (userConfirmed)
        {
            try
            {
                var resultado = await _apiService.PostDataAsync<existeUsuario>("deleteNotificacionEstado.php", new { idnotificacion = notificacion.idnotificacionpedido });
                bool existe = resultado.existe;

                if (!existe)
                {
                    Notificaciones.Remove(tappedItem);

                }
                else
                {
                    await DisplayAlert("Aviso", "Se ha producido un error, por favor intente de nuevo", "OK");
                }
            }
            catch (Exception ex) 
            {

            }
        }
    }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void btnRefresh_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Notificaciones.notificacionesEstadoPedidos());
    }
}