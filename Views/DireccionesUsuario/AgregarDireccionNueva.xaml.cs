using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Config;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using System.Collections.ObjectModel;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.DireccionesUsuario;

public partial class AgregarDireccionNueva : ContentPage
{
    private readonly GeocodingService _geocodingService;
    private ApiService _apiService = new ApiService();

    private int selectedCiudadId = 0;
    private int selectedDepartamentoId = 0;
    private string? selectedCiudadNombre = null;
    private int tipoNavegacion;

    public ObservableCollection<DepartamentoModel> Departamentos { get; set; }
    public ObservableCollection<CiudadModel> Ciudades { get; set; }
    public List<SucursalModel>? Sucursales { get; set; }

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

    private async Task getLocationService()
    {

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

                var result = await _geocodingService.GetCoordinateDetailsAsync(Lat, Lng);

                if (Sucursales?.Any(sucursal => sucursal.ciudad == result.Ciudad) != true)
                {
                    await DisplayAlert("Alerta", $"Por el momento no contamos con servicio de entrega en {result.Ciudad} !", "OK");
                    await Navigation.PopAsync();
                }

                entryDireccion.Text = result.Direccion;
                ciudadPicker.Items.Add(result.Ciudad);
                ciudadPicker.SelectedIndex = 0;
                departamentoPicker.Items.Add(result.Departamento);
                departamentoPicker.SelectedIndex = 0;
                labelLatitud.Text = Lat.ToString();
                labelLongitud.Text = Lng.ToString();

                var mapImageUrl = _geocodingService.GetStaticMapImageUrl(Lat, Lng);
                imgLocation.Source = mapImageUrl;

                selectedCiudadNombre = result.Ciudad;
                btnAgregar.IsEnabled = true;
            }
            else
            {
                // Cuando la ubicacion es nula
                await DisplayAlert("Alerta", "El GPS se encuentra desactivado. Porfavor active su GPS y abra la aplicación de nuevo!", "Ok");
            }
        }
        else
        {
            // Cuando el permiso no es otorgado
            await DisplayAlert("Error", "Permiso de Ubicación no otorgado. El Permiso es necesario para utilizar la aplicacion.", "OK");
            Application.Current.Quit();
        }



        
    }

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
            // Request was successful
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
            // Request failed
            await DisplayAlert("Error", "Error al agregar la dirección!", "OK");
        }

    }

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

    private void btnBorrar_Clicked(object sender, EventArgs e)
    {
        entryDireccion.Text = string.Empty;
        imgLocation.Source = null;
    }

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