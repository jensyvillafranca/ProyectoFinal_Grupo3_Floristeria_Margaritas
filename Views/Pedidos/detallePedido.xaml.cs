/*
 * Descripción:
 * Este código define la lógica de backend para la página 'detallePedido' de la aplicación Floristeria Margaritas.
 * Permite al usuario ver los detalles de un pedido específico, incluidos los productos, el estado, la dirección de entrega, etc.
 * Además, ofrece funcionalidades como comunicarse con el repartidor, cancelar el pedido y descargar la factura en formato PDF.
 */

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Config;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using System;
using System.Collections.ObjectModel;
using PdfSharp.Pdf;
using Color = Microsoft.Maui.Graphics.Color;
using Java.Time;

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

    // Método que se ejecuta cuando la página se muestra
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
            // Manejar cualquier excepción que ocurra durante la obtención de productos del detalle del pedido
        }

        string? image = null;
        string? estado = null;
        Color? color = null;
        string? entregar = $"Entregar a: {_pedidoModel.titulonota}";

        string? nombreRepartidor = null;
        string? telefonoRepartidor = null;
        string? horaEntrega = null;

        if(string.IsNullOrEmpty(_pedidoModel.motivocalificacion))
        {
            labelMotivo.Text = "El Pedido aun no ha sido calificado";
        }
        else
        {
            //variables imagenes
            string? star1 = null;
            string? star2 = null;
            string? star3 = null;
            string? star4 = null;
            string? star5 = null;

            labelMotivo.Text = _pedidoModel.motivocalificacion;

            string[] stars = new string[5];

            for (int i = 0; i < 5; i++)
            {
                if (int.Parse(_pedidoModel.calificacion) > i)
                {
                    stars[i] = "ImagenesCalificacion/estrella_llena1.png";
                }
                else
                {
                    stars[i] = "ImagenesCalificacion/estrella_vacia.png";
                }
            }

            star1 = stars[0];
            star2 = stars[1];
            star3 = stars[2];
            star4 = stars[3];
            star5 = stars[4];

            imgStar1.Source = star1;
            imgStar2.Source = star2;
            imgStar3.Source = star3;
            imgStar4.Source = star4;
            imgStar5.Source = star5;
        }

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
            btnPDF.IsEnabled = false;
            btnCalificar.IsEnabled = false;
        }
        else if (_pedidoModel.idestadopedido == 3)
        {
            image = "Estados/encamino.png";
            estado = "En Camino";
            color = Color.FromRgb(0, 191, 255);
            horaEntrega = (_pedidoModel.fechaestimadaentrega).ToString("h:mm tt");
            btnCalificar.IsEnabled=false;
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
        labelEntregarA.Text = entregar;


    }

    // Método para manejar el evento Clicked del botón de retroceso
    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    // Método para manejar el evento Clicked del botón de WhatsApp del repartidor
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

    // Método para manejar el evento Clicked del botón de llamada al repartidor
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

    // Método para manejar el evento Clicked del botón de calificación
    private async void btnCalificar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(_pedidoModel.motivocalificacion))
        {
            await Navigation.PushAsync(new Views.Calificacion.PantallaCalificacion(_pedidoModel, 1));
        }
        else
        {
            await Navigation.PushAsync(new Views.Calificacion.PantallaCalificacion(_pedidoModel, 2));
        }
        
    }

    // Método para manejar el evento Clicked del botón de cancelación del pedido
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
                        idrepartidor = _pedidoModel.fk_idrepartidor,
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

    // Método para manejar el evento Clicked del botón de descarga de PDF de la factura
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


    // Método para abrir una URL en el navegador
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