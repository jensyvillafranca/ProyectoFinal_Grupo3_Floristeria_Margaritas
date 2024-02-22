using System.Collections.ObjectModel;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Profile;

public partial class HistorialCompras : ContentPage
{
    public ObservableCollection<DireccionesAgregadas> ListaPedidosRepartidor { get; set; }
    public HistorialCompras()
	{

		InitializeComponent();

        ListaPedidosRepartidor = new ObservableCollection<DireccionesAgregadas>
            {
                new DireccionesAgregadas {  imagenCasa = "Profile/fondo.png", cantidad = "50 Rosas ", entregado = "Villeda Morales", evaluacion = "Profile/primerextrenia.png", comprobante="Profile/listo.png"},
                new DireccionesAgregadas {  imagenCasa = "Profile/fondo.png", cantidad = "50 Rosas ", entregado = "Villeda Morales", evaluacion = "Profile/primerextrenia.png", comprobante="Profile/listo.png"},
                new DireccionesAgregadas {  imagenCasa = "Profile/fondo.png", cantidad = "50 Rosas",  entregado = "Villeda Morales", evaluacion = "Profile/primerextrenia.png", comprobante="Profile/listo.png"},
                new DireccionesAgregadas {  imagenCasa = "Profile/fondo.png", cantidad = "50 Rosas ", entregado = "Villeda Morales", evaluacion = "Profile/primerextrenia.png", comprobante="Profile/listo.png"},
                new DireccionesAgregadas {  imagenCasa = "Profile/fondo.png", cantidad = "50 Rosas", entregado = "Villeda Morales", evaluacion = "Profile/primerextrenia.png",  comprobante="Profile/listo.png"},
                new DireccionesAgregadas {  imagenCasa = "Profile/fondo.png", cantidad = "50 Rosas", entregado = "Villeda Morales", evaluacion = "Profile/primerextrenia.png",  comprobante="Profile/listo.png"},
                new DireccionesAgregadas {  imagenCasa = "Profile/fondo.png", cantidad = "50 Rosas  ",entregado = "Villeda Morales", evaluacion = "Profile/primerextrenia.png", comprobante="Profile/listo.png"},
                new DireccionesAgregadas {  imagenCasa = "Profile/fondo.png", cantidad = "50 Rosas ", entregado = "Villeda Morales", evaluacion = "Profile/primerextrenia.png", comprobante="Profile/listo.png"},
            };
        collectionViewHistorialGuardas.ItemsSource = ListaPedidosRepartidor;
    }

    public class DireccionesAgregadas
    {

        public string imagenCasa { get; set; }
        public string cantidad { get; set; }
        public string entregado { get; set; }
        public string evaluacion { get; set; }
        public string comprobante { get; set; }
    }

    private void btnImagenAtras_Clicked(object sender, EventArgs e)
    {

    }

}