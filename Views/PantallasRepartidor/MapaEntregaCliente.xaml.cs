namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallasRepartidor;

public partial class MapaEntregaCliente : ContentPage
{
    public MapaEntregaCliente()
    {
        InitializeComponent();
    }

    private void btnEntregarPedidoCliente_Clicked(object sender, EventArgs e)
    {
        //Una prueba únicamente
        string mensaje = "Es momento de entregar el producto al cliente.";
        Navigation.PushModalAsync(new PantallasRepartidor.Modal_Pedidos(mensaje));
    }
}