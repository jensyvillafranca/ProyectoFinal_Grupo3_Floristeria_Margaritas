
/* Cambio no fusionado mediante combinaci�n del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-maccatalyst)'
Antes:
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
Despu�s:
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel;
using System.Windows.Input;
*/

/* Cambio no fusionado mediante combinaci�n del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-windows10.0.19041.0)'
Antes:
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
Despu�s:
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel;
using System.Windows.Input;
*/

/* Cambio no fusionado mediante combinaci�n del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-ios)'
Antes:
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
Despu�s:
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel;
using System.Windows.Input;
*/
namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login;

public partial class RestaurarContra : ContentPage
{


    public RestaurarContra()
    {
        InitializeComponent();

    }

    private void btnRestaurar_Clicked(object sender, EventArgs e)
    {

    }

    private async void BtnBack_Clicked(object sender, System.EventArgs e)
    {
        // Aqu� puedes navegar a la p�gina deseada
        await Navigation.PushAsync(new Views.Login.login());
    }

    private void BtnBack_Pressed(object sender, System.EventArgs e)
    {
        // Animaci�n de escala al presionar el bot�n
        btnBack.ScaleTo(0.8, 100, Easing.Linear);
    }

    private void BtnBack_Released(object sender, System.EventArgs e)
    {
        // Animaci�n de retorno a la escala original cuando se suelta el bot�n
        btnBack.ScaleTo(1, 100, Easing.Linear);
    }


}





