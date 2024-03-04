namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallaAdministrador;
using System.Collections.ObjectModel;

public partial class notificacionesAdministrador : ContentPage
{
    public ObservableCollection<SolicitudItem> Solicitudes { get; set; }
    public notificacionesAdministrador()
    {
        InitializeComponent();
        Solicitudes = new ObservableCollection<SolicitudItem>
            {
                new SolicitudItem { LabelIntro = "Juan López", LabelFecha = "12-05-2024", LabelNombre = "Juan José López", LabelCorreo = "juanjose@gmail.com", LabelTelefono = "9548-9132", LabelDNI = "0502-2003-00820", LabelReqEdad = "Cumple con el requerimiento de ser mayor de edad. (20 años)", LabelDireccion = "Cortés, Ciudad Choloma", LabelGenero = "Masculino", LabelLicencia = "Cuenta con licencia de vehículo liviano"},
                new SolicitudItem { LabelIntro = "Juan López", LabelFecha = "12-05-2024", LabelNombre = "Juan José López", LabelCorreo = "juanjose@gmail.com", LabelTelefono = "9548-9132", LabelDNI = "0502-2003-00820", LabelReqEdad = "No cumple con el requerimiento de ser mayor de edad. (20 años)", LabelDireccion = "Cortés, Ciudad Choloma", LabelGenero = "Masculino", LabelLicencia = "No cuenta con licencia de vehículo liviano"},
                new SolicitudItem { LabelIntro = "Juan López", LabelFecha = "12-05-2024", LabelNombre = "Juan José López", LabelCorreo = "juanjose@gmail.com", LabelTelefono = "9548-9132", LabelDNI = "0502-2003-00820", LabelReqEdad = "Cumple con el requerimiento de ser mayor de edad. (20 años)", LabelDireccion = "Cortés, Ciudad Choloma", LabelGenero = "Masculino", LabelLicencia = "Cuenta con licencia de vehículo liviano"},
                new SolicitudItem {LabelIntro = "Juan López", LabelFecha = "12-05-2024", LabelNombre = "Juan José López", LabelCorreo = "juanjose@gmail.com", LabelTelefono = "9548-9132", LabelDNI = "0502-2003-00820", LabelReqEdad = "No cumple con el requerimiento de ser mayor de edad. (20 años)", LabelDireccion = "Cortés, Ciudad Choloma", LabelGenero = "Masculino" , LabelLicencia = "No cuenta con licencia de vehículo liviano"},
                new SolicitudItem {LabelIntro = "Juan López", LabelFecha = "12-05-2024", LabelNombre = "Juan José López", LabelCorreo = "juanjose@gmail.com", LabelTelefono = "9548-9132", LabelDNI = "0502-2003-00820", LabelReqEdad = "Cumple con el requerimiento de ser mayor de edad. (20 años)", LabelDireccion = "Cortés, Ciudad Choloma", LabelGenero = "Masculino", LabelLicencia = "Cuenta con licencia de vehículo liviano"},
            };

        collectionViewNotificaciones.ItemsSource = Solicitudes;
    }

    public class SolicitudItem
    {
        public string LabelIntro { get; set; }
        public string LabelFecha { get; set; }
        public string LabelNombre { get; set; }
        public string LabelCorreo { get; set; }
        public string LabelTelefono { get; set; }
        public string LabelDNI { get; set; }
        public string LabelReqEdad { get; set; }
        public string LabelDireccion { get; set; }
        public string LabelGenero { get; set; }
        public string LabelLicencia { get; set; }
    }

    private void btnAtras_Clicked(object sender, EventArgs e)
    {

    }

    private void btnNotification_Clicked(object sender, EventArgs e)
    {

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

    private void btnLogout_Clicked(object sender, EventArgs e)
    {

    }
}