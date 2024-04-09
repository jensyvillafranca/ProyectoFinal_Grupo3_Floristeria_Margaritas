namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallasRepartidor{
	public partial class ModalPhoto : ContentPage{
        public ModalPhoto(string url_photo) {
            InitializeComponent();
            img_photo.Source=url_photo;
        }

        private void cancel_modal(object sender,EventArgs e) {
            Navigation.PopModalAsync();
        }
    }
}