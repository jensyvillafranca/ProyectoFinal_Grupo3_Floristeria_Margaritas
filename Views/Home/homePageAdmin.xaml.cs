using System.Collections.ObjectModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using System.Globalization;
using System.Text.Json;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Home{
public partial class homePageAdmin : ContentPage
{
    ObservableCollection<string> departamentos;
    ObservableCollection<string> repartidores;
    private ApiService _apiService = new ApiService();
    int id_department = 0, id_repartidor = 0;
    string? start_date = null, end_date = null;


    public homePageAdmin()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        departamentos = new ObservableCollection<string>
        {
            "Atlántida",
            "Choluteca",
            "Colón",
            "Comayagua",
            "Copán",
            "Cortés",
            "El Paraíso",
            "Francisco Morazán",
            "Gracias a Dios",
            "Intibucá",
            "Islas de la Bahía",
            "La Paz",
            "Lempira",
            "Ocotepeque",
            "Olancho",
            "Santa Bárbara",
            "Valle",
            "Yoro"
        };

        repartidores = new ObservableCollection<string>
        {
            "Repartidor 1",
            "Repartidor 2",
            "Repartidor 3",
            "Repartidor 4",
            "Repartidor 5"
            };

            departamentoPicker.ItemsSource = departamentos;
            repartidorPicker.ItemsSource = repartidores;
    }

    private async void btnNotification_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Notificaciones.notificacionesAdmin());
    }

    private void TapGestureGanancias_Tapped(object sender, TappedEventArgs e)
    {

    }

    private void TapGestureStock_Tapped(object sender, TappedEventArgs e)
    {

    }

    private void btnLogout_Clicked(object sender, EventArgs e)
    {

    }

    private void TapGestureProductos_Tapped(object sender, TappedEventArgs e)
    {

    }

    private void TapGestureCarrito_Tapped(object sender, TappedEventArgs e)
    {

    }

    private void TapGestureEntregados_Tapped(object sender, TappedEventArgs e)
    {
            
    }

    private void btnVerMasEntregados_Clicked(object sender, EventArgs e)
    {

    }

    private void TapGesturePendientes_Tapped(object sender, TappedEventArgs e)
    {

    }

    private void btnVerMasPendientes_Clicked(object sender, EventArgs e)
    {

    }

    private void TapGestureCancelados_Tapped(object sender, TappedEventArgs e)
    {

    }

    private void btnVerMasCancelados_Clicked(object sender, EventArgs e)
    {

    }

    private void TapGestureFiltros_Tapped(object sender, TappedEventArgs e)
    {

    }

    private void departamentoPicker_SelectedIndexChanged(object sender, EventArgs e){
        id_department=departamentoPicker.SelectedIndex;
        
    }

    private void repartidorPicker_SelectedIndexChanged(object sender, EventArgs e){
        id_repartidor=repartidorPicker.SelectedIndex;
        
    }

    private void btnHome_Clicked(object sender, EventArgs e)
    {

    }

    private async void btnStock_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.CreacionProductos.HistorialProductosAgregados());
    }

    private void btnEstadisticas_Clicked(object sender, EventArgs e)
    {

    }

    private void btnAnuncios_Clicked(object sender, EventArgs e)
    {

    }

    private async void btnPerfil_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.PantallaAdministrador.notificacionesAdministrador());
    }

    private void btnVerVencidos_Clicked(object sender, EventArgs e)
    {

    }

    private void datePickerStartDate_DateSelected(object sender,DateChangedEventArgs e) {
        DateTime parsedDate = DateTime.ParseExact(e.NewDate.ToString(),"M/dd/yyyy hh:mm:ss tt",CultureInfo.InvariantCulture);
        start_date = parsedDate.ToString("yyyy-MM-dd");
        
    }

    private void datePickerEndDate_DateSelected(object sender,DateChangedEventArgs e) {
        DateTime parsedDate = DateTime.ParseExact(e.NewDate.ToString(),"M/dd/yyyy hh:mm:ss tt",CultureInfo.InvariantCulture);
        end_date= parsedDate.ToString("yyyy-MM-dd");
        
    }
    }
}