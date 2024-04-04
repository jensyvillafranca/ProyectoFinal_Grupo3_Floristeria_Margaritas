using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Config;
using System.Text.RegularExpressions;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.CreacionProductos;

public partial class ActualizarProductos : ContentPage
{
    private ApiService _apiService = new ApiService();
    //Imagen
    private byte[] imagenPrducto;
    private string imagenFilePath;
    private string base64Imagen;
    private ProductoModel actualizacionProducto; 
    FileResult imagen;

    public ObservableCollection<FiltroModel> Categorias { get; set; }
    private int tipoNavegacion;

    //Clase que permite guardar objetos que estan vinculados a un elemento de interface
    ObservableCollection<string> categoria;
    private int selectedCategoriaId = 0;

    //Para el picker de categorias
    private FiltroModel _selectedCategoria;
    /*
 * Propiedad que representa la categoría seleccionada.
 */
    public FiltroModel SelectedCategoria
    {
        get { return _selectedCategoria; } // Obtiene la categoría seleccionada.
        set
        {
            // Verifica si la categoría seleccionada es diferente a la categoría actualmente seleccionada.
            if (_selectedCategoria != value)
            {
                // Establece la nueva categoría seleccionada.
                _selectedCategoria = value;

                // Notifica a los suscriptores que la propiedad SelectedCategoria ha cambiado.
                OnPropertyChanged(nameof(SelectedCategoria));

                // Imprime en la consola la categoría seleccionada.
                Console.WriteLine($"Selected Categoria: {SelectedCategoria}");
            }
        }
    }
    string categorias;
    public ActualizarProductos(int tipo, ProductoModel producto)
    {
        InitializeComponent();
        Categorias = new ObservableCollection<FiltroModel>();
        LoadFiltrosCategoriaDataAsync();
        tipoNavegacion = tipo;
        actualizacionProducto = producto;
        txtNombreproductos.Text = producto.nombreproducto;
        txtPresioVenta.Text = (producto.precioventa).ToString();
        txtStock.Text = (producto.stock ).ToString();
        txtAgregarDescuento.Text = (producto.descuento).ToString();
        SelectorImagenes.Source = producto.enlacefoto;
        entryDescripcion.Text = producto.descripcion;

    }


    private void SwitchDescuento_Toggled(object sender, ToggledEventArgs e)
    {
        txtAgregarDescuento.IsEnabled = e.Value;
    }

    //Para poder seleccionar los productos
    private void cotegoriaPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        categorias = categoriaPicker.SelectedItem as string;
    }

    private async Task LoadFiltrosCategoriaDataAsync()
    {
        // Realiza una solicitud al servidor para obtener los filtros de categoría.
        var filtroCategorias = await _apiService.GetDataAsync<FiltroModel[]>("obtenerFiltros.php");

        // Imprime en la consola la cantidad de categorías recibidas del servidor.
        Console.WriteLine($"Received {filtroCategorias.Length} categorias from the server.");

        // Limpia la lista de categorías.
        Categorias.Clear();

        // Agrega cada categoría recibida del servidor a la lista de categorías.
        foreach (var categoria in filtroCategorias)
        {
            Categorias.Add(categoria);
        }

        // Desvincula el evento SelectedIndexChanged del selector de categorías para evitar duplicaciones.
        categoriaPicker.SelectedIndexChanged -= cotegoriaPicker_SelectedIndexChanged;

        // Limpia los elementos del selector de categorías.
        categoriaPicker.Items.Clear();

        // Agrega cada categoría recibida del servidor como un elemento del selector de categorías.
        foreach (var categoria in filtroCategorias)
        {
            categoriaPicker.Items.Add(categoria.categoria);
        }

        // Desvincula nuevamente el evento SelectedIndexChanged del selector de categorías para evitar duplicaciones.
        categoriaPicker.SelectedIndexChanged -= cotegoriaPicker_SelectedIndexChanged;

        // Establece el índice seleccionado del selector de categorías a -1 para deseleccionar cualquier categoría.
        categoriaPicker.SelectedIndex = -1;
    }

    public string? GetImg64()
    {
        if (imagen != null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Stream stream = imagen.OpenReadAsync().Result;
                stream.CopyTo(ms);
                byte[] data = ms.ToArray();

                String Base64 = Convert.ToBase64String(data);

                return Base64;
            }
        }
        return null;
    }

    private async void btnAgregarproducto_clickend(object sender, EventArgs e)
    {
        // Verifica si el campo de texto para el nombre del producto está vacío o contiene caracteres no alfabéticos.
        if (string.IsNullOrEmpty(txtNombreproductos.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese el nombre del producto", "OK");
            return;
        }
        // Verifica si no se ha seleccionado una categoría para el producto.
        else if (categoriaPicker.SelectedIndex == 0)
        {
            await DisplayAlert("Alerta", "Por favor seleccione una categoría para el producto", "OK");
            return;
        }
        // Verifica si el campo de texto para el precio de venta está vacío.
        else if (string.IsNullOrEmpty(txtPresioVenta.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese el precio de venta del producto", "OK");
            return;
        }
        // Verifica si el campo de texto para el stock está vacío.
        else if (string.IsNullOrEmpty(txtStock.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese el stock del producto", "OK");
            return;
        }
        // Verifica si el valor de stock ingresado es mayor que 50.
        else if (Convert.ToInt32(txtStock.Text) > 50)
        {
            await DisplayAlert("Alerta", "El stock no puede ser mayor que 50", "OK");
            return;
        }

        // Verifica si no se ha ingresado una imagen del producto.
        if (string.IsNullOrEmpty(base64Imagen))
        {
            await DisplayAlert("Alerta", "Por favor ingrese una imagen del producto", "OK");
            return;
        }
        // Verifica si el campo de texto para el descuento está vacío.
        else if (string.IsNullOrEmpty(txtAgregarDescuento.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese el descuento del producto", "OK");
            return;
        }
        else
        {
            // Convierte el valor ingresado a un número entero.
            int descuento;
            if (!int.TryParse(txtAgregarDescuento.Text, out descuento))
            {
                await DisplayAlert("Alerta", "El descuento debe ser un número entero", "OK");
                return;
            }

            // Verifica si el descuento está dentro del rango permitido (1 - 75).
            if (descuento < 1 || descuento > 75)
            {
                await DisplayAlert("Alerta", "El descuento debe estar entre 1 y 75", "OK");
                return;
            }
        }

        // Verifica si el campo de texto para la descripción del producto está vacío.
        if (string.IsNullOrEmpty(entryDescripcion.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese una descripción del producto", "OK");
            return;
        }

        var data = new
        {
            // Define un objeto de datos con la información del producto.
            nombreproducto = txtNombreproductos.Text,
            precioventa = txtPresioVenta.Text,
            stock = txtStock.Text,
            enlacefoto = SelectorImagenes?.ToString(),
            descuento = txtAgregarDescuento.Text,
            descripcion = entryDescripcion.Text
        };

        // Realiza una solicitud POST al servidor para agregar el producto.
        bool isSuccess = await _apiService.PostSuccessAsync("RegistrosProductos.php", data);

        if (isSuccess)
        {
            // Si la solicitud fue exitosa, limpia los campos y muestra un mensaje de éxito.
            labelProductos.Text = string.Empty;
            categoriaPicker.SelectedIndex = -1;
            labelPrecioVenta.Text = string.Empty;
            labelStock.Text = string.Empty;
            base64Imagen = string.Empty;
            labelIngresaDescuento.Text = string.Empty;
            labelDescripcion.Text = string.Empty;

            await DisplayAlert("Alerta", "El producto ha sido agregado correctamente!", "OK");

            // Si el tipo de navegación es 1, navega a la página de historial de productos agregados.
            if (tipoNavegacion == 1)
            {
                await Navigation.PushAsync(new Views.CreacionProductos.HistorialProductosAgregados());
            }
            // De lo contrario, envía un mensaje para actualizar la colección y regresa a la página anterior.
            else
            {
                MessagingCenter.Send<object>(this, "UpdateCollectionView");
                await Navigation.PopAsync();
            }
        }
        else
        {
            // Si la solicitud falla, muestra un mensaje de error.
            await DisplayAlert("Error", "Error al agregar el producto!", "OK");
        }
    }

    private async void btnSubirGaleria_Cliecked(object sender, EventArgs e)
    {
        try
        {
            // Abre la galería de medios para que el usuario seleccione una foto.
            var galeria = await MediaPicker.PickPhotoAsync();

            // Verifica si se ha seleccionado una foto desde la galería.
            if (galeria != null)
            {
                // Abre un flujo de memoria para la foto seleccionada.
                var memoriaStream = await galeria.OpenReadAsync();

                // Establece la imagen seleccionada como origen de la imagen en la interfaz de usuario.
                SelectorImagenes.Source = ImageSource.FromFile(galeria.FullPath);
            }
            else
            {
                Console.WriteLine("Photo selection cancelled or failed."); // Mensaje en caso de que se cancele o falle la selección de la foto.
            }
        }
        catch (FeatureNotSupportedException)
        {
            Console.WriteLine("Photo picking is not supported on this device."); // Mensaje en caso de que la selección de fotos no esté soportada en el dispositivo.
        }
        catch (PermissionException)
        {
            Console.WriteLine("Gallery permission denied."); // Mensaje en caso de que se deniegue el permiso para acceder a la galería.
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error picking photo: {ex.Message}"); // Mensaje en caso de que ocurra un error al seleccionar la foto.
        }
    }


    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private void btnHome_Clicked(object sender, EventArgs e)
    {

    }

    private void btnStock_Clicked(object sender, EventArgs e)
    {

    }

    private void btnEstadisticas_Clicked(object sender, EventArgs e)
    {

    }

    private void btnAnuncios_Clicked(object sender, EventArgs e)
    {

    }

    private void btnPerfil_Clicked(object sender, EventArgs e)
    {

    }

    private void btnVerVencidos_Clicked(object sender, EventArgs e)
    {

    }

    private void btnLogout_Clicked(object sender, EventArgs e)
    {

    }


}