using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using System.Collections.ObjectModel;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallaAdministrador;

public partial class verImagenesSolicitud : ContentPage
{
    private ApiService _apiService = new ApiService();
    private int idsolicitud;
    public ObservableCollection<ImagenesModel> Imagenes { get; set; }
    public verImagenesSolicitud(SolicitudModel solicitud)
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
        idsolicitud = solicitud.idsolicitud;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var imagenes = await _apiService.PostDataAsync<ImagenesModel[]>("obtenerImagenes.php", new { idsolicitud =  idsolicitud});
            Imagenes = new ObservableCollection<ImagenesModel>();

            foreach (var imagen in imagenes)
            {

                ImagenesModel imagenesModel = new ImagenesModel
                {
                    idimagen = imagen.idimagen,
                    imagen = imagen.imagen
                };

                Imagenes.Add(imagenesModel);
            }

            collectionViewImagenes.ItemsSource = Imagenes;
        }
        catch (Exception ex)
        {
            // Manejar cualquier excepción que ocurra durante la verificación de notificaciones
        }
    }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}