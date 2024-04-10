using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.RestApi;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallasRepartidor{
    public partial class IngresosRepartidor:ContentPage {
        private enum Options{Normal, Search}
        private ApiService _apiService = new ApiService();
        private int id_repartidor = Config.Config.activeRepartidorId;

        public ObservableCollection<TableJoinQueries> Items{
            get; set;
        }

        public IngresosRepartidor() {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            Items =new ObservableCollection<TableJoinQueries>();
        }

        protected override async void OnAppearing() {
            base.OnAppearing();

            await load_data(Options.Normal);
            await get_total();
            this.BindingContext=this;
        }

        private async Task load_data(Options option) {
            Items.Clear();

            try {
                var services_bd = await get_data(option);

                if(services_bd!=null) {
                    foreach(var data in services_bd) {
                        if(data.ped_idestadopedido==3) {
                            data.enlace_status="ImagenesRepartidor/cliente.png";
                            data.color="#fac939";//B2B509
                        } else {
                            data.enlace_status="ImagenesRepartidor/check.png";
                            data.color="#00ba00";
                        }

                        Items.Add(data);
                    }
                }
            } catch(Exception ex) {
                await DisplayAlert("Advertencia","error: "+ex.ToString(),"OK");
            }
        }

        private async Task<List<TableJoinQueries>?> get_data(Options option) {
            TableJoinQueries data = new TableJoinQueries();
            data.ped_fk_idrepartidor=id_repartidor;
            string response = "";

            try {
                if(option==Options.Normal) {
                    response=await Task.Run(() => Methods.select_async(data,RestApiData.select_ingresos_repartidor));
                } else {
                    data.search_data=txt_search_data.Text;
                    response=await Task.Run(() => Methods.select_async(data,RestApiData.select_search_ingresos_pedidos));
                }

                if(!string.IsNullOrEmpty(response)) {
                    List<TableJoinQueries> list = JsonSerializer.Deserialize<List<TableJoinQueries>>(response);
                    return list;
                }
            } catch(Exception ex) {
                await DisplayAlert("Advertencia",""+ex,"OK");
            }

            return null;
        }

        private async Task get_total() {
            TableJoinQueries data = new TableJoinQueries();
            data.ped_fk_idrepartidor=id_repartidor;
            string response = "";

            try {
                response=await Task.Run(() => Methods.select_async(data,RestApiData.select_total_repartidor));

                Debug.WriteLine(response);
                if(!string.IsNullOrEmpty(response)) {
                    List<TableJoinQueries> list = JsonSerializer.Deserialize<List<TableJoinQueries>>(response);
                    double total = double.Parse(list[0].ped_sum_totales);
                    lbl_total.Text=list[0].ped_sum_totales!=null ? $"L. {total:N2}" : "L. 0.00";
                } else{
                    lbl_total.Text="L. 0.00";
                }
            } catch(Exception ex) {
                await DisplayAlert("Advertencia",""+ex,"OK");
            }
        }

        private async void txt_search_data_TextChanged(object sender,TextChangedEventArgs e) {
            await load_data(string.IsNullOrEmpty(txt_search_data.Text) ? Options.Normal : Options.Search);
        }

        private async void btnAtras_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void btnNotification_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.Notificaciones.notificacionesRepartidor());
        }

        private async void btnInicioRepartidor_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.Home.homePageRepartidor());
        }

        private async void btnPedidosRepartidor_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.PantallasRepartidor.PantallaPedidosEntrantes());
        }

        private async void btnIngresosRepartidor_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.PantallasRepartidor.IngresosRepartidor());
        }

        private async void btnHistorialPedidosRepartidor_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.PantallasRepartidor.historialEntregas());
        }

        private async void btnPerfilRepartidor_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.PantallasRepartidor.profilePageRepartidor());
        }

        private async void btnLogOutRepartidor_Clicked(object sender, EventArgs e)
        {
            bool isSuccess = await logout.PerformLogoutAsync(_apiService);

            if (isSuccess)
            {
                await Navigation.PushAsync(new Views.Login.login());
            }
        }
    }
}