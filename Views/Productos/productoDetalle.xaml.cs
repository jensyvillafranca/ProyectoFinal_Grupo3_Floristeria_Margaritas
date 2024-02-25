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
        labelDisponible.Text = $"Cantidad Disponible: {CalculateQuantityLimit()}";
    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void btnAgregar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NavigationPage(customPopup));
    }

    private void btnSubstract_Clicked(object sender, EventArgs e)
    {
        if (int.TryParse(quantityEntry.Text, out int quantity) && quantity > 1)
        {
            // La cantidad no baja de 1
            quantity--;

            quantityEntry.Text = quantity.ToString();

            // Actualizar el precio total basado en la cantidad
            UpdateTotalPrice();
        }
    }

    private void btnAdd_Clicked(object sender, EventArgs e)
    {
        if (int.TryParse(quantityEntry.Text, out int quantity))
        {
            quantity++;

            // Revisa si la cantidad no sobrepasa el limite
            if (quantity <= CalculateQuantityLimit())
            {
                quantityEntry.Text = quantity.ToString();

                UpdateTotalPrice();
            }
            else
            {
                // Muestra mensaje cuando se llega al limite
                CheckQuantityLimit();
            }
        }
    }

    private void UpdateTotalPrice()
    {
        precioTotal = double.Parse(quantityEntry.Text) * precioProducto;
        labelPrecio.Text = $"L {precioTotal:N2}";
    }

    private void CheckQuantityLimit()
    {
        // Calcula el limite de productos disponibles basado en el stock
        int limit = CalculateQuantityLimit();

        DisplayAlert("Límite de Productos Alcanzado", $"Puedes comprar hasta {limit} de este producto.", "OK");
    }

    private int CalculateQuantityLimit()
    {
        // Calcula el limite basado en el stock (La mitad del stock)
        int limit = _productoModel.stock / 2;

        // Si la mitad es un numero inpar lo redondea abajo
        if (_productoModel.stock % 2 != 0)
        {
            limit--;
        }

        return limit > 0 ? limit : 1; // Se asegura que el limite sea mayor a uno
    }
}