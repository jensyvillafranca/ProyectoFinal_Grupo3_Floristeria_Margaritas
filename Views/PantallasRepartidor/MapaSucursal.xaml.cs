using System.Text.Json;
namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallasRepartidor;

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.RestApi;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using System.Net;
using System.Text;
using System.Globalization;

public partial class MapaSucursal:ContentPage {
    int ped_idpedido;
    private readonly GeocodingService _geocodingService;
    private string? direccionActual;
    private string? ciudadActual;
    private string? lat;
    private string? lng;
    private string? sucursal;

    public MapaSucursal(int id_pedido) {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        _geocodingService = new GeocodingService(Config.Config.GoogleApiKey);
        ped_idpedido =id_pedido;
    }

    protected override async void OnAppearing() {
        base.OnAppearing();
        await getLocationService();
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
        string parametro2 = Uri.EscapeDataString(lat);
        string parametro3 = Uri.EscapeDataString(lng);
        sucursal = destinoSucursal;
        PreferencesManager.SaveString("sucursal", destinoSucursal);
        string url = $"https://phpclusters-164276-0.cloudclusters.net/mostrarMapa.php?destinoSucursal={parametros}&idPedido={ped_idpedido}&lat={parametro2}&lng={parametro3}";
        Console.Write("El mensaje: "+url);
        url_map.Source=url;
    }

    private string RemoveAccents(string text)
    {
        string normalizedString = text.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new StringBuilder();

        foreach (char c in normalizedString)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                stringBuilder.Append(c);
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    private async Task getLocationService()
    {
        // Verificar el permiso de ubicación
        var locationPermissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (locationPermissionStatus == PermissionStatus.Granted)
        {
            // Obtiene la ubicacion
            var location = await Geolocation.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Default,
                Timeout = TimeSpan.FromSeconds(10)
            });

            if (location != null)
            {
                double Lat = location.Latitude;
                double Lng = location.Longitude;

                // Obtener detalles de la ubicación a partir de las coordenadas
                var result = await _geocodingService.GetCoordinateDetailsAsync(Lat, Lng);

                direccionActual = result.Direccion;
                ciudadActual = result.Ciudad;
                lat = Lat.ToString();
                lng = Lng.ToString();
            }
            else
            {
                // Si la ubicación es nula, mostrar un mensaje de alerta
                await DisplayAlert("Alerta", "El GPS se encuentra desactivado. Porfavor active su GPS y abra la aplicación de nuevo!", "Ok");
            }
        }
        else
        {
            // Si no se otorga el permiso de ubicación, mostrar un mensaje de error y salir de la aplicación
            await DisplayAlert("Error", "Permiso de Ubicación no otorgado. El Permiso es necesario para utilizar la aplicacion.", "OK");
            Application.Current.Quit();
        }
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
            response=await Task.Run(() => Methods.insert_update_async(data,RestApiData.update_pedido2));

            if(response=="exitoso") {
                await DisplayAlert("Exitoso","Puede inciar su recorrido hacia el cleinte","OK");
                await Navigation.PushAsync(new MapaEntregaCliente(ped_idpedido, sucursal));
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