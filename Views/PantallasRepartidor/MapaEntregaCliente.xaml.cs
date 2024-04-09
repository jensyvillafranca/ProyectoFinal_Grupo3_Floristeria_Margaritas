using ProyectoFinal_Grupo3_Floristeria_Margaritas.RestApi;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Services;
using System.Text.Json;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallasRepartidor{
    public partial class MapaEntregaCliente : ContentPage{
        int ped_idpedido;
        string phone;

        public MapaEntregaCliente(int id_pedido){
            InitializeComponent();

            ped_idpedido=id_pedido;
        }

        protected override async void OnAppearing() {
            base.OnAppearing();
            await direccionSucursal();
        }

        private async Task direccionSucursal() {
            TableJoinQueries direccion = new TableJoinQueries();
            direccion.ped_idpedido=ped_idpedido;
            string response = "";

            try {
                response=await Task.Run(() => Methods.select_async(direccion,RestApiData.select_direccion));
            } catch(Exception ex) {
                await DisplayAlert("Advertencia",""+ex,"OK");
            }

            List<TableJoinQueries> list = JsonSerializer.Deserialize<List<TableJoinQueries>>(response);
            phone=list[0].cli_telefono;
            mostrarMapa(list[0].dir_direccion);
        }

        private void mostrarMapa(string destinoSucursal) {
            string parametros = Uri.EscapeDataString(destinoSucursal);
            string url = $"https://phpclusters-164276-0.cloudclusters.net/mostrarMapaClient.php?destinoSucursal="+parametros+"&idPedido="+ped_idpedido;
            Console.Write("El mensaje: "+url);
            url_map.Source=url;
        }

        private async Task<bool> validate_pedido() {
            TableJoinQueries direccion = new TableJoinQueries();
            direccion.ped_idpedido=ped_idpedido;
            string response = "";

            try {
                response=await Task.Run(() => Methods.select_async(direccion,RestApiData.estado_pedido));
            } catch(Exception ex) {
                await DisplayAlert("Advertencia",""+ex,"OK");
            }

            List<TableJoinQueries> list = JsonSerializer.Deserialize<List<TableJoinQueries>>(response);
            return list[0].ped_idestadopedido!=6;
        }

        private async Task update_pedido() {
            if(await validate_pedido()) {
                await DisplayAlert("Advertencia","Hasta que el mapa marque que haz llegado al cliente puedes inicar el proceso de entregar el pedido","OK");
                return;
            }

            TableJoinQueries data = new TableJoinQueries();
            data.ped_idpedido=ped_idpedido;
            data.ped_idestadopedido=4;
            string response = "";

            try {
                response=await Task.Run(() => Methods.insert_update_async(data,RestApiData.update_pedido));

                if(response=="exitoso") {
                    await DisplayAlert("Exitoso","Ha finalizado su pedido","OK");
                    await Navigation.PushAsync(new PantallaPedidosEntrantes());
                } else {
                    await DisplayAlert("Advertencia","No se modifico: "+response,"OK");
                }
            } catch(Exception ex) {
                await DisplayAlert("Advertencia",""+ex,"OK");
            }
        }

        private async void btnEntregarPedidoCliente_Clicked(object sender, EventArgs e){
            await update_pedido();
        }

        private async void make_phone_call_Clicked(object sender,EventArgs e) {
            await PhoneService.make_phone_call(phone);
        }

        private async void open_whatsapp_Clicked(object sender,EventArgs e) {
            await WhatsAppService.open_whatsapp(phone);
        }
    }
}

