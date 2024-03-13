using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login;

public partial class CrearCuenta : ContentPage
{
    private ApiService _apiService;

    private string? Nombre;
    private string? Apellido;
    private string? Correo;
    private string? Telefono;
    private string? Usuario;
    private string? Contrasenia;
    private string? EnlaceFoto = "https://firebasestorage.googleapis.com/v0/b/floristeriamargaritas-c74d1.appspot.com/o/Usuarios%2FdefaultUser.png?alt=media";
    private string? codigoVerificacion;
    private bool notValid = false;

    //Conteo de Reenvio
    private int countdownSeconds = 30;
    private bool isCountdownRunning = false;

    public CrearCuenta()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
        _apiService = new ApiService();

        labelEnviarCog.Text = "Enviar de nuevo";
        labelEnviarCog.GestureRecognizers.Add(new TapGestureRecognizer
        {
            NumberOfTapsRequired = 1,
            Command = new Command(StartCountdown)
        });
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is crearCuentaModel data)
        {
            codigoVerificacion = data.Codigo;
            Nombre = data.Nombre;
            Apellido = data.Apellido;
            Correo = data.Correo;
            Telefono = data.Telefono;
            Usuario = data.Usuario;
            Contrasenia = data.Contrasenia;           
        }
    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void btnVerificar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(entryCodigo.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese el c�digo de verificaci�n que recibi� en su correo electr�nico o pida un nuevo c�digo.", "OK");
            return;
        }
        
        if(notValid)
        {
            await DisplayAlert("Alerta", "Esta sesi�n ya no es v�lida.", "OK");
            await Navigation.PushAsync(new Views.Login.login());
            return;
        }

        if(entryCodigo.Text == codigoVerificacion)
        {
            string encryptedPassword = PasswordHandler.EncryptPassword(Contrasenia);

            var data = new
            {
                nombres = Nombre,
                apellidos = Apellido,
                correo = Correo,
                enlacefoto = EnlaceFoto,
                telefono = Telefono,
                usuario = Usuario,
                contrasenia = encryptedPassword,
                fk_idtipousuario = 1
            };

            bool isSuccess = await _apiService.PostSuccessAsync("insertarCliente.php", data);
            if (isSuccess)
            {
                notValid = true;
                await DisplayAlert("�Felicidades!", "Su cuenta ha sido creada, haga clic en OK e ingrese sus datos para iniciar sesi�n.", "OK");
                await Navigation.PushAsync(new Views.Login.login());
            }
        }
        else
        {
            await DisplayAlert("Alerta", "El c�digo de verificaci�n ingresado es incorrecto porfavor ingr�selo de nuevo o pida un nuevo c�digo.", "OK");
            return;
        }
    }

    private void StartCountdown()
    {
        if (!isCountdownRunning)
        {
            isCountdownRunning = true;
            _ = RunCountdown();
        }
    }

    private async Task RunCountdown()
    {
        while (countdownSeconds > 0)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                labelEnviarCog.Text = $"Enviar de nuevo ({countdownSeconds}s)";
                labelEnviarCog.IsEnabled = false; // Disable the label during countdown
            });

            await Task.Delay(1000); // Wait for 1 second
            countdownSeconds--;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            labelEnviarCog.Text = "Enviar de nuevo";
            labelEnviarCog.IsEnabled = true; // Enable the label after countdown finishes
            isCountdownRunning = false;
            countdownSeconds = 30; // Reset the countdown for the next time
        });
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var resultadoCorreo = await _apiService.PostDataAsync<codigoVerificacionModel>("correoCodigoVerificacion.php", new { email = Correo, name = Nombre });
        codigoVerificacion = resultadoCorreo.verification_code;
        await DisplayAlert("Alerta", "C�digo de Verificaci�n Reenviado", "OK");
    }
}