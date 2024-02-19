namespace ProyectoFinal_Grupo3_Floristeria_Margaritas
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void CounterBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Home.homePageUser());
        }

        private void btnHomeRepartidor_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Home.homePageRepartidor());
        }

        private void btnHomeAdmin_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Home.homePageAdmin());
        }

        private void btnProductos_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Productos.productos());
        }

        private void btnProductoDetalle_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Productos.productoDetalle());
        }

        private void btnPedidosRepartidor_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.PantallasRepartidor.PantallaPedidosEntrantes());

        }

        private void btnCarrito_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Productos.carritoCompras());
        }

        private void btnlogin_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Login.login());
        }
        private void btnsignin_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Login.singin());

        }

        private void btnDireccion_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Productos.pagoDireccion());
        }

        private void btnPagoTarjeta_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Productos.PagoTarjeta());
        }

        private void btnFinalizar_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Productos.confirmarOrden());
        }


        private void btnCalificacionFinalizada_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Calificacion.CalificacionFinalizada());
        }

        private void btnPantallaCalificacion_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Calificacion.PantallaCalificacion());
        }

        private void btnAplicarRepartidor_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.PantallasRepartidor.AplicarRepartidor());
        }

        private void btnCrearPromociones_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.PantallaAdministrador.CrearPromociones());
        }
    }

}
