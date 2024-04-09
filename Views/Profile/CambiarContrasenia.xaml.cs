using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Profile;

public partial class CambiarContrasenia : ContentPage
{
    private ApiService _apiService = new ApiService();
    private bool _Coinsiden = false;
    private string? storedPassword;

    public CambiarContrasenia()
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
            // Manejar la excepción
        }
    }

    // Método para validar confirmación de contraseña
    private void ValidatePasswordConfirmation()
    {
        string password = entryNuevaContrasenia.Text;
        string confirmPassword = entryConfirmarContrasenia.Text;

        if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(confirmPassword))
        {
            if (password == confirmPassword)
            {
                // Passwords coinciden
                validationLabel.Text = "Contraseñas Coinciden";
                validationLabel.TextColor = Color.FromRgb(255, 255, 255);
                _Coinsiden = true;
            }
            else
            {
                // Passwords no coinciden
                validationLabel.Text = "Las contraseñas no Coinciden";
                validationLabel.TextColor = Color.FromRgb(244, 67, 54);
                _Coinsiden = false;
            }
        }
        else
        {
            // One or both entries are empty
            validationLabel.Text = "";
        }
    }

    private void entryNuevaContrasenia_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidatePasswordConfirmation();
    }

    private void entryConfirmarContrasenia_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidatePasswordConfirmation();
    }

    private async void btnActualizar_Clicked(object sender, EventArgs e)
    {
        //Validaciones
        if (string.IsNullOrEmpty(entryPassword.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese su contraseña actual", "OK");
            return;
        }
        else if (string.IsNullOrEmpty(entryNuevaContrasenia.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese su nueva contraseña", "OK");
            return;
        }
        else if (string.IsNullOrEmpty(entryConfirmarContrasenia.Text))
        {
            await DisplayAlert("Alerta", "Por favor confirme su nueva contraseña", "OK");
            return;
        }
        else if (!_Coinsiden)
        {
            await DisplayAlert("Alerta", "Las Contraseñas ingresadas no coinciden", "OK");
            return;
        }

        string enteredPassword = entryPassword.Text;
        bool passwordMatch = PasswordHandler.VerifyPassword(enteredPassword, storedPassword);

        if (passwordMatch)
        {
            string encryptedPassword = PasswordHandler.EncryptPassword(entryConfirmarContrasenia.Text);


            var data = new
            {
                idusuario = Config.Config.idUsuario,
                contrasenia = encryptedPassword
            };

            bool isSuccess = await _apiService.PostSuccessAsync("updateContrasenia.php", data);

            if (isSuccess)
            {
                await DisplayAlert("Alerta", "Su contraseña ha sido actualizada", "OK");
                entryPassword.Text = string.Empty;
                entryNuevaContrasenia.Text = string.Empty;
                entryConfirmarContrasenia.Text = string.Empty;
                await Navigation.PopAsync();
            }
        }
        else
        {
            await DisplayAlert("Alerta", "La contraseña que ha ingresado es incorrecta", "OK");
            return;
        }
    }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}