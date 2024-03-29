using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using Plugin.Firebase.CloudMessaging;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login
{
    public partial class login : ContentPage
    {
        private Color originalBackgroundColor;
        private ApiService _apiService = new ApiService();
        private int recordarValue = 0;

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
                await DisplayAlert("Alerta", "Por favor ingrese su Contrase�a", "OK");
                return;
            }

            try
            {
                var loginDetails = await _apiService.PostDataAsync<loginModel>("login.php", new { usuario = entryUsername.Text });
                if (loginDetails != null)
                {
                    string enteredPassword = entryPassword.Text;
                    string storedPassword = loginDetails.contrasenia;
                    bool passwordMatch = PasswordHandler.VerifyPassword(enteredPassword, storedPassword);

                    if (passwordMatch)
                    {
                        if (loginDetails.fk_idtipousuario == 1)
                        {
                            //Login Cliente
                            var clientDetails = await _apiService.PostDataAsync<clientIdModel>("loginToken.php", new { idusuario = loginDetails.idusuario });

                            try
                            {
                                await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
                                var deviceToken = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();

                                var data = new
                                {
                                    idcliente = clientDetails.idcliente,
                                    token = deviceToken
                                };

                                bool isSuccess = await _apiService.PostSuccessAsync("updateTokenCliente.php", data);

                                if (isSuccess)
                                {
                                    PreferencesManager.SaveInt("clienteID", clientDetails.idcliente);
                                    PreferencesManager.SaveInt("userID", loginDetails.idusuario);
                                    PreferencesManager.SaveString("usuario", loginDetails.usuario);
                                    PreferencesManager.SaveInt("tipoUsuario", loginDetails.fk_idtipousuario);
                                    PreferencesManager.SaveInt("stayLogged", recordarValue);
                                    await Navigation.PushAsync(new Views.Home.homePageUser());
                                }
                            }
                            catch (Exception ex)
                            {
                                await DisplayAlert("Alerta", "Se produjo un error, intente de nuevo", "OK");
                                return;
                            }

                            
                        }
                        else if (loginDetails.fk_idtipousuario == 3)
                        {
                            //Login Repartidor
                            var repartidorDetails = await _apiService.PostDataAsync<repartidorIdModel>("loginTokenRepartidor.php", new { idusuario = loginDetails.idusuario });

                            try
                            {
                                await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
                                var deviceToken = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();

                                var data = new
                                {
                                    idrepartidor = repartidorDetails.idrepartidor,
                                    token = deviceToken
                                };

                                bool isSuccess = await _apiService.PostSuccessAsync("updateTokenRepartidor.php", data);

                                if (isSuccess)
                                {
                                    PreferencesManager.SaveInt("repartidorID", repartidorDetails.idrepartidor);
                                    PreferencesManager.SaveInt("userID", loginDetails.idusuario);
                                    PreferencesManager.SaveString("usuario", loginDetails.usuario);
                                    PreferencesManager.SaveInt("tipoUsuario", loginDetails.fk_idtipousuario);
                                    PreferencesManager.SaveInt("stayLogged", recordarValue);
                                    await Navigation.PushAsync(new Views.Home.homePageRepartidor());
                                }
                            }
                            catch (Exception ex)
                            {
                                await DisplayAlert("Alerta", "Se produjo un error, intente de nuevo", "OK");
                                return;
                            }       
                        }
                        else if (loginDetails.fk_idtipousuario == 2)
                        {
                            
                            await Navigation.PushAsync(new Views.Home.homePageAdmin());
                        }


                    }
                    else
                    {
                        await DisplayAlert("Alerta", "Su contrase�a es incorrecta.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Alerta", "El Usuario que ha ingresado no existe.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alerta", "El Usuario que ha ingresado no existe.", "OK");
                return;
            }
        }

        private async void btnRegistrarse_Clicked(object sender, EventArgs e)
        {
            // Aqu� puedes navegar a la p�gina deseada
            await Navigation.PushAsync(new Views.Login.singin());
        }


        private void btnAplicar_Clicked(object sender, EventArgs e)
        {

        }

        private async void LabelRecuperar_Tapped(object sender, System.EventArgs e)
        {
            // Animaci�n de escala al hacer clic en el Label
            await labelRecuperar.ScaleTo(0.8, 50, Easing.Linear);
            await labelRecuperar.ScaleTo(1, 50, Easing.Linear);

            // Aqu� puedes navegar a la p�gina deseada
            await Navigation.PushAsync(new Views.Login.EnviarCodigo());
        }

        private void switchRecordar_Toggled(object sender, ToggledEventArgs e)
        {
            recordarValue = e.Value ? 1 : 0;
        }
    }
}
