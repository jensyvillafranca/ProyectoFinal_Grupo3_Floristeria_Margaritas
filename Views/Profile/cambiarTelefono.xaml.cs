using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Profile;

public partial class cambiarTelefono : ContentPage
{
    private ApiService _apiService = new ApiService();
    public cambiarTelefono()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void btnActualizar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(entryNumero.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese el nuevo número de teléfono", "OK");
            return;
        }

        bool userConfirmed = await DisplayAlert("Confirmación", "Por favor confirme si desea actualizar su número de teléfono", "Si", "No");

        if (userConfirmed)
        {
            try
            {
                var data = new
                {
                    idcliente = Config.Config.activeUserId,
                    telefono = entryNumero.Text
                };

                bool isSuccess = await _apiService.PostSuccessAsync("updateTelefono.php", data);

                if (isSuccess)
                {
                    await DisplayAlert("Alerta", "Su número de teléfono ha sido actualizado", "OK");
                    entryNumero.Text = string.Empty;
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alerta", "Se produjo un error al intentar cambiar el número de teléfono, intente de nuevo", "OK");
                return;
            }
        }
    }
}