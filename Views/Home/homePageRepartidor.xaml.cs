using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Home;

public partial class homePageRepartidor : ContentPage
{
    public homePageRepartidor()
    {
        InitializeComponent();
        //SizeChanged += OnSizeChanged;
    }

    private void OnSizeChanged(object sender, EventArgs e)
    {
        // Obtiene la altura de la pantalla
        double screenHeight = DeviceDisplay.MainDisplayInfo.Height;

        // Ajusta la altura del frame como un porcentaje de la altura de la pantalla
        double frameHeightPercentage = 0.08;

        frameIngresos.HeightRequest = screenHeight * frameHeightPercentage;
        frameHistorial.HeightRequest = screenHeight * frameHeightPercentage;
        framePedidos.HeightRequest = screenHeight * frameHeightPercentage;
        framePerfil.HeightRequest = screenHeight * frameHeightPercentage;
    }

    private void btnNotification_Clicked(object sender, EventArgs e)
    {

    }

    private async void TapGesturePedidos_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(framePedidos, Color.FromRgb(46, 117, 182), Color.FromRgb(65, 185, 254), 250);
    }

    private async void TapGestureIngresos_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameIngresos, Color.FromRgb(33, 52, 91), Color.FromRgb(65, 185, 254), 250);
    }

    private async void TapGestureHistorial_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameHistorial, Color.FromRgb(33, 52, 91), Color.FromRgb(65, 185, 254), 250);
    }

    private async void TapGesturePerfil_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(framePerfil, Color.FromRgb(46, 117, 182), Color.FromRgb(65, 185, 254), 250);
    }

    private void btnLogout_Clicked(object sender, EventArgs e)
    {

    }
}