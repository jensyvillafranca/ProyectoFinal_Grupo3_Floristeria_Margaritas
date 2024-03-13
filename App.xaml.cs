using ProyectoFinal_Grupo3_Floristeria_Margaritas.Config;
namespace ProyectoFinal_Grupo3_Floristeria_Margaritas
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            int stayLogged = Config.Config.stayLogged;

            if (stayLogged == 0 || stayLogged == -1)
            {
                MainPage = new AppShell();
            }
            else
            {
                MainPage = new NavigationPage(new Views.Home.homePageUser());
            }

            
        }
    }
}
