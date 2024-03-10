namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.CustomViews;

public partial class CustomPopupPagoRealizado : ContentPage
{
    public CustomPopupPagoRealizado()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is DateTime deliveryTime)
        {
            labelHoraEstimada.Text = deliveryTime.ToString("h:mm tt");
        }
    }
}