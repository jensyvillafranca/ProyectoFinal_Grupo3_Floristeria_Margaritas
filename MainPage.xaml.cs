﻿namespace ProyectoFinal_Grupo3_Floristeria_Margaritas
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private void CounterBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Home.homePageUser());
        }

        private void btnHomeRepartidor_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Home.homePageRepartidor());
        }

        private void btnHomeAdmin_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Home.homePageAdmin());
        }

        private void btnProductos_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Views.Productos.productos());
        }
    }

}
