using System.Collections.ObjectModel;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.CreacionProductos;

public partial class ActualizarProductos : ContentPage
{
    ObservableCollection<string> categoria;

    string categorias;
    public ActualizarProductos()
    {
        InitializeComponent();

        categoria = new ObservableCollection<string>
            {
                "Golden Sunrise",
                "Fancy Fantasy",
                "Canasta de amor",
                "Flores de boda",
                "Flores con chocolates",
                "Flores de cumpleaños",

        };

        cotegoriaPicker.ItemsSource = categoria;
    }




    private void cotegoriaPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        categorias = cotegoriaPicker.SelectedItem as string;
    }


    private void btnImagenAtras_Clicked(object sender, EventArgs e)
    {

    }

    private void YesButton_Clicked(object sender, EventArgs e)
    {
        YesButton.IsVisible = false;
        NoButton.IsVisible = true;
    }

    private void NoButton_Clicked(object sender, EventArgs e)
    {
        YesButton.IsVisible = true;
        NoButton.IsVisible = false;
    }



    private void btnHome_Clicked(object sender, EventArgs e)
    {

    }

    private void btnStock_Clicked(object sender, EventArgs e)
    {

    }

    private void btnEstadisticas_Clicked(object sender, EventArgs e)
    {

    }

    private void btnAnuncios_Clicked(object sender, EventArgs e)
    {

    }

    private void btnPerfil_Clicked(object sender, EventArgs e)
    {

    }

    private void btnVerVencidos_Clicked(object sender, EventArgs e)
    {

    }

    private void btnLogout_Clicked(object sender, EventArgs e)
    {

    }


}