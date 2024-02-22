using System.Collections.ObjectModel;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallasRepartidor;

public partial class IngresosRepartidor : ContentPage
{
    public ObservableCollection<ListaIngresos> ListaIngresosRepartidor { get; set; }
    public IngresosRepartidor()
    {
        InitializeComponent();

        // Inicia objetos de prueba para collection view
        ListaIngresosRepartidor = new ObservableCollection<ListaIngresos>
            {
                new ListaIngresos {codPedido="PED-56546", fechaEntrega = "2024-03-03", estadoIngreso = "Acreditado", ingresosRepartidor = "1,500"},
                new ListaIngresos {codPedido="PED-56546", fechaEntrega = "2024-03-03", estadoIngreso = "Acreditado", ingresosRepartidor = "1,500"},
                new ListaIngresos {codPedido = "PED-56546", fechaEntrega = "2024-03-03", estadoIngreso = "Acreditado",ingresosRepartidor = "1,500"},
                new ListaIngresos {codPedido = "PED-56546", fechaEntrega = "2024-03-03", estadoIngreso = "Acreditado",ingresosRepartidor = "1,500"},
                new ListaIngresos {codPedido = "PED-56546", fechaEntrega = "2024-03-03", estadoIngreso = "Acreditado",ingresosRepartidor = "1,500"},
                new ListaIngresos {codPedido = "PED-56546", fechaEntrega = "2024-03-03", estadoIngreso = "Acreditado",ingresosRepartidor = "1,500"},
                new ListaIngresos {codPedido="PED-56546", fechaEntrega = "2024-03-03", estadoIngreso = "Acreditado",ingresosRepartidor = "1,500"},
                new ListaIngresos {codPedido = "PED-56546", fechaEntrega = "2024-03-03", estadoIngreso = "Acreditado",ingresosRepartidor = "1,500"},
                new ListaIngresos {codPedido = "PED-56546", fechaEntrega = "2024-03-03", estadoIngreso = "Acreditado",ingresosRepartidor = "1,500"},
                new ListaIngresos {codPedido = "PED-56546", fechaEntrega = "2024-03-03", estadoIngreso = "Acreditado",ingresosRepartidor = "1,500"},
                new ListaIngresos {codPedido="PED-56546", fechaEntrega = "2024-03-03", estadoIngreso = "Acreditado",ingresosRepartidor = "1,500"},
                new ListaIngresos {codPedido = "PED-56546", fechaEntrega = "2024-03-03", estadoIngreso = "Acreditado",ingresosRepartidor = "1,500"},
            };
        collectionViewIngresosRepartidor.ItemsSource = ListaIngresosRepartidor;
    }

    //Modelo Temporal para cargar los datos de ingresos.
    public class ListaIngresos
    {
        public string codPedido { get; set; }
        public string fechaEntrega { get; set; }
        public string estadoIngreso { get; set; }
        public string ingresosRepartidor { get; set; }
    }
}