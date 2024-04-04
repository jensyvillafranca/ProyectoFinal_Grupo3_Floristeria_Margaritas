/*
 * Descripción:
 * Este código define la lógica de backend para la página 'VerificarContra' de la aplicación Floristeria Margaritas, la cual maneja la verificación del código de restablecimiento de contraseña.
 * Incluye validación del código de verificación y redireccionamiento a la página de restablecimiento de contraseña.
 */

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login;

public partial class VerificarContra : ContentPage
{
    // Variables para el código de verificación y el correo electrónico
    private string? codigoVerificacion;
    private string? Correo;

    // Instancia de ApiService para interacciones con la API
    private ApiService _apiService = new ApiService();

    //Conteo de Reenvio
    private int countdownSeconds = 30;
    private bool isCountdownRunning = false;

    // Modelo de datos para el ID de usuario
    public class idUsuarioModel
    {
        public int idusuario { get; set; }
    }

    // Constructor para la página 'VerificarContra'
    public VerificarContra()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;

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

        if (BindingContext is recuperarPassModel data)
        {
            codigoVerificacion = data.Codigo;
            Correo = data.Correo;
        }
    }

    // Controlador de eventos para clic en el botón de verificar código
    private async void btnVerificar_Clicked(object sender, EventArgs e)
    {
        // Validar si se ingresó un código de verificación
        if (string.IsNullOrEmpty(entryCodigo.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese el código de verificación que recibió en su correo electrónico o pida un nuevo código.", "OK");
            return;
        }

        // Verificar si el código ingresado es correcto
        if (entryCodigo.Text == codigoVerificacion)
        {
            // Obtener el ID de usuario asociado al correo electrónico
            var userDetails = await _apiService.PostDataAsync<idUsuarioModel>("obtenerIdUsuarioCorreo.php", new { correo = Correo });
            int idUsuario = userDetails.idusuario;

            var data = new clientIdModel
            {
                idcliente = idUsuario
            };

            // Redireccionar a la página de restablecimiento de contraseña
            await Navigation.PushAsync(new Views.Login.RestaurarContra { BindingContext = data });
        }
        else
        {
            // Mostrar alertas en caso de errores
            await DisplayAlert("Alerta", "El código de verificación ingresado es incorrecto porfavor ingréselo de nuevo o pida un nuevo código.", "OK");
            return;
        }
    }

    // Controlador de eventos para clic en el botón de retroceso
    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
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
        // Ejecutar el conteo regresivo y actualizar la etiqueta correspondiente
        while (countdownSeconds > 0)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                labelEnviarCog.Text = $"Enviar de nuevo ({countdownSeconds}s)";
                labelEnviarCog.IsEnabled = false;
            });

            await Task.Delay(1000);
            countdownSeconds--;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            labelEnviarCog.Text = "Enviar de nuevo";
            labelEnviarCog.IsEnabled = true;
            isCountdownRunning = false;
            countdownSeconds = 30;
        });
    }

    // Controlador de eventos para toque en la etiqueta de reenvío de código
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var resultadoCorreo = await _apiService.PostDataAsync<codigoVerificacionModel>("correoCodigoVerificacionReestablecerPass.php", new { email = Correo, name = "Floristeria Margaritas" });
        codigoVerificacion = resultadoCorreo.verification_code;
        await DisplayAlert("Alerta", "Código de Verificación Reenviado", "OK");
    }
}