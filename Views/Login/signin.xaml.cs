using System.Text.RegularExpressions;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login;

public partial class singin : ContentPage
{
    private bool formatoCorreo = false;
    private bool usuarioValido = false;
    private System.Threading.Timer textChangedTimer;
    private ApiService _apiService = new ApiService();

    public class existeUsuario
    {
        public bool existe { get; set; }

    }

    public singin()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        textChangedTimer = new System.Threading.Timer(TextChangedDelayed);
    }

    private async void btnRegistrarse_Clicked(object sender, EventArgs e)
    {
        if (ValidateRegistrationFields())
        {
            var resultadoExiste = await _apiService.PostDataAsync<existeUsuario>("existeCorreo.php", new { correo = entryCorreo.Text });
            bool existe = resultadoExiste.existe;

            if (!existe)
            {
                var resultadoCorreo = await _apiService.PostDataAsync<codigoVerificacionModel>("correoCodigoVerificacion.php", new { email = entryCorreo.Text, name = entryPrimerNombre.Text });
                string? codigo = resultadoCorreo.verification_code;

                var data = new crearCuentaModel
                {
                    Nombre = entryPrimerNombre.Text,
                    Apellido = entryPrimerApellido.Text,
                    Correo = entryCorreo.Text,
                    Telefono = entryTelefono.Text,
                    Usuario = entryUsuario.Text,
                    Contrasenia = entryConfirmarContrasena.Text,
                    Codigo = codigo
                };

                await Navigation.PushAsync(new Views.Login.CrearCuenta { BindingContext = data });
            }
            else
            {
                await DisplayAlert("Alerta", "Ya existe una cuenta con el correo electrónico ingresado.", "Iniciar Sesión", "Recuperar Cuenta");
            }
        }
    }

    //Validaciones
    private bool ValidateRegistrationFields()
    {
        if (string.IsNullOrEmpty(entryPrimerNombre.Text))
        {
            DisplayAlert("Alerta", "Por favor ingrese su nombre", "OK");
            return false;
        }
        else if (string.IsNullOrEmpty(entryPrimerApellido.Text))
        {
            DisplayAlert("Alerta", "Por favor ingrese su apellido", "OK");
            return false;
        }
        else if (string.IsNullOrEmpty(entryCorreo.Text))
        {
            DisplayAlert("Alerta", "Por favor ingrese su correo electrónico", "OK");
            return false;
        }
        else if (formatoCorreo == false)
        {
            DisplayAlert("Alerta", "Por favor ingrese un correo electrónico valido", "OK");
            return false;
        }
        else if (string.IsNullOrEmpty(entryTelefono.Text))
        {
            DisplayAlert("Alerta", "Por favor ingrese su número de teléfono", "OK");
            return false;
        }
        else if (string.IsNullOrEmpty(entryUsuario.Text))
        {
            DisplayAlert("Alerta", "Por favor ingrese un nombre de usuario", "OK");
            return false;
        }
        else if (!usuarioValido)
        {
            DisplayAlert("Alerta", "El nombre de usuario ingresado ya existe", "OK");
            return false;
        }
        else if (string.IsNullOrEmpty(entryContrasena.Text))
        {
            DisplayAlert("Alerta", "Por favor ingrese una contraseña", "OK");
            return false;
        }
        else if (entryContrasena.Text != entryConfirmarContrasena.Text)
        {
            DisplayAlert("Alerta", "Las contraseñas no coinciden", "OK");
            return false;
        }

        return true;
    }

    private void ValidatePasswordConfirmation()
    {
        string password = entryContrasena.Text;
        string confirmPassword = entryConfirmarContrasena.Text;

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

    private async void TextChangedDelayed(object state)
    {
        // Code to be executed after the delay (e.g., perform a search or validation)
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            var resultadoExiste = await _apiService.PostDataAsync<existeUsuario>("existeUsername.php", new { usuario = entryUsuario.Text });
            bool existe = resultadoExiste.existe;

            if (!existe)
            {
                existeUsuarioLabel.Text = "Usuario Disponible";
                existeUsuarioLabel.TextColor = Color.FromRgb(255, 255, 255);
                usuarioValido = true;
            }
            else
            {
                existeUsuarioLabel.Text = "Usuario No Disponible";
                existeUsuarioLabel.TextColor = Color.FromRgb(244, 67, 54);
                usuarioValido = false;
            }
        });
    }

    private async void LabelIngresar_Tapped(object sender, System.EventArgs e)
    {
        // Animación de escala al hacer clic en el Label
        await labelIngresar.ScaleTo(0.8, 50, Easing.Linear);
        await labelIngresar.ScaleTo(1, 50, Easing.Linear);

        // Aquí puedes navegar a la página deseada
        await Navigation.PushAsync(new Views.Login.login());
    }

    private void entryCorreo_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Valida el correo Electonico
        bool isValid = (Regex.IsMatch(e.NewTextValue, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"));

        // Cambia el color del texto
        ((Entry)sender).TextColor = isValid ? Color.FromRgb(0,0,0) : Color.FromRgb(244, 67, 54);

        formatoCorreo = isValid;
    }

    private void entryConfirmarContrasena_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidatePasswordConfirmation();
    }

    private void entryContrasena_TextChanged(object sender, TextChangedEventArgs e)
    {
        ValidatePasswordConfirmation();
    }

    private void entryUsuario_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (e.NewTextValue.Length > 2)
        {
            textChangedTimer.Change(1000, System.Threading.Timeout.Infinite);
        }
    }
}