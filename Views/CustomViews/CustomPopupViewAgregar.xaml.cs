/*
 * Descripci�n:
 * Este c�digo define la l�gica de backend para la vista emergente 'CustomPopupViewAgregar' de la aplicaci�n Floristeria Margaritas.
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

    // M�todo para manejar el evento de clic en el bot�n 'Agregar M�s'
    private void btnAgregarMas_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Productos.productos());
    }

    // M�todo para manejar el evento de clic en el bot�n 'Carrito'
    private void btnCarrito_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Productos.carritoCompras());
    }
}