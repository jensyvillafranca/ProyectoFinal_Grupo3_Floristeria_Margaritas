/*
 * Descripci�n:
 * Este c�digo define la l�gica de backend para la p�gina 'cambiarCorreo' de la aplicaci�n Floristeria Margaritas.
 * Permite al usuario cambiar su direcci�n de correo electr�nico despu�s de verificar su contrase�a.
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

    // M�todo para cargar la p�gina
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            if (Config.Config.tipoUsuario == 1)
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
            else if (Config.Config.tipoUsuario == 3)
            {
                var passDetails = await _apiService.PostDataAsync<loginModel>("passwordDetailsRepartidor.php", new { idrepartidor = Config.Config.activeRepartidorId });

                if (passDetails != null)
                {
                    storedPassword = passDetails.contrasenia;
                }
                else
                {
                    await DisplayAlert("Alerta", "El Usuario no existe.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar la excepci�n
        }
    }

    // M�todo para manejar el evento Clicked del bot�n 'btnActualizar'
    private async void btnActualizar_Clicked(object sender, EventArgs e)
    {
        //Validaciones
        if (string.IsNullOrEmpty(entryPassword.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese su contrase�a", "OK");
            return;
        }
        else if (string.IsNullOrEmpty(entryCorreo.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese su nuevo correo electr�nico", "OK");
            return;
        }
        else if (!formatoCorreo)
        {
            await DisplayAlert("Alerta", "Por favor ingrese un correo electr�nico valido", "OK");
            return;
        }
        else if (string.IsNullOrEmpty(entryConfirmarCorreo.Text))
        {
            await DisplayAlert("Alerta", "Por favor confirme su nuevo correo electr�nico", "OK");
            return;
        }
        

        string enteredPassword = entryPassword.Text;
        bool passwordMatch = PasswordHandler.VerifyPassword(enteredPassword, storedPassword);

        if (passwordMatch)
        {
            if(Config.Config.tipoUsuario == 1)
            {
                var data = new
                {
                    idcliente = Config.Config.activeUserId,
                    correo = entryConfirmarCorreo.Text
                };
                bool isSuccess = await _apiService.PostSuccessAsync("updateCorreo.php", data);
                if (isSuccess)
                {
                    await DisplayAlert("Alerta", "Su correo electr�nico ha sido actualizado", "OK");
                    entryPassword.Text = string.Empty;
                    entryCorreo.Text = string.Empty;
                    entryConfirmarCorreo.Text = string.Empty;
                    await Navigation.PopAsync();
                }
            }
            else if(Config.Config.tipoUsuario == 3)
            {
                var data = new
                {
                    idrepartidor = Config.Config.activeRepartidorId,
                    correo = entryConfirmarCorreo.Text
                };
                bool isSuccess = await _apiService.PostSuccessAsync("updateCorreoRepartidor.php", data);
                if (isSuccess)
                {
                    await DisplayAlert("Alerta", "Su correo electr�nico ha sido actualizado", "OK");
                    entryPassword.Text = string.Empty;
                    entryCorreo.Text = string.Empty;
                    entryConfirmarCorreo.Text = string.Empty;
                    await Navigation.PopAsync();
                }
            }
        }
        else
        {
            await DisplayAlert("Alerta", "La contrase�a que ha ingresado es incorrecta", "OK");
            return;
        }
    }

    // M�todo para validar el formato del correo electr�nico al cambiar el texto en el campo 'entryCorreo'
    private void entryCorreo_TextChanged(object sender, TextChangedEventArgs e)
    {
        bool isValid = (Regex.IsMatch(e.NewTextValue, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"));

        // Cambia el color del texto
        ((Entry)sender).TextColor = isValid ? Color.FromRgb(0, 0, 0) : Color.FromRgb(244, 67, 54);

        formatoCorreo = isValid;
    }

    // M�todo para validar el formato del correo electr�nico al cambiar el texto en el campo 'entryConfirmarCorreo'
    private void entryConfirmarCorreo_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Valida el correo Electonico
        bool isValid = (Regex.IsMatch(e.NewTextValue, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"));

        // Cambia el color del texto
        ((Entry)sender).TextColor = isValid ? Color.FromRgb(0, 0, 0) : Color.FromRgb(244, 67, 54);
    }

    // M�todo para manejar el evento Clicked del bot�n 'btnBack'
    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}