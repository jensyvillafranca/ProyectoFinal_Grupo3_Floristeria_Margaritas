/*
 * Descripción:
 * Este código define la lógica de backend para la página 'notificacionesEstadoPedidos' de la aplicación Floristeria Margaritas, destinada a mostrar notificaciones de cambios de estado de pedidos para los clientes.
 * Incluye la carga y gestión de notificaciones de estado específicas para los pedidos de los clientes, como la visualización y eliminación de notificaciones.
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

    // Constructor para la página 'notificacionesEstadoPedidos'
    public notificacionesEstadoPedidos()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
    }

    // Método para cargar las notificaciones al aparecer la página
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // Obtiene las notificaciones de estado del pedido del cliente desde la API
            var notificaciones = await _apiService.PostDataAsync<notificacionEstadoModel[]>("obtenerNotificacionesEstado.php", new { idcliente = Config.Config.activeUserId});
            Notificaciones = new ObservableCollection<notificacionEstadoViewModel>();

            // Procesa las notificaciones y las agrega a la colección
            foreach (var notificacion in notificaciones)
            {
                string? image = null;
                string? estado = null;
                Color? color = null;

                // Determina la imagen, estado y color de fondo de la notificación según el estado del pedido
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

                // Crea un objeto notificacionEstadoViewModel para cada notificación
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

                // Agrega el ViewModel a la colección de notificaciones
                Notificaciones.Add(notificacionEstadoViewModel);

                // Actualiza el estado de la notificación a leída si aún no lo está
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

            // Actualiza el origen de datos de la colección
            collectionViewNotificaciones.ItemsSource = Notificaciones;
        }
        catch (Exception ex)
        {
            // Maneja los errores de carga de notificaciones
        }
    }

    // Controlador de eventos para el clic en una notificación
    private async void HandleTappedCommand(notificacionEstadoModel notificacion)
    {
        await Navigation.PushAsync(new Views.Pedidos.pedidosPrincipal());
    }

    // Controlador de eventos para el clic en el botón de eliminar notificación
    private async void HandleEliminarCommand(notificacionEstadoModel notificacion)
    {
        // Busca la notificación en la colección
        var tappedItem = Notificaciones.FirstOrDefault(item => item.IdItem == notificacion.idnotificacionpedido);

        // Solicita confirmación al usuario para eliminar la notificación
        bool userConfirmed = await DisplayAlert("Confirmación", "¿Está seguro de que desea eliminar esta notificación?", "Si", "No");

        // Si el usuario confirma, procede a eliminar la notificación
        if (userConfirmed)
        {
            try
            {
                var resultado = await _apiService.PostDataAsync<existeUsuario>("deleteNotificacionEstado.php", new { idnotificacion = notificacion.idnotificacionpedido });
                bool existe = resultado.existe;

                //Si la notificación se eliminó correctamente, se quita de la colección
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

    // Controlador de eventos para el clic en el botón de retroceso
    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    // Controlador de eventos para el clic en el botón de actualización
    private async void btnRefresh_Clicked(object sender, EventArgs e)
    {
        OnAppearing();
    }
}