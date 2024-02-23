namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.CreacionProductos;
using System.Collections.ObjectModel;

/* Cambio no fusionado mediante combinación del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-windows10.0.19041.0)'
Antes:
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
public partial class HistorialProductosAgregados : ContentPage
Después:
public partial class HistorialProductosAgregados : ContentPage
*/

/* Cambio no fusionado mediante combinación del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-ios)'
Antes:
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
public partial class HistorialProductosAgregados : ContentPage
Después:
public partial class HistorialProductosAgregados : ContentPage
*/

/* Cambio no fusionado mediante combinación del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-maccatalyst)'
Antes:
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
public partial class HistorialProductosAgregados : ContentPage
Después:
public partial class HistorialProductosAgregados : ContentPage
*/
public partial class HistorialProductosAgregados : ContentPage
{

    public ObservableCollection<HistorialProductos> ListaPedidosRepartidor { get; set; }
    public HistorialProductosAgregados()
    {
        InitializeComponent();

        ListaPedidosRepartidor = new ObservableCollection<HistorialProductos>
            {
                new HistorialProductos {  imagenCasa = "Profile/fondo.png", Categoria = "50 rosas ", stock = "10", descuento = "10", precio="1700.10"},
                new HistorialProductos {  imagenCasa = "Profile/fondo.png", Categoria = "50 rosas ", stock = "10", descuento = "10", precio="1700.10"},
                new HistorialProductos {  imagenCasa = "Profile/fondo.png", Categoria = "50 rosas",  stock = "10", descuento = "10", precio="1700.10"},
                new HistorialProductos {  imagenCasa = "Profile/fondo.png", Categoria = "50 rosas ", stock = "10", descuento = "10", precio="1700.10"},
                new HistorialProductos {  imagenCasa = "Profile/fondo.png", Categoria = "50 rosas", stock = "10", descuento = "10", precio="1700.10"},
                new HistorialProductos {  imagenCasa = "Profile/fondo.png", Categoria = "50 rosas", stock = "10", descuento = "10", precio="1700.10"},
                new HistorialProductos {  imagenCasa = "Profile/fondo.png", Categoria = "50 rosas  ",stock = "10", descuento = "10", precio="1700.10"},
                new HistorialProductos {  imagenCasa = "Profile/fondo.png", Categoria = "50 rosas", stock = "10", descuento = "10", precio="1700.10"},
            };
        collectionViewHistorialAgregados.ItemsSource = ListaPedidosRepartidor;
    }

    public class HistorialProductos
    {

        public string imagenCasa { get; set; }
        public string Categoria { get; set; }
        public string stock { get; set; }
        public string descuento { get; set; }
        public string precio { get; set; }
    }
    private void btnImagenAtras_Clicked(object sender, EventArgs e)
    {

    }

    private void btnActualizarProducto_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ActualizarProductos());
    }

}