using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos;

public partial class productos : ContentPage
{
    public ObservableCollection<FrameItem> Items { get; set; }
    public ObservableCollection<FiltroItem> Filtros { get; set; }
    private int currentIndex = 0; // Para saber el indice del carrusel
    public productos()
	{
		InitializeComponent();

        carouselViewFiltros.Scrolled += CarouselViewFiltros_Scrolled;

        // Inicia objetos de prueba para collection view
        Items = new ObservableCollection<FrameItem>
            {
                new FrameItem { ImageSource = "flowers.png", LabelText = "Item 1", LabelPrecio = "L 1,700.00", TappedCommand = new Command(() => HandleItemTapped("Item 1")) },
                new FrameItem { ImageSource = "flowers.png", LabelText = "Item 2", LabelPrecio = "L 1,700.00", TappedCommand = new Command(() => HandleItemTapped("Item 2")) },
                new FrameItem { ImageSource = "flowers.png", LabelText = "Item 3", LabelPrecio = "L 1,700.00", TappedCommand = new Command(() => HandleItemTapped("Item 3")) },
                new FrameItem { ImageSource = "flowers.png", LabelText = "Item 4", LabelPrecio = "L 1,700.00", TappedCommand = new Command(() => HandleItemTapped("Item 4")) },
                new FrameItem { ImageSource = "flowers.png", LabelText = "Item 5", LabelPrecio = "L 1,700.00", TappedCommand = new Command(() => HandleItemTapped("Item 5")) },
                new FrameItem { ImageSource = "flowers.png", LabelText = "Item 6", LabelPrecio = "L 1,700.00", TappedCommand = new Command(() => HandleItemTapped("Item 6")) },
                new FrameItem { ImageSource = "flowers.png", LabelText = "Item 7", LabelPrecio = "L 1,700.00", TappedCommand = new Command(() => HandleItemTapped("Item 7")) },
                new FrameItem { ImageSource = "flowers.png", LabelText = "Item 8", LabelPrecio = "L 1,700.00", TappedCommand = new Command(() => HandleItemTapped("Item 8")) },
                new FrameItem { ImageSource = "flowers.png", LabelText = "Item 9", LabelPrecio = "L 1,700.00", TappedCommand = new Command(() => HandleItemTapped("Item 9")) },
                new FrameItem { ImageSource = "flowers.png", LabelText = "Item 10", LabelPrecio = "L 1,700.00", TappedCommand = new Command(() => HandleItemTapped("Item 10")) },
                new FrameItem { ImageSource = "flowers.png", LabelText = "Item 11", LabelPrecio = "L 1,700.00", TappedCommand = new Command(() => HandleItemTapped("Item 11")) },
                new FrameItem { ImageSource = "flowers.png", LabelText = "Item 12", LabelPrecio = "L 1,700.00", TappedCommand = new Command(() => HandleItemTapped("Item 12")) },
                new FrameItem { ImageSource = "flowers.png", LabelText = "Item 13", LabelPrecio = "L 1,700.00", TappedCommand = new Command(() => HandleItemTapped("Item 13")) },
                new FrameItem { ImageSource = "flowers.png", LabelText = "Item 14", LabelPrecio = "L 1,700.00", TappedCommand = new Command(() => HandleItemTapped("Item 14")) },
                new FrameItem { ImageSource = "flowers.png", LabelText = "Item 15", LabelPrecio = "L 1,700.00", TappedCommand = new Command(() => HandleItemTapped("Item 15")) },
                // Add more items as needed
            };

        // Filtros de busqueda
        Filtros = new ObservableCollection<FiltroItem>
        {
                new FiltroItem { LabelText = "Ninguno" },
                new FiltroItem { LabelText = "Romance" },
                new FiltroItem { LabelText = "Amistad" },
                new FiltroItem { LabelText = "Cumpleaños" },
                new FiltroItem { LabelText = "Aniversario" },
                new FiltroItem { LabelText = "Graduación" },
                new FiltroItem { LabelText = "Agradecimiento" },
                new FiltroItem { LabelText = "Condolencias" }
        };
        collectionViewProductos.ItemsSource = Items;
        carouselViewFiltros.ItemsSource = Filtros;
    }

    private void CarouselViewFiltros_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        // Calculate the current index based on the scroll position
        currentIndex = (int)(e.HorizontalOffset / carouselViewFiltros.Width);
    }



    public class FrameItem
    {
        public string ImageSource { get; set; }
        public string LabelText { get; set; }
        public string LabelPrecio { get; set; }
        public ICommand TappedCommand { get; set; }
    }

    public class FiltroItem
    {
        public string? LabelText { get; set; }
    }

    private void HandleItemTapped(string itemName)
    {
        // Handle the tapped item
    }

    private void btnNotification_Clicked(object sender, EventArgs e)
    {

    }

    private void searchBarProducots_SearchButtonPressed(object sender, EventArgs e)
    {

    }

    private void searchBarProducots_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void btnLogout_Clicked(object sender, EventArgs e)
    {

    }

    private void btnCarrito_Clicked(object sender, EventArgs e)
    {

    }

    private void btnHome_Clicked(object sender, EventArgs e)
    {

    }

    private void btnProductos_Clicked(object sender, EventArgs e)
    {

    }

    private void btnPedidos_Clicked(object sender, EventArgs e)
    {

    }

    private void btnPerfil_Clicked(object sender, EventArgs e)
    {

    }

    private void TapGestureCarouselFiltros_Tapped(object sender, TappedEventArgs e)
    {

    }

    private void btnCancelarFiltro_Clicked(object sender, EventArgs e)
    {
        currentIndex = 0;
        carouselViewFiltros.Position = currentIndex;
    }

    private void btnSiguiente_Clicked(object sender, EventArgs e)
    {
        currentIndex = (currentIndex + 1) % Filtros.Count;
        carouselViewFiltros.Position = currentIndex;
    }
}