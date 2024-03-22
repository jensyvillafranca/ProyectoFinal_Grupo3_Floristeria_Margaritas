using ProyectoFinal_Grupo3_Floristeria_Margaritas.Config;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using System;
using System.Collections.ObjectModel;
using PdfSharp.Pdf;
using Color = Microsoft.Maui.Graphics.Color;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Pedidos;

public partial class detallePedido : ContentPage
{
    private pedidoModel _pedidoModel;
    private ApiService _apiService;
    private PdfDocument pdf = new PdfDocument();

    public ObservableCollection<productoDetalleModel>? ProductosList { get; set; }

    public class enlaceFactura
    {
        public string? factura { get; set; }
    }

    public detallePedido(pedidoModel pedido)
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
        _apiService = new ApiService();
        _pedidoModel = pedido;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var productos = await _apiService.PostDataAsync<productoDetalleModel[]>("obtenerProductosDetalle.php", new { idpedido = _pedidoModel.idpedido });
            ProductosList = new ObservableCollection<productoDetalleModel>();

            foreach(var producto in productos)
            {
                ProductosList.Add(producto);
            }

            collectionViewProductos.ItemsSource = ProductosList;

        }
        catch (Exception ex)
        {

        }

        string? image = null;
        string? estado = null;
        Color? color = null;

        string? nombreRepartidor = null;
        string? telefonoRepartidor = null;
        string? horaEntrega = null;

        if(string.IsNullOrEmpty(_pedidoModel.nombresrepartidor))
        {
            nombreRepartidor = "No Asignado";
            telefonoRepartidor = "No Asignado";
        }
        else
        {
            nombreRepartidor = _pedidoModel.nombresrepartidor;
            telefonoRepartidor = _pedidoModel.telefonorepartidor;
        }

        if (_pedidoModel.idestadopedido == 2 || _pedidoModel.idestadopedido == 1)
        {
            image = "Estados/procesando.png";
            estado = "Procesando";
            color = Color.FromRgb(204, 204, 0);
            horaEntrega = (_pedidoModel.fechaestimadaentrega).ToString("h:mm tt");
        }
        else if (_pedidoModel.idestadopedido == 3)
        {
            image = "Estados/encamino.png";
            estado = "En Camino";
            color = Color.FromRgb(0, 191, 255);
            horaEntrega = (_pedidoModel.fechaestimadaentrega).ToString("h:mm tt");
        }
        else if (_pedidoModel.idestadopedido == 4)
        {
            image = "Estados/delivery.jpg";
            estado = "Entregado";
            color = Color.FromRgb(0, 128, 0);
            DateTime horaEntregado;
            DateTime.TryParse(_pedidoModel.fechaentregado, out horaEntregado);
            horaEntrega = horaEntregado.ToString("MM/dd/yyyy");
            btnCancelar.IsEnabled = false;
        }
        else if (_pedidoModel.idestadopedido == 5)
        {
            image = "Estados/cancelado.jpg";
            estado = "Cancelado";
            color = Color.FromRgb(255, 0, 0);
            horaEntrega = "Pedido Cancelado";
            btnCancelar.IsEnabled = false;
        }

        labelTitulo.Text = $"#PED-{_pedidoModel.idpedido}";
        labelHora.Text = horaEntrega;
        imgProducto.Source = image;
        labelEstadoPedido.Text = estado;
        labelEstadoPedido.TextColor = color;
        frameDetalle.BorderColor = color;
        labelNombreSurursal.Text = _pedidoModel.nombresucursal;
        labelDireccionSucursal.Text = _pedidoModel.direccionsucursal;
        labelNombreRepartidor.Text = $"Repartidor: {nombreRepartidor}";
        labelTelefonoRepartidor.Text = $"Telefono: {telefonoRepartidor}";
        labelNombreCliente.Text = $"Cliente: {_pedidoModel.nombrescliente}";
        labelDireccionEntrega.Text = $"Dirección de Entrega: {_pedidoModel.direccion}";
        labelNombreTarjeta.Text = $"Nombre: {_pedidoModel.nombretarjeta}";
        labelNumeroTarjeta.Text = $"Número de Tarjeta: {_pedidoModel.numerotarjeta}";
        labelFechaPedido.Text = (_pedidoModel.fechapedido).ToString("MM/dd/yyyy hh:mm:ss tt");
        labelHoraEntrega.Text = horaEntrega;
        labelSubtotal.Text = $"L {Math.Round(_pedidoModel.subtotal, 2):F2}";
        labelISV.Text = $"L {Math.Round(_pedidoModel.isv, 2):F2}";
        labelEnvio.Text = $"L {Math.Round(_pedidoModel.envio, 2):F2}";
        labelPropina.Text = $"L {Math.Round(_pedidoModel.propina, 2):F2}";
        labelTotal.Text = $"L {Math.Round(_pedidoModel.total, 2):F2}";


    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void btnWhatsappRepartidor_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_pedidoModel.telefonorepartidor))
        {
            await DisplayAlert("Atención", "Aun no se asigna ningún repartidor al pedido", "OK");
            return;
        }

        bool userConfirmed = await DisplayAlert("Atención", "¿Desea abrir la aplicación de WhatsApp para comunicarse con el repartidor?", "Si", "No");
        if (userConfirmed)
        {
            string? phone = _pedidoModel.telefonorepartidor;
            var uri = $"whatsapp://send?phone={phone}";
            await Launcher.OpenAsync(uri);
        }
        
    }

    private async void btnLlamadaRepartidor_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_pedidoModel.telefonorepartidor))
        {
            await DisplayAlert("Atención", $"Aun no se asigna ningún repartidor al pedido", "OK");
            return;
        }

        bool userConfirmed = await DisplayAlert("Atención", "¿Desea realizar una llamada al repartidor?", "Si", "No");
        if (userConfirmed)
        {
            string? phone = _pedidoModel.telefonorepartidor;
            var uri = $"tel:{phone}";
            await Launcher.OpenAsync(uri);
        }
        
    }

    private void btnCalificar_Clicked(object sender, EventArgs e)
    {

    }

    private async void btnCancelar_Clicked(object sender, EventArgs e)
    {
        if(_pedidoModel.idestadopedido == 1 || _pedidoModel.idestadopedido == 2 || _pedidoModel.idestadopedido == 3)
        {
            double tempSubtotal = _pedidoModel.subtotal * 0.10;
            double tempISV = tempSubtotal * 0.12;
            double tempTotal = tempSubtotal + tempISV + _pedidoModel.envio;

            bool userConfirmed = await DisplayAlert("Atención", $" Por favor confirme si desea cancelar el pedido, se le cobrara un 10% del valor " +
                $"total de los productos mas el costo de envió. Su costo de cancelación seria: L{Math.Round(tempTotal, 2):F2}. ¡Esta acción es FINAL!", "Si", "No");

            if (userConfirmed)
            {
                try
                {
                    var data = new
                    {
                        idpedido = _pedidoModel.idpedido,
                        nombresucursal = _pedidoModel.nombresucursal,
                        direccionsucursal = _pedidoModel.direccionsucursal,
                        nombrecliente = _pedidoModel.nombrescliente,
                        direccioncliente = _pedidoModel.direccion,
                        nombretarjeta = _pedidoModel.nombretarjeta,
                        numerotarjeta = _pedidoModel.numerotarjeta,
                        fechapedido = (_pedidoModel.fechapedido).ToString("MM/dd/yyyy hh:mm:ss tt"),
                        subtotal = _pedidoModel.subtotal,
                        isv = _pedidoModel.isv,
                        envio = _pedidoModel.envio,
                        propina = _pedidoModel.propina,
                        total = _pedidoModel.total
                    };

                    var result = await _apiService.PostDataAsync<enlaceFactura>("cancelarPedido.php", data);

                    string? urlFactura = result.factura;

                    if (!string.IsNullOrEmpty(urlFactura))
                    {
                        await DisplayAlert("Atención", $"El pedido #PED:{_pedidoModel.idpedido} ha sido cancelado.", "OK");
                        await Navigation.PushAsync(new Views.Pedidos.pedidosPrincipal());
                    }

                }
                catch (Exception ex)
                {

                }
            }
        }
        
    }

    private async void btnPDF_Clicked(object sender, EventArgs e)
    {
        bool userConfirmed = await DisplayAlert("Atención", "¿Desea descargar su factura en formato PDF?", "Si", "No");

        if (userConfirmed)
        {
            if (string.IsNullOrEmpty(_pedidoModel.factura))
            {
                try
                {
                    var data = new
                    {
                        idpedido = _pedidoModel.idpedido,
                        nombresucursal = _pedidoModel.nombresucursal,
                        direccionsucursal = _pedidoModel.direccionsucursal,
                        nombrecliente = _pedidoModel.nombrescliente,
                        direccioncliente = _pedidoModel.direccion,
                        nombretarjeta = _pedidoModel.nombretarjeta,
                        numerotarjeta = _pedidoModel.numerotarjeta,
                        fechapedido = (_pedidoModel.fechapedido).ToString("MM/dd/yyyy hh:mm:ss tt"),
                        subtotal = _pedidoModel.subtotal,
                        isv = _pedidoModel.isv,
                        envio = _pedidoModel.envio,
                        propina = _pedidoModel.propina,
                        total = _pedidoModel.total
                    };

                    var result = await _apiService.PostDataAsync<enlaceFactura>("subirPDF.php", data);

                    string? urlFactura = result.factura;

                    if (!string.IsNullOrEmpty(urlFactura))
                    {
                        _pedidoModel.factura = urlFactura;
                        await OpenUrlAsync(urlFactura);
                    }

                }
                catch (Exception ex)
                {

                }
                return;
            }
            else
            {
                await OpenUrlAsync(_pedidoModel.factura);
                return;
            }
        }
        else
        {
            return;
        }     
    }



    async Task OpenUrlAsync(string url)
    {
        try
        {
            await Launcher.OpenAsync(new Uri(url));
        }
        catch (Exception ex)
        {
            // Handle any exceptions
            Console.WriteLine($"Error opening URL: {ex.Message}");
        }
    }
}