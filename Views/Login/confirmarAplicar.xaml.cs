using ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using static ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Productos.confirmarOrden;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Login;

public partial class confirmarAplicar : ContentPage
{
    private ApiService _apiService = new ApiService();
    private crearCuentaRepartidorModel model;
    private bool notValid = false;
    private string? codigoVerificacion;

    //Conteo de Reenvio
    private int countdownSeconds = 30;
    private bool isCountdownRunning = false;

    public class ImagenesItems
    {
        public List<string> IMG { get; set; }
    }

    public confirmarAplicar()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        BindingContext = this;

        labelEnviarCog.Text = "Enviar de nuevo";
        labelEnviarCog.GestureRecognizers.Add(new TapGestureRecognizer
        {
            NumberOfTapsRequired = 1,
            Command = new Command(StartCountdown)
        });
    }

    // M�todo que se ejecuta al aparecer la p�gina
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is crearCuentaRepartidorModel data)
        {
            model = data;
            codigoVerificacion = data.Codigo;
            Console.WriteLine($"Data: {data}");
        }
    }

    private async void btnBack_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var resultadoCorreo = await _apiService.PostDataAsync<codigoVerificacionModel>("correoCodigoVerificacion.php", new { email = model.correo, name = model.nombre });
        codigoVerificacion = resultadoCorreo.verification_code;
        await DisplayAlert("Alerta", "C�digo de Verificaci�n Reenviado", "OK");
    }

    private async void btnVerificar_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(entryCodigo.Text))
        {
            await DisplayAlert("Alerta", "Por favor ingrese el c�digo de verificaci�n que recibi� en su correo electr�nico o pida un nuevo c�digo.", "OK");
            return;
        }

        if (notValid)
        {
            await DisplayAlert("Alerta", "Esta sesi�n ya no es v�lida.", "OK");
            await Navigation.PushAsync(new Views.Login.login());
            return;
        }

        if (entryCodigo.Text == codigoVerificacion)
        {

            var data = new
            {
                nombre = model.nombre,
                apellido = model.apellido,
                correo = model.correo,
                telefono = model.telefono,
                dni = model.dni,
                fechanacimiento = model.fechanacimiento,
                iddepartamento = model.iddepartamento,
                idciudad = model.idciudad,
                genero = model.genero,
                licencia_motocicleta = model.licencia_motocicleta,
                licencia_pesada = model.licencia_pesada,
                licencia_liviana = model.licencia_liviana,
                imagenesRepartidor = new ImagenesItems
                {
                    IMG = model.base64Images
                }
            };

            bool isSuccess = await _apiService.PostSuccessAsync("agregarSolicitud.php", data);
            if (isSuccess)
            {
                notValid = true;
                await DisplayAlert("�Felicidades!", "Su solicitud ha sido enviada, se le avisara por correo de la decisi�n.", "OK");
                await Navigation.PushAsync(new Views.Login.login());
            }
        }
        else
        {
            await DisplayAlert("Alerta", "El c�digo de verificaci�n ingresado es incorrecto porfavor ingr�selo de nuevo o pida un nuevo c�digo.", "OK");
            return;
        }
    }

    // M�todo para iniciar el conteo regresivo
    private void StartCountdown()
    {
        if (!isCountdownRunning)
        {
            isCountdownRunning = true;
            _ = RunCountdown();
        }
    }

    // M�todo para ejecutar el conteo regresivo
    private async Task RunCountdown()
    {
        while (countdownSeconds > 0)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                labelEnviarCog.Text = $"Enviar de nuevo ({countdownSeconds}s)";
                labelEnviarCog.IsEnabled = false; // Disable the label during countdown
            });

            await Task.Delay(1000); // Wait for 1 second
            countdownSeconds--;
        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            labelEnviarCog.Text = "Enviar de nuevo";
            labelEnviarCog.IsEnabled = true; // Enable the label after countdown finishes
            isCountdownRunning = false;
            countdownSeconds = 30; // Reset the countdown for the next time
        });
    }
}