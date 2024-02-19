using ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.CustomViews;
namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos;

public partial class productoDetalle : ContentPage
{

    private CustomPopupViewAgregar customPopup;
    //Variables de prueba
    double precioTotal = 0;
    double precioProducto = 1700;

    public productoDetalle()
    {
        InitializeComponent();
        customPopup = new CustomPopupViewAgregar();
    }

    private void btnBack_Clicked(object sender, EventArgs e)
    {

    }

    private async void btnAgregar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new NavigationPage(customPopup));
    }

    private void btnSubstract_Clicked(object sender, EventArgs e)
    {
        if (int.TryParse(quantityEntry.Text, out int quantity) && quantity > 1)
        {
            // Decrement the quantity, but ensure it doesn't go below 1
            quantity--;
            quantityEntry.Text = quantity.ToString();
            precioTotal = double.Parse(quantityEntry.Text) * precioProducto;
            labelPrecio.Text = $"L{precioTotal:N2}";
        }
    }

    private void btnAdd_Clicked(object sender, EventArgs e)
    {
        if (int.TryParse(quantityEntry.Text, out int quantity))
        {
            // Increment the quantity
            quantity++;
            quantityEntry.Text = quantity.ToString();
            precioTotal = double.Parse(quantityEntry.Text) * precioProducto;
            labelPrecio.Text = $"L {precioTotal:N2}";
        }
    }
}