using ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.CustomViews;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using Microsoft.Extensions.Primitives;
namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos;

public partial class productoDetalle : ContentPage
{
    private DetalleProductoViewModel _viewModel;
    private ProductoModel _productoModel;
    private CustomPopupViewAgregar customPopup;
    //Variables de prueba
    double precioTotal = 0;
    double precioProducto = 0;

    public productoDetalle(ProductoModel selectedProduct)
    {
        InitializeComponent();
        _productoModel = selectedProduct;
        _viewModel = new DetalleProductoViewModel(selectedProduct);
        customPopup = new CustomPopupViewAgregar();

        // Coloca el contexto de la pagina al viewmodel
        this.BindingContext = _viewModel;
        precioProducto = Double.Parse(_productoModel.precioventa);
    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {

    }

    private async void btnAgregar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NavigationPage(customPopup));
    }

    private void btnSubstract_Clicked(object sender, EventArgs e)
    {
        if (int.TryParse(quantityEntry.Text, out int quantity) && quantity > 1)
        {
            // Decrement the quantity, but ensure it doesn't go below 1
            quantity--;
            quantityEntry.Text = quantity.ToString();
            precioTotal = double.Parse(quantityEntry.Text) * precioProducto;
            labelPrecio.Text = $"L{precioTotal:N2}";
        }
    }

    private void btnAdd_Clicked(object sender, EventArgs e)
    {
        if (int.TryParse(quantityEntry.Text, out int quantity))
        {
            // Increment the quantity
            quantity++;
            quantityEntry.Text = quantity.ToString();
            precioTotal = double.Parse(quantityEntry.Text) * precioProducto;
            labelPrecio.Text = $"L {precioTotal:N2}";
        }
    }
}