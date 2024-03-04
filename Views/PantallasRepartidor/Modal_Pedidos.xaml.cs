namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallasRepartidor;

public partial class Modal_Pedidos : ContentPage
{
    string mensaje_pantalla;
    public Modal_Pedidos(String mensaje)
    {
        InitializeComponent();
        mensaje_pantalla = mensaje;
        labelMensaje.Text = mensaje_pantalla;
    }
    private void cancel_modal(object sender, EventArgs e)
    {
        //Una prueba únicamente
        Navigation.PopModalAsync();
    }
}