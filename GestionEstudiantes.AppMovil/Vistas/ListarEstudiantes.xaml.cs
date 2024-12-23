using System.Collections.ObjectModel;
using Firebase.Database;
using Firebase.Database.Query;
using RegistroEstudiantes.Modelos.Modelos;

namespace GestionEstudiantes.AppMovil.Vistas;

public partial class ListarEstudiantes : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://gestionestudiantes-40900-default-rtdb.firebaseio.com/");
    public ObservableCollection<Estudiante> Lista { get; set; } = new ObservableCollection<Estudiante>();
    public ListarEstudiantes()
	{
		InitializeComponent();
        BindingContext = this;
        CargarLista();
	}

    private async void CargarLista()
    {
        Lista.Clear();
        var estudiantes = await client.Child("Estudiantes").OnceAsync<Estudiante>();

        var estudiantesActivos = estudiantes.Where(e=>e.Object.Estado==true).ToList();

        foreach (var estudiante in estudiantesActivos)
        {
            Lista.Add(new Estudiante
            {
                Id = estudiante.Key,
                PrimerNombre = estudiante.Object.PrimerNombre,
                PrimerApellido = estudiante.Object.PrimerApellido,
                SegundoApellido = estudiante.Object.SegundoApellido,
                CorreoElectronico = estudiante.Object.CorreoElectronico,
                Edad = estudiante.Object.Edad,
                Estado = estudiante.Object.Estado,
                Curso = estudiante.Object.Curso
            });
        }
    }

    private void filtroSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string filtro = filtroSearchBar.Text.ToLower();

        if(filtro.Length > 0 )
        {
            listaCollection.ItemsSource = Lista.Where(x => x.NombreCompleto.ToLower().Contains(filtro));
        }
        else
        {
            listaCollection.ItemsSource = Lista;
        }
    }

    private async void NuevoEstudienteBoton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CrearEstudiante());
    }

    private async void editarButton_Clicked(object sender, EventArgs e)
    {
        var boton = sender as ImageButton;
        var estudiante = boton?.CommandParameter as Estudiante;

        if (estudiante != null && !string.IsNullOrEmpty(estudiante.Id))
        {
            await Navigation.PushAsync(new EditarEstudiante(estudiante.Id));
        }
        else
        {
            await DisplayAlert("Error","No se pudo obtener la información del estudiante","OK");
        }
    }

    private async void deshabilitarButton_Clicked(object sender, EventArgs e)
    {
        var boton = sender as ImageButton;

        var estudiante = boton?.CommandParameter as Estudiante;

        if (estudiante == null)
        {
            await DisplayAlert("Error", "No se pudo obtener la información del estudiante", "OK");
            return;
        }

        bool confirmacion = await DisplayAlert("Confirmación", $"¿Estpa seguro que quiere deshabilitar al estudiante {estudiante.NombreCompleto}?", "Si", "No");

        if (confirmacion)
        {
            try
            {
                estudiante.Estado = false;

                await client.Child("Estudiantes").Child(estudiante.Id).PutAsync(estudiante);
                await DisplayAlert("Éxito", $"Se ha deshabilitado correctamente al estudiante {estudiante.NombreCompleto}?","OK");

                CargarLista();


            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}