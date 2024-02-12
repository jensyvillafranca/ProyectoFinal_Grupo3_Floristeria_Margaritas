using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallasRepartidor;

public partial class PantallaPedidosEntrantes : ContentPage
{
    public ObservableCollection<ListaPedidos> ListaPedidosRepartidor { get; set; }
    public PantallaPedidosEntrantes()
	{
		InitializeComponent();
        // Inicia objetos de prueba para collection view
        ListaPedidosRepartidor = new ObservableCollection<ListaPedidos>
            {
                new ListaPedidos { codPedido="PED-56546", imagenProductoPedido = "Productos/flowers2.png", clientePedido = "Jensy Villafranca", lugarEntregaPedido = "Choloma Cortés, Colonia Sierra Verde", productoPedido = "Margaritas + Peluche", sucursalPedido="Sucursal Circunvalación"},
                new ListaPedidos { codPedido="PED-56546", imagenProductoPedido = "Productos/flowers2.png", clientePedido = "Jensy Villafranca", lugarEntregaPedido = "Choloma Cortés, Colonia Sierra Verde", productoPedido = "Margaritas + Peluche", sucursalPedido="Sucursal Circunvalación"},
                new ListaPedidos { codPedido="PED-56546", imagenProductoPedido = "Productos/flowers2.png", clientePedido = "Jensy Villafranca", lugarEntregaPedido = "Choloma Cortés, Colonia Sierra Verde", productoPedido = "Margaritas + Peluche", sucursalPedido="Sucursal Circunvalación"},
                new ListaPedidos { codPedido="PED-56546", imagenProductoPedido = "Productos/flowers2.png", clientePedido = "Jensy Villafranca", lugarEntregaPedido = "Choloma Cortés, Colonia Sierra Verde", productoPedido = "Margaritas + Peluche", sucursalPedido="Sucursal Circunvalación"},
                new ListaPedidos { codPedido="PED-56546", imagenProductoPedido = "Productos/flowers2.png", clientePedido = "Jensy Villafranca", lugarEntregaPedido = "Choloma Cortés, Colonia Sierra Verde", productoPedido = "Margaritas + Peluche", sucursalPedido="Sucursal Circunvalación"},
                new ListaPedidos { codPedido="PED-56546", imagenProductoPedido = "Productos/flowers2.png", clientePedido = "Jensy Villafranca", lugarEntregaPedido = "Choloma Cortés, Colonia Sierra Verde", productoPedido = "Margaritas + Peluche", sucursalPedido="Sucursal Circunvalación"},
                new ListaPedidos { codPedido="PED-56546", imagenProductoPedido = "Productos/flowers2.png", clientePedido = "Jensy Villafranca", lugarEntregaPedido = "Choloma Cortés, Colonia Sierra Verde", productoPedido = "Margaritas + Peluche", sucursalPedido="Sucursal Circunvalación"},
                new ListaPedidos { codPedido="PED-56546", imagenProductoPedido = "Productos/flowers2.png", clientePedido = "Jensy Villafranca", lugarEntregaPedido = "Choloma Cortés, Colonia Sierra Verde", productoPedido = "Margaritas + Peluche", sucursalPedido="Sucursal Circunvalación"},
            };
        collectionViewPedidosRepartidor.ItemsSource = ListaPedidosRepartidor;
    }

    //Modelo Temporal para cargar los datos de los pedidos.
    public class ListaPedidos
    {
        public string codPedido { get; set; }
        public string imagenProductoPedido { get; set; }
        public string clientePedido { get; set; }
        public string lugarEntregaPedido { get; set; }
        public string productoPedido { get; set; }
        public string sucursalPedido { get; set; }
    }
}