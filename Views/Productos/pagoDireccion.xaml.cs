using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos;

public partial class pagoDireccion : ContentPage
{
    public ObservableCollection<DireccionItem> Direcciones { get; set; }
    public pagoDireccion()
    {
        InitializeComponent();

        Direcciones = new ObservableCollection<DireccionItem>
        {
                new DireccionItem { LabelDescripcion = "Casa Principal", LabelDireccion = "3 Ave, 12 Calle", LabelCiudadDept = "San Pedro Sula, Cortes", TappedCommand = new Command<DireccionItem>(item => HandleTappedCommand(item))},
                new DireccionItem { LabelDescripcion = "Maria", LabelDireccion = "4 Ave, 6 Calle", LabelCiudadDept = "Puerto Cortes, Cortes", TappedCommand = new Command<DireccionItem>(item => HandleTappedCommand(item)) },
                new DireccionItem { LabelDescripcion = "Madre", LabelDireccion = "1 2 Calle S", LabelCiudadDept = "San Pedro Sula, Cortes", TappedCommand = new Command<DireccionItem>(item => HandleTappedCommand(item)) },
        };
        collectionViewDirecciones.ItemsSource = Direcciones;
    }

    public class DireccionItem
    {
        public string LabelDescripcion { get; set; }
        public string LabelDireccion { get; set; }
        public string LabelCiudadDept { get; set; }
        public ICommand TappedCommand { get; set; }
    }

    private void HandleTappedCommand(DireccionItem item)
    {

    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {

    }

    private void btnCancelar_Clicked(object sender, EventArgs e)
    {

    }

    private void btnRealizarPago_Clicked(object sender, EventArgs e)
    {

    }

    private void TapGestureNuevaDireccion_Tapped(object sender, TappedEventArgs e)
    {

    }

    private void TapGestureUbicacionActual_Tapped(object sender, TappedEventArgs e)
    {

    }
}