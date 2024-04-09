/*
 * Descripci�n:
 * Este c�digo define la l�gica de backend para la p�gina 'cambiarTelefono' de la aplicaci�n Floristeria Margaritas.
 * Permite al usuario cambiar su n�mero de tel�fono despu�s de confirmar la operaci�n.
 */

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Profile;

public partial class cambiarTelefono : ContentPage
{
    //Api service
    private ApiService _apiService = new ApiService();

    // Constructor
    public cambiarTelefono()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    // M�todo para manejar el evento Clicked del bot�n 'btnBack'
    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    // M�todo para manejar el evento Clicked del bot�n 'btnActualizar'
    private async void btnActualizar_Clicked(object sender, EventArgs e)
    {
        // Verificar si el campo de n�mero de tel�fono est� vac�o
        if (string.IsNullOrEmpty(entryNumero.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese el nuevo n�mero de tel�fono", "OK");
            return;
        }

        // Solicitar confirmaci�n al usuario antes de actualizar el n�mero de tel�fono
        bool userConfirmed = await DisplayAlert("Confirmaci�n", "Por favor confirme si desea actualizar su n�mero de tel�fono", "Si", "No");

        if (userConfirmed)
        {           
            try
            {
                if (Config.Config.tipoUsuario == 1)
                {
                    // Construir el objeto de datos con el ID del cliente y el nuevo n�mero de tel�fono
                    var data = new
                    {
                        idcliente = Config.Config.activeUserId,
                        telefono = entryNumero.Text
                    };

                    // Enviar la solicitud para actualizar el n�mero de tel�fono al servidor
                    bool isSuccess = await _apiService.PostSuccessAsync("updateTelefono.php", data);

                    // Mostrar un mensaje de �xito si la operaci�n se realiza correctamente
                    if (isSuccess)
                    {
                        await DisplayAlert("Alerta", "Su n�mero de tel�fono ha sido actualizado", "OK");
                        entryNumero.Text = string.Empty;
                        await Navigation.PopAsync();
                    }
                }
                else if (Config.Config.tipoUsuario == 3)
                {
                    // Construir el objeto de datos con el ID del cliente y el nuevo n�mero de tel�fono
                    var data = new
                    {
                        idrepartidor = Config.Config.activeRepartidorId,
                        telefono = entryNumero.Text
                    };

                    // Enviar la solicitud para actualizar el n�mero de tel�fono al servidor
                    bool isSuccess = await _apiService.PostSuccessAsync("updateTelefonoRepartidor.php", data);

                    // Mostrar un mensaje de �xito si la operaci�n se realiza correctamente
                    if (isSuccess)
                    {
                        await DisplayAlert("Alerta", "Su n�mero de tel�fono ha sido actualizado", "OK");
                        entryNumero.Text = string.Empty;
                        await Navigation.PopAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepci�n que ocurra durante el proceso de actualizaci�n
                await DisplayAlert("Alerta", "Se produjo un error al intentar cambiar el n�mero de tel�fono, intente de nuevo", "OK");
                return;
            }
        }
    }
}