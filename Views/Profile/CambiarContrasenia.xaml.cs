namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Profile;

public partial class CambiarContrasenia : ContentPage
{
    public CambiarContrasenia()
    {
        InitializeComponent();
    }

    private void btnImagenAtras_Clicked(object sender, EventArgs e)
    {

    }

    private void BtnCambiar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ConfirmacionContrasenia());
    }
}