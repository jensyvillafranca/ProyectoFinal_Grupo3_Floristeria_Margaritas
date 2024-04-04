using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using System.Collections.ObjectModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Config;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos;

public partial class AgregarTarjetas : ContentPage
{
	private ApiService _apiService =  new ApiService();
    private int tipoNavegacion;

    //public ObservableCollection<TarjetasModel> TarjetasCreditos { get; set; }
    public AgregarTarjetas(int tipo)
	{
		InitializeComponent();

        NavigationPage.SetHasNavigationBar(this, false);
        tipoNavegacion = tipo;

    }

    private void EntryNumeroTarjeta(object sender, TextChangedEventArgs e)
    {
        // Obtiene el texto ingresado en el Entry.
        string texto = e.NewTextValue;

        // Asigna el texto del Entry al contenido de un control de interfaz de usuario (Label) destinado para mostrar el n�mero de tarjeta.
        NumeroTarjeta.Text = texto;
    }


    private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        // Convierte la fecha seleccionada a una cadena con el formato "yyyy-MM-dd" y la asigna al Texto de un control en la interfaz de usuario.
        FechaVencimientoss.Text = e.NewDate.ToString("yyyy-MM-dd");
    }

    private void EntryNombres(object sender, TextChangedEventArgs e)
    {
        // Obtiene el texto ingresado en el Entry.
        string texto = e.NewTextValue;

        // Asigna el texto del Entry al Texto del Label asociado.
        NombreTarjetas.Text = texto;
    }


    private async void btnAgregarTarjeta_Clicked(object sender, EventArgs e)
    {
        // Validaci�n: Verifica si el campo de ingreso de nombre est� vac�o.
        if (string.IsNullOrEmpty(entryIngreseNombre.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese su nombre completo", "OK");
            return;
        }

        // Validaci�n: Verifica si el campo de ingreso de n�mero de tarjeta est� vac�o.
        if (string.IsNullOrEmpty(entryNumeroTarjeta.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese su n�mero de tarjeta", "OK");
            return;
        }
        // Validaci�n: Verifica si el n�mero de tarjeta tiene la longitud correcta (16 d�gitos).
        else if (entryNumeroTarjeta.Text.Length != 16)
        {
            await DisplayAlert("Alerta", "El n�mero de tarjeta debe tener 16 d�gitos", "OK");
            return;
        }

        // Actualiza el Label mientras escribes en el Entry para el nombre.
        entryIngreseNombre.TextChanged += (s, args) =>
        {
            // Asigna el texto del Entry al Label
            NombreTarjetas.Text = entryIngreseNombre.Text;
        };

        // Actualiza el Label mientras escribes en el Entry para el PIN.
        Pin.TextChanged += (s, args) =>
        {
            // Asigna el texto del Entry al Label
            IngresoPin.Text = Pin.Text;
        };

        // Validaci�n: Verifica si el campo de ingreso de PIN est� vac�o.
        if (string.IsNullOrEmpty(Pin.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese el PIN de la tarjeta", "OK");
            return;
        }
        // Validaci�n: Verifica si el PIN tiene la longitud correcta (3 d�gitos).
        else if (Pin.Text.Length != 3)
        {
            await DisplayAlert("Alerta", "El n�mero de PIN debe tener 3 d�gitos", "OK");
            return;
        }

        // Validaci�n: Verifica si el campo de ingreso de descripci�n est� vac�o.
        if (string.IsNullOrEmpty(entryDescripcion.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese qu� tipo de tarjeta tiene", "OK");
            return;
        }

        // Construye un objeto con los datos de la tarjeta.
        var data = new
        {
            fk_idcliente = Config.Config.activeUserId, // ID del cliente activo
            nombre = entryIngreseNombre.Text, // Nombre completo
            numerotarjeta = entryNumeroTarjeta.Text, // N�mero de tarjeta
            fechavencimiento = FechaVencimiento.Date.ToString("yyyy-MM-dd"), // Fecha de vencimiento
            pin = Pin.Text, // PIN de la tarjeta
            descripcion = entryDescripcion.Text // Descripci�n de la tarjeta
        };

        // Realiza una solicitud POST al servidor para agregar la tarjeta.
        bool isSuccess = await _apiService.PostSuccessAsync("TarjetaCredito.php", data);

        // Verifica si la solicitud fue exitosa.
        if (isSuccess)
        {
            // Se restablecen los campos de la interfaz de usuario.
            labelNombre.Text = string.Empty;
            labelNumeroTarjeta.Text = string.Empty;
            labelFechaVencimiento.Text = string.Empty;
            labelPin.Text = string.Empty;
            labelDescripcionTarjeta.Text = string.Empty;

            // Muestra un mensaje de �xito.
            await DisplayAlert("Alerta", "La tarjeta ha sido agregada!", "OK");

            // Navega a la p�gina de pago con tarjeta si el tipo de navegaci�n es 1, de lo contrario, actualiza la vista y retrocede.
            if (tipoNavegacion == 1)
            {
                await Navigation.PushAsync(new Views.Productos.PagoTarjeta());
            }
            else
            {
                MessagingCenter.Send<object>(this, "UpdateCollectionView");
                await Navigation.PopAsync();
            }
        }
        else
        {
            // Muestra un mensaje de error si la solicitud falla.
            await DisplayAlert("Error", "Error al agregar la tarjeta!", "OK");
        }
    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }
}