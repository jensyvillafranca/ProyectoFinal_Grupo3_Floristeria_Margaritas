/*
 * Descripción:
 * Este código define la lógica de backend para la vista emergente 'CustomPopupViewAgregar' de la aplicación Floristeria Margaritas.
 * Permite al usuario agregar productos adicionales a su carrito de compras o ver el contenido actual del carrito.
 */

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.CustomViews;

public partial class CustomPopupViewAgregar : ContentPage
{
    //Constructor
    public CustomPopupViewAgregar()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    // Método para manejar el evento de clic en el botón 'Agregar Más'
    private void btnAgregarMas_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Productos.productos());
    }

    // Método para manejar el evento de clic en el botón 'Carrito'
    private void btnCarrito_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Productos.carritoCompras());
    }
}