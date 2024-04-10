using System.Collections.ObjectModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using System.Globalization;
using System.Text.Json;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using static Android.Graphics.ColorSpace;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Home{
public partial class homePageAdmin : ContentPage
{
    private ApiService _apiService = new ApiService();


    public homePageAdmin()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        // Método que se ejecuta cuando la página se muestra
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            List<ChartModel> data;

            data = new List<ChartModel>()
            {
                new ChartModel("Pedidos Entregados", 28),
                new ChartModel("Pedidos Pendientes", 14),
                new ChartModel("Pedidos Cancelados", 31),
            };

            try
            {
                var infomation = await _apiService.GetDataAsync<homeAdminModel[]>("HomePageAdmin.php");
                foreach (var info in infomation)
                {
                    labelPedidosEntregados.Text = (info.pedidosentregados).ToString();
                    labelPedidosPendientes.Text = (info.pedidospendientes).ToString();
                    labelPedidosCancelados.Text = (info.pedidoscancelados).ToString();
                    labelTotalGenerado.Text = $"L. {double.Parse(info.totalpedidos):N2}";

                    data = new List<ChartModel>()
                    {
                        new ChartModel("Pedidos Entregados", info.pedidosentregados),
                        new ChartModel("Pedidos Pendientes", info.pedidospendientes),
                        new ChartModel("Pedidos Cancelados", info.pedidoscancelados),
                    };
                }
                
            }
            catch (Exception ex) 
            {

            }

            series.ItemsSource = data;
            series.XBindingPath = "Pedidos";
            series.YBindingPath = "Counts";
            series.ShowDataLabels = true;
            series.DataLabelSettings.LabelStyle.TextColor = Color.FromRgb(255, 255, 255);
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

        private async void btnLogout_Clicked(object sender, EventArgs e)
        {
            bool isSuccess = await logout.PerformLogoutAsync(_apiService);

            if (isSuccess)
            {
                await Navigation.PushAsync(new Views.Login.login());
            }
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
        
        }

        private void repartidorPicker_SelectedIndexChanged(object sender, EventArgs e){
        
        }

        private async void btnHome_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.Home.homePageAdmin());
        }

        private async void btnStock_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.CreacionProductos.AgregarProducto(1));
        }

        private async void btnEstadisticas_Clicked(object sender, EventArgs e)
        {
                await Navigation.PushAsync(new Views.CreacionProductos.HistorialProductosAgregados());
        }

        private void btnAnuncios_Clicked(object sender, EventArgs e)
        {

        }

        private async void btnPerfil_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.PantallaAdministrador.notificacionesAdministrador());
        }

        private async void btnVerVencidos_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.PantallaAdministrador.notificacionesAdministrador());
        }

        private void datePickerStartDate_DateSelected(object sender,DateChangedEventArgs e) {

        
        }

        private void datePickerEndDate_DateSelected(object sender,DateChangedEventArgs e) {

        
        }
    }
}