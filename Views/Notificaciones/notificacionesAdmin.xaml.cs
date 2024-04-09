using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using System.Collections.ObjectModel;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Notificaciones;

public partial class notificacionesAdmin : ContentPage
{
    private ApiService _apiService = new ApiService();
    public ObservableCollection<notificacionRepartidorViewModel> Notificaciones { get; set; }
    public notificacionesAdmin()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
    }

    // Método para cargar las notificaciones al aparecer la página
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Obtiene las notificaciones del repartidor desde la API
        try
        {
            var notificaciones = await _apiService.PostDataAsync<notificacionAdminModel[]>("obtenerNotificacionesAdmin.php", new { idadmin = Config.Config.activeAdminId });
            Notificaciones = new ObservableCollection<notificacionRepartidorViewModel>();

            // Procesa las notificaciones y las agrega a la colección
            foreach (var notificacion in notificaciones)
            {
                string? image = "ImagenesAdministrador/solicitud.png";
                string? estado = null;
                Color? color = null;

                if (notificacion.lectura == 0)
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

                // Crea un objeto notificacionRepartidorViewModel para cada notificación
                notificacionRepartidorViewModel notificacionRepartidorViewModel = new notificacionRepartidorViewModel
                {
                    IdItem = notificacion.idnotificacion,
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
                Notificaciones.Add(notificacionRepartidorViewModel);

                // Actualiza el estado de la notificación a leída si aún no lo está
                if (notificacion.lectura == 0)
                {
                    try
                    {
                        var resultado = await _apiService.PostDataAsync<existeUsuario>("updateNotificacionAdmin.php", new { idnotificacion = notificacion.idnotificacion });
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

    private async void HandleTappedCommand(notificacionAdminModel notificacion)
    {
        await Navigation.PushAsync(new Views.PantallaAdministrador.notificacionesAdministrador());
    }

    // Controlador de eventos para el clic en el botón de eliminar notificación
    private async void HandleEliminarCommand(notificacionAdminModel notificacion)
    {
        // Busca la notificación en la colección
        var tappedItem = Notificaciones.FirstOrDefault(item => item.IdItem == notificacion.idnotificacion);

        // Solicita confirmación al usuario para eliminar la notificación
        bool userConfirmed = await DisplayAlert("Confirmación", "¿Está seguro de que desea eliminar esta notificación?", "Si", "No");

        // Si el usuario confirma, procede a eliminar la notificación
        if (userConfirmed)
        {
            try
            {
                var resultado = await _apiService.PostDataAsync<existeUsuario>("deleteNotificacionAdmin.php", new { idnotificacion = notificacion.idnotificacion });
                bool existe = resultado.existe;

                // Si la notificación se eliminó correctamente, se quita de la colección
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
                // Maneja los errores de eliminación de notificaciones
            }
        }
    }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Home.homePageAdmin());
    }

    private async void btnRefresh_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Notificaciones.notificacionesAdmin());
    }
}