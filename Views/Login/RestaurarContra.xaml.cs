/*
 * Descripción:
 * Este código define la lógica de backend para la página 'RestaurarContra' de la aplicación Floristeria Margaritas, la cual maneja el proceso de restablecimiento de contraseña.
 * Incluye validación de la contraseña y su confirmación, encriptación de la contraseña, y actualización de la contraseña en la base de datos.
 */

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login;

public partial class RestaurarContra : ContentPage
{
    // Instancia de ApiService para interacciones con la API
    private ApiService _apiService = new ApiService();

    // Variables para el ID de usuario y validez de sesión
    private int idUsuario;
    private bool isValid = true;

    // Constructor para la página 'RestaurarContra'
    public RestaurarContra()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
    }

    // Método que se ejecuta al aparecer la página
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is clientIdModel data)
        {
            idUsuario = data.idcliente;
        }
    }

    // Método para validar la confirmación de la contraseña
    private void ValidatePasswordConfirmation()
    {
        string password = entryContrasenia.Text;
        string confirmPassword = entryConfirmarContrasenia.Text;

        // Validar la confirmación de la contraseña y actualizar la etiqueta de validación
        if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(confirmPassword))
        {
            if (password == confirmPassword)
            {
                // Passwords coinciden
                validationLabel.Text = "Contraseñas Coinciden";
                validationLabel.TextColor = Color.FromRgb(255, 255, 255);
            }
            else
            {
                // Passwords no coinciden
                validationLabel.Text = "Las contraseñas no Coinciden";
                validationLabel.TextColor = Color.FromRgb(244, 67, 54);
            }
        }
        else
        {
            // One or both entries are empty
            validationLabel.Text = "";
        }
    }

    // Controlador de eventos para clic en el botón de retroceso
    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    // Controlador de eventos para clic en el botón de guardar contraseña
    private async void btnGuardarContrasenia_Clicked(object sender, EventArgs e)
    {
        // Validar si la sesión es válida
        if (!isValid)
        {
            await DisplayAlert("Alerta", "Esta sesión ya no es valida", "OK");
            await Navigation.PushAsync(new Views.Login.login());
            return;
        }

        // Validar si se ingresó una contraseña
        if (string.IsNullOrEmpty(entryContrasenia.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese una contraseña", "OK");
            return;
        }
        else if (entryContrasenia.Text != entryConfirmarContrasenia.Text)
        {
            // Validar si las contraseñas coinciden
            await DisplayAlert("Alerta", "Las contraseñas no coinciden", "OK");
            return;
        }

        // Encriptar la contraseña y actualizar en la base de datos
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
            await DisplayAlert("Alerta", "Su contraseña ha sido actualizada, por favor espere a ser redirigido e ingrese sus credenciales de acceso.", "OK");
            await Navigation.PushAsync(new Views.Login.login());
        }
        else
        {
            // Mostrar alertas en caso de errores
            await DisplayAlert("Alerta", "Hubo un error, por favor intente de nuevo.", "OK");
            return;
        }
    }

    // Controlador de eventos para cambio de texto en la entrada de contraseña
    private void entryContrasenia_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidatePasswordConfirmation();
    }

    // Controlador de eventos para cambio de texto en la entrada de confirmación de contraseña
    private void entryConfirmarContrasenia_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidatePasswordConfirmation();
    }
}