/*
 * Descripción:
 * Este código define la lógica de backend para la página 'carritoCompras' de la aplicación Floristeria Margaritas.
 * Permite al usuario ver los productos en su carrito de compras, actualizar los precios si es necesario, modificar la cantidad de productos
 * y proceder a realizar la orden de compra.
 */

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using System.Collections.ObjectModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos;

public partial class carritoCompras : ContentPage
{
    private ShoppingCartController shoppingCartController;
    private ApiService _apiService;
    private List<Modelos.ShoppingCartItem> shoppingCartItems;
    public ObservableCollection<FrameOrden> Ordenes { get; set; }
    private double TotalPrecio { get; set; }
    private double ISV { get; set; }
    private double Envio { get; set; }
    private double Total { get; set; }

    // Constructor
    public carritoCompras()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
        _apiService = new ApiService();

        InitializeAsync();
    }

    // Método para inicializar la página
    private async void InitializeAsync()
    {
        shoppingCartController = new ShoppingCartController();

        shoppingCartItems = await shoppingCartController.getListProductos();

        //Para guardar items actualizados
        List<Modelos.ShoppingCartItem> shoppingCartItemsUpdated = new List<ShoppingCartItem>();

        Ordenes = new ObservableCollection<FrameOrden>();

        // Itera sobre los productos en el carrito
        foreach (var item in shoppingCartItems)
        {
            var productos = await _apiService.PostDataAsync<ProductoModel[]>("actualizarPrecios.php", new { idproducto = item.idproducto });

            foreach (var producto in productos)
            {
                if (producto.precioventa != item.precioventa || producto.descuento != item.descuento)
                {
                    item.precioventaOutdated = item.precioventa;
                    item.descuentoOutdated = item.descuento;
                    item.precioventa = producto.precioventa;
                    item.descuento = producto.descuento;
                    await shoppingCartController.updateProducto(item);

                    //agrega el item con el nuevo precio y descuento
                    shoppingCartItemsUpdated.Add(item);
                }
            }

            double precioProducto = Double.Parse(item.precioventa);
            double discountPercentage = Double.Parse(item.descuento) / 100.0;
            double discountedPrice = Math.Round(precioProducto - (precioProducto * discountPercentage), 2);

            // Crea un objeto FrameOrden para mostrar el producto en la interfaz de usuario
            FrameOrden frameOrden = new FrameOrden
            {
                IdItem = item.Id,
                IdProducto = item.idproducto,
                ImageSource = item.enlacefoto,
                LabelNombreProducto = item.nombreproducto,
                LabelPrecioTotal = $"{item.cantidad * discountedPrice:N2}",
                PrecioProducto = discountedPrice,
                EntryQuantity = item.cantidad,
                StockQuantity = item.stock,
                TappedCommand = new Command(() => HandleItemTapped(item.Id)),
                substractCommand = new Command<FrameOrden>(item => HandleSubtractTapped(item, discountedPrice)),
                addCommand = new Command<FrameOrden>(item => HandleAddTapped(item, discountedPrice))
            };

            Ordenes.Add(frameOrden);
        }

        CalculateTotalPrecio();

        collectionViewCarrito.ItemsSource = Ordenes;

        labelSubtotal.Text = $"L {TotalPrecio:N2}";

        showPrecioActualizadoAlert(shoppingCartItemsUpdated);
    }

    // Método para calcular el precio total
    private void CalculateTotalPrecio()
    {
        TotalPrecio = Ordenes.Sum(item => item.EntryQuantity * item.PrecioProducto);
        ISV = TotalPrecio * 0.12;
        Envio = 50.00;
        Total = TotalPrecio + ISV + Envio;

        labelSubtotal.Text = $"L {TotalPrecio:N2}";
        labelISV.Text = $"L {ISV:N2}";
        labelEnvio.Text = $"L {Envio:N2}";
        labelTotal.Text = $"L {Total:N2}";
    }

    // Método para convertir el precio de string a double
    private double ParsePrecio(string precio)
    {
        if (double.TryParse(precio, out double result))
            return result;

        return 0.0;
    }

    // Método para manejar el límite de cantidad de productos
    private void CheckQuantityLimit(FrameOrden frameOrden)
    {
        // Calcula el limite de productos disponibles basado en el stock
        int limit = CalculateQuantityLimit(frameOrden);

        DisplayAlert("Límite de Productos Alcanzado", $"Puedes comprar hasta {limit} de este producto.", "OK");
    }

    // Método para calcular el límite de cantidad de productos
    private int CalculateQuantityLimit(FrameOrden frameOrden)
    {
        // Calcula el limite basado en el stock (La mitad del stock)
        int limit = frameOrden.StockQuantity / 2;

        // Si la mitad es un numero inpar lo redondea abajo
        if (frameOrden.StockQuantity % 2 != 0)
        {
            limit--;
        }

        return limit > 0 ? limit : 1; // Se asegura que el limite sea mayor a uno
    }

    // Método para mostrar la alerta de precio actualizado
    private async void showPrecioActualizadoAlert(List<Modelos.ShoppingCartItem> items)
    {
        foreach (var item in items)
        {
            int idProducto = item.idproducto;

            //calculo de precio con descuento anterior
            double precioProductoOutdated = Double.Parse(item.precioventaOutdated);
            double discountPercentageOutdated = Double.Parse(item.descuentoOutdated) / 100.0;
            double discountedPriceOutdated = Math.Round(precioProductoOutdated - (precioProductoOutdated * discountPercentageOutdated), 2);

            //calculo de precio con descuento nuevo
            double precioProductoUpdated = Double.Parse(item.precioventa);
            double discountPercentageUpdated = Double.Parse(item.descuento) / 100.0;
            double discountedPriceUpdated = Math.Round(precioProductoUpdated - (precioProductoUpdated * discountPercentageUpdated), 2);

            bool userResponse = await DisplayAlert(
            "Cambio de precio",
            $"El producto '{item.nombreproducto}' ha cambiado de precio de L{discountedPriceOutdated} a L{discountedPriceUpdated}. ¿Desea mantenerlo en el carrito?",
            "Sí",
            "No");

            if (!userResponse)
            {
                var tappedItem = Ordenes.FirstOrDefault(item => item.IdProducto == idProducto);
                await shoppingCartController.deleteProducto(item.Id);
                Ordenes.Remove(tappedItem);
                CalculateTotalPrecio();
                collectionViewCarrito.ItemsSource = Ordenes;
            }
        }

    }

    // Método para manejar el evento de toque en un elemento del carrito de compras
    private async void HandleItemTapped(int itemID)
    {
        var tappedItem = Ordenes.FirstOrDefault(item => item.IdItem == itemID);

        if (tappedItem != null)
        {
            bool userConfirmed = await DisplayAlert("Confirmación", "¿Está seguro de que desea eliminar este producto del carrito de compras?", "Si", "No");

            if (userConfirmed)
            {
                await shoppingCartController.deleteProducto(tappedItem.IdItem);

                Ordenes.Remove(tappedItem);

                CalculateTotalPrecio();
                collectionViewCarrito.ItemsSource = Ordenes;
            }
        }
    }

    // Método para manejar la resta de la cantidad de un producto en el carrito
    private void HandleSubtractTapped(FrameOrden item, double precio)
    {
        int quantity = item.EntryQuantity;
        if (quantity > 1)
        {
            quantity--;
            item.EntryQuantity = quantity;
            item.LabelPrecioTotal = string.Format("{0:N2}", item.EntryQuantity * precio);
            CalculateTotalPrecio();
            OnPropertyChanged(nameof(item.EntryQuantity));
            OnPropertyChanged(nameof(item.LabelPrecioTotal));
            updateCantidad(item);
        }
    }

    // Método para manejar la adición de la cantidad de un producto en el carrito
    private void HandleAddTapped(FrameOrden item, double precio)
    {
        if (item.EntryQuantity + 1 <= CalculateQuantityLimit(item))
        {
            item.EntryQuantity++;
            item.LabelPrecioTotal = string.Format("{0:N2}", item.EntryQuantity * precio);
            CalculateTotalPrecio();
            OnPropertyChanged(nameof(item.EntryQuantity));
            OnPropertyChanged(nameof(item.LabelPrecioTotal));
            updateCantidad(item);
        }
        else
        {
            CheckQuantityLimit(item);
        }
    }

    // Método para actualizar la cantidad de un producto en el carrito
    private async void updateCantidad(FrameOrden frame)
    {
        foreach (var item in shoppingCartItems)
        {
            if(item.idproducto == frame.IdProducto)
            {
                item.cantidad = frame.EntryQuantity;
                await shoppingCartController.updateProducto(item);
            }   
        }
    }

    // Método para manejar el evento Clicked del botón 'btnBack'
    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Home.homePageUser());
    }

    // Método para manejar el evento Clicked del botón 'btnProductos'
    private void btnProductos_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Productos.productos());
    }

    // Método para manejar el evento Clicked del botón 'btnRealizarOrden'
    private async void btnRealizarOrden_Clicked(object sender, EventArgs e)
    {
        if(Ordenes.Count()==0)
        {
            await DisplayAlert("Aviso", "Su carrito de compras esta vacío", "OK");
            return;
        }

        var data = new PagoDireccionTotalesModel
        {
            TotalPrecio = TotalPrecio,
            ISV = ISV,
            Envio = Envio,
            Total = Total
        };

        await Navigation.PushAsync(new Views.Productos.pagoDireccion { BindingContext = data });
    }
}