using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using System;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas
{
    internal class Program : MauiApplication
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
}
