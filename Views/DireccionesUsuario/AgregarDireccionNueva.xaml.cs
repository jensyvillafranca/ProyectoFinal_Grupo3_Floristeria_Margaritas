/*
 * Descripción:
 * Este código define la lógica de backend para la página 'AgregarDireccionNueva' de la aplicación Floristeria Margaritas.
 * Permite al usuario agregar una nueva dirección, ya sea ingresándola manualmente o utilizando la ubicación actual del dispositivo.
 */

using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Config;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using System.Collections.ObjectModel;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.DireccionesUsuario;

public partial class AgregarDireccionNueva : ContentPage
{
    // Servicio para geocodificación de direcciones
    private readonly GeocodingService _geocodingService;
    private ApiService _apiService = new ApiService();

    // Variables de estado y colecciones para departamentos, ciudades y sucursales
    private int selectedCiudadId = 0;
    private int selectedDepartamentoId = 0;
    private string? selectedCiudadNombre = null;
    private int tipoNavegacion;

    public ObservableCollection<DepartamentoModel> Departamentos { get; set; }
    public ObservableCollection<CiudadModel> Ciudades { get; set; }
    public List<SucursalModel>? Sucursales { get; set; }

    // Propiedades para manejar la selección de departamento y ciudad en los pickers
    //Para el Picker de Departamentos
    private DepartamentoModel _selectedDepartamento;
    public DepartamentoModel SelectedDepartamento
    {
        get { return _selectedDepartamento; }
        set
        {
            if (_selectedDepartamento != value)
            {
                _selectedDepartamento = value;
                OnPropertyChanged(nameof(SelectedDepartamento));
                Console.WriteLine($"Selected Departamento: {SelectedDepartamento?.departamento}");
                UpdateDepartamentoItems();
            }
        }
    }

    //Para el picker de ciudad
    private CiudadModel _selectedCiudad;
    public CiudadModel SelectedCiudad
    {
        get { return _selectedCiudad; }
        set
        {
            if (_selectedCiudad != value)
            {
                _selectedCiudad = value;
                OnPropertyChanged(nameof(SelectedCiudad));
                Console.WriteLine($"Selected Ciudad: {SelectedCiudad?.ciudad}");
                UpdateCiudadItems();
            }
        }
    }

    // Constructor
    public AgregarDireccionNueva(int tipo)
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        _geocodingService = new GeocodingService(Config.Config.GoogleApiKey);
        Departamentos = new ObservableCollection<DepartamentoModel>();
        Ciudades = new ObservableCollection<CiudadModel>();
        tipoNavegacion = tipo;

        AsyncTaskExec();     
    }

    // Método para ejecutar tareas asíncronas durante la inicialización de la página
    private async void AsyncTaskExec()
    {
        var sucursales = await _apiService.GetDataAsync<SucursalModel[]>("obtenerSucursales.php");

        Sucursales = new List<SucursalModel>();

        foreach (var sucursal in sucursales)
        {
            SucursalModel sucursalModel = new SucursalModel
            {
                idsucursal = sucursal.idsucursal,
                ciudad = sucursal.ciudad
            };

            Sucursales.Add(sucursalModel);
        }

        if (tipoNavegacion != 0)
        {
            await LoadDepartamentosDataAsync();
        }
        else
        {
            entryDireccion.IsEnabled = false;
            ciudadPicker.IsEnabled = false;
            departamentoPicker.IsEnabled = false;
            btnBorrar.IsEnabled = false;
            btnAgregar.IsEnabled = false;
            await getLocationService();
        }
        
    }

    // Método para obtener la ubicación actual del dispositivo
    private async Task getLocationService()
    {
        // Verificar el permiso de ubicación
        var locationPermissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (locationPermissionStatus == PermissionStatus.Granted)
        {
            // Obtiene la ubicacion
            var location = await Geolocation.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Default,
                Timeout = TimeSpan.FromSeconds(10)
            });

            if (location != null)
            {
                double Lat = location.Latitude;
                double Lng = location.Longitude;

                // Obtener detalles de la ubicación a partir de las coordenadas
                var result = await _geocodingService.GetCoordinateDetailsAsync(Lat, Lng);

                // Verificar si hay servicio de entrega en la ciudad actual
                if (Sucursales?.Any(sucursal => sucursal.ciudad == result.Ciudad) != true)
                {
                    await DisplayAlert("Alerta", $"Por el momento no contamos con servicio de entrega en {result.Ciudad} !", "OK");
                    await Navigation.PopAsync();
                }

                // Mostrar los detalles de la ubicación en la interfaz de usuario
                entryDireccion.Text = result.Direccion;
                ciudadPicker.Items.Add(result.Ciudad);
                ciudadPicker.SelectedIndex = 0;
                departamentoPicker.Items.Add(result.Departamento);
                departamentoPicker.SelectedIndex = 0;
                labelLatitud.Text = Lat.ToString();
                labelLongitud.Text = Lng.ToString();

                // Mostrar el mapa de la ubicación
                var mapImageUrl = _geocodingService.GetStaticMapImageUrl(Lat, Lng);
                imgLocation.Source = mapImageUrl;

                selectedCiudadNombre = result.Ciudad;
                btnAgregar.IsEnabled = true;
            }
            else
            {
                // Si la ubicación es nula, mostrar un mensaje de alerta
                await DisplayAlert("Alerta", "El GPS se encuentra desactivado. Porfavor active su GPS y abra la aplicación de nuevo!", "Ok");
            }
        }
        else
        {
            // Si no se otorga el permiso de ubicación, mostrar un mensaje de error y salir de la aplicación
            await DisplayAlert("Error", "Permiso de Ubicación no otorgado. El Permiso es necesario para utilizar la aplicacion.", "OK");
            Application.Current.Quit();
        }



        
    }

    // Métodos para cargar datos de departamentos y ciudades desde el servidor
    private async Task LoadDepartamentosDataAsync()
    {
        var departamentos = await _apiService.GetDataAsync<DepartamentoModel[]>("obtenerDepartamentos.php");

        Console.WriteLine($"Received {departamentos.Length} departamentos from the server.");

        Departamentos.Clear();

        foreach (var departamento in departamentos)
        {
            Departamentos.Add(departamento);
        }

        // Temporarily detach the event handler to prevent issues with manipulating Items while ItemsSource is set
        departamentoPicker.SelectedIndexChanged -= departamentoPicker_SelectedIndexChanged;

        // Clear existing items and add new items to the Items collection
        departamentoPicker.Items.Clear();
        foreach (var departamento in Departamentos)
        {
            departamentoPicker.Items.Add(departamento.departamento);
        }

        // Reattach the event handler
        departamentoPicker.SelectedIndexChanged += departamentoPicker_SelectedIndexChanged;

        departamentoPicker.SelectedIndex = -1;
    }

    //Ciudades
    private async Task LoadCiudadesDataAsync(int selectedDepartmentId)
    {
        var ciudades = await _apiService.PostDataAsync<CiudadModel[]>("obtenerCiudades.php", new { iddepartamento = selectedDepartmentId });


        Console.WriteLine($"Received {ciudades.Length} products from the server.");

        Ciudades.Clear();

        foreach (var ciudad in ciudades)
        {
            Ciudades.Add(ciudad);
        }

        ciudadPicker.SelectedIndexChanged -= ciudadPicker_SelectedIndexChanged;

        ciudadPicker.Items.Clear();

        foreach (var ciudad in Ciudades)
        {
            ciudadPicker.Items.Add(ciudad.ciudad);
        }

        ciudadPicker.SelectedIndexChanged += ciudadPicker_SelectedIndexChanged;

        ciudadPicker.SelectedIndex = -1;

    }

    // Métodos para actualizar los elementos del picker de departamento y ciudad
    private async void UpdateDepartamentoItems()
    {
        if (SelectedDepartamento != null)
        {
            selectedDepartamentoId = SelectedDepartamento.iddepartamento;
            

            await LoadCiudadesDataAsync(selectedDepartamentoId);
        }
    }

    private void UpdateCiudadItems()
    {
        if (SelectedCiudad != null)
        {
            selectedCiudadId = SelectedCiudad.idciudad;
        }
    }

    // Manejadores de eventos para navegación y acciones del usuario
    private void btnBack_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
    }

    private async void ciudadPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnAgregar.IsEnabled = true;
        entryDireccion.IsEnabled = true;

        int selectedIndex = ciudadPicker.SelectedIndex;

        if (selectedIndex >= 0 && selectedIndex < Ciudades.Count)
        {
            SelectedCiudad = Ciudades[selectedIndex];
            Console.WriteLine($"Selected Ciudad: {SelectedCiudad?.ciudad}");

            if (Sucursales?.Any(sucursal => sucursal.ciudad == SelectedCiudad.ciudad) != true)
            {
                await DisplayAlert("Alerta", "Por el momento no contamos con servicio de entrega en la Municipalidad seleccionada!", "OK");
                btnAgregar.IsEnabled = false;
                entryDireccion.IsEnabled = false;
                return;
            }
        }
        else
        {
            Console.WriteLine($"Invalid SelectedIndex: {selectedIndex}, Departamentos Count: {Departamentos.Count}");
        }
    }

    private async void btnAgregar_Clicked(object sender, EventArgs e)
    {
        // Validar la entrada de datos del usuario
        if (string.IsNullOrEmpty(entryDescripcion.Text))
        {
            await DisplayAlert("Alerta", "Porfavor ingrese una descripción", "OK");
            return;
        } 
        else if (departamentoPicker.SelectedIndex == 0)
        {
            await DisplayAlert("Alerta", "Porfavor seleccione un departamento de la lista", "OK");
            return;
        } 
        else if (ciudadPicker.SelectedIndex == 0)
        {
            await DisplayAlert("Alerta", "Porfavor seleccione una ciudad de la lista", "OK");
            return;
        } 
        else if (string.IsNullOrEmpty(entryDireccion.Text))
        {
            await DisplayAlert("Alerta", "Porfavor ingrese su dirección", "OK");
            return;
        } 
        else if (string.IsNullOrEmpty(entryreferencia.Text))
        {
            await DisplayAlert("Alerta", "Porfavor ingrese un punto referencia a la dirección", "OK");
            return;
        }
        else if(labelLatitud.Text == "00.00" || labelLongitud.Text == "00.00")
        {
            await DisplayAlert("Alerta", "Porfavor asegúrese de ingresar la dirección correctamente", "OK");
            return;
        }
        else if(selectedCiudadNombre != SelectedCiudad.ciudad)
        {
            await DisplayAlert("Alerta", "El municipio seleccionado no concuerda con la dirección ingresada", "OK");
            return;
        }

        // Enviar datos al servidor para agregar la dirección
        var data = new
        {
            fk_idcliente = Config.Config.activeUserId,
            fk_idciudad = selectedCiudadId,
            direccion = entryDireccion.Text,
            descripcion = entryDescripcion.Text,
            longitud = labelLongitud.Text,
            latitude = labelLatitud.Text,
            referencia = entryreferencia.Text
        };

        bool isSuccess = await _apiService.PostSuccessAsync("crearDireccion.php", data);

        if (isSuccess)
        {
            // Limpiar los campos y mostrar un mensaje de éxito
            labelDescripcion.Text = string.Empty;
            entryDireccion.Text = string.Empty;
            labelReferencia.Text = string.Empty;
            labelLatitud.Text = "00.00";
            labelLongitud.Text = "00.00";
            departamentoPicker.SelectedIndex = -1;
            ciudadPicker.SelectedIndex = -1;
            imgLocation.Source = null;

            await DisplayAlert("Alerta", "La dirección ha sido agregada!", "OK");
            if(tipoNavegacion == 1)
            {
                await Navigation.PushAsync(new Views.DireccionesUsuario.DireccionesGuardas());
            }
            else
            {
                MessagingCenter.Send<object>(this, "UpdateCollectionView");
                await Navigation.PopAsync();
            }
        }
        else
        {
            // Mostrar un mensaje de error si falla la solicitud
            await DisplayAlert("Error", "Error al agregar la dirección!", "OK");
        }

    }

    // Método para obtener la ubicación y mostrar detalles al completar la entrada de la dirección
    private async void entryDireccion_Completed(object sender, EventArgs e)
    {
        string address = entryDireccion.Text;

        if (string.IsNullOrEmpty(entryDireccion.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese una dirección", "OK");
            return;
        }
        else
        {
            var result = await _geocodingService.GetCoordinatesAndCityAsync(address);

            double latitude = result.Latitude;
            double longitude = result.Longitude;
            string? ciudad = result.City;

            labelLatitud.Text = latitude.ToString();
            labelLongitud.Text = longitude.ToString();

            var mapImageUrl = _geocodingService.GetStaticMapImageUrl(latitude, longitude);
            imgLocation.Source = mapImageUrl;

            selectedCiudadNombre = ciudad;           
        }
        
    }

    // Método para limpiar el campo de dirección y la imagen del mapa
    private void btnBorrar_Clicked(object sender, EventArgs e)
    {
        entryDireccion.Text = string.Empty;
        imgLocation.Source = null;
    }

    // Manejador de evento para la selección de departamento en el picker
    private void departamentoPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        int selectedIndex = departamentoPicker.SelectedIndex;

        if (selectedIndex >= 0 && selectedIndex < Departamentos.Count)
        {
            SelectedDepartamento = Departamentos[selectedIndex];
            Console.WriteLine($"Selected Departamento: {SelectedDepartamento?.departamento}");
        }
        else
        {
            Console.WriteLine($"Invalid SelectedIndex: {selectedIndex}, Departamentos Count: {Departamentos.Count}");
        }
    }
}