namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallasRepartidor;

public partial class MapaSucursal : ContentPage
{
    public MapaSucursal()
    {
        InitializeComponent();
    }

    private void btnEntregarPedido_Clicked(object sender, EventArgs e)
    {
        //Una prueba �nicamente
        string mensaje = "Dir�gete a la sucursal y realiza el prcoeso de recepci�n del producto.";
        Navigation.PushModalAsync(new PantallasRepartidor.Modal_Pedidos(mensaje));
    }
}