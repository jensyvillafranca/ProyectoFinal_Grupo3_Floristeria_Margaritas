using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using System.Text.RegularExpressions;
namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login;

public partial class EnviarCodigo : ContentPage
{
    private ApiService _apiService = new ApiService();
    private bool formatoCorreo = false;

    public EnviarCodigo()
    {
        InitializeComponent();
    }

    private async void btnEnviarCodig_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(entryCorreo.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese su correo electrónico", "OK");
            return;
        }

        if (!formatoCorreo)
        {
            await DisplayAlert("Alerta", "Por favor ingrese un correo electrónico valido", "OK");
            return;
        }

        var resultadoExiste = await _apiService.PostDataAsync<existeUsuario>("existeCorreo.php", new { correo = entryCorreo.Text });
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

    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private void entryCorreo_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Valida el correo Electonico
        bool isValid = (Regex.IsMatch(e.NewTextValue, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"));

        // Cambia el color del texto
        ((Entry)sender).TextColor = isValid ? Color.FromRgb(0, 0, 0) : Color.FromRgb(244, 67, 54);

        formatoCorreo = isValid;
    }
}