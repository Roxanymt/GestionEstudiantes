using GestionEstudiantes.AppMovil.Vistas;

namespace GestionEstudiantes.AppMovil
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage ( new ListarEstudiantes());
        }
    }
}
