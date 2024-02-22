namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallaAdministrador;

public partial class CrearPromociones : ContentPage
{
    public CrearPromociones()
    {
        InitializeComponent();
    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {

    }

    private void btnBuscarProductosOfertas_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.PantallaAdministrador.BuscarProducto_Promociones());

    }


    //Metodo para saber cuando el switch este habilitado o no, de esa forma habilitar los demás controles.
    public void switchEstado(object sender, ToggledEventArgs e)
    {
        bool estadoSwitch = SwitchAplicarPromocion.IsToggled; //Obtener el valor en el que esta el switch.

        if (estadoSwitch)
        {
            lblBuscarProductos.IsEnabled = true;
            btnBuscarProductosOfertas.IsEnabled = true;
        }
        else
        {
            lblBuscarProductos.IsEnabled = false;
            btnBuscarProductosOfertas.IsEnabled = false;
        }
    }
}