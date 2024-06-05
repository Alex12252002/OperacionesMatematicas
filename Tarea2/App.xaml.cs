using System;
using System.IO;
using Tarea2.Controlador;
using Tarea2.Vista;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tarea2
{
    public partial class App : Application
    {
        static DatabaseConexion databaseConexion;

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new HomePage());
        }

        public static DatabaseConexion DatabaseConexion
        {
            get
            {
                if (databaseConexion == null)
                {
                    var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Operacion.db3");
                    databaseConexion = new DatabaseConexion(databasePath);
                }
                return databaseConexion;
            }
        }
    }
}
