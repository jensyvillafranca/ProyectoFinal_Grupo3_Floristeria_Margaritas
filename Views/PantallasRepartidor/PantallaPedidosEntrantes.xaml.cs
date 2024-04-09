using ProyectoFinal_Grupo3_Floristeria_Margaritas.RestApi;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallasRepartidor;

public partial class PantallaPedidosEntrantes : ContentPage{
    public ObservableCollection<TableJoinQueries> Items{
        get; set;
    }

    public PantallaPedidosEntrantes(){
        InitializeComponent();
        Items=new ObservableCollection<TableJoinQueries>();
    }

    protected override async void OnAppearing() {
        base.OnAppearing();
        Items.Clear();

        await load_data();
        this.BindingContext=this;
    }

    private async Task load_data() {
        try {
            var services_bd = await get_data();

            if(services_bd!=null) {
                foreach(var data in services_bd){
                    if(data.ped_idestadopedido!=3){
                        data.enlace_status="ImagenesRepartidor/check.png";
                    }else{
                        data.enlace_status="ImagenesRepartidor/cliente.png";
                    }

                    Items.Add(data);
                }
            }

        } catch(Exception ex) {
            await DisplayAlert("Advertencia","error: "+ex.ToString(),"OK");
        }
    }

    private async Task<List<TableJoinQueries>?> get_data(){
        TableJoinQueries data = new TableJoinQueries();
        data.ped_fk_idrepartidor=1; 
        string response = "";

        try {
            response=await Task.Run(() => Methods.select_async(data,RestApiData.select_pedidos));

            if(!string.IsNullOrEmpty(response)) {
                List<TableJoinQueries> list = JsonSerializer.Deserialize<List<TableJoinQueries>>(response);

                return list;
            }
        } catch(Exception ex){
            await DisplayAlert("Advertencia",""+ex,"OK");
        }

        return null;
    }

    private async void get_specific_data(object sender,TappedEventArgs e) {
        var stackLayout = (StackLayout) sender;
        var item = (TableJoinQueries) stackLayout.BindingContext;

        if(item.ped_idpedido==4){
            return;
        }

        await Navigation.PushAsync(new DetallePedido(item.ped_idpedido));
    }
}