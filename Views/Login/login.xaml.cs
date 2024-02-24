namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login
{
    public partial class login : ContentPage
    {
        private Color originalBackgroundColor;

        public login()
        {
            InitializeComponent();

        
        }

        private async void btnEntrar_Clicked(object sender, EventArgs e)
        {

        }

        private async void btnRegistrarse_Clicked(object sender, EventArgs e)
        {
            // Aquí puedes navegar a la página deseada
            await Navigation.PushAsync(new Views.Login.singin());
        }


        private void btnAplicar_Clicked(object sender, EventArgs e)
        {

        }

        private async void LabelRecuperar_Tapped(object sender, System.EventArgs e)
        {
            // Animación de escala al hacer clic en el Label
            await labelRecuperar.ScaleTo(0.8, 50, Easing.Linear);
            await labelRecuperar.ScaleTo(1, 50, Easing.Linear);

            // Aquí puedes navegar a la página deseada
            await Navigation.PushAsync(new Views.Login.RestaurarContra());
        }
    }
}
