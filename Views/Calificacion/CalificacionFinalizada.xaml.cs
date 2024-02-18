namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Calificacion
{
    public partial class CalificacionFinalizada : ContentPage
    {
        public CalificacionFinalizada()
        {
            InitializeComponent();
        }

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Calificacion.PantallaCalificacion());
        }

        private async void btnConfirmar_Clicked(object sender, EventArgs e)
        {

        }
    }
}




