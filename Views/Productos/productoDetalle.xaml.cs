using ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.CustomViews;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using Microsoft.Extensions.Primitives;
namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos;

public partial class productoDetalle : ContentPage
{
    private DetalleProductoViewModel _viewModel;
    private ProductoModel _productoModel;
    private CustomPopupViewAgregar customPopup;
    private ShoppingCartController _shoppingCartController;

    //Variables de prueba
    double precioTotal = 0;
    double precioProducto = 0;
    double discountPercentage = 0;
    double discountedPrice = 0;

    public productoDetalle(ProductoModel selectedProduct)
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        _productoModel = selectedProduct;
        _viewModel = new DetalleProductoViewModel(selectedProduct);
        _shoppingCartController = new ShoppingCartController();
        customPopup = new CustomPopupViewAgregar();

        // Coloca el contexto de la pagina al viewmodel
        this.BindingContext = _viewModel;

        if(int.Parse(_productoModel.descuento) != 0)
        {
            labelDescuento.IsVisible = true;
            imgDescuento.IsVisible = true;
            labelPrecioOriginal.IsVisible = true;
            frameDetalle.BorderColor = Color.FromHex("#F44336");
        }
        else
        {
            labelDescuento.IsVisible= false;
            imgDescuento.IsVisible= false;
            labelPrecioOriginal.IsVisible= false;
            frameDetalle.BorderColor = Color.FromHex("#41B9FE");
        }

        precioProducto = Double.Parse(_productoModel.precioventa);
        discountPercentage = Double.Parse(_productoModel.descuento) / 100.0;
        discountedPrice = precioProducto - (precioProducto * discountPercentage);
        labelDisponible.Text = $"Cantidad Disponible: {CalculateQuantityLimit()}";
    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void btnAgregar_Clicked(object sender, EventArgs e)
    {
        var producto = new Modelos.ShoppingCartItem
        {
            idproducto = _productoModel.idproducto,
            nombreproducto = _productoModel.nombreproducto,
            categoria = _productoModel.categoria,
            precioventa = _productoModel.precioventa,
            stock = _productoModel.stock,
            enlacefoto = _productoModel.enlacefoto,
            descuento = _productoModel.descuento,
            descripcion = _productoModel.descripcion,
            cantidad = int.Parse(quantityEntry.Text)  
        };

        try
        {
            if (_shoppingCartController != null)
            {
                bool addedSuccessfully = await _shoppingCartController.storeProducto(producto);

                if (addedSuccessfully)
                {
                    await Navigation.PushModalAsync(new NavigationPage(customPopup));
                }
                else
                {
                    var result = await DisplayAlert("Aviso", "El producto ya está en el carrito", "Ir a Carrito", "Seguir Comprando");
                    if(result == true)
                    {
                        await Navigation.PushAsync(new Views.Productos.carritoCompras());
                    }
                    else
                    {
                        await Navigation.PushAsync(new Views.Productos.productos());
                    }             
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrio un Error: {ex.Message}", "OK");
        }
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
        precioTotal = double.Parse(quantityEntry.Text) * discountedPrice;
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