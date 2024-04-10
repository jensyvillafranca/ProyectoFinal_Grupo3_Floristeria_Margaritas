using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.RestApi;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Utilities;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.PantallasRepartidor;

public partial class AplicarRepartidor : ContentPage
{
    private ApiService _apiService = new ApiService();
    private int selectedCiudadId = 0;
    private int selectedDepartamentoId = 0;
    private bool formatoCorreo = false;
    private bool validAge = false;
    private string? birthdate = null;
    private List<string> base64Images = new List<string>();
    private string? DNIFront = null;
    private string? DNIBack = null;
    private string? LICFront = null;
    private string? LICBack = null;

    public ObservableCollection<DepartamentoModel> Departamentos { get; set; }
    public ObservableCollection<CiudadModel> Ciudades { get; set; }
    public List<SucursalModel>? Sucursales { get; set; }

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

    public AplicarRepartidor(){
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        Departamentos = new ObservableCollection<DepartamentoModel>();
        Ciudades = new ObservableCollection<CiudadModel>();
    }

    protected override async void OnAppearing() {
        base.OnAppearing();

        try
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

            await LoadDepartamentosDataAsync();

        }
        catch(Exception ex)
        {

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
        pk_deparments.SelectedIndexChanged -= pk_deparments_SelectedIndexChanged;

        // Clear existing items and add new items to the Items collection
        pk_deparments.Items.Clear();
        foreach (var departamento in Departamentos)
        {
            pk_deparments.Items.Add(departamento.departamento);
        }

        // Reattach the event handler
        pk_deparments.SelectedIndexChanged += pk_deparments_SelectedIndexChanged;

        pk_deparments.SelectedIndex = -1;
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

        pk_city.SelectedIndexChanged -= pk_city_SelectedIndexChanged;

        pk_city.Items.Clear();

        foreach (var ciudad in Ciudades)
        {
            pk_city.Items.Add(ciudad.ciudad);
        }

        pk_city.SelectedIndexChanged += pk_city_SelectedIndexChanged;

        pk_city.SelectedIndex = -1;

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

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void pk_deparments_SelectedIndexChanged(object sender, EventArgs e)
    {
        int selectedIndex = pk_deparments.SelectedIndex;

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

    private async void pk_city_SelectedIndexChanged(object sender, EventArgs e)
    {
        int selectedIndex = pk_city.SelectedIndex;

        if (selectedIndex >= 0 && selectedIndex < Ciudades.Count)
        {
            SelectedCiudad = Ciudades[selectedIndex];
            Console.WriteLine($"Selected Ciudad: {SelectedCiudad?.ciudad}");

            if (Sucursales?.Any(sucursal => sucursal.ciudad == SelectedCiudad.ciudad) != true)
            {
                pk_city.SelectedIndex = -1;
                await DisplayAlert("Alerta", "Lo sentimos, Por el momento no contamos con una sucursal en la Municipalidad seleccionada!", "OK");
                return;
            }
        }
        else
        {
            Console.WriteLine($"Invalid SelectedIndex: {selectedIndex}, Departamentos Count: {Departamentos.Count}");
        }
    }

    private void txt_email_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Valida el correo Electonico
        bool isValid = (Regex.IsMatch(e.NewTextValue, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"));

        // Cambia el color del texto
        ((Entry)sender).TextColor = isValid ? Color.FromRgb(0, 0, 0) : Color.FromRgb(244, 67, 54);

        formatoCorreo = isValid;
    }

    private async void selectImage(ImageButton btn, int tipo)
    {
        string? imagenFilePath = null;
        try
        {

            // Abre la galería de medios para que el usuario seleccione una foto.
            var galeria = await MediaPicker.PickPhotoAsync();

            // Verifica si se ha seleccionado una foto desde la galería.
            if (galeria != null)
            {
                // Abre un flujo de memoria para la foto seleccionada.
                var memoriaStream = await galeria.OpenReadAsync();

                // Establece la imagen seleccionada como origen de la imagen en la interfaz de usuario.
                btn.Source = ImageSource.FromFile(galeria.FullPath);
                imagenFilePath = galeria.FullPath;
                base64Images.Add(photoHelper.ImageToBase64(imagenFilePath));
                if(tipo == 1)
                {
                    DNIFront = photoHelper.ImageToBase64(imagenFilePath);
                }
                else if (tipo == 2)
                {
                    DNIBack = photoHelper.ImageToBase64(imagenFilePath);
                }
                else if (tipo == 3)
                {
                    LICFront = photoHelper.ImageToBase64(imagenFilePath);
                }
                else if (tipo == 4)
                {
                    LICBack = photoHelper.ImageToBase64(imagenFilePath);
                }
            }
            else
            {
                Console.WriteLine("Photo selection cancelled or failed."); // Mensaje en caso de que se cancele o falle la selección de la foto.
            }
        }
        catch (FeatureNotSupportedException)
        {
            Console.WriteLine("Photo picking is not supported on this device."); // Mensaje en caso de que la selección de fotos no esté soportada en el dispositivo.
        }
        catch (PermissionException)
        {
            Console.WriteLine("Gallery permission denied."); // Mensaje en caso de que se deniegue el permiso para acceder a la galería.
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error picking photo: {ex.Message}"); // Mensaje en caso de que ocurra un error al seleccionar la foto.
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if(DNIFront != null)
        {
            base64Images.Remove(DNIFront);
        }

        selectImage(btnDNIFrente, 1);
    }

    private void btnDNIFrente_Clicked(object sender, EventArgs e)
    {
        if (DNIFront != null)
        {
            base64Images.Remove(DNIFront);
        }

        selectImage(btnDNIFrente, 1);
    }

    private void btnDNIAtras_Clicked(object sender, EventArgs e)
    {
        if (DNIBack != null)
        {
            base64Images.Remove(DNIBack);
        }
        selectImage(btnDNIAtras, 2);
    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        if (DNIBack != null)
        {
            base64Images.Remove(DNIBack);
        }
        selectImage(btnDNIAtras, 2);
    }

    private void btnLICFrente_Clicked(object sender, EventArgs e)
    {
        if (LICFront != null)
        {
            base64Images.Remove(LICFront);
        }
        selectImage(btnLICFrente, 3);
    }

    private void btnLICAtras_Clicked(object sender, EventArgs e)
    {
        if (LICBack != null)
        {
            base64Images.Remove(LICBack);
        }
        selectImage(btnLICAtras, 4);
    }

    private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        if (LICFront != null)
        {
            base64Images.Remove(LICFront);
        }
        selectImage(btnLICFrente, 3);
    }

    private void TapGestureRecognizer_Tapped_3(object sender, TappedEventArgs e)
    {
        if (LICBack != null)
        {
            base64Images.Remove(LICBack);
        }
        selectImage(btnLICAtras, 4);
    }

    // Método para validar campos de registro
    private bool ValidateRegistrationFields()
    {
        if (string.IsNullOrEmpty(txt_name.Text))
        {
            DisplayAlert("Alerta", "Por favor ingrese su nombre", "OK");
            return false;
        }
        else if (string.IsNullOrEmpty(txt_surname.Text))
        {
            DisplayAlert("Alerta", "Por favor ingrese su apellido", "OK");
            return false;
        }
        else if (string.IsNullOrEmpty(txt_email.Text))
        {
            DisplayAlert("Alerta", "Por favor ingrese su correo electrónico", "OK");
            return false;
        }
        else if (formatoCorreo == false)
        {
            DisplayAlert("Alerta", "Por favor ingrese un correo electrónico valido", "OK");
            return false;
        }
        else if (string.IsNullOrEmpty(txt_phone.Text))
        {
            DisplayAlert("Alerta", "Por favor ingrese su número de teléfono", "OK");
            return false;
        }
        else if (string.IsNullOrEmpty(txt_dni.Text))
        {
            DisplayAlert("Alerta", "Por favor ingrese su DNI", "OK");
            return false;
        }
        else if (pk_city.SelectedIndex == -1)
        {
            DisplayAlert("Alerta", "Por favor seleccione una ciudad", "OK");
            return false;
        }
        else if (pk_deparments.SelectedIndex == -1)
        {
            DisplayAlert("Alerta", "Por favor seleccione un departamento", "OK");
            return false;
        }
        else if (pk_gender.SelectedIndex == -1)
        {
            DisplayAlert("Alerta", "Por favor seleccione un género", "OK");
            return false;
        }
        else if (checkMoto.IsChecked == false && checkLiviana.IsChecked == false && checkPesada.IsChecked == false)
        {
            DisplayAlert("Alerta", "Por favor seleccione el tipo de licencia que posee", "OK");
            return false;
        }
        else if (string.IsNullOrEmpty(DNIFront))
        {
            DisplayAlert("Alerta", "Por favor seleccione una imagen frontal del dni", "OK");
            return false;
        }
        else if (string.IsNullOrEmpty(DNIBack))
        {
            DisplayAlert("Alerta", "Por favor seleccione una imagen de atras del dni", "OK");
            return false;
        }
        else if (string.IsNullOrEmpty(LICFront))
        {
            DisplayAlert("Alerta", "Por favor seleccione una imagen frontal de la licencia", "OK");
            return false;
        }
        else if (string.IsNullOrEmpty(LICBack))
        {
            DisplayAlert("Alerta", "Por favor seleccione una imagen de atras de la licencia", "OK");
            return false;
        }

        return true;
    }

    private async void dp_birthdate_DateSelected(object sender, DateChangedEventArgs e)
    {
        DateTime selectedDate = e.NewDate;

        // Check if a date was selected
        if (selectedDate != DateTime.MinValue)
        {
            // Calculate age
            int age = DateTime.Today.Year - selectedDate.Year;
            if (selectedDate > DateTime.Today.AddYears(-age))
                age--;

            // Check if age is at least 18 years
            if (age >= 18)
            {
                validAge = true;
                birthdate = (e.NewDate).ToString();
            }
            else
            {
                // Date is not valid (age is less than 18 years)
                // Display an error message or handle the validation accordingly
                DisplayAlert("Error", "Necesita tener al menos 18 años.", "OK");

                // Reset the DatePicker value to clear the invalid selection
                ((DatePicker)sender).Date = DateTime.Today.AddYears(-18);
            }
        }
    }

    private async void btnEnviar_Clicked(object sender, EventArgs e)
    {
        if (ValidateRegistrationFields())
        {
            try
            {
                int idgenero = -1;
                if(pk_gender.SelectedIndex == 0){ idgenero = 1; }
                else if (pk_gender.SelectedIndex == 1) { idgenero = 2; }

                int moto = 0;
                if(checkMoto.IsChecked) { moto = 1; }

                int liviana = 0;
                if(checkLiviana.IsChecked) {  liviana = 1; }

                int pesada = 0;
                if(checkPesada.IsChecked) {  pesada = 1; }

                var resultadoCorreo = await _apiService.PostDataAsync<codigoVerificacionModel>("correoCodigoVerificacion.php", new { email = txt_email.Text, name = txt_name.Text });
                string? codigo = resultadoCorreo.verification_code;

                var data = new crearCuentaRepartidorModel
                {
                    nombre = txt_name.Text,
                    apellido = txt_surname.Text,
                    correo = txt_email.Text,
                    telefono = txt_phone.Text,
                    dni = txt_dni.Text,
                    fechanacimiento = birthdate,
                    iddepartamento = selectedDepartamentoId,
                    idciudad = selectedCiudadId,
                    genero = idgenero,
                    licencia_motocicleta = moto,
                    licencia_liviana = liviana,
                    licencia_pesada = pesada,
                    base64Images = base64Images,
                    Codigo = codigo
                };

                Console.WriteLine($"Data: {data}");
                await Navigation.PushAsync(new Views.Login.confirmarAplicar { BindingContext = data });
            }
            catch (Exception ex)
            {

            }
        }
        else
        {
            DisplayAlert("Alerta", "Not Valid", "OK");
        }
    }
}