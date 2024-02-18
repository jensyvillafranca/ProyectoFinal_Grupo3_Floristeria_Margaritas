using System.Collections.ObjectModel;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos;

public partial class PagoTarjeta : ContentPage
{
    public ObservableCollection<TarjetaItem> Tarjetas { get; set; }
    public PagoTarjeta()
    {
        InitializeComponent();

        Tarjetas = new ObservableCollection<TarjetaItem>
        {
                new TarjetaItem { ImageSource = "Productos/tarjetacredito.png", LabelDescripcion = "Tarjeta Credito Ficohsa", LabelNumeroTarjeta = "4234 5678 9123 4567", LabelNombreTarjeta = "Gustavo Brocato"},
                new TarjetaItem { ImageSource = "Productos/tarjetacredito.png", LabelDescripcion = "Tarjeta Debito BAC", LabelNumeroTarjeta = "3759 876543 21001", LabelNombreTarjeta = "Gustavo Brocato"},
        };

        collectionViewTarjetas.ItemsSource = Tarjetas;
    }

    public class TarjetaItem
    {
        public string ImageSource { get; set; }
        public string LabelDescripcion { get; set; }
        public string LabelNumeroTarjeta { get; set; }
        public string LabelNombreTarjeta { get; set; }
    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {

    }

    private void btnCancelar_Clicked(object sender, EventArgs e)
    {

    }

    private void btnSiguiente_Clicked(object sender, EventArgs e)
    {

    }

    private void TapGestureNuevaDireccion_Tapped(object sender, TappedEventArgs e)
    {

    }
}