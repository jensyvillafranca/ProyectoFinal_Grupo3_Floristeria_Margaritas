using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallasRepartidor;

public partial class historialEntregas : ContentPage
{
    public ObservableCollection<EntregaItem> Entregas { get; set; }
    ObservableCollection<string> filtros;
    public historialEntregas()
	{
		InitializeComponent();

        // Inicia objetos de prueba para collection view
        Entregas = new ObservableCollection<EntregaItem>
            {
                new EntregaItem { ImageSource = "Productos/flowers.png", LabelCodigoPedido = "#PED-56546", LabelFechaEntrega = "2024-02-01", LabelComentario = "El Repartidor Llego Tarde Esta es una prueba de cantidad de texto en el item!", TappedCommand = new Command(() => HandleItemTapped("Item 1")) },
                new EntregaItem { ImageSource = "Productos/flowers.png", LabelCodigoPedido = "#PED-56546", LabelFechaEntrega = "2024-02-01", LabelComentario = "El Pedido ha llegado bien!", TappedCommand = new Command(() => HandleItemTapped("Item 2")) },
                new EntregaItem { ImageSource = "Productos/flowers.png", LabelCodigoPedido = "#PED-56546", LabelFechaEntrega = "2024-02-01", LabelComentario = "El Repartidor Llego Tarde!", TappedCommand = new Command(() => HandleItemTapped("Item 3")) },
                new EntregaItem { ImageSource = "Productos/flowers.png", LabelCodigoPedido = "#PED-56546", LabelFechaEntrega = "2024-02-01", LabelComentario = "El Pedido ha llegado bien!", TappedCommand = new Command(() => HandleItemTapped("Item 4")) },
                new EntregaItem { ImageSource = "Productos/flowers.png", LabelCodigoPedido = "#PED-56546", LabelFechaEntrega = "2024-02-01", LabelComentario = "El Repartidor Llego Tarde!", TappedCommand = new Command(() => HandleItemTapped("Item 5")) },
                new EntregaItem { ImageSource = "Productos/flowers.png", LabelCodigoPedido = "#PED-56546", LabelFechaEntrega = "2024-02-01", LabelComentario = "El Pedido ha llegado bien!", TappedCommand = new Command(() => HandleItemTapped("Item 6")) },
            };

        filtros = new ObservableCollection<string>
            {
                "Ninguno",
                "Esta Semana",
                "Esta Quincena",
                "Este Mes",
                "Este Año",
        };

        collectionViewEntregas.ItemsSource = Entregas;
        filtroPicker.ItemsSource = filtros;
    }

    public class EntregaItem
    {
        public string ImageSource { get; set; }
        public string LabelCodigoPedido { get; set; }
        public string LabelFechaEntrega { get; set; }
        public string LabelComentario { get; set; }
        public ICommand TappedCommand { get; set; }
    }

    private void HandleItemTapped(string itemName)
    {
        // Handle the tapped item
    }

    private void btnAtras_Clicked(object sender, EventArgs e)
    {

    }

    private void btnNotification_Clicked(object sender, EventArgs e)
    {

    }

    private void searchBarEntregas_SearchButtonPressed(object sender, EventArgs e)
    {

    }

    private void searchBarEntregas_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void btnInicioRepartidor_Clicked(object sender, EventArgs e)
    {

    }

    private void btnPedidosRepartidor_Clicked(object sender, EventArgs e)
    {

    }

    private void btnIngresosRepartidor_Clicked(object sender, EventArgs e)
    {

    }

    private void btnHistorialPedidosRepartidor_Clicked(object sender, EventArgs e)
    {

    }

    private void btnPerfilRepartidor_Clicked(object sender, EventArgs e)
    {

    }

    private void btnLogOutRepartidor_Clicked(object sender, EventArgs e)
    {

    }

    private void filtroPicker_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}