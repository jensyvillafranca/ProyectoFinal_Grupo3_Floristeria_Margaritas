
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

        private void OnStar_Clicked(object sender, EventArgs e)
        {
            if (sender is ImageButton starButton)
            {
                // Obtener el índice de la estrella clicada
                int indiceEstrella = int.Parse(starButton.AutomationId);

                // Verificar la imagen actual y cambiarla
                if (starButton.Source.ToString().Contains("estrella_vacia.png"))
                {
                    // Llenar la estrella clicada y vaciar las siguientes
                    for (int i = 0; i <= indiceEstrella; i++)
                    {
                        estrellasLlenas[i] = true;
                    }

                    // Vaciar las estrellas restantes
                    for (int i = indiceEstrella + 1; i < estrellasLlenas.Length; i++)
                    {
                        estrellasLlenas[i] = false;
                    }

                    starButton.Source = "PantallaCalificacion/estrella_llena1.png";
                }
                else
                {
                    // Vaciar todas las estrellas a partir de la clicada
                    for (int i = indiceEstrella; i < estrellasLlenas.Length; i++)
                    {
                        estrellasLlenas[i] = false;
                    }

                    starButton.Source = "PantallaCalificacion/estrella_vacia.png";
                }

                // Actualizar las imágenes de las estrellas
                for (int i = 0; i < estrellasLlenas.Length; i++)
                {
                    string imagenEstrella = estrellasLlenas[i] ? "PantallaCalificacion/estrella_llena1.png" : "PantallaCalificacion/estrella_vacia.png";
                    // Encuentra la ImageButton correspondiente según el índice
                    var estrella = this.FindByName<ImageButton>($"btnEstrella{i + 1}");
                    estrella.Source = imagenEstrella;
                }
            }
        }

        private void BtnOpinion_Clicked(object sender, EventArgs e)
        {

        }

        private void btnConfirmar_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Calificacion.CalificacionFinalizada());
        }
    }
}
