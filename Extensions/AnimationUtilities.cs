using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
