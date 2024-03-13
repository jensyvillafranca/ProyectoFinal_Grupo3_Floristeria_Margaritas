using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login;

public partial class RestaurarContra : ContentPage
{
    private ApiService _apiService = new ApiService();
    private int idUsuario;
    private bool isValid = true;

    public RestaurarContra()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is clientIdModel data)
        {
            idUsuario = data.idcliente;
        }
    }

    private void ValidatePasswordConfirmation()
    {
        string password = entryContrasenia.Text;
        string confirmPassword = entryConfirmarContrasenia.Text;

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

    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void btnGuardarContrasenia_Clicked(object sender, EventArgs e)
    {
        if (!isValid)
        {
            await DisplayAlert("Alerta", "Esta sesión ya no es valida", "OK");
            await Navigation.PushAsync(new Views.Login.login());
            return;
        }

        if (string.IsNullOrEmpty(entryContrasenia.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese una contraseña", "OK");
            return;
        }
        else if (entryContrasenia.Text != entryConfirmarContrasenia.Text)
        {
            await DisplayAlert("Alerta", "Las contraseñas no coinciden", "OK");
            return;
        }


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
            await DisplayAlert("Alerta", "Hubo un error, por favor intente de nuevo.", "OK");
            return;
        }
    }

    private void entryContrasenia_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidatePasswordConfirmation();
    }

    private void entryConfirmarContrasenia_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidatePasswordConfirmation();
    }
}





