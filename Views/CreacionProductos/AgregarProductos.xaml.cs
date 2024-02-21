namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.CreacionProductos;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
public partial class AgregarProductos : ContentPage
{

    //Clase que permite guardar objetos que estan vinculados a un elemento de interface
    ObservableCollection<string> categoria;

    string categorias;
    public AgregarProductos()
	{
		InitializeComponent();

        categoria = new ObservableCollection<string>
            {
                "Argentina",
                "Belize",
                "Costa Rica",
                "El Salvador",
                "Guatemala",
                "Honduras",
                "Nicaragua",
                "Panama",
                "Mexico",
                "Colombia"
        };

        cotegoriaPicker.ItemsSource = categoria;
    }

    private void cotegoriaPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        categorias = cotegoriaPicker.SelectedItem as string;
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


    private void btnImagenAtras_Clicked(object sender, EventArgs e)
    {

    }

    private void btnAgregados_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new HistorialProductosAgregados());

    }

    private void btnAgregarProducto_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new HistorialProductosAgregados());

    }


    private void btnHome_Clicked(object sender, EventArgs e)
    {

    }

    private void btnStock_Clicked(object sender, EventArgs e)
    {
       
    }

    private void btnEstadisticas_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new HistorialProductosAgregados());
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