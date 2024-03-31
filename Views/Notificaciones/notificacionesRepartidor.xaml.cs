using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using System.Collections.ObjectModel;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Notificaciones;

public partial class notificacionesRepartidor : ContentPage
{
    private ApiService _apiService = new ApiService();
    public ObservableCollection<notificacionRepartidorViewModel> Notificaciones { get; set; }
    public notificacionesRepartidor()
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
            var notificaciones = await _apiService.PostDataAsync<notificacionRepartidorModel[]>("obtenerNotificacionesRepartidor.php", new { idrepartidor = Config.Config.activeRepartidorId });
            Notificaciones = new ObservableCollection<notificacionRepartidorViewModel>();

            foreach (var notificacion in notificaciones)
            {
                string? image = null;
                string? estado = null;
                Color? color = null;

                if (notificacion.tipo == 0)
                {
                    image = "Estados/procesando.png";
                }
                else if (notificacion.tipo == 1)
                {
                    image = "Estados/cancelado.jpg";
                }

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


                notificacionRepartidorViewModel notificacionRepartidorViewModel = new notificacionRepartidorViewModel
                {
                    IdItem = notificacion.idnotificacionrepartidor,
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

                Notificaciones.Add(notificacionRepartidorViewModel);

                if (notificacion.lectura == 0)
                {
                    try
                    {
                        var resultado = await _apiService.PostDataAsync<existeUsuario>("updateNotificacionRepartidor.php", new { idnotificacion = notificacion.idnotificacionrepartidor });
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

    private async void HandleTappedCommand(notificacionRepartidorModel notificacion)
    {
        
    }

    private async void HandleEliminarCommand(notificacionRepartidorModel notificacion)
    {
        var tappedItem = Notificaciones.FirstOrDefault(item => item.IdItem == notificacion.idnotificacionrepartidor);

        bool userConfirmed = await DisplayAlert("Confirmación", "¿Está seguro de que desea eliminar esta notificación?", "Si", "No");

        if (userConfirmed)
        {
            try
            {
                var resultado = await _apiService.PostDataAsync<existeUsuario>("deleteNotificacionRepartidor.php", new { idnotificacion = notificacion.idnotificacionrepartidor });
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
        await Navigation.PushAsync(new Views.Notificaciones.notificacionesRepartidor());
    }
}