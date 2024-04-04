﻿using ProyectoFinal_Grupo3_Floristeria_Margaritas.Config;
namespace ProyectoFinal_Grupo3_Floristeria_Margaritas
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            int stayLogged = Config.Config.stayLogged;
            int tipoUsuario = Config.Config.tipoUsuario;

            if (stayLogged == 0 || stayLogged == -1)
            {
                MainPage = new AppShell();
            }
            else if (stayLogged == 1 && tipoUsuario == 1)
            {
                MainPage = new NavigationPage(new Views.Home.homePageUser());
            }
            else if (stayLogged == 1 && tipoUsuario == 3)
            {
                MainPage = new NavigationPage(new Views.Home.homePageRepartidor());
            }          
        }
    }
}
