namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.DireccionesUsuario;

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Config;
using System.Collections.ObjectModel;
using System.Windows.Input;
*/

/* Cambio no fusionado mediante combinación del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-maccatalyst)'
Antes:
using System.Collections.ObjectModel;
using System.Windows.Input;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
Después:
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using System.Collections.ObjectModel;
using System.Windows.Input;
*/
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using System.Collections.ObjectModel;

public partial class DireccionesGuardas : ContentPage
{
    public ObservableCollection<DireccionesViewModel>? Direcciones { get; set; }
    private ApiService _apiService;

    public DireccionesGuardas()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;
        _apiService = new ApiService();

        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        var direcciones = await _apiService.PostDataAsync<DireccionModel[]>("obtenerDireccionesPorID.php", new { idcliente = Config.activeUserId });

        Direcciones = new ObservableCollection<DireccionesViewModel>();

        foreach (var direccion in direcciones)
        {

            DireccionesViewModel direccionesViewModel = new DireccionesViewModel
            {
                IdDireccion = direccion.iddireccion,
                Direccion = direccion.direccion,
                Ciudad = direccion.ciudad,
                Departamento = direccion.departamento,
                IdCliente = direccion.fk_idcliente,
                Descripcion = direccion.descripcion,
                Longitud = direccion.longitud,
                Latitud = direccion.latitude,
                Referencia = direccion.referencia,
                TappedCommand = new Command(() => HandleTappedCommand(direccion.iddireccion, direccion.fk_idcliente)),
            };

            Direcciones.Add(direccionesViewModel);
        }

        collectionViewDirecciones.ItemsSource = Direcciones;
    }

    private async void HandleTappedCommand(int idDireccion, int idCliente)
    {
        bool userConfirmed = await DisplayAlert("Confirmación", "¿Está seguro de que desea eliminar esta dirección?", "Si", "No");

        if (userConfirmed)
        {
            var data = new
            {
                iddireccion = idDireccion,
                idcliente = idCliente
            };

            bool isSuccess = await _apiService.PostSuccessAsync("eliminarDireccion.php", data);

            if (isSuccess)
            {
                // Find the index of the item to be removed
                int indexToRemove = -1;

                for (int i = 0; i < Direcciones.Count; i++)
                {
                    if (Direcciones[i].IdDireccion == idDireccion)
                    {
                        indexToRemove = i;
                        break;
                    }
                }

                if (indexToRemove != -1)
                {
                    // Remove the item from the ObservableCollection
                    Direcciones.RemoveAt(indexToRemove);

                    // Update the ItemsSource of the CollectionView
                    collectionViewDirecciones.ItemsSource = null;
                    collectionViewDirecciones.ItemsSource = Direcciones;

                    await DisplayAlert("Alerta", "La dirección ha sido eliminada!", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "No se encontró la dirección en la colección.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Error al eliminar la dirección!", "OK");
            }
        }
    }


    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void TapGestureNuevaDireccion_Tapped(object sender, TappedEventArgs e)
    {
        await AnimationUtilities.ChangeFrameColor(frameAgregarDireccion, Color.FromRgb(255, 250, 240), Color.FromRgb(65, 185, 254), 250);
        await Navigation.PushAsync(new Views.DireccionesUsuario.AgregarDireccionNueva(1));
    }

    private void btnHome_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Home.homePageUser());
    }

    private void btnProductos_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Views.Productos.productos());
    }

    private void btnPedidos_Clicked(object sender, EventArgs e)
    {

    }

    private void btnPerfil_Clicked(object sender, EventArgs e)
    {

    }

    private void btnLogout_Clicked(object sender, EventArgs e)
    {

    }
}