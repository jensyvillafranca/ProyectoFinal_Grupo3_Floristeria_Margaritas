namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Profile;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;

public partial class profilePageUser : ContentPage
{
    public profilePageUser()
    {
        InitializeComponent();
    }


    private void btnImagenAtras_Clicked(object sender, EventArgs e)
    {

    }

    private async void TapGesturePerfil_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameUbicaciones, Color.FromRgb(255, 255, 255), Color.FromRgb(65, 185, 254), 250);
        await AnimationUtilities.ChangeFrameColor(frameUbicaciones, Color.FromRgb(53, 172, 226), Color.FromRgb(211, 211, 211), 250);
        await Navigation.PushAsync(new DireccionesUsuario.DireccionesGuardas());
    }

    private async void TapGestureHistorial_Tapped(object sender, TappedEventArgs e)
    {
         
        await AnimationUtilities.ChangeFrameColor(frameHistorial, Color.FromRgb(255, 255, 255), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new HistorialCompras());
    }

    private async void TapGestureCambiarcontra_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameCambiarcontrasenia, Color.FromRgb(255, 255, 255), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new CambiarContrasenia());
    }

    private void btnEditarPefil_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Profile.editPageUserProfile());
    }

}