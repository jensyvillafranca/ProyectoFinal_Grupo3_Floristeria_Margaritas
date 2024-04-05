/*
 * Descripción:
 * Este código define la lógica de backend para la página 'CrearCuenta' de la aplicación Floristeria Margaritas, que maneja el proceso de creación de cuentas de usuario.
 * Incluye la verificación del código de verificación, el envío de nuevos códigos de verificación, y el inicio de sesión del usuario después de la creación de la cuenta.
 */

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login;

public partial class CrearCuenta : ContentPage
{
    // Instancia de ApiService para interacciones con la API
    private ApiService _apiService;

    // Variables para los datos de la cuenta a crear
    private string? Nombre;
    private string? Apellido;
    private string? Correo;
    private string? Telefono;
    private string? Usuario;
    private string? Contrasenia;
    private string? EnlaceFoto = "https://storage.googleapis.com/floristeriamargaritas-c74d1.appspot.com/Usuarios/defaultUser.png";
    private string? codigoVerificacion;
    private bool notValid = false;

    //Conteo de Reenvio
    private int countdownSeconds = 30;
    private bool isCountdownRunning = false;

    // Constructor para la página 'CrearCuenta'
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

    // Método que se ejecuta al aparecer la página
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

    // Controlador de eventos para clic en el botón de retroceso
    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    // Controlador de eventos para clic en el botón de verificación
    private async void btnVerificar_Clicked(object sender, EventArgs e)
    {
        // Validar el código de verificación ingresado
        // Crear la cuenta si el código es correcto
        // Mostrar alertas en caso de errores
        if (string.IsNullOrEmpty(entryCodigo.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese el código de verificación que recibió en su correo electrónico o pida un nuevo código.", "OK");
            return;
        }
        
        if(notValid)
        {
            await DisplayAlert("Alerta", "Esta sesión ya no es válida.", "OK");
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
                await DisplayAlert("¡Felicidades!", "Su cuenta ha sido creada, haga clic en OK e ingrese sus datos para iniciar sesión.", "OK");
                await Navigation.PushAsync(new Views.Login.login());
            }
        }
        else
        {
            await DisplayAlert("Alerta", "El código de verificación ingresado es incorrecto porfavor ingréselo de nuevo o pida un nuevo código.", "OK");
            return;
        }
    }

    // Método para iniciar el conteo regresivo
    private void StartCountdown()
    {
        if (!isCountdownRunning)
        {
            isCountdownRunning = true;
            _ = RunCountdown();
        }
    }

    // Método para ejecutar el conteo regresivo
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

    // Controlador de eventos para toque en la etiqueta de reenvío de código
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var resultadoCorreo = await _apiService.PostDataAsync<codigoVerificacionModel>("correoCodigoVerificacion.php", new { email = Correo, name = Nombre });
        codigoVerificacion = resultadoCorreo.verification_code;
        await DisplayAlert("Alerta", "Código de Verificación Reenviado", "OK");
    }
}