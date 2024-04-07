using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Calificacion
{
    public partial class PantallaCalificacion : ContentPage
    {
        private bool[] estrellasLlenas = new bool[5];
        private int calificacion = 0;
        private pedidoModel? _pedido = null;
        private ApiService _apiService = new ApiService();

        public PantallaCalificacion( pedidoModel pedido, int tipo)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _pedido = pedido;

            if (tipo == 1)
            {
                // Inicializar todas las estrellas como vacías al principio
                for (int i = 0; i < estrellasLlenas.Length; i++)
                {
                    estrellasLlenas[i] = false;
                }
            }
            else
            {
                imgEstrella1.IsEnabled = false;
                imgEstrella2.IsEnabled = false;
                imgEstrella3.IsEnabled = false;
                imgEstrella4.IsEnabled = false;
                imgEstrella5.IsEnabled = false;
                entryMotivo.IsEnabled = false;
                entryMotivo.Text = pedido.motivocalificacion;
                btnConfirmar.IsEnabled = false;

                //variables imagenes
                string? star1 = null;
                string? star2 = null;
                string? star3 = null;
                string? star4 = null;
                string? star5 = null;

                string[] stars = new string[5];

                for (int i = 0; i < 5; i++)
                {
                    if (int.Parse(pedido.calificacion) > i)
                    {
                        stars[i] = "ImagenesCalificacion/estrella_llena1.png";
                    }
                    else
                    {
                        stars[i] = "ImagenesCalificacion/estrella_vacia.png";
                    }
                }

                star1 = stars[0];
                star2 = stars[1];
                star3 = stars[2];
                star4 = stars[3];
                star5 = stars[4];

                imgEstrella1.Source = star1;
                imgEstrella2.Source = star2;
                imgEstrella3.Source = star3;
                imgEstrella4.Source = star4;
                imgEstrella5.Source = star5;

            }
        }

        private void OnStar_Checked(object sender, CheckedChangedEventArgs e)
        {
            /*if (sender is CheckBox starCheckBox)
            {
                int indiceEstrella = int.Parse(starCheckBox.AutomationId);

                if (starCheckBox.IsChecked)
                {
                    // Si se selecciona una estrella, llenar hasta la estrella seleccionada
                    for (int i = 0; i <= indiceEstrella; i++)
                    {
                        estrellasLlenas[i] = true;
                    }
                }
                else
                {
                    // Si se deselecciona, vaciar todas las estrellas
                    for (int i = 0; i < estrellasLlenas.Length; i++)
                    {
                        estrellasLlenas[i] = false;
                    }
                }

                // Actualizar las imágenes de todas las estrellas
                for (int i = 0; i < estrellasLlenas.Length; i++)
                {
                    string imagenEstrella = estrellasLlenas[i] ? "PantallaCalificacion/estrella_llena3.png" : "PantallaCalificacion/estrella_vacia.png";
                    var estrellaImage = this.FindByName<Image>($"imgEstrella{i + 1}");
                    estrellaImage.Source = imagenEstrella;
                }
            }*/
        }

        private void OnStarTapped(object sender, EventArgs e)
        {
            if (sender is Image starImage)
            {
                int indiceEstrella = Convert.ToInt32(starImage.AutomationId);

                // Cambiar la imagen y actualizar el estado para la estrella tocada
                estrellasLlenas[indiceEstrella] = !estrellasLlenas[indiceEstrella];

                // Actualizar las imágenes de todas las estrellas
                for (int i = 0; i < estrellasLlenas.Length; i++)
                {
                    // Llenar estrellas hasta la estrella tocada y vaciar las siguientes
                    if (i <= indiceEstrella)
                    {
                        estrellasLlenas[i] = true;
                        calificacion = indiceEstrella + 1;

                    }
                    else if (i >= indiceEstrella)
                    {
                        estrellasLlenas[i] = false;
                        calificacion = indiceEstrella + 1;
                    }

                    string imagenEstrella = estrellasLlenas[i] ? "PantallaCalificacion/estrella_llena3.png" : "PantallaCalificacion/estrella_vacia.png";
                    var estrellaImage = this.FindByName<Image>($"imgEstrella{i + 1}");
                    estrellaImage.Source = imagenEstrella;
                }
            }
        }

        private async void btnConfirmar_Clicked(object sender, EventArgs e)
        {
            if (calificacion == 0)
            {
                await DisplayAlert("Alerta", $"Por favor asigne una calificación dando click en las estrellas", "OK");
                return;
            }
            else if (string.IsNullOrEmpty(entryMotivo.Text))
            {
                await DisplayAlert("Alerta", $"Por favor ingrese un motivo o mensaje para su calificación", "OK");
                return;
            }

            try
            {
                var data = new
                {
                    idpedido = _pedido.idpedido,
                    calificacion = calificacion,
                    motivo = entryMotivo.Text,
                };

                bool isSuccess = await _apiService.PostSuccessAsync("updateCalificacion.php", data);
                if (isSuccess)
                {
                    await DisplayAlert("Alerta", "La calificación ha sido agregada", "OK");
                    entryMotivo.Text = string.Empty;
                    await Navigation.PushAsync(new Views.Pedidos.pedidosPrincipal());
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Alerta", "Ocurrio un error, porfavor intente de nuevo", "OK");
                return;
            }
        }

        private void btnBack_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }


}
