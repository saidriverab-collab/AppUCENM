using System;
using System.IO;
using AppUCENM.Controllers;
using AppUCENM.Models;
using Microsoft.Maui.Media;
using Microsoft.Maui.ApplicationModel;

namespace AppUCENM.Views;

public partial class PageEditPersona : ContentPage
{
    private PersonasController personasController;
    private Personas persona;
    private string fotoBase64 = string.Empty;

    public event EventHandler<Personas> PersonUpdated;

    public PageEditPersona(Personas persona)
    {
        InitializeComponent();
        personasController = new PersonasController();
        this.persona = persona;
        LoadData();
    }

    private void LoadData()
    {
        NombreEntry.Text = persona.Nombre;
        ApellidoEntry.Text = persona.Apellido;
        FechaNacimientoPicker.Date = persona.FechaNacimiento;
        CorreoEntry.Text = persona.Correo;
        TelefonoEntry.Text = persona.Telefono;

        fotoBase64 = persona.FotoBase64;

        if (!string.IsNullOrEmpty(persona.FotoBase64))
        {
            byte[] bytes = Convert.FromBase64String(persona.FotoBase64);

            FotoPersona.Source = ImageSource.FromStream(() =>
                new MemoryStream(bytes));
        }
    }

    private async void BtnCambiarFoto_Clicked(object sender, EventArgs e)
    {
        var status = await Permissions.RequestAsync<Permissions.Camera>();

        if (status != PermissionStatus.Granted)
        {
            await DisplayAlert("Permiso requerido", "Se necesita acceso a la cámara","OK");

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

            fotoBase64 = Convert.ToBase64String(bytes);

            FotoPersona.Source = ImageSource.FromStream(() =>
                new MemoryStream(bytes));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudo tomar la foto: " + ex.Message, "OK");
        }
    }

    private async void BtnGuardar_Clicked(object sender, EventArgs e)
    {
        persona.Nombre = NombreEntry.Text ?? string.Empty;
        persona.Apellido = ApellidoEntry.Text ?? string.Empty;
        persona.FechaNacimiento = FechaNacimientoPicker.Date ?? persona.FechaNacimiento;
        persona.Correo = CorreoEntry.Text ?? string.Empty;
        persona.Telefono = TelefonoEntry.Text ?? string.Empty;

        // Guardar la nueva foto
        persona.FotoBase64 = fotoBase64;

        try
        {
            int rows = await personasController.ActualizarPerson(persona);

            if (rows > 0)
            {
                await DisplayAlert("Informacion", "Registro actualizado", "OK");

                PersonUpdated?.Invoke(this, persona);

                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Advertencia", "No se pudo actualizar el registro.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudo actualizar: " + ex.Message,"OK");
        }
    }
}