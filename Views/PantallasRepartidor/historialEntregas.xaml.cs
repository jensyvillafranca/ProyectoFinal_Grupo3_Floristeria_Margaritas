/*
 * Descripci�n:
 * Este c�digo define la l�gica de backend para la p�gina 'historialEntregas' de la aplicaci�n Floristeria Margaritas.
 * Permite al repartidor ver un historial de entregas, incluyendo detalles como la fecha de entrega, el estado del pedido,
 * la calificaci�n del cliente, etc. Adem�s, ofrece funcionalidades de b�squeda y filtrado por fecha.
 */

using Java.Time;
using Org.Apache.Http.Cookies;
using Plugin.Firebase.Firestore.Platforms.Android.Extensions;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos.productos;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallasRepartidor;

public partial class historialEntregas : ContentPage
{
    private ApiService _apiService = new ApiService();
    public ObservableCollection<historialEntregasViewModel> Historiales { get; set; }
    ObservableCollection<string> filtros;

    private string _searchQuery;
    public string SearchQuery
    {
        get { return _searchQuery; }
        set
        {
            if (_searchQuery != value)
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
                UpdateFilteredItems();
            }
        }
    }

    private int _selectedFilter = -1;
    public int SelectedFilter
    {
        get { return _selectedFilter; }
        set
        {
            if (_selectedFilter != value)
            {
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                UpdateFilteredItems();
            }
        }
    }

    // Constructor
    public historialEntregas()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;

        // Inicia objetos de prueba para collection view

        filtros = new ObservableCollection<string>
            {
                "Ninguno",
                "Hoy",
                "Esta Semana",
                "Este Mes",
                "Este A�o",
        };

        filtroPicker.ItemsSource = filtros;
    }

    // M�todo que se ejecuta cuando la p�gina se muestra
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            // Obtener historial de entregas
            var historial = await _apiService.PostDataAsync<historialEntregasModel[]>("historialEntregas.php", new { idrepartidor = Config.Config.activeRepartidorId });
            Historiales = new ObservableCollection<historialEntregasViewModel>();

            foreach (var item in historial)
            {
                string? image = null;
                string? estado = null;
                Color? color = null;

                string? comentario = null;

                //variables imagenes
                string? star1 = null;
                string? star2 = null;
                string? star3 = null;
                string? star4 = null;
                string? star5 = null;

                if (string.IsNullOrEmpty(item.motivocalificacion))
                {
                    comentario = "El cliente aun no deja una calificaci�n";
                    star1 = "ImagenesCalificacion/estrella_vacia.png";
                    star2 = "ImagenesCalificacion/estrella_vacia.png";
                    star3 = "ImagenesCalificacion/estrella_vacia.png";
                    star4 = "ImagenesCalificacion/estrella_vacia.png";
                    star5 = "ImagenesCalificacion/estrella_vacia.png";
                }
                else
                {
                    comentario = item.motivocalificacion;

                    string[] stars = new string[5];

                    for (int i = 0; i < 5; i++)
                    {
                        if (item.calificacion > i)
                        {
                            stars[i] = "ImagenesCalificacion/estrella_llena1.png";
                        }
                        else
                        {
                            stars[i] = "ImagenesCalificacion/estrella_vacia.png";
                        }
                    }

                    star1 = stars[0];
                    star2 = stars[1];
                    star3 = stars[2];
                    star4 = stars[3];
                    star5 = stars[4];
                }


                if (item.idestadopedido == 4)
                {
                    image = "Estados/delivery.jpg";
                    estado = "Entregado";
                    color = Color.FromRgb(0, 128, 0);
                }
                else if (item.idestadopedido == 5)
                {
                    image = "Estados/cancelado.jpg";
                    estado = "Cancelado";
                    color = Color.FromRgb(255, 0, 0);
                }

                string? fechaPedidoFormateada = (item.fechapedido).ToString("d/M/yyyy h:mm tt");

                var historialItem = new historialEntregasViewModel
                {
                    IdItem = $"Pedido: #PED-{item.idpedido}",
                    FechaPedido = item.fechapedido,
                    NombresCliente = $"Nombre Cliente: {item.nombrescliente}",
                    Ganancia = item.ganancia,
                    EstadoPedido = $"Estado: {estado}",
                    Calificacion = item.calificacion,
                    MotivoCalificacion = $"Comentario: {comentario}",
                    FechaPedidoFormateada = fechaPedidoFormateada,
                    FrameBackgroundColor = color,
                    TextColor = color,
                    ImageSource = image,
                    ImageStar1 = star1,
                    ImageStar2 = star2,
                    ImageStar3 = star3,
                    ImageStar4 = star4,
                    ImageStar5 = star5,
                    Visibilidad = true
                };

                Historiales.Add(historialItem);
            }

            collectionViewEntregas.ItemsSource = Historiales;
        }
        catch (Exception ex)
        {
            Historiales = new ObservableCollection<historialEntregasViewModel>();

            var historialItem = new historialEntregasViewModel
            {
                IdItem = "�Atenci�n!",
                ImageSource = "Estados/empty.png",
                Visibilidad = false,
                MotivoCalificacion = "No existe un historial de Pedidos",
                FrameBackgroundColor = Color.FromRgb(255, 0, 0)
        };

            Historiales.Add(historialItem);

            filtroPicker.IsEnabled = false;
            searchBarEntregas.IsEnabled = false;
            searchBarEntregas.IsReadOnly = true;

            collectionViewEntregas.ItemsSource = Historiales;
        }
        
    }

    // M�todo para manejar el evento Clicked del bot�n de retroceso
    private void btnAtras_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    // M�todo para manejar el evento Clicked del bot�n de notificaciones
    private async void btnNotification_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Notificaciones.notificacionesRepartidor());
    }

    // M�todo para manejar el evento de b�squeda
    private void searchBarEntregas_SearchButtonPressed(object sender, EventArgs e)
    {
        SearchQuery = searchBarEntregas.Text;
    }

    // M�todo para manejar el cambio de texto en la barra de b�squeda
    private void searchBarEntregas_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            collectionViewEntregas.ItemsSource = Historiales;
        }
        else
        {
            SearchQuery = e.NewTextValue;
        }
    }

    // M�todo para actualizar los elementos filtrados
    private void UpdateFilteredItems()
    {
        // Get the start and end dates based on the selected filter
        DateTime startDate;
        DateTime endDate = DateTime.Now;

        switch (filtroPicker.SelectedIndex)
        {
            case 1: // Today
                startDate = DateTime.Today;
                break;
            case 2: // This week
                startDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                break;
            case 3: // This month
                startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                break;
            case 4: // This year
                startDate = new DateTime(DateTime.Today.Year, 1, 1);
                break;
            default:
                // No filter selected
                startDate = DateTime.MinValue; // Set a default value
                break;
        }

        // Filter items based on the selected category and search query
        var filteredItems = Historiales.Where(item =>
            (SelectedFilter == null || SelectedFilter == -1 || // Check if no filter selected
            (item.FechaPedido >= startDate && item.FechaPedido <= endDate)) &&
            (string.IsNullOrEmpty(SearchQuery) ||
            item.IdItem.ToLower().Contains(SearchQuery.ToLower()) ||
            item.NombresCliente.ToLower().Contains(SearchQuery.ToLower()))
        );

        // Show all items when no filter is selected and no search query
        if (SelectedFilter == null && string.IsNullOrEmpty(SearchQuery))
        {
            collectionViewEntregas.ItemsSource = Historiales;
        }
        else
        {
            collectionViewEntregas.ItemsSource = new ObservableCollection<historialEntregasViewModel>(filteredItems);
        }
    }

    // M�todos para manejar eventos de botones
    private async void btnInicioRepartidor_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Home.homePageRepartidor());
    }

    private async void btnPedidosRepartidor_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.PantallasRepartidor.PantallaPedidosEntrantes());
    }

    private async void btnIngresosRepartidor_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.PantallasRepartidor.IngresosRepartidor());
    }

    private async void btnHistorialPedidosRepartidor_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.PantallasRepartidor.historialEntregas());
    }

    private async void btnPerfilRepartidor_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.PantallasRepartidor.profilePageRepartidor());
    }

    private async void btnLogOutRepartidor_Clicked(object sender, EventArgs e)
    {
        bool isSuccess = await logout.PerformLogoutAsync(_apiService);

        if (isSuccess)
        {
            await Navigation.PushAsync(new Views.Login.login());
        }
    }

    // M�todo para manejar el cambio de selecci�n en el filtro
    private void filtroPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectedFilter = filtroPicker.SelectedIndex;
        UpdateFilteredItems();
    }
}