using System.Collections.ObjectModel;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallaAdministrador;

public partial class NotificacionStockProductos : ContentPage
{
    public ObservableCollection<ListaAlertas> ListaNotificacionesStock { get; set; }
    public NotificacionStockProductos()
    {
        InitializeComponent();
        ListaNotificacionesStock = new ObservableCollection<ListaAlertas>
            {
                new ListaAlertas { nombreProducto="Ramo de Margaritas", alertaStock="Próximo a Vencer", unidadesRestantes="5"},
                new ListaAlertas { nombreProducto="Ramo de Margaritas", alertaStock="Próximo a Vencer", unidadesRestantes="5"},
                new ListaAlertas { nombreProducto="Ramo de Margaritas", alertaStock="Próximo a Vencer", unidadesRestantes="5"},
            };
        collectionViewAlertasAdministrador.ItemsSource = ListaNotificacionesStock;
        this.BindingContext = this;
    }
    //Modelo Temporal para cargar los datos de los pedidos.
    public class ListaAlertas
    {
        public string nombreProducto { get; set; }
        public string alertaStock { get; set; }
        public string unidadesRestantes { get; set; }
    }
}