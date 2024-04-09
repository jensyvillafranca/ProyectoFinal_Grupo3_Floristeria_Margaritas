using System.Text.Json;
namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallasRepartidor;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.RestApi;

public partial class MapaSucursal:ContentPage {
    int ped_idpedido;

    public MapaSucursal(int id_pedido) {
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
            response=await Task.Run(() => Methods.select_async(direccion,RestApiData.buscar_sucursal));
        } catch(Exception ex) {
            await DisplayAlert("Advertencia",""+ex,"OK");
        }

        List<TableJoinQueries> list = JsonSerializer.Deserialize<List<TableJoinQueries>>(response);
        mostrarMapa(list[0].direccion);
    }

    private void mostrarMapa(string destinoSucursal) {
        string parametros = Uri.EscapeDataString(destinoSucursal);
        string url = $"https://phpclusters-164276-0.cloudclusters.net/mostrarMapa.php?destinoSucursal="+parametros+"&idPedido="+ped_idpedido;
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
        return list[0].ped_idestadopedido!=1;
    }

    private async Task update_pedido() {
        if(await validate_pedido()){
            await DisplayAlert("Advertencia","Hasta que el mapa marque que haz llegado a la sucursal puedes inicar el proceso de entregar el pedido","OK");
            return;
        }

        TableJoinQueries data = new TableJoinQueries();
        data.ped_idpedido=ped_idpedido;
        data.ped_idestadopedido=3;
        string response = "";

        try {
            response=await Task.Run(() => Methods.insert_update_async(data,RestApiData.update_pedido));

            if(response=="exitoso") {
                await DisplayAlert("Exitoso","Puede inciar su recorrido hacia el cleinte","OK");
                await Navigation.PushAsync(new MapaEntregaCliente(ped_idpedido));
            } else {
                await DisplayAlert("Advertencia","No se modifico: "+response,"OK");
            }
        } catch(Exception ex) {
            await DisplayAlert("Advertencia",""+ex,"OK");
        }
    }

    private async void btnEntregarPedido_Clicked(object sender,EventArgs e) {
        await update_pedido();
    }
}