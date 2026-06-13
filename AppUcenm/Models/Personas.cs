using System;
using System.IO;
using Microsoft.Maui.Controls;
using SQLite;

namespace AppUCENM.Models
{
    public class Personas
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Apellido { get; set; } = string.Empty;

        public DateTime FechaNacimiento { get; set; }

        [Unique]
        public string Correo { get; set; } = string.Empty;

        [Unique]
        public string Telefono { get; set; } = string.Empty;

        public string FotoBase64 { get; set; } = string.Empty;

        [Ignore]
        public ImageSource? FotoImage
        {
            get
            {
                if (string.IsNullOrEmpty(FotoBase64))
                    return null;

                byte[] bytes = Convert.FromBase64String(FotoBase64);

                return ImageSource.FromStream(() =>
                    new MemoryStream(bytes));
            }
        }

        [Ignore]
        public bool IsSelected { get; set; }
    }
}