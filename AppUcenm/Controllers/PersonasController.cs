using AppUCENM.Servicios;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppUCENM.Controllers
{
    public class PersonasController
    {
        private readonly DatabaseServices databaseServices;
            public PersonasController()
        {
               databaseServices = new DatabaseServices();
        }
        public async Task GuardarPerson(Models.Personas personas)
        {
           await databaseServices.InsertPerson(personas);
        }
        public async Task<List<Models.Personas>> ObtenerPersonas()
        {
            return await databaseServices.ObtenerListaPersonas();
        }

        public async Task<int> ActualizarPerson(Models.Personas personas)
        {
            return await databaseServices.UpdatePerson(personas);
        }

        public async Task EliminarPerson(Models.Personas personas)
        {
            await databaseServices.DeletePerson(personas);
        }
    }
}
