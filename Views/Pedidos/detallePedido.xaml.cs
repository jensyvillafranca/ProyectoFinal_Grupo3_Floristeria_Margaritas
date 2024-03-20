using ProyectoFinal_Grupo3_Floristeria_Margaritas.Config;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using System;
using System.Collections.ObjectModel;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Net;
using System.Diagnostics;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Pedidos;

public partial class detallePedido : ContentPage
{
    private pedidoModel _pedidoModel;
    private ApiService _apiService;
    private PdfDocument pdf = new PdfDocument();
    public ObservableCollection<productoDetalleModel>? ProductosList { get; set; }

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

        if (_pedidoModel.idestadopedido == 2)
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
        }
        else if (_pedidoModel.idestadopedido == 5)
        {
            image = "Estados/cancelado.jpg";
            estado = "Cancelado";
            color = Color.FromRgb(255, 0, 0);
            horaEntrega = "Pedido Cancelado";
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
            await DisplayAlert("Atención", "Aun no se asigna ningún repartidor al pedido", "OK");
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

    private void btnCancelar_Clicked(object sender, EventArgs e)
    {

    }

    private async void btnPDF_Clicked(object sender, EventArgs e)
    {
        PdfPage page = pdf.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        string imageUrl = "https://firebasestorage.googleapis.com/v0/b/floristeriamargaritas-c74d1.appspot.com/o/Encabezados%2FBannerFactura.png?alt=media";
        string localImagePath = DownloadImageToLocalFile(imageUrl);
        XImage image = XImage.FromFile(localImagePath);
        gfx.DrawImage(image, 0, 0, page.Width, 100);
        string pdfFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "output.pdf");
        pdf.Save(pdfFilePath);
        string pdfUrl = "https://firebasestorage.googleapis.com/v0/b/floristeriamargaritas-c74d1.appspot.com/o/Prueba%2FDiploma.pdf?alt=media";
        await OpenUrlAsync(pdfUrl);
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

    private string DownloadImageToLocalFile(string imageUrl)
    {
        string localFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp_image.jpg");

        try
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(imageUrl, localFilePath);
            }

            return localFilePath;
        }
        catch (Exception ex)
        {

        }
        return string.Empty;
    }
}