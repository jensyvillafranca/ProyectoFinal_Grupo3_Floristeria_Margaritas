
/* Cambio no fusionado mediante combinación del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-maccatalyst)'
Antes:
using System.Collections.ObjectModel;
using static ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Home.homePageUser;
using Microsoft.Maui.Graphics;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
Después:
using Microsoft.Maui.Graphics;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Home.homePageUser;
using Microsoft.Collections.ObjectModel;
using static ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Home.homePageUser;
*/

/* Cambio no fusionado mediante combinación del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-windows10.0.19041.0)'
Antes:
using System.Collections.ObjectModel;
using static ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Home.homePageUser;
using Microsoft.Maui.Graphics;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
Después:
using Microsoft.Maui.Graphics;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Home.homePageUser;
using Microsoft.Collections.ObjectModel;
using static ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Home.homePageUser;
*/

/* Cambio no fusionado mediante combinación del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-ios)'
Antes:
using System.Collections.ObjectModel;
using static ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Home.homePageUser;
using Microsoft.Maui.Graphics;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
Después:
using Microsoft.Maui.Graphics;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Home.homePageUser;
using Microsoft.Collections.ObjectModel;
using static ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Home.homePageUser;
*/
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using System.Collections.ObjectModel;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Home;

public partial class homePageUser : ContentPage
{
    public ObservableCollection<TestItem> TestItems { get; set; }
    private int currentIndex = 0; // Para saber el indice del carrusel

    public homePageUser()
    {
        InitializeComponent();

        // Inicia objetos de prueba para el carrusel
        TestItems = new ObservableCollection<TestItem>
        {
                new TestItem { ImagePath = "Home/descuento.png", LabelText = "Item 1" },
                new TestItem { ImagePath = "Home/descuento.png", LabelText = "Item 2" },
                new TestItem { ImagePath = "Home/descuento.png", LabelText = "Item 3" },
                new TestItem { ImagePath = "Home/descuento.png", LabelText = "Item 4" },
                new TestItem { ImagePath = "Home/descuento.png", LabelText = "Item 5" }
            };

        // Coloca la coleccion como la fuente de objetos para el carrusel
        carouselView.ItemsSource = TestItems;

        // Coloca el binding context
        BindingContext = this;

        SizeChanged += OnSizeChanged;

        // Empieza un timer para cambiar las ofertas
        Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(3));
                MoveToNextItem();
            }
        });
    }

    private void MoveToNextItem()
    {
        currentIndex = (currentIndex + 1) % TestItems.Count;
        carouselView.Position = currentIndex;
    }

    private void OnSizeChanged(object sender, EventArgs e)
    {
        // Obtiene la altura de la pantalla
        double screenHeight = DeviceDisplay.MainDisplayInfo.Height;

        // Ajusta la altura del frame como un porcentaje de la altura de la pantalla
        double frameHeightPercentage = 0.08;

        frameProductos.HeightRequest = screenHeight * frameHeightPercentage;
        frameCarrito.HeightRequest = screenHeight * frameHeightPercentage;
        framePedidos.HeightRequest = screenHeight * frameHeightPercentage;
        framePerfil.HeightRequest = screenHeight * frameHeightPercentage;

        // Set CarouselView height as a percentage of the screen height
        //double carouselHeightPercentage = 0.09; // 50% of the screen height
        //carouselView.HeightRequest = screenHeight * carouselHeightPercentage;
    }

    public class TestItem
    {
        public string? ImagePath { get; set; }
        public string? LabelText { get; set; }
    }

    private void btnLogout_Clicked(object sender, EventArgs e)
    {

    }

    private async void TapGestureProductos_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameProductos, Color.FromRgb(46, 117, 182), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new Views.Productos.productos());
    }

    private async void TapGestureCarrito_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameCarrito, Color.FromRgb(33, 52, 91), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new Views.Productos.carritoCompras());
    }

    private async void TapGesturePedidos_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(framePedidos, Color.FromRgb(33, 52, 91), Color.FromRgb(65, 185, 254), 250);
    }

    private async void TapGesturePerfil_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(framePerfil, Color.FromRgb(0, 0, 0), Color.FromRgb(211, 211, 211), 250);
        await Navigation.PushAsync(new Profile.profilePageUser());
    }

    private async void TapGestureCarousel_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor((Frame)sender, Color.FromRgb(46, 117, 182), Color.FromRgb(65, 185, 254), 250);
    }

    private void btnNotification_Clicked(object sender, EventArgs e)
    {

    }
}