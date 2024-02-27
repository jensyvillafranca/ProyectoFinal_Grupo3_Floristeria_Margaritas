namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.DireccionesUsuario;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;

public partial class DireccionesGuardas : ContentPage

{

    public ObservableCollection<DireccionesAgregadas> ListaPedidosRepartidor { get; set; }
    public DireccionesGuardas()
    {
        InitializeComponent();

        ListaPedidosRepartidor = new ObservableCollection<DireccionesAgregadas>
            {
                new DireccionesAgregadas {  imagenCasa = "Profile/casahistorial.png", referencia = "Avenida la hacienda ", direccion = "Villeda Morales", ciudad = "Tegucigalpa", departamento="Francisco Morazan"},
                new DireccionesAgregadas {  imagenCasa = "Profile/casahistorial.png", referencia = "Avenida la hacienda ", direccion = "Villeda Morales", ciudad = "Tegucigalpa", departamento="Francisco Morazan"},
                new DireccionesAgregadas {  imagenCasa = "Profile/casahistorial.png", referencia = "Avenida la hacienda", direccion = "Villeda Morales", ciudad = "Tegucigalpa", departamento="Francisco Morazan"},
                new DireccionesAgregadas {  imagenCasa = "Profile/casahistorial.png", referencia = "Avenida la hacienda ", direccion = "Villeda Morales", ciudad = "Tegucigalpa", departamento="Francisco Morazan"},
                new DireccionesAgregadas {  imagenCasa = "Profile/casahistorial.png", referencia = "Avenida la hacienda ", direccion = "Villeda Morales", ciudad = "Tegucigalpa", departamento="Francisco Morazan"},
                new DireccionesAgregadas {  imagenCasa = "Profile/casahistorial.png", referencia = "Avenida la hacienda ", direccion = "Villeda Morales", ciudad = "Tegucigalpa", departamento="Francisco Morazan"},
                new DireccionesAgregadas {  imagenCasa = "Profile/casahistorial.png", referencia = "Avenida la hacienda  ", direccion = "Villeda Morales", ciudad = "Tegucigalpa", departamento="Francisco Morazan"},
                new DireccionesAgregadas {  imagenCasa = "Profile/casahistorial.png", referencia = "Avenida la hacienda ", direccion = "Villeda Morales", ciudad = "Tegucigalpa", departamento="Francisco Morazan"},
            };
        collectionViewPedidosRepartidor.ItemsSource = ListaPedidosRepartidor;
    }
    public class DireccionesAgregadas
    {

        public string imagenCasa { get; set; }
        public string referencia { get; set; }
        public string direccion { get; set; }
        public string ciudad { get; set; }
        public string departamento { get; set; }
    }


    private void btnImagenAtras_Clicked(object sender, EventArgs e)
    {

    }

    private void BtnEditarDireccion_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new DireccionesUsuario.EditarDireccion());
    }
    private async void TapGestureAgregarDireccion_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameAgregarDirecciones, Color.FromRgb(255, 255, 255), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new AgregarDireccionNueva());
    }

}