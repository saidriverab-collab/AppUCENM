using AppUCENM.Controllers;
using AppUCENM.Models;

namespace AppUCENM.Views;

public partial class PageListPersonas : ContentPage
{
    private PersonasController personasController;
    private List<Personas> allPersons = new();

    public PageListPersonas()
    {
        InitializeComponent();
        personasController = new PersonasController();
        _ = LoadPersons();
    }

    private async Task LoadPersons()
    {
        allPersons = await personasController.ObtenerPersonas();
        CollectionViewPersons.ItemsSource = allPersons;
        BtnEditar.IsEnabled = false;
        BtnEliminar.IsEnabled = false;
    }

    private void UpdateButtonsState()
    {
        var anySelected = allPersons.Any(p => p.IsSelected);
        BtnEditar.IsEnabled = anySelected;
        BtnEliminar.IsEnabled = anySelected;
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        // Update selection state reflecting bound IsSelected flags
        UpdateButtonsState();
    }

    private async void BtnRefrescar_Clicked(object sender, EventArgs e)
    {
        await LoadPersons();
    }

    private async void BtnEliminar_Clicked(object sender, EventArgs e)
    {
        var selected = allPersons.FirstOrDefault(p => p.IsSelected);
        if (selected == null)
            return;

        bool ok = await DisplayAlert("Confirmar", $"¿Eliminar a {selected.Nombre} {selected.Apellido}?", "Si", "No");
        if (!ok) return;

        await personasController.EliminarPerson(selected);
        await LoadPersons();
    }

    private async void BtnEditar_Clicked(object sender, EventArgs e)
    {
        var selected = allPersons.FirstOrDefault(p => p.IsSelected);
        if (selected == null)
            return;

        // Navegar a la página de edición con el objeto seleccionado
        var editPage = new PageEditPersona(selected);
        editPage.PersonUpdated += async (s, updated) =>
        {
            await LoadPersons();
        };
        await Navigation.PushAsync(editPage);
    }

    private void ApplyFilter(string q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            CollectionViewPersons.ItemsSource = allPersons;
            return;
        }

        var filtered = allPersons.Where(p =>
            (!string.IsNullOrEmpty(p.Nombre) && p.Nombre.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrEmpty(p.Apellido) && p.Apellido.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrEmpty(p.Correo) && p.Correo.Contains(q, StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrEmpty(p.Telefono) && p.Telefono.Contains(q, StringComparison.OrdinalIgnoreCase))
        ).ToList();

        CollectionViewPersons.ItemsSource = filtered;
    }

    private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        ApplyFilter(SearchBar.Text?.Trim());
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        ApplyFilter(e.NewTextValue?.Trim());
    }
}
