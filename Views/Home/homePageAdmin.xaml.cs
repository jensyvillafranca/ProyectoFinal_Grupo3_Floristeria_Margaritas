using System.Collections.ObjectModel;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Home;

public partial class homePageAdmin : ContentPage
{
    ObservableCollection<string> departamentos;
    ObservableCollection<string> repartidores;
    public homePageAdmin()
    {
        InitializeComponent();
        departamentos = new ObservableCollection<string>
        {
            "Atl�ntida",
            "Choluteca",
            "Col�n",
            "Comayagua",
            "Cop�n",
            "Cort�s",
            "El Para�so",
            "Francisco Moraz�n",
            "Gracias a Dios",
            "Intibuc�",
            "Islas de la Bah�a",
            "La Paz",
            "Lempira",
            "Ocotepeque",
            "Olancho",
            "Santa B�rbara",
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

    private void btnNotification_Clicked(object sender, EventArgs e)
    {

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

    private void departamentoPicker_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void repartidorPicker_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void btnHome_Clicked(object sender, EventArgs e)
    {

    }

    private void btnStock_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.CreacionProductos.AgregarProductos());
    }

    private void btnEstadisticas_Clicked(object sender, EventArgs e)
    {

    }

    private void btnAnuncios_Clicked(object sender, EventArgs e)
    {

    }

    private void btnPerfil_Clicked(object sender, EventArgs e)
    {

    }

    private void btnVerVencidos_Clicked(object sender, EventArgs e)
    {

    }
}