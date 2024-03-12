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
        public ShoppingCartItems shoppingCartItems { get; set; }
    }

    public confirmarOrden()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        if (allowed == false)
        {
            Navigation.PopToRootAsync();
        }

        BindingContext = this;
        _apiService = new ApiService();

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is Tuple<PagoDireccionTotalesModel, DireccionModel, TarjetaModel> combinedData)
        {
            PagoDireccionTotalesModel data = combinedData.Item1;
            selectedDireccion = combinedData.Item2;
            selectedTarjeta = combinedData.Item3;

            labelDescripcionDireccion.Text = selectedDireccion.descripcion;
            labelDireccion.Text = selectedDireccion.direccion;
            labelCiudadDept.Text = $"{selectedDireccion.ciudad}, {selectedDireccion.departamento}";

            labelDescripcionTarjeta.Text = selectedTarjeta.descripcion;
            labelNumeroTarjeta.Text = selectedTarjeta.numerotarjeta;
            labelNombreTarjeta.Text = selectedTarjeta.nombre;

            labelSubtotal.Text = $"L {Math.Round(data.TotalPrecio, 2):F2}";
            labelISV.Text = $"L {Math.Round(data.ISV, 2):F2}";
            labelEnvio.Text = $"L {Math.Round(data.Envio, 2):F2}";
            labelTotal.Text = $"L {Math.Round(data.Total, 2):F2}";

            InitializeAsync();
        }
    }

    private async void InitializeAsync()
    {
        shoppingCartController = new ShoppingCartController();

        List<Modelos.ShoppingCartItem> shoppingCartItems = await shoppingCartController.getListProductos();

        Ordenes = new ObservableCollection<FrameOrden>();

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

        collectionViewCarrito.ItemsSource = Ordenes;
    }

    private double ParsePrecio(string precio)
    {
        if (double.TryParse(precio, out double result))
            return result;

        return 0.0;
    }

    private void btnCancelar_Clicked(object sender, EventArgs e)
    {
        Navigation.PopToRootAsync();
    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void btnFinalizar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(entryTitulo.Text))
        {
            await DisplayAlert("Alerta", "Porfavor ingrese un título", "OK");
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

            bool userConfirmed = await DisplayAlert("Confirmación", "¿Está seguro que su orden es correcta?", "Si", "No");

            if (userConfirmed)
            {
                List<int> ID = new List<int>();
                List<int> Cantidad = new List<int>();

                foreach(var item in Ordenes)
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
                    shoppingCartItems = new ShoppingCartItems
                    {
                        ID = ID,
                        Cantidad = Cantidad
                    }
                };

                var tiempoEstimado = await _apiService.PostDataAsync<deliveryTimeModel>("agregarPedido.php", shoppingCartRequest);
                DateTime deliveryTime = tiempoEstimado.deliverytime;

                await shoppingCartController.DeleteAllProductos();

                var pagoRealizadoPage = new CustomPopupPagoRealizado();
                pagoRealizadoPage.BindingContext = deliveryTime;

                await Navigation.PushModalAsync(new NavigationPage(pagoRealizadoPage));       
            }
        }
        else
        {
            await DisplayAlert("Error", "Este pedido ya esta en Proceso", "OK");
            await Navigation.PopToRootAsync();
        }
    }
}