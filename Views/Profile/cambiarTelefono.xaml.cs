/*
 * Descripción:
 * Este código define la lógica de backend para la página 'cambiarTelefono' de la aplicación Floristeria Margaritas.
 * Permite al usuario cambiar su número de teléfono después de confirmar la operación.
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

    // Método para manejar el evento Clicked del botón 'btnBack'
    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    // Método para manejar el evento Clicked del botón 'btnActualizar'
    private async void btnActualizar_Clicked(object sender, EventArgs e)
    {
        // Verificar si el campo de número de teléfono está vacío
        if (string.IsNullOrEmpty(entryNumero.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese el nuevo número de teléfono", "OK");
            return;
        }

        // Solicitar confirmación al usuario antes de actualizar el número de teléfono
        bool userConfirmed = await DisplayAlert("Confirmación", "Por favor confirme si desea actualizar su número de teléfono", "Si", "No");

        if (userConfirmed)
        {
            try
            {
                // Construir el objeto de datos con el ID del cliente y el nuevo número de teléfono
                var data = new
                {
                    idcliente = Config.Config.activeUserId,
                    telefono = entryNumero.Text
                };

                // Enviar la solicitud para actualizar el número de teléfono al servidor
                bool isSuccess = await _apiService.PostSuccessAsync("updateTelefono.php", data);

                // Mostrar un mensaje de éxito si la operación se realiza correctamente
                if (isSuccess)
                {
                    await DisplayAlert("Alerta", "Su número de teléfono ha sido actualizado", "OK");
                    entryNumero.Text = string.Empty;
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra durante el proceso de actualización
                await DisplayAlert("Alerta", "Se produjo un error al intentar cambiar el número de teléfono, intente de nuevo", "OK");
                return;
            }
        }
    }
}