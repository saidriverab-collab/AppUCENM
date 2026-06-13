using System;
using AppUCENM.Controllers;
using AppUCENM.Models;

namespace AppUCENM.Views;

public partial class PageEditPersona : ContentPage
{
    private PersonasController personasController;
    private Personas persona;

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
    }

    private async void BtnGuardar_Clicked(object sender, EventArgs e)
    {
        persona.Nombre = NombreEntry.Text;
        persona.Apellido = ApellidoEntry.Text;
        // DatePicker.Date may be nullable; use null-coalescing to keep previous value if null
        persona.FechaNacimiento = FechaNacimientoPicker.Date ?? persona.FechaNacimiento;
        persona.Correo = CorreoEntry.Text;
        persona.Telefono = TelefonoEntry.Text;

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
            await DisplayAlert("Error", "No se pudo actualizar: " + ex.Message, "OK");
        }
    }
}
