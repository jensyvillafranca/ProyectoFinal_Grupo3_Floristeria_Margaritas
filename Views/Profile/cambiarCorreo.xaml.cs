/*
 * Descripción:
 * Este código define la lógica de backend para la página 'cambiarCorreo' de la aplicación Floristeria Margaritas.
 * Permite al usuario cambiar su dirección de correo electrónico después de verificar su contraseña.
 */

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using System.Text.RegularExpressions;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Profile;

public partial class cambiarCorreo : ContentPage
{
    private ApiService _apiService = new ApiService();
    private string? storedPassword;
    private bool formatoCorreo = false;

    // Constructor
    public cambiarCorreo()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    // Método para cargar la página
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var passDetails = await _apiService.PostDataAsync<loginModel>("passwordDetails.php", new { idcliente = Config.Config.activeUserId });
            if (passDetails != null)
            {
                storedPassword = passDetails.contrasenia;
            }
            else
            {
                await DisplayAlert("Alerta", "El Usuario no existe.", "OK");
            }
        }
        catch (Exception ex)
        {
            // Manejar la excepción
        }
    }

    // Método para manejar el evento Clicked del botón 'btnActualizar'
    private async void btnActualizar_Clicked(object sender, EventArgs e)
    {
        //Validaciones
        if (string.IsNullOrEmpty(entryPassword.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese su contraseña", "OK");
            return;
        }
        else if (string.IsNullOrEmpty(entryCorreo.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese su nuevo correo electrónico", "OK");
            return;
        }
        else if (!formatoCorreo)
        {
            await DisplayAlert("Alerta", "Por favor ingrese un correo electrónico valido", "OK");
            return;
        }
        else if (string.IsNullOrEmpty(entryConfirmarCorreo.Text))
        {
            await DisplayAlert("Alerta", "Por favor confirme su nuevo correo electrónico", "OK");
            return;
        }
        

        string enteredPassword = entryPassword.Text;
        bool passwordMatch = PasswordHandler.VerifyPassword(enteredPassword, storedPassword);

        if (passwordMatch)
        {
            var data = new
            {
                idcliente = Config.Config.activeUserId,
                correo = entryConfirmarCorreo.Text
            };
            bool isSuccess = await _apiService.PostSuccessAsync("updateCorreo.php", data);
            if (isSuccess)
            {
                await DisplayAlert("Alerta", "Su correo electrónico ha sido actualizado", "OK");
                entryPassword.Text = string.Empty;
                entryCorreo.Text = string.Empty;
                entryConfirmarCorreo.Text = string.Empty;
                await Navigation.PopAsync();
            }
        }
        else
        {
            await DisplayAlert("Alerta", "La contraseña que ha ingresado es incorrecta", "OK");
            return;
        }
    }

    // Método para validar el formato del correo electrónico al cambiar el texto en el campo 'entryCorreo'
    private void entryCorreo_TextChanged(object sender, TextChangedEventArgs e)
    {
        bool isValid = (Regex.IsMatch(e.NewTextValue, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"));

        // Cambia el color del texto
        ((Entry)sender).TextColor = isValid ? Color.FromRgb(0, 0, 0) : Color.FromRgb(244, 67, 54);

        formatoCorreo = isValid;
    }

    // Método para validar el formato del correo electrónico al cambiar el texto en el campo 'entryConfirmarCorreo'
    private void entryConfirmarCorreo_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Valida el correo Electonico
        bool isValid = (Regex.IsMatch(e.NewTextValue, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"));

        // Cambia el color del texto
        ((Entry)sender).TextColor = isValid ? Color.FromRgb(0, 0, 0) : Color.FromRgb(244, 67, 54);
    }

    // Método para manejar el evento Clicked del botón 'btnBack'
    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}