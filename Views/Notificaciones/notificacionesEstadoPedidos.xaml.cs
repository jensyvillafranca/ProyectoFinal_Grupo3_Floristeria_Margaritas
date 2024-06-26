/*
 * Descripci�n:
 * Este c�digo define la l�gica de backend para la p�gina 'notificacionesEstadoPedidos' de la aplicaci�n Floristeria Margaritas, destinada a mostrar notificaciones de cambios de estado de pedidos para los clientes.
 * Incluye la carga y gesti�n de notificaciones de estado espec�ficas para los pedidos de los clientes, como la visualizaci�n y eliminaci�n de notificaciones.
 */

using Google.Api;
using Org.Apache.Http.Cookies;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using System.Collections.ObjectModel;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Notificaciones;

public partial class notificacionesEstadoPedidos : ContentPage
{
    // Variables de instancia y servicio de API
    private ApiService _apiService = new ApiService();
    public ObservableCollection<notificacionEstadoViewModel> Notificaciones { get; set; }

    // Constructor para la p�gina 'notificacionesEstadoPedidos'
    public notificacionesEstadoPedidos()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
    }

    // M�todo para cargar las notificaciones al aparecer la p�gina
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // Obtiene las notificaciones de estado del pedido del cliente desde la API
            var notificaciones = await _apiService.PostDataAsync<notificacionEstadoModel[]>("obtenerNotificacionesEstado.php", new { idcliente = Config.Config.activeUserId});
            Notificaciones = new ObservableCollection<notificacionEstadoViewModel>();

            // Procesa las notificaciones y las agrega a la colecci�n
            foreach (var notificacion in notificaciones)
            {
                string? image = null;
                string? estado = null;
                Color? color = null;

                // Determina la imagen, estado y color de fondo de la notificaci�n seg�n el estado del pedido
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
                    estado = "Nueva Notificaci�n";
                    color = Color.FromRgb(255, 0, 0);
                }
                else
                {
                    estado = "Le�da";
                    color = Color.FromRgb(0, 191, 255);
                }

                string? fechaFormateada = (notificacion.created).ToString("d/M/yyyy h:mm tt");

                // Crea un objeto notificacionEstadoViewModel para cada notificaci�n
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

                // Agrega el ViewModel a la colecci�n de notificaciones
                Notificaciones.Add(notificacionEstadoViewModel);

                // Actualiza el estado de la notificaci�n a le�da si a�n no lo est�
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

            // Actualiza el origen de datos de la colecci�n
            collectionViewNotificaciones.ItemsSource = Notificaciones;
        }
        catch (Exception ex)
        {
            // Maneja los errores de carga de notificaciones
        }
    }

    // Controlador de eventos para el clic en una notificaci�n
    private async void HandleTappedCommand(notificacionEstadoModel notificacion)
    {
        await Navigation.PushAsync(new Views.Pedidos.pedidosPrincipal());
    }

    // Controlador de eventos para el clic en el bot�n de eliminar notificaci�n
    private async void HandleEliminarCommand(notificacionEstadoModel notificacion)
    {
        // Busca la notificaci�n en la colecci�n
        var tappedItem = Notificaciones.FirstOrDefault(item => item.IdItem == notificacion.idnotificacionpedido);

        // Solicita confirmaci�n al usuario para eliminar la notificaci�n
        bool userConfirmed = await DisplayAlert("Confirmaci�n", "�Est� seguro de que desea eliminar esta notificaci�n?", "Si", "No");

        // Si el usuario confirma, procede a eliminar la notificaci�n
        if (userConfirmed)
        {
            try
            {
                var resultado = await _apiService.PostDataAsync<existeUsuario>("deleteNotificacionEstado.php", new { idnotificacion = notificacion.idnotificacionpedido });
                bool existe = resultado.existe;

                //Si la notificaci�n se elimin� correctamente, se quita de la colecci�n
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

    // Controlador de eventos para el clic en el bot�n de retroceso
    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // Controlador de eventos para el clic en el bot�n de actualizaci�n
    private async void btnRefresh_Clicked(object sender, EventArgs e)
    {
        OnAppearing();
    }
}