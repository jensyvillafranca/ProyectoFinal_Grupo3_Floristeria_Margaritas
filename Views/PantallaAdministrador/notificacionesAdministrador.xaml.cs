namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallaAdministrador;

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using System.Collections.ObjectModel;

public partial class notificacionesAdministrador : ContentPage
{
    private ApiService _apiService = new ApiService();
    public ObservableCollection<SolicitudesViewModel> SolicitudesPendientes { get; set; }
    public ObservableCollection<SolicitudesViewModel> SolicitudesProcesadas { get; set; }
    public notificacionesAdministrador()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;

        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        //CollectionView Solicitudes Pendientes
        try
        {
            var pendientes = await _apiService.PostDataAsync<SolicitudModel[]>("obtenerSolicitudes.php", new { tipo = 1 });
            SolicitudesPendientes = new ObservableCollection<SolicitudesViewModel>();

            foreach (var solicitud in pendientes)
            {
                bool moto = false;
                bool pesada = false;
                bool liviana = false;

                Color? color = Color.FromRgb(0, 191, 255);

                AgeCalculator ageCalculator = new AgeCalculator();

                int age = ageCalculator.CalculateAge(solicitud.fechanacimiento);

                string? fechaFormateada = (solicitud.fechasolicitud).ToString("d/M/yyyy");

                if(solicitud.licencia_motocicleta == 1)
                {
                    moto = true;
                }

                if(solicitud.licencia_pesada == 1)
                {
                    pesada = true;
                }

                if(solicitud.licencia_liviana == 1)
                {
                    liviana = true;
                }


                SolicitudesViewModel solicitudesViewModel = new SolicitudesViewModel
                {
                    IdSolicitud = solicitud.idsolicitud,
                    NombreCompleto = solicitud.nombrecompleto,
                    Correo = solicitud.correo,
                    Telefono = solicitud.telefono,
                    Dni = solicitud.dni,
                    FechaNacimiento = age.ToString(),
                    Departamento = solicitud.departamento,
                    Genero = solicitud.genero,
                    Motocicleta = moto,
                    Pesada = pesada,
                    Liviana = liviana,
                    FechaSolicitud = fechaFormateada,
                    FrameBackgroundColor = color,
                    TextColor = color,
                    FotoCommand = new Command(() => HandleFotoCommand(solicitud)),
                    AceptarCommand = new Command(() => HandleAceptarCommand(solicitud)),
                    DenegarCommand = new Command(() => HandleDenegarCommand(solicitud))
                };

                SolicitudesPendientes.Add(solicitudesViewModel);
            }

            collectionViewPendientes.ItemsSource = SolicitudesPendientes;
        }
        catch (Exception ex)
        {

        }

        //CollectionView Historial
        try
        {
            var procesadas = await _apiService.PostDataAsync<SolicitudModel[]>("obtenerSolicitudes.php", new { tipo = 2 });
            SolicitudesProcesadas = new ObservableCollection<SolicitudesViewModel>();

            foreach (var solicitud in procesadas)
            {
                bool moto = false;
                bool pesada = false;
                bool liviana = false;
                bool enabledAceptar = false;
                bool enabledDenegar = false;
                string? estado = null;

                Color? color = null;

                if (solicitud.idestado == 1)
                {
                    color = Color.FromRgb(0, 128, 0);
                    enabledAceptar = false;
                    enabledDenegar = false;
                    estado = "Aceptada";
                }
                else if (solicitud.idestado == 2)
                {
                    color = Color.FromRgb(255, 0, 0);
                    enabledAceptar = true;
                    enabledDenegar = false;
                    estado = "Denegada";
                }

                AgeCalculator ageCalculator = new AgeCalculator();

                int age = ageCalculator.CalculateAge(solicitud.fechanacimiento);

                string? fechaFormateada = (solicitud.fechasolicitud).ToString("d/M/yyyy");

                if (solicitud.licencia_motocicleta == 1)
                {
                    moto = true;
                }

                if (solicitud.licencia_pesada == 1)
                {
                    pesada = true;
                }

                if (solicitud.licencia_liviana == 1)
                {
                    liviana = true;
                }


                SolicitudesViewModel solicitudesViewModel = new SolicitudesViewModel
                {
                    IdSolicitud = solicitud.idsolicitud,
                    NombreCompleto = solicitud.nombrecompleto,
                    Correo = solicitud.correo,
                    Telefono = solicitud.telefono,
                    Dni = solicitud.dni,
                    FechaNacimiento = age.ToString(),
                    Departamento = solicitud.departamento,
                    Genero = solicitud.genero,
                    Motocicleta = moto,
                    Pesada = pesada,
                    Liviana = liviana,
                    FechaSolicitud = fechaFormateada,
                    FrameBackgroundColor = color,
                    TextColor = color,
                    EnabledAceptar = enabledAceptar,
                    EnabledDenegar = enabledDenegar,
                    Estado = estado,
                    FotoCommand = new Command(() => HandleFotoCommand(solicitud)),
                    AceptarCommand = new Command(() => HandleAceptarCommand(solicitud)),
                    DenegarCommand = new Command(() => HandleDenegarCommand(solicitud))
                };

                SolicitudesProcesadas.Add(solicitudesViewModel);
            }

            collectionViewProcesadas.ItemsSource = SolicitudesProcesadas;
        }
        catch (Exception ex)
        {

        }

    }

    private async void HandleFotoCommand(SolicitudModel solicitud)
    {
        await Navigation.PushAsync(new Views.PantallaAdministrador.verImagenesSolicitud(solicitud));
    }

    private async void HandleAceptarCommand(SolicitudModel solicitud)
    {
        bool userConfirmed = await DisplayAlert("Confirmación", $"¿Está seguro de que desea aceptar la solicitud de repartidor para: {solicitud.nombrecompleto}?", "Si", "No");

        if (userConfirmed)
        {
            var data = new
            {
                idsolicitud = solicitud.idsolicitud,
                nombres = solicitud.nombre,
                apellidos = solicitud.apellido,
                correo = solicitud.correo,
                telefono = solicitud.telefono,
                dni = solicitud.dni,
                fechanacimiento = solicitud.fechanacimiento,
                idciudad = solicitud.idciudad,
                genero = solicitud.genero
            };

            bool isSuccess = await _apiService.PostSuccessAsync("aceptarSolicitud.php", data);

            if (isSuccess)
            {
                await DisplayAlert("Alerta", "La Solicitud ha sido aceptada", "OK");
                await Navigation.PushAsync(new Views.PantallaAdministrador.notificacionesAdministrador());
            }
            else
            {
                await DisplayAlert("Alerta", "Ocurrio un error, intente de nuevo", "OK");
                return;
            }
        }

    }

    private async void HandleDenegarCommand(SolicitudModel solicitud)
    {
        bool userConfirmed = await DisplayAlert("Confirmación", $"¿Está seguro de que desea denegar la solicitud de repartidor para: {solicitud.nombrecompleto}?", "Si", "No");
        if (userConfirmed)
        {
            var data = new
            {
                idsolicitud = solicitud.idsolicitud,
                nombre = solicitud.nombrecompleto,
                correo = solicitud.correo
            };

            bool isSuccess = await _apiService.PostSuccessAsync("denegarSolicitud.php", data);

            if (isSuccess)
            {
                await DisplayAlert("Alerta", "La Solicitud ha sido denegada", "OK");
                await Navigation.PushAsync(new Views.PantallaAdministrador.notificacionesAdministrador());
            }
            else
            {
                await DisplayAlert("Alerta", "Ocurrio un error, intente de nuevo", "OK");
                return;
            }
        }
    }

    private async void btnAtras_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void btnNotification_Clicked(object sender, EventArgs e)
    {

    }

    private async void btnHome_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Home.homePageAdmin());
    }

    private async void btnStock_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.CreacionProductos.HistorialProductosAgregados());
    }

    private void btnEstadisticas_Clicked(object sender, EventArgs e)
    {

    }

    private void btnAnuncios_Clicked(object sender, EventArgs e)
    {

    }

    private void btnPerfil_Clicked(object sender, EventArgs e)
    {

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