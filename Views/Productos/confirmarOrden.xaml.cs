/*
 * Descripci�n:
 * Este c�digo define la l�gica de backend para la p�gina 'confirmarOrden' de la aplicaci�n Floristeria Margaritas.
 * Permite al usuario confirmar los detalles de su orden antes de finalizarla, incluyendo la direcci�n de entrega, tarjeta de pago,
 * propina y mensaje adicional.
 */

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos;

using System.Collections.ObjectModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.CustomViews;

public partial class confirmarOrden : ContentPage
{
    private ApiService _apiService;
    private ShoppingCartController shoppingCartController;
    public ObservableCollection<FrameOrden> Ordenes { get; set; }
    private DireccionModel selectedDireccion { get; set; }
    private TarjetaModel selectedTarjeta { get; set; }
    private ShoppingCartRequest shoppingCartRequest { get; set; }
    private double propina = 0;
    private double total = 0;


    //variable para controlar si el usuario preciona el boton de atras una vez hecho el pedido
    private bool allowed = true;

    //Modelo de las listas de productos y cantidades
    public class ShoppingCartItems
    {
        public List<int> ID { get; set; }
        public List<int> Cantidad { get; set; }
    }

    //Modelo para enviar el request al api
    public class ShoppingCartRequest
    {
        public int idCliente { get; set; }
        public int idDireccion { get; set; }
        public int idTarjeta { get; set; }
        public string titulonota { get; set; }
        public string nota { get; set; }
        public double propina { get; set; }
        public ShoppingCartItems shoppingCartItems { get; set; }
    }

    // Constructor
    public confirmarOrden()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        // Si no est� permitido, redirige al usuario a la p�gina de inicio
        if (allowed == false)
        {
            Navigation.PushAsync(new Views.Home.homePageUser());
        }

        propinaPicker.IsEnabled = false;
        
        BindingContext = this;
        _apiService = new ApiService();

    }

    // M�todo que se ejecuta al aparecer la p�gina
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Si el contexto de enlace es una tupla de modelos combinados de pago, direcci�n y tarjeta
        if (BindingContext is Tuple<PagoDireccionTotalesModel, DireccionModel, TarjetaModel> combinedData)
        {
            PagoDireccionTotalesModel data = combinedData.Item1;
            selectedDireccion = combinedData.Item2;
            selectedTarjeta = combinedData.Item3;

            // Actualiza la interfaz de usuario con los detalles de la direcci�n y tarjeta seleccionados
            labelDescripcionDireccion.Text = selectedDireccion.descripcion;
            labelDireccion.Text = selectedDireccion.direccion;
            labelCiudadDept.Text = $"{selectedDireccion.ciudad}, {selectedDireccion.departamento}";

            labelDescripcionTarjeta.Text = selectedTarjeta.descripcion;
            labelNumeroTarjeta.Text = selectedTarjeta.numerotarjeta;
            labelNombreTarjeta.Text = selectedTarjeta.nombre;

            total = data.Total;

            labelSubtotal.Text = $"L {Math.Round(data.TotalPrecio, 2):F2}";
            labelISV.Text = $"L {Math.Round(data.ISV, 2):F2}";
            labelEnvio.Text = $"L {Math.Round(data.Envio, 2):F2}";
            labelPropina.Text = $"L {Math.Round(propina, 2):F2}";
            labelTotal.Text = $"L {Math.Round(data.Total, 2):F2}";

            InitializeAsync();
        }
    }

    // M�todo para inicializar los elementos del carrito de compras
    private async void InitializeAsync()
    {
        shoppingCartController = new ShoppingCartController();

        // Obtiene los elementos del carrito de compras desde el controlador
        List<Modelos.ShoppingCartItem> shoppingCartItems = await shoppingCartController.getListProductos();

        Ordenes = new ObservableCollection<FrameOrden>();

        // Itera sobre los elementos del carrito y los agrega a la colecci�n observable
        foreach (var item in shoppingCartItems)
        {
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
                StockQuantity = item.stock
            };

            Ordenes.Add(frameOrden);
        }

        // Asigna la colecci�n de �rdenes a la vista
        collectionViewCarrito.ItemsSource = Ordenes;
    }

    // M�todo para convertir el precio de string a double
    private double ParsePrecio(string precio)
    {
        if (double.TryParse(precio, out double result))
            return result;

        return 0.0;
    }

    // M�todo para manejar el evento de clic en el bot�n de cancelar
    private async void btnCancelar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Productos.carritoCompras());
    }

    // M�todo para manejar el evento de clic en el bot�n de retroceso
    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    // M�todo para manejar el evento de clic en el bot�n de finalizar pedido
    private async void btnFinalizar_Clicked(object sender, EventArgs e)
    {
        // Verifica si se ha ingresado un t�tulo y un mensaje
        if (string.IsNullOrEmpty(entryTitulo.Text))
        {
            await DisplayAlert("Alerta", "Porfavor ingrese la persona de destino", "OK");
            return;
        }
        else if (string.IsNullOrEmpty(entryMensaje.Text))
        {
            await DisplayAlert("Alerta", "Porfavor ingrese un Mensaje", "OK");
            return;
        }

        if (allowed)
        {
            allowed = false;

            // Solicita confirmaci�n al usuario antes de finalizar el pedido
            bool userConfirmed = await DisplayAlert("Confirmaci�n", "�Est� seguro que su orden es correcta?", "Si", "No");

            if (userConfirmed)
            {
                List<int> ID = new List<int>();
                List<int> Cantidad = new List<int>();

                // Construye la lista de IDs y cantidades para la solicitud al API
                foreach (var item in Ordenes)
                {
                    ID.Add(item.IdProducto);
                    Cantidad.Add(item.EntryQuantity);
                }

                shoppingCartRequest = new ShoppingCartRequest
                {
                    idCliente = Config.Config.activeUserId,
                    idDireccion = selectedDireccion.iddireccion,
                    idTarjeta = selectedTarjeta.idtarjeta,
                    titulonota = entryTitulo.Text.ToString(),
                    nota = entryMensaje.Text.ToString(),
                    propina = propina,
                    shoppingCartItems = new ShoppingCartItems
                    {
                        ID = ID,
                        Cantidad = Cantidad
                    }
                };

                // Realiza la solicitud al API para agregar el pedido
                var tiempoEstimado = await _apiService.PostDataAsync<deliveryTimeModel>("agregarPedido.php", shoppingCartRequest);
                DateTime deliveryTime = tiempoEstimado.deliverytime;

                // Elimina todos los productos del carrito de compras despu�s de realizar el pedido
                await shoppingCartController.DeleteAllProductos();

                // Muestra una p�gina modal de pago realizado con el tiempo estimado de entrega
                var pagoRealizadoPage = new CustomPopupPagoRealizado();
                pagoRealizadoPage.BindingContext = deliveryTime;

                await Navigation.PushModalAsync(new NavigationPage(pagoRealizadoPage));       
            }
        }
        else
        {
            // Si el pedido ya est� en proceso, muestra un mensaje de error
            await DisplayAlert("Error", "Este pedido ya esta en Proceso", "OK");
            await Navigation.PushAsync(new Views.Home.homePageUser());
        }
    }

    // M�todo para manejar el cambio de �ndice en el selector de propina
    private void propinaPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        double holder = total;
        propina =  double.Parse(propinaPicker.SelectedItem.ToString());
        holder = holder + propina;
        labelPropina.Text = $"L {Math.Round(propina, 2):F2}";
        labelTotal.Text = $"L {Math.Round(holder, 2):F2}";
    }

    // M�todo para manejar el cambio de estado en el interruptor de propina
    private void switchPropina_Toggled(object sender, ToggledEventArgs e)
    {
        if(switchPropina.IsToggled)
        {
            labelMsjPropina.Text = "Si";
            labelMsjPropina.TextColor = Color.FromRgb(0, 0, 0);
            propinaPicker.IsEnabled = true;
            
            if(propinaPicker.SelectedIndex != -1) 
            {
                double holder = total; 
                propina = int.Parse(propinaPicker.SelectedItem.ToString());
                holder = holder + propina;
                labelPropina.Text = $"L {Math.Round(propina, 2):F2}";
                labelTotal.Text = $"L {Math.Round(holder, 2):F2}";
            }        
        }
        else
        {
            labelMsjPropina.Text = "Ahora No";
            labelMsjPropina.TextColor = Color.FromRgb(128, 128, 128);
            propinaPicker.IsEnabled = false;
            propina = 0;
            labelPropina.Text = $"L {Math.Round(propina, 2):F2}";
            labelTotal.Text = $"L {Math.Round(total, 2):F2}";
        }
        
    }
}