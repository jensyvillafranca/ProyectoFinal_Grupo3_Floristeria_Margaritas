using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login
{
    public partial class login : ContentPage
    {
        private Color originalBackgroundColor;
        private ApiService _apiService = new ApiService();

        public login()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

        }

        private async void btnEntrar_Clicked(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(entryUsername.Text))
            {
                await DisplayAlert("Alerta", "Por favor ingrese su Usuario", "OK");
                return;
            }
            else if (string.IsNullOrEmpty(entryPassword.Text))
            {
                await DisplayAlert("Alerta", "Por favor ingrese su Contraseña", "OK");
                return;
            }

            var loginDetails = await _apiService.PostDataAsync<loginModel>("login.php", new { usuario = entryUsername.Text });
            if (loginDetails != null)
            {
                string enteredPassword = entryPassword.Text;
                string storedPassword = loginDetails.contrasenia;
                bool passwordMatch = PasswordHandler.VerifyPassword(enteredPassword, storedPassword);

                if (passwordMatch)
                {
                    var clientDetails = await _apiService.PostDataAsync<clientIdModel>("loginToken.php", new { idusuario = loginDetails.idusuario });

                    await Navigation.PushAsync(new Views.Home.homePageUser());
                }
                else
                {
                    await DisplayAlert("Alerta", "Su contraseña es incorrecta.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Alerta", "El Usuario que ha ingresado no existe.", "OK");
            }
        }

        private async void btnRegistrarse_Clicked(object sender, EventArgs e)
        {
            // Aquí puedes navegar a la página deseada
            await Navigation.PushAsync(new Views.Login.singin());
        }


        private void btnAplicar_Clicked(object sender, EventArgs e)
        {

        }

        private async void LabelRecuperar_Tapped(object sender, System.EventArgs e)
        {
            // Animación de escala al hacer clic en el Label
            await labelRecuperar.ScaleTo(0.8, 50, Easing.Linear);
            await labelRecuperar.ScaleTo(1, 50, Easing.Linear);

            // Aquí puedes navegar a la página deseada
            await Navigation.PushAsync(new Views.Login.RestaurarContra());
        }
    }
}
