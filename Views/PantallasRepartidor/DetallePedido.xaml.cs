using ProyectoFinal_Grupo3_Floristeria_Margaritas.RestApi;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Services;
using SQLitePCL;
using System.Text.Json;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallasRepartidor{ 
    public partial class DetallePedido : ContentPage{
        int ped_idpedido;
        string phone;
        string url_photo;

        public DetallePedido(int id_pedido){
            InitializeComponent();

            ped_idpedido=id_pedido;
        }

        protected override async void OnAppearing(){
            base.OnAppearing();
            await get_data();
        }

        private async Task get_data(){
            TableJoinQueries data=new TableJoinQueries();
            data.ped_idpedido=ped_idpedido;
            string response="";

            try{
                response=await Task.Run(() => Methods.select_async(data,RestApiData.select_detalle_pedido));

                if(!string.IsNullOrEmpty(response)) {
                    List<TableJoinQueries> list=JsonSerializer.Deserialize<List<TableJoinQueries>>(response);

                    img_product.Source=list[0].pro_enlacefoto;
                    lbl_client.Text="Cliente: "+list[0].cli_nombrecompleto;
                    lbl_product.Text="Producto: "+list[0].pro_nombreproducto;
                    lbl_location.Text="Ubicación: "+list[0].dir_direccion;
                    lbl_date.Text="Fecha Estimada de Entrega: "+list[0].ped_fechaestimadaentrega;
                    lbl_phone.Text="Teléfono: "+list[0].cli_telefono;
                    lbl_sucursal.Text="Sucursal: "+list[0].suc_nombresucursal;
                    lbl_note.Text="Nota del usuario: "+list[0].ped_notapedido;

                    phone=list[0].cli_telefono;
                    url_photo=list[0].pro_enlacefoto;
                }
            } catch(Exception ex) {
                await DisplayAlert("Advertencia",""+ex,"OK");
            }
        }

        private async Task update_pedido() {
            TableJoinQueries data = new TableJoinQueries();
            data.ped_idpedido=ped_idpedido;
            data.ped_idestadopedido=2;
            string response = "";

            try {
                response=await Task.Run(() => Methods.insert_update_async(data,RestApiData.update_pedido));

                if(response=="exitoso") {
                    await DisplayAlert("Exitoso","Puede inciar su recorrido a la sucursal","OK");
                    await Navigation.PushAsync(new MapaSucursal(ped_idpedido));
                } else {
                    await DisplayAlert("Advertencia","No se modifico: "+response,"OK");
                }
            } catch(Exception ex) {
                await DisplayAlert("Advertencia",""+ex,"OK");
            }
        }

        private async void make_phone_call_Clicked(object sender,EventArgs e) {
            await PhoneService.make_phone_call(phone);
        }

        private async void open_whatsapp_Clicked(object sender,EventArgs e) {
            await WhatsAppService.open_whatsapp(phone);
        }

        private async void view_photo_Tapped(object sender,TappedEventArgs e) {
            await Navigation.PushModalAsync(new ModalPhoto(url_photo));
        }

        private async void start_order_Clicked(object sender,EventArgs e) {
            await update_pedido();
        }
    }
}