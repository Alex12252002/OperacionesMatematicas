using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tarea2.Modelo;

namespace Tarea2.Controlador
{
    public class DatabaseConexion
    {
        public SQLiteAsyncConnection Connection { get; set; }

        public DatabaseConexion(string path)
        {
            Connection = new SQLiteAsyncConnection(path);
            Connection.CreateTableAsync<Operaciones>().Wait();
        }

        public async Task InsertarOperacion(Operaciones operacion)
        {
            try
            {
                await Connection.InsertAsync(operacion);
            }
            catch (Exception ex)
            {
                // Manejar la excepción (por ejemplo, mostrar un mensaje de error)
                Console.WriteLine($"Error al insertar operación: {ex.Message}");
            }
        }

        public async Task<List<Operaciones>> ObtenerOperaciones()
        {
            try
            {
                return await Connection.Table<Operaciones>().ToListAsync();
            }
            catch (Exception ex)
            {
                // Manejar la excepción (por ejemplo, mostrar un mensaje de error)
                Console.WriteLine($"Error al obtener operaciones: {ex.Message}");
                return new List<Operaciones>(); // Retornar una lista vacía en caso de error
            }
        }


        public async Task EliminarOperacion(int id)
        {
            await Connection.DeleteAsync<Operaciones>(id);
        }
    }
}
