using System.Collections.ObjectModel;
using static ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Home.homePageUser;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Home;

public partial class homePageUser : ContentPage
{
    public ObservableCollection<TestItem> TestItems { get; set; }
    private int currentIndex = 0; // Keep track of the current index in the CarouselView

    public homePageUser()
	{
		InitializeComponent();

        // Initialize the collection and add test items
        TestItems = new ObservableCollection<TestItem>
        {
                new TestItem { ImagePath = "descuento.png", LabelText = "Item 1" },
                new TestItem { ImagePath = "descuento.png", LabelText = "Item 2" },
                new TestItem { ImagePath = "descuento.png", LabelText = "Item 3" },
                new TestItem { ImagePath = "descuento.png", LabelText = "Item 4" },
                new TestItem { ImagePath = "descuento.png", LabelText = "Item 5" }
            };

        // Set the collection as the ItemsSource for the CarouselView
        carouselView.ItemsSource = TestItems;

        // Set the binding context to this instance of the page
        BindingContext = this;

        SizeChanged += OnSizeChanged;

        // Start a timer to move to the next item every 5 seconds (adjust as needed)
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
        // Get the actual screen height
        double screenHeight = DeviceDisplay.MainDisplayInfo.Height;

        // Set frame heights as a percentage of the screen height
        double frameHeightPercentage = 0.08; // 25% of the screen height

        frameProductos.HeightRequest = screenHeight * frameHeightPercentage;
        frameCarrito.HeightRequest = screenHeight * frameHeightPercentage;
        framePedidos.HeightRequest = screenHeight * frameHeightPercentage;
        framePerfil.HeightRequest = screenHeight * frameHeightPercentage;

        // Set CarouselView height as a percentage of the screen height
        double carouselHeightPercentage = 0.09; // 50% of the screen height
        //carouselView.HeightRequest = screenHeight * carouselHeightPercentage;
    }

    public class TestItem
    {
        public string ImagePath { get; set; }
        public string LabelText { get; set; }
    }

    private void btnNotification_Clicked(object sender, EventArgs e)
    {

    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {

    }

    private void btnProductos_Clicked(object sender, EventArgs e)
    {

    }

    private void btnCarrito_Clicked(object sender, EventArgs e)
    {

    }

    private void btnPedidos_Clicked(object sender, EventArgs e)
    {

    }

    private void btnPerfil_Clicked(object sender, EventArgs e)
    {

    }

    private void btnLogout_Clicked(object sender, EventArgs e)
    {

    }
}