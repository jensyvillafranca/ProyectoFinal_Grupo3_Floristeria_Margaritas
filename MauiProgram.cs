using Microsoft.Extensions.Logging;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("JosefinSans-Bold.ttf", "JosefinSansBold");
                    fonts.AddFont("JosefinSans-Thin.ttf", "JosefinSansBold");
                    fonts.AddFont("Fredoka-Light.ttf", "FredokaLight");
                }).UseMauiMaps();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
