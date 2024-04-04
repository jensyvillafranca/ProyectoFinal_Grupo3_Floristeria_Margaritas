/*
 * Descripción:
 * Este código define la lógica de backend para la página 'EnviarCodigo' de la aplicación Floristeria Margaritas, la cual maneja el envío de códigos de verificación para restablecimiento de contraseña.
 * Incluye validación de formato de correo electrónico, verificación de existencia de usuario y envío de correo electrónico con código de verificación.
 */

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using System.Text.RegularExpressions;
namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login;

public partial class EnviarCodigo : ContentPage
{
    // Instancia de ApiService para interacciones con la API
    private ApiService _apiService = new ApiService();

    // Variable para el formato de correo electrónico
    private bool formatoCorreo = false;

    // Constructor para la página 'EnviarCodigo'
    public EnviarCodigo()
    {
        InitializeComponent();
    }

    // Controlador de eventos para clic en el botón de enviar código
    private async void btnEnviarCodig_Clicked(object sender, EventArgs e)
    {
        // Validar si se ingresó un correo electrónico
        if (string.IsNullOrEmpty(entryCorreo.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese su correo electrónico", "OK");
            return;
        }

        // Validar el formato del correo electrónico
        if (!formatoCorreo)
        {
            await DisplayAlert("Alerta", "Por favor ingrese un correo electrónico valido", "OK");
            return;
        }

        // Verificar la existencia del correo electrónico en la base de datos
        var resultadoExiste = await _apiService.PostDataAsync<existeUsuario>("existeCorreo.php", new { correo = entryCorreo.Text });

        // Enviar código de verificación si el correo existe
        bool existe = resultadoExiste.existe;

        if (existe)
        {
            var resultadoCorreo = await _apiService.PostDataAsync<codigoVerificacionModel>("correoCodigoVerificacionReestablecerPass.php", new { email = entryCorreo.Text, name = "Floristeria Margaritas" });
            string? codigo = resultadoCorreo.verification_code;

            var data = new recuperarPassModel
            {
                Codigo = codigo,
                Correo = entryCorreo.Text
            };

            await Navigation.PushAsync(new Views.Login.VerificarContra { BindingContext = data });
        }
        else
        {
            await DisplayAlert("Alerta", "El correo electrónico que ha ingresado no existe", "OK");
            return;
        }
    }

    // Controlador de eventos para clic en el botón de retroceso
    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    // Controlador de eventos para cambio de texto en la entrada de correo electrónico
    private void entryCorreo_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Valida el correo Electonico
        bool isValid = (Regex.IsMatch(e.NewTextValue, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"));

        // Cambia el color del texto
        ((Entry)sender).TextColor = isValid ? Color.FromRgb(0, 0, 0) : Color.FromRgb(244, 67, 54);

        formatoCorreo = isValid;
    }
}