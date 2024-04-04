/*
 * Descripci�n:
 * Este c�digo define la l�gica de backend para la p�gina 'EnviarCodigo' de la aplicaci�n Floristeria Margaritas, la cual maneja el env�o de c�digos de verificaci�n para restablecimiento de contrase�a.
 * Incluye validaci�n de formato de correo electr�nico, verificaci�n de existencia de usuario y env�o de correo electr�nico con c�digo de verificaci�n.
 */

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using System.Text.RegularExpressions;
namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login;

public partial class EnviarCodigo : ContentPage
{
    // Instancia de ApiService para interacciones con la API
    private ApiService _apiService = new ApiService();

    // Variable para el formato de correo electr�nico
    private bool formatoCorreo = false;

    // Constructor para la p�gina 'EnviarCodigo'
    public EnviarCodigo()
    {
        InitializeComponent();
    }

    // Controlador de eventos para clic en el bot�n de enviar c�digo
    private async void btnEnviarCodig_Clicked(object sender, EventArgs e)
    {
        // Validar si se ingres� un correo electr�nico
        if (string.IsNullOrEmpty(entryCorreo.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese su correo electr�nico", "OK");
            return;
        }

        // Validar el formato del correo electr�nico
        if (!formatoCorreo)
        {
            await DisplayAlert("Alerta", "Por favor ingrese un correo electr�nico valido", "OK");
            return;
        }

        // Verificar la existencia del correo electr�nico en la base de datos
        var resultadoExiste = await _apiService.PostDataAsync<existeUsuario>("existeCorreo.php", new { correo = entryCorreo.Text });

        // Enviar c�digo de verificaci�n si el correo existe
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
            await DisplayAlert("Alerta", "El correo electr�nico que ha ingresado no existe", "OK");
            return;
        }
    }

    // Controlador de eventos para clic en el bot�n de retroceso
    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    // Controlador de eventos para cambio de texto en la entrada de correo electr�nico
    private void entryCorreo_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Valida el correo Electonico
        bool isValid = (Regex.IsMatch(e.NewTextValue, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"));

        // Cambia el color del texto
        ((Entry)sender).TextColor = isValid ? Color.FromRgb(0, 0, 0) : Color.FromRgb(244, 67, 54);

        formatoCorreo = isValid;
    }
}