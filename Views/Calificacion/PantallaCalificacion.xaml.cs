
/* Cambio no fusionado mediante combinación del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-maccatalyst)'
Antes:
using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
Después:
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
*/

/* Cambio no fusionado mediante combinación del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-windows10.0.19041.0)'
Antes:
using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
Después:
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
*/

/* Cambio no fusionado mediante combinación del proyecto 'ProyectoFinal_Grupo3_Floristeria_Margaritas (net8.0-ios)'
Antes:
using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
Después:
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
*/
namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Views.Calificacion
{
    public partial class PantallaCalificacion : ContentPage
    {
        private bool[] estrellasLlenas = new bool[5];

        public PantallaCalificacion()
        {
            InitializeComponent();

            // Inicializar todas las estrellas como vacías al principio
            for (int i = 0; i < estrellasLlenas.Length; i++)
            {
                estrellasLlenas[i] = false;
            }
        }

        private void OnStar_Checked(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox starCheckBox)
            {
                int indiceEstrella = int.Parse(starCheckBox.AutomationId);

                if (starCheckBox.IsChecked)
                {
                    // Si se selecciona una estrella, llenar hasta la estrella seleccionada
                    for (int i = 0; i <= indiceEstrella; i++)
                    {
                        estrellasLlenas[i] = true;
                    }
                }
                else
                {
                    // Si se deselecciona, vaciar todas las estrellas
                    for (int i = 0; i < estrellasLlenas.Length; i++)
                    {
                        estrellasLlenas[i] = false;
                    }
                }

                // Actualizar las imágenes de todas las estrellas
                for (int i = 0; i < estrellasLlenas.Length; i++)
                {
                    string imagenEstrella = estrellasLlenas[i] ? "PantallaCalificacion/estrella_llena3.png" : "PantallaCalificacion/estrella_vacia.png";
                    var estrellaImage = this.FindByName<Image>($"imgEstrella{i + 1}");
                    estrellaImage.Source = imagenEstrella;
                }
            }
        }

        private void OnStarTapped(object sender, EventArgs e)
        {
            if (sender is Image starImage)
            {
                int indiceEstrella = Convert.ToInt32(starImage.AutomationId);

                // Cambiar la imagen y actualizar el estado para la estrella tocada
                estrellasLlenas[indiceEstrella] = !estrellasLlenas[indiceEstrella];

                // Actualizar las imágenes de todas las estrellas
                for (int i = 0; i < estrellasLlenas.Length; i++)
                {
                    // Llenar estrellas hasta la estrella tocada y vaciar las siguientes
                    if (i < indiceEstrella)
                    {
                        estrellasLlenas[i] = true;
                    }
                    else if (i > indiceEstrella)
                    {
                        estrellasLlenas[i] = false;
                    }

                    string imagenEstrella = estrellasLlenas[i] ? "PantallaCalificacion/estrella_llena3.png" : "PantallaCalificacion/estrella_vacia.png";
                    var estrellaImage = this.FindByName<Image>($"imgEstrella{i + 1}");
                    estrellaImage.Source = imagenEstrella;
                }
            }
        }

        private void btnConfirmar_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Calificacion.CalificacionFinalizada());
        }
    }


}
