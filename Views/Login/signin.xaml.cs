namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login;

public partial class singin : ContentPage
{
    public singin()
    {
        InitializeComponent();
    }

    private void btnRegistrarse_Clicked(object sender, EventArgs e)
    {

    }

    private async void LabelIngresar_Tapped(object sender, System.EventArgs e)
    {
        // Animaci�n de escala al hacer clic en el Label
        await labelIngresar.ScaleTo(0.8, 50, Easing.Linear);
        await labelIngresar.ScaleTo(1, 50, Easing.Linear);

        // Aqu� puedes navegar a la p�gina deseada
        await Navigation.PushAsync(new Views.Login.login());
    }
}