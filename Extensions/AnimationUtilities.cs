/*
 * Descripción:
 * Este código define una clase llamada AnimationUtilities en el espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions.
 * La clase proporciona un método estático para cambiar el color de un marco (Frame) con una animación simple.
 */

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Extensions
{
    class AnimationUtilities
    {
        public static async Task ChangeFrameColor(Frame targetFrame, Color fromColor, Color toColor, int delay)
        {
            Frame frame = targetFrame;
            if (targetFrame != null)
            {
                // Cambiar a otro color
                targetFrame.BackgroundColor = toColor;

                // Timer
                await Task.Delay(delay);

                // Regresar a color Original
                targetFrame.BackgroundColor = fromColor;
            }
        }



    }
        }

}

