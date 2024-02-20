namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallaAdministrador;
using System.Collections.ObjectModel;

public partial class BuscarProducto_Promociones : ContentPage
{
    public ObservableCollection<ListaProductos_Buscar> ListaProductos { get; set; }
    public BuscarProducto_Promociones()
	{
		InitializeComponent();
        //RemoveLinePickercs.remove_stripe();
        // Inicia objetos de prueba para collection view
        ListaProductos = new ObservableCollection<ListaProductos_Buscar>
            {
                new ListaProductos_Buscar { imagenProducto = "Productos/flowers2.png", nomProducto = "Ramo de rosas", precio = "L. 590", cantidad = "5", fechaVencimiento = "Ninguna"},
                new ListaProductos_Buscar { imagenProducto = "Productos/flowers2.png", nomProducto = "Ramo de rosas", precio = "L. 590", cantidad = "5", fechaVencimiento = "Ninguna"},
                new ListaProductos_Buscar { imagenProducto= "Productos/flowers2.png", nomProducto = "Chocolates", precio = "L. 590", cantidad = "5", fechaVencimiento ="2024-02-03"}
              
            };
        collectionViewPedidosRepartidor.ItemsSource = ListaProductos;
    }



    //Modelo Temporal para cargar los datos de los pedidos.
    public class ListaProductos_Buscar
    {
        public string imagenProducto { get; set; }
        public string nomProducto { get; set; }
        public string precio { get; set; }
        public string cantidad { get; set; }
        public string fechaVencimiento { get; set; }
    }

    private void filtroProductos_Promociones_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}