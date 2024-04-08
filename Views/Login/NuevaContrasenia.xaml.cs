using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login;

public partial class NuevaContrasenia : ContentPage
{
    private ApiService _apiService = new ApiService();
    private bool isValid = true;
    public NuevaContrasenia()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
    }

    // M�todo para validar la confirmaci�n de la contrase�a
    private void ValidatePasswordConfirmation()
    {
        string password = entryContrasenia.Text;
        string confirmPassword = entryConfirmarContrasenia.Text;

        // Validar la confirmaci�n de la contrase�a y actualizar la etiqueta de validaci�n
        if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(confirmPassword))
        {
            if (password == confirmPassword)
            {
                // Passwords coinciden
                validationLabel.Text = "Contrase�as Coinciden";
                validationLabel.TextColor = Color.FromRgb(255, 255, 255);
            }
            else
            {
                // Passwords no coinciden
                validationLabel.Text = "Las contrase�as no Coinciden";
                validationLabel.TextColor = Color.FromRgb(244, 67, 54);
            }
        }
        else
        {
            // One or both entries are empty
            validationLabel.Text = "";
        }
    }

    // Controlador de eventos para clic en el bot�n de guardar contrase�a
    private async void btnGuardarContrasenia_Clicked(object sender, EventArgs e)
    {
        // Validar si la sesi�n es v�lida
        if (!isValid)
        {
            await DisplayAlert("Alerta", "Esta sesi�n ya no es valida", "OK");

            bool isSuccess = await logout.PerformLogoutAsync(_apiService);

            if (isSuccess)
            {
                await Navigation.PushAsync(new Views.Login.login());
            }
        }
        // Validar si se ingres� una contrase�a
        if (string.IsNullOrEmpty(entryContrasenia.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese una contrase�a", "OK");
            return;
        }
        else if (entryContrasenia.Text != entryConfirmarContrasenia.Text)
        {
            // Validar si las contrase�as coinciden
            await DisplayAlert("Alerta", "Las contrase�as no coinciden", "OK");
            return;
        }

        // Encriptar la contrase�a y actualizar en la base de datos
        string encryptedPassword = PasswordHandler.EncryptPassword(entryConfirmarContrasenia.Text);

        var data = new
        {
            idusuario = Config.Config.idUsuario,
            contrasenia = encryptedPassword
        };

        var resultadoExiste = await _apiService.PostDataAsync<existeUsuario>("updateContrasenia.php", data);

        bool existe = resultadoExiste.existe;

        if (existe)
        {
            isValid = false;
            await DisplayAlert("Alerta", "Su contrase�a ha sido actualizada, por favor espere a ser redirigido.", "OK");
            await Navigation.PushAsync(new Views.Home.homePageRepartidor());
        }
        else
        {
            // Mostrar alertas en caso de errores
            await DisplayAlert("Alerta", "Hubo un error, por favor intente de nuevo.", "OK");
            return;
        }
    }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        bool isSuccess = await logout.PerformLogoutAsync(_apiService);

        if (isSuccess)
        {
            await Navigation.PushAsync(new Views.Login.login());
        }
    }

    // Controlador de eventos para cambio de texto en la entrada de contrase�a
    private void entryContrasenia_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidatePasswordConfirmation();
    }

    // Controlador de eventos para cambio de texto en la entrada de confirmaci�n de contrase�a
    private void entryConfirmarContrasenia_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidatePasswordConfirmation();
    }
}