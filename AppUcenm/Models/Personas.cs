using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AppUCENM.Models
{
    public class Personas
    {
        [PrimaryKey, AutoIncrement] 
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        [Unique]
        public string Correo { get; set; }
        [Unique]
        public string Telefono { get; set; }

        [Ignore]
        // Property used only for UI selection, not persisted in the database
        public bool IsSelected { get; set; }
    }
}

