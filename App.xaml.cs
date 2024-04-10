using ProyectoFinal_Grupo3_Floristeria_Margaritas.Config;
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
                MainPage = new NavigationPage(new Views.Login.login());
            }
            else if (stayLogged == 1 && tipoUsuario == 1)
            {
                MainPage = new NavigationPage(new Views.Home.homePageUser());
            }
            else if (stayLogged == 1 && tipoUsuario == 3)
            {
                MainPage = new NavigationPage(new Views.Home.homePageRepartidor());
            }
            else if (stayLogged == 1 && tipoUsuario == 2)
            {
                MainPage = new NavigationPage(new Views.Home.homePageAdmin());
            }
        
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NAaF1cXmhNYVJ3WmFZfVpgdVRMYV1bR35PIiBoS35RckVgWH1cc3VURWlcU0x+");
        }
    }
}
