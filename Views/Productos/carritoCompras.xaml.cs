using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using System.Collections.ObjectModel;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos;

public partial class carritoCompras : ContentPage
{
    private ShoppingCartController shoppingCartController;
    private ApiService _apiService;
    public ObservableCollection<FrameOrden> Ordenes { get; set; }
    private double TotalPrecio { get; set; }
    private double ISV { get; set; }
    private double Envio { get; set; }
    private double Total { get; set; }

    public carritoCompras()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
        _apiService = new ApiService();

        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        shoppingCartController = new ShoppingCartController();

        List<Modelos.ShoppingCartItem> shoppingCartItems = await shoppingCartController.getListProductos();

        //Para guardar items actualizados
        List<Modelos.ShoppingCartItem> shoppingCartItemsUpdated = new List<ShoppingCartItem>();

        Ordenes = new ObservableCollection<FrameOrden>();

        foreach (var item in shoppingCartItems)
        {
            var productos = await _apiService.PostDataAsync<ProductoModel[]>("actualizarPrecios.php", new { idproducto = item.idproducto });

            foreach (var producto in productos)
            {
                if(producto.precioventa != item.precioventa || producto.descuento != item.descuento)
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

            FrameOrden frameOrden = new FrameOrden
            {
                IdItem = item.Id,
                IdProducto = item.idproducto,
                ImageSource = item.enlacefoto,
                LabelNombreProducto = item.nombreproducto,
                LabelPrecioTotal = $"{item.cantidad * discountedPrice:N2}",
                PrecioProducto = ParsePrecio(item.precioventa),
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

    private double ParsePrecio(string precio)
    {
        if (double.TryParse(precio, out double result))
            return result;

        return 0.0;
    }

    private void CheckQuantityLimit(FrameOrden frameOrden)
    {
        // Calcula el limite de productos disponibles basado en el stock
        int limit = CalculateQuantityLimit(frameOrden);

        DisplayAlert("Límite de Productos Alcanzado", $"Puedes comprar hasta {limit} de este producto.", "OK");
    }

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

    private async void showPrecioActualizadoAlert(List<Modelos.ShoppingCartItem> items)
    {
        foreach(var item in items)
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
        }
    }

    private void HandleAddTapped(FrameOrden item, double precio)
    {     
        if (item.EntryQuantity+1 <= CalculateQuantityLimit(item))
        {
            item.EntryQuantity++;
            item.LabelPrecioTotal = string.Format("{0:N2}", item.EntryQuantity * precio);
            CalculateTotalPrecio();
            OnPropertyChanged(nameof(item.EntryQuantity));
            OnPropertyChanged(nameof(item.LabelPrecioTotal));
        }
        else
        {
            CheckQuantityLimit(item);
        }       
    }


    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Home.homePageUser());
    }

    private void btnProductos_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Productos.productos());
    }

    private void btnRealizarOrden_Clicked(object sender, EventArgs e)
    {

    }
}