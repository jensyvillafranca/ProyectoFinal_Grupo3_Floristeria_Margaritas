
/* Cambio no fusionado mediante combinación del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-maccatalyst)'
Antes:
using System.Collections.ObjectModel;
Después:
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using System.Collections.ObjectModel;
*/

/* Cambio no fusionado mediante combinación del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-windows10.0.19041.0)'
Antes:
using System.Collections.ObjectModel;
Después:
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using System.Collections.ObjectModel;
*/

/* Cambio no fusionado mediante combinación del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-ios)'
Antes:
using System.Collections.ObjectModel;
Después:
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using System.Collections.ObjectModel;
*/
using
/* Cambio no fusionado mediante combinación del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-maccatalyst)'
Antes:
using System.Windows.Input;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
Después:
using System.Windows.Input;
*/

/* Cambio no fusionado mediante combinación del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-windows10.0.19041.0)'
Antes:
using System.Windows.Input;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
Después:
using System.Windows.Input;
*/

/* Cambio no fusionado mediante combinación del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-ios)'
Antes:
using System.Windows.Input;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
Después:
using System.Windows.Input;
*/
ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using System.Collections.ObjectModel;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos;

public partial class carritoCompras : ContentPage
{
    public ObservableCollection<FrameOrden> Ordenes { get; set; }
    private double TotalPrecio { get; set; }
    private double ISV { get; set; }
    private double Envio { get; set; }
    private double Total { get; set; }
    private double PrecioProducto { get; set; }
    public carritoCompras()
    {
        InitializeComponent();
        BindingContext = this;

        Ordenes = new ObservableCollection<FrameOrden>
            {
                new FrameOrden { ImageSource = "Productos/flowers.png", LabelNombreProducto = "Esplendor Floral", LabelPrecioTotal = "1,500.00", PrecioProducto = 1500.00, EntryQuantity=1, TappedCommand = new Command(() => HandleItemTapped("Item 1")),
                 substractCommand = new Command<FrameOrden>(item => HandleSubtractTapped(item)),  addCommand = new Command<FrameOrden>(item => HandleAddTapped(item))},
                new FrameOrden { ImageSource = "Productos/flowers.png", LabelNombreProducto = "Agradecimiento Floral", LabelPrecioTotal = "1,700.00", PrecioProducto = 1700.00, EntryQuantity=1, TappedCommand = new Command(() => HandleItemTapped("Item 2")),
                substractCommand = new Command<FrameOrden>(item => HandleSubtractTapped(item)),  addCommand = new Command<FrameOrden>(item => HandleAddTapped(item))},
                new FrameOrden { ImageSource = "Productos/flowers.png", LabelNombreProducto = "Esplendor Floral", LabelPrecioTotal = "1,500.00", PrecioProducto = 1500.00, EntryQuantity = 1, TappedCommand = new Command(() => HandleItemTapped("Item 3")),
                substractCommand = new Command<FrameOrden>(item => HandleSubtractTapped(item)),  addCommand = new Command<FrameOrden>(item => HandleAddTapped(item))},
                // Add more items as needed
            };

        CalculateTotalPrecio();

        collectionViewCarrito.ItemsSource = Ordenes;

        // Display the totalPrecio in a label
        labelSubtotal.Text = $"L {TotalPrecio:N2}";
    }


    private void CalculateTotalPrecio()
    {
        TotalPrecio = Ordenes.Sum(item => item.EntryQuantity * item.PrecioProducto);
        ISV = TotalPrecio * 0.12;
        Envio = 50.00;
        Total = TotalPrecio + ISV + Envio;


        // Update the label displaying the totalPrecio
        labelSubtotal.Text = $"L {TotalPrecio:N2}";
        labelISV.Text = $"L {ISV:N2}";
        labelEnvio.Text = $"L {Envio:N2}";
        labelTotal.Text = $"L {Total:N2}";
    }

    private double ParsePrecio(string precio)
    {
        // Add your logic to parse the precio string and return a numeric value
        // For simplicity, assuming the precio is a valid double
        if (double.TryParse(precio, out double result))
            return result;

        return 0.0; // Default value if parsing fails
    }

    private void HandleItemTapped(string itemName)
    {
        // Handle the tapped item
    }

    private void HandleSubtractTapped(FrameOrden item)
    {
        int quantity = item.EntryQuantity;
        if (quantity > 1)
        {
            quantity--;
            item.EntryQuantity = quantity;
            item.LabelPrecioTotal = string.Format("{0:N2}", item.EntryQuantity * item.PrecioProducto);
            CalculateTotalPrecio();
            OnPropertyChanged(nameof(item.EntryQuantity));
            OnPropertyChanged(nameof(item.LabelPrecioTotal));
        }
    }

    private void HandleAddTapped(FrameOrden item)
    {
        item.EntryQuantity++;
        item.LabelPrecioTotal = string.Format("{0:N2}", item.EntryQuantity * item.PrecioProducto);
        CalculateTotalPrecio();
        OnPropertyChanged(nameof(item.EntryQuantity));
        OnPropertyChanged(nameof(item.LabelPrecioTotal));
    }


    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private void btnProductos_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Productos.productos());
    }

    private void btnRealizarOrden_Clicked(object sender, EventArgs e)
    {

    }
}