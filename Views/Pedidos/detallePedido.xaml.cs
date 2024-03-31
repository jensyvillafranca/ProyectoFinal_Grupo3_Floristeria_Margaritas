/*
 * Descripci�n:
 * Este c�digo define la l�gica de backend para la p�gina 'detallePedido' de la aplicaci�n Floristeria Margaritas.
 * Permite al usuario ver los detalles de un pedido espec�fico, incluidos los productos, el estado, la direcci�n de entrega, etc.
 * Adem�s, ofrece funcionalidades como comunicarse con el repartidor, cancelar el pedido y descargar la factura en formato PDF.
 */

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

    // Lista de productos del pedido
    public ObservableCollection<productoDetalleModel>? ProductosList { get; set; }

    // Clase para el enlace de la factura
    public class enlaceFactura
    {
        public string? factura { get; set; }
    }

    // Constructor
    public detallePedido(pedidoModel pedido)
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
        _apiService = new ApiService();
        _pedidoModel = pedido;
    }

    // M�todo que se ejecuta cuando la p�gina se muestra
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // Obtener los productos del detalle del pedido
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
            // Manejar cualquier excepci�n que ocurra durante la obtenci�n de productos del detalle del pedido
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

        // Determinar el estado del pedido y asignar valores correspondientes a las etiquetas
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
        labelDireccionEntrega.Text = $"Direcci�n de Entrega: {_pedidoModel.direccion}";
        labelNombreTarjeta.Text = $"Nombre: {_pedidoModel.nombretarjeta}";
        labelNumeroTarjeta.Text = $"N�mero de Tarjeta: {_pedidoModel.numerotarjeta}";
        labelFechaPedido.Text = (_pedidoModel.fechapedido).ToString("MM/dd/yyyy hh:mm:ss tt");
        labelHoraEntrega.Text = horaEntrega;
        labelSubtotal.Text = $"L {Math.Round(_pedidoModel.subtotal, 2):F2}";
        labelISV.Text = $"L {Math.Round(_pedidoModel.isv, 2):F2}";
        labelEnvio.Text = $"L {Math.Round(_pedidoModel.envio, 2):F2}";
        labelPropina.Text = $"L {Math.Round(_pedidoModel.propina, 2):F2}";
        labelTotal.Text = $"L {Math.Round(_pedidoModel.total, 2):F2}";


    }

    // M�todo para manejar el evento Clicked del bot�n de retroceso
    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    // M�todo para manejar el evento Clicked del bot�n de WhatsApp del repartidor
    private async void btnWhatsappRepartidor_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_pedidoModel.telefonorepartidor))
        {
            await DisplayAlert("Atenci�n", "Aun no se asigna ning�n repartidor al pedido", "OK");
            return;
        }

        bool userConfirmed = await DisplayAlert("Atenci�n", "�Desea abrir la aplicaci�n de WhatsApp para comunicarse con el repartidor?", "Si", "No");
        if (userConfirmed)
        {
            string? phone = _pedidoModel.telefonorepartidor;
            var uri = $"whatsapp://send?phone={phone}";
            await Launcher.OpenAsync(uri);
        }
        
    }

    // M�todo para manejar el evento Clicked del bot�n de llamada al repartidor
    private async void btnLlamadaRepartidor_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_pedidoModel.telefonorepartidor))
        {
            await DisplayAlert("Atenci�n", $"Aun no se asigna ning�n repartidor al pedido", "OK");
            return;
        }

        bool userConfirmed = await DisplayAlert("Atenci�n", "�Desea realizar una llamada al repartidor?", "Si", "No");
        if (userConfirmed)
        {
            string? phone = _pedidoModel.telefonorepartidor;
            var uri = $"tel:{phone}";
            await Launcher.OpenAsync(uri);
        }
        
    }

    // M�todo para manejar el evento Clicked del bot�n de calificaci�n
    private void btnCalificar_Clicked(object sender, EventArgs e)
    {

    }

    // M�todo para manejar el evento Clicked del bot�n de cancelaci�n del pedido
    private async void btnCancelar_Clicked(object sender, EventArgs e)
    {
        if(_pedidoModel.idestadopedido == 1 || _pedidoModel.idestadopedido == 2 || _pedidoModel.idestadopedido == 3)
        {
            double tempSubtotal = _pedidoModel.subtotal * 0.10;
            double tempISV = tempSubtotal * 0.12;
            double tempTotal = tempSubtotal + tempISV + _pedidoModel.envio;

            bool userConfirmed = await DisplayAlert("Atenci�n", $" Por favor confirme si desea cancelar el pedido, se le cobrara un 10% del valor " +
                $"total de los productos mas el costo de envi�. Su costo de cancelaci�n seria: L{Math.Round(tempTotal, 2):F2}. �Esta acci�n es FINAL!", "Si", "No");

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
                        await DisplayAlert("Atenci�n", $"El pedido #PED:{_pedidoModel.idpedido} ha sido cancelado.", "OK");
                        await Navigation.PushAsync(new Views.Pedidos.pedidosPrincipal());
                    }

                }
                catch (Exception ex)
                {

                }
            }
        }
        
    }

    // M�todo para manejar el evento Clicked del bot�n de descarga de PDF de la factura
    private async void btnPDF_Clicked(object sender, EventArgs e)
    {
        bool userConfirmed = await DisplayAlert("Atenci�n", "�Desea descargar su factura en formato PDF?", "Si", "No");

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


    // M�todo para abrir una URL en el navegador
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