using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AppUCENM.Models;
using SQLite;

namespace AppUCENM.Servicios
{
    public class DatabaseServices
    {
        private SQLiteAsyncConnection? _connection;

        public async Task Init()
        {

            if (_connection != null)
            {
                return;
            }


            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "DBPersons.db3");
            _connection = new SQLiteAsyncConnection(dbPath);
            await _connection.CreateTableAsync<Personas>();
        }

        // CRUD - Create, Read, Update, Delete
        public async Task<int> InsertPerson(Models.Personas personas)
        {
            await Init();
            return await _connection.InsertAsync(personas);
        }

        public async Task<List<Models.Personas>> ObtenerListaPersonas()
        {
            await Init();
            return await _connection.Table<Models.Personas>().ToListAsync();
        }

        public async Task<int> UpdatePerson(Models.Personas personas)
        {
            await Init();
            return await _connection.UpdateAsync(personas);
        }

        public async Task<int> DeletePerson(Models.Personas personas)
        {
            await Init();
            return await _connection.DeleteAsync(personas);
        }
    }
}
