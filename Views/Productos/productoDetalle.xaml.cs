namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos;

public partial class productoDetalle : ContentPage
{
    //Variables de prueba
    double precioTotal = 0;
    double precioProducto = 1700;

	public productoDetalle()
	{
		InitializeComponent();
	}

    private void btnBack_Clicked(object sender, EventArgs e)
    {

    }

    private void btnAgregar_Clicked(object sender, EventArgs e)
    {

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