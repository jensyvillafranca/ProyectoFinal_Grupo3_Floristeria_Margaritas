/*
 * Descripci�n:
 * Este c�digo define la l�gica de backend para la p�gina 'login' de la aplicaci�n Floristeria Margaritas, encargada del proceso de inicio de sesi�n de los usuarios.
 * Incluye la autenticaci�n de usuarios, gesti�n de tokens de dispositivo, redireccionamiento seg�n el tipo de usuario y configuraci�n para recordar sesi�n.
 */

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using Plugin.Firebase.CloudMessaging;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login
{
    public partial class login : ContentPage
    {
        // Variables de instancia y servicio de API
        private Color originalBackgroundColor;
        private ApiService _apiService = new ApiService();
        private int recordarValue = 0;

        // Constructor para la p�gina 'login'
        public login()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

        }

        // Controlador de eventos para clic en el bot�n de iniciar sesi�n
        private async void btnEntrar_Clicked(object sender, EventArgs e)
        {
            // Validar campos de usuario y contrase�a
            if (string.IsNullOrEmpty(entryUsername.Text))
            {
                await DisplayAlert("Alerta", "Por favor ingrese su Usuario", "OK");
                return;
            }
            else if (string.IsNullOrEmpty(entryPassword.Text))
            {
                await DisplayAlert("Alerta", "Por favor ingrese su Contrase�a", "OK");
                return;
            }

            // Autenticar usuario y gestionar sesi�n
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
                        // Redireccionar seg�n el tipo de usuario
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

        }

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
                            var adminDetails = await _apiService.PostDataAsync<adminIdModel>("loginTokenAdmin.php", new { idusuario = loginDetails.idusuario });

                            try
                            {
                                await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
                                var deviceToken = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();

                                var data = new
                                {
                                    idadmin = adminDetails.idadmin,
                                    token = deviceToken
                                };

                                bool isSuccess = await _apiService.PostSuccessAsync("updateTokenAdmin.php", data);

                                if (isSuccess)
                                {
                                    PreferencesManager.SaveInt("adminID", adminDetails.idadmin);
                                    PreferencesManager.SaveInt("userID", loginDetails.idusuario);
                                    PreferencesManager.SaveString("usuario", loginDetails.usuario);
                                    PreferencesManager.SaveInt("tipoUsuario", loginDetails.fk_idtipousuario);
                                    PreferencesManager.SaveInt("stayLogged", recordarValue);
                                    await Navigation.PushAsync(new Views.Home.homePageAdmin());
                                }
                            }
                            catch (Exception ex)
                            {
                                await DisplayAlert("Alerta", "Se produjo un error, intente de nuevo", "OK");
                                return;
                            }
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

        // Controlador de eventos para clic en el bot�n de registro
        private async void btnRegistrarse_Clicked(object sender, EventArgs e)
        {
            // Aqu� puedes navegar a la p�gina deseada
            await Navigation.PushAsync(new Views.Login.singin());
        }

        // Controlador de eventos para clic en el bot�n de registro
        private void btnAplicar_Clicked(object sender, EventArgs e)
        {

        }

        // Controlador de eventos para clic en el Label de recuperar contrase�a
        private async void LabelRecuperar_Tapped(object sender, System.EventArgs e)
        {
            // Animaci�n de escala al hacer clic en el Label
            await labelRecuperar.ScaleTo(0.8, 50, Easing.Linear);
            await labelRecuperar.ScaleTo(1, 50, Easing.Linear);

            // Aqu� puedes navegar a la p�gina deseada
            await Navigation.PushAsync(new Views.Login.EnviarCodigo());
        }

        // Controlador de eventos para cambiar el estado del switch de recordar sesi�n
        private void switchRecordar_Toggled(object sender, ToggledEventArgs e)
        {
            recordarValue = e.Value ? 1 : 0;
        }
    }
}
