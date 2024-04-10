using Java.Time;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.RestApi;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Services;
using SQLitePCL;
using System.Text.Json;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallasRepartidor{ 
    public partial class DetallePedido : ContentPage{
        int ped_idpedido;
        string? NumPhone;
        string? url_photo;
        int tipoAccion;

        public DetallePedido(int idPedido, int tipo){
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            ped_idpedido = idPedido;
            tipoAccion = tipo;

            if(tipoAccion == 1)
            {
                btnAceptar.Text = "Aceptar Pedido";
            }
            else
            {
                btnAceptar.Text = "Iniciar Pedido";
            }
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

                    string? horaFormateada;
                    string dateString = list[0].ped_fechaestimadaentrega;

                    // Specify the format of the date string
                    string format = "yyyy-MM-dd HH:mm:ss";

                    // Create a DateTime variable to store the converted value
                    DateTime dateTime;

                    if (DateTime.TryParseExact(dateString, format, null, System.Globalization.DateTimeStyles.None, out dateTime))
                    {
                        horaFormateada = (dateTime).ToString("h:mm tt");
                    }
                    else
                    {
                        horaFormateada = list[0].ped_fechaestimadaentrega;
                    }

                    img_product.Source=list[0].pro_enlacefoto;
                    lbl_client.Text="Cliente: "+list[0].cli_nombrecompleto;
                    lbl_product.Text="Producto: "+list[0].pro_nombreproducto;
                    lbl_location.Text="Ubicación: "+list[0].dir_direccion;
                    lbl_date.Text=$"Fecha Estimada de Entrega: \n{horaFormateada}";
                    lbl_phone.Text="Teléfono: "+list[0].cli_telefono;
                    lbl_sucursal.Text="Sucursal: "+list[0].suc_nombresucursal;
                    lbl_note.Text="Nota del usuario: "+list[0].ped_notapedido;

                    NumPhone=list[0].cli_telefono;
                    url_photo=list[0].pro_enlacefoto;
                }
            } catch(Exception ex) {
                await DisplayAlert("Advertencia",""+ex,"OK");
            }
        }

        private async Task update_pedido() {
            TableJoinQueries data = new TableJoinQueries();
            data.ped_idpedido=ped_idpedido;
            data.ped_idestadopedido=1;
            data.ped_fk_idrepartidor=Config.Config.activeRepartidorId;
            string response = "";

            try {
                response=await Task.Run(() => Methods.insert_update_async(data,RestApiData.update_pedido));

                if(response=="exitoso") {
                    await DisplayAlert("Exitoso", "El Pedido ha sido Aceptado", "OK");
                    await Navigation.PushAsync(new Views.PantallasRepartidor.PantallaPedidosEntrantes());
                } else {
                    await DisplayAlert("Advertencia","El pedido ya no esta Disponible","OK");
                    await Navigation.PushAsync(new Views.PantallasRepartidor.PantallaPedidosEntrantes());
                }
            } catch(Exception ex) {
                await DisplayAlert("Advertencia",""+ex,"OK");
            }
        }

        private async void Iniciar()
        {
            await DisplayAlert("Atencion", "Puede inciar su recorrido a la sucursal", "OK");
            await Navigation.PushAsync(new MapaSucursal(ped_idpedido));
        }

        private async void make_phone_call_Clicked(object sender,EventArgs e) {
            bool userConfirmed = await DisplayAlert("Atención", "¿Desea realizar una llamada al repartidor?", "Si", "No");
            if (userConfirmed)
            {
                string? phone = NumPhone;
                var uri = $"tel:{phone}";
                await Launcher.OpenAsync(uri);
            }
        }

        private async void open_whatsapp_Clicked(object sender,EventArgs e) {
            bool userConfirmed = await DisplayAlert("Atención", "¿Desea abrir la aplicación de WhatsApp para comunicarse con el repartidor?", "Si", "No");
            if (userConfirmed)
            {
                string? phone = NumPhone;
                var uri = $"whatsapp://send?phone={phone}";
                await Launcher.OpenAsync(uri);
            }
        }

        private async void view_photo_Tapped(object sender,TappedEventArgs e) {
            await Navigation.PushModalAsync(new ModalPhoto(url_photo));
        }

        private async void start_order_Clicked(object sender,EventArgs e) {
            if(tipoAccion == 1)
            {
                await update_pedido();
            }
            else
            {
                Iniciar();
            }
            
        }
    }
}