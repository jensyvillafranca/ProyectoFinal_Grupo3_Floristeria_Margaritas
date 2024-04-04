/*
 * Descripci�n:
 * Este c�digo define la l�gica de backend para la p�gina 'RestaurarContra' de la aplicaci�n Floristeria Margaritas, la cual maneja el proceso de restablecimiento de contrase�a.
 * Incluye validaci�n de la contrase�a y su confirmaci�n, encriptaci�n de la contrase�a, y actualizaci�n de la contrase�a en la base de datos.
 */

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login;

public partial class RestaurarContra : ContentPage
{
    // Instancia de ApiService para interacciones con la API
    private ApiService _apiService = new ApiService();

    // Variables para el ID de usuario y validez de sesi�n
    private int idUsuario;
    private bool isValid = true;

    // Constructor para la p�gina 'RestaurarContra'
    public RestaurarContra()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
    }

    // M�todo que se ejecuta al aparecer la p�gina
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is clientIdModel data)
        {
            idUsuario = data.idcliente;
        }
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

    // Controlador de eventos para clic en el bot�n de retroceso
    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    // Controlador de eventos para clic en el bot�n de guardar contrase�a
    private async void btnGuardarContrasenia_Clicked(object sender, EventArgs e)
    {
        // Validar si la sesi�n es v�lida
        if (!isValid)
        {
            await DisplayAlert("Alerta", "Esta sesi�n ya no es valida", "OK");
            await Navigation.PushAsync(new Views.Login.login());
            return;
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
            idusuario = idUsuario,
            contrasenia = encryptedPassword
        };

        var resultadoExiste = await _apiService.PostDataAsync<existeUsuario>("updateContrasenia.php", data);

        bool existe = resultadoExiste.existe;

        if (existe)
        {
            isValid = false;
            await DisplayAlert("Alerta", "Su contrase�a ha sido actualizada, por favor espere a ser redirigido e ingrese sus credenciales de acceso.", "OK");
            await Navigation.PushAsync(new Views.Login.login());
        }
        else
        {
            // Mostrar alertas en caso de errores
            await DisplayAlert("Alerta", "Hubo un error, por favor intente de nuevo.", "OK");
            return;
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