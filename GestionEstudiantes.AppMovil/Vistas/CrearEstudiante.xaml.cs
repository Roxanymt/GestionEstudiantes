using Firebase.Database;
using Firebase.Database.Query;
using RegistroEstudiantes.Modelos.Modelos;

namespace GestionEstudiantes.AppMovil.Vistas;

public partial class CrearEstudiante : ContentPage
{
	FirebaseClient client = new FirebaseClient("https://gestionestudiantes-40900-default-rtdb.firebaseio.com/");

	public List<Curso> Cursos { get; set; }
	public CrearEstudiante()
	{
		InitializeComponent();
		ListarCursos();
		BindingContext = this;
	}

    private void ListarCursos()
    {
		var cursos = client.Child("Cursos").OnceAsync<Curso>();
		Cursos = cursos.Result.Select(x => x.Object).ToList();

    }

    private async void guardarBoton_Clicked(object sender, EventArgs e)
    {
		Curso curso = cursoPicker.SelectedItem as Curso;

		var estudiante = new Estudiante
		{
			PrimerNombre = primerNombreentry.Text,
			PrimerApellido = primerApellidoentry.Text,
			SegundoApellido = segundoApellidoentry.Text,
			CorreoElectronico = correoEntry.Text,
			Edad = int.Parse(edadEntry.Text),
			Curso = curso
		};

		try
		{
            await client.Child("Estudiantes").PostAsync(estudiante);
            await DisplayAlert("Éxito", $"Estudiante {estudiante.PrimerNombre} {estudiante.PrimerApellido} se guardó correctamente", "OK");
            await Navigation.PopAsync();
        }
		catch (Exception ex)
		{
            await DisplayAlert("Error", ex.Message, "OK");

        }
    }
}