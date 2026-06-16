using AppUCENM.Controllers;
using AppUCENM.Models;
using Microsoft.Maui.Media;
using Microsoft.Maui.ApplicationModel;

namespace AppUCENM.Views;

public partial class PageAddPersonas : ContentPage
{
    private PersonasController personasController;
    private string fotoBase64 = string.Empty;

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
            FechaNacimiento = FechaNac.Date ?? DateTime.Today,
            Correo = Correo.Text ?? string.Empty,
            Telefono = Telefono.Text ?? string.Empty,
            FotoBase64 = fotoBase64
        };

        try
        {
            await personasController.GuardarPerson(person);
            await DisplayAlert("Información", "Registro Guardado", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudo guardar: " + ex.Message, "OK");
        }
    }

    private async void BtnVerLista_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PageListPersonas());
    }

    private async void BtnTomarFoto_Clicked(object sender, EventArgs e)
    {
        // Solicitar permiso de cámara
        var status = await Permissions.RequestAsync<Permissions.Camera>();

        if (status != PermissionStatus.Granted)
        {
            await DisplayAlert("Permiso requerido", "Se necesita acceso a la cámara", "OK");

            return;
        }

        try
        {
            var foto = await MediaPicker.CapturePhotoAsync();

            if (foto == null)
                return;

            using var stream = await foto.OpenReadAsync();

            using MemoryStream ms = new MemoryStream();
            await stream.CopyToAsync(ms);

            byte[] bytes = ms.ToArray();

            // Convertir imagen a Base64
            fotoBase64 = Convert.ToBase64String(bytes);

            // Mostrar imagen en la aplicación
            FotoPersona.Source = ImageSource.FromStream(() =>
                new MemoryStream(bytes));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudo tomar la foto: " + ex.Message, "OK");
        }
    }
}