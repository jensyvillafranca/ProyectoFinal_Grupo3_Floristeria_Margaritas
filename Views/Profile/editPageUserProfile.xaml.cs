namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Profile;

public partial class editPageUserProfile : ContentPage
{
	public editPageUserProfile()
	{
		InitializeComponent();
	}

    private void btnImagenAtras_Clicked(object sender, EventArgs e)
    {

    }
    private void btnActualizarPefil_Clicked(object sender, EventArgs e)
    {
        
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        // Mostrar u ocultar el men� emergente seg�n su estado actual
        PopupMenu.IsVisible = !PopupMenu.IsVisible;
    }

 

    private void SubirGaleria_Clicked(object sender, EventArgs e)
    {
        // L�gica para subir una imagen desde la galer�a
    }

    private void Fotos_Clicked(object sender, EventArgs e)
    {
        // L�gica para subir una imagen desde la galer�a
    }



}