using ProyectoFinal_Grupo3_Floristeria_Margaritas.RestApi;
using System.Text.Json;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallasRepartidor;

public partial class AplicarRepartidor : ContentPage{

    public AplicarRepartidor(){
        InitializeComponent();
    }

    protected override async void OnAppearing() {
        base.OnAppearing();
        await get_departments();
    }

    private async Task get_departments() {
        string response = "";

        try {
            response=await Task.Run(() => Methods.select_async(new TableJoinQueries(),RestApiData.select_departments));

            if(!string.IsNullOrEmpty(response)) {
                List<TableJoinQueries>? list = JsonSerializer.Deserialize<List<TableJoinQueries>>(response);
                pk_deparments.ItemsSource=list.Select(item=>item.dep_departamento).ToList();
            }
        } catch(Exception ex) {
            await DisplayAlert("Advertencia",""+ex,"OK");
        }
    }

    private async Task get_city(int id_depto){
        TableJoinQueries data=new TableJoinQueries();
        data.ciu_iddepartamento=id_depto;
        string response = "";

        try {
            response=await Task.Run(() => Methods.select_async(data,RestApiData.select_city));

            if(!string.IsNullOrEmpty(response)) {
                List<TableJoinQueries>? list = JsonSerializer.Deserialize<List<TableJoinQueries>>(response);
                pk_city.ItemsSource=list.Select(item => item.ciu_ciudad).ToList();
            }
        } catch(Exception ex) {
            await DisplayAlert("Advertencia",""+ex,"OK");
        }
    }

    private async Task insert_repartidor() {
        TableJoinQueries data = new TableJoinQueries();
        data.cli_telefono=txt_name.Text;
        data.cli_telefono=txt_surname.Text;
        data.cli_telefono=txt_email.Text;
        data.cli_telefono=txt_phone.Text;
        data.cli_telefono=txt_dni.Text;
        /*data.cli_telefono=dp_birthdate.Date;
        data.cli_telefono=pk_city.SelectedIndex+1;
        data.cli_telefono=pk_gender.SelectedItem;*/
        string response = "";

        try {
            response=await Task.Run(() => Methods.select_async(data,RestApiData.select_city));

            if(!string.IsNullOrEmpty(response)) {
                List<TableJoinQueries>? list = JsonSerializer.Deserialize<List<TableJoinQueries>>(response);
                pk_city.ItemsSource=list.Select(item => item.ciu_ciudad).ToList();
            }
        } catch(Exception ex) {
            await DisplayAlert("Advertencia",""+ex,"OK");
        }
    }

    private async void selected_department(object sender, EventArgs e){
        var picker=(Picker) sender;
        await get_city(picker.SelectedIndex+1);
    }

    private void btnAtras_Clicked(object sender, EventArgs e){

    }
}