using AppUCENM.Controllers;
using AppUCENM.Models;


namespace AppUCENM.Views;

public partial class PageAddPersonas : ContentPage
{
	private PersonasController personasController;

	public PageAddPersonas()
	{
		InitializeComponent();
		personasController = new PersonasController();
	}

	private async void btnGuardar_Clicked(object sender, EventArgs e)
	{
		Personas person = new()
		{
			Nombre = Nombre.Text ?? string.Empty,
			Apellido = Apellido.Text ?? string.Empty,
			FechaNacimiento = FechaNac.Date ?? DateTime.Now,
			Correo = Correo.Text ?? string.Empty,
			Telefono = Telefono.Text ?? string.Empty
		};

		try
		{
			await personasController.GuardarPerson(person);
			await DisplayAlert("Informacion", "Registro Guardado", "OK");
		}
		catch(Exception ex)
		{
			await DisplayAlert("Error","No se pudo guardar: " + ex.Message, "OK");
		}

    }

	private async void BtnVerLista_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new PageListPersonas());
	}
}