/*
 * Descripción:
 * Este código define una clase llamada ShoppingCartController en el espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers.
 * La clase gestiona las operaciones CRUD (crear, leer, actualizar y eliminar) en la tabla de carrito de compras en una base de datos SQLite.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Controllers
{
    class ShoppingCartController
    {
        SQLiteAsyncConnection? _connection = null;

        public ShoppingCartController() { }

        //Conexion a la base de datos
        public async Task Init()
        {
            try
            {
                if (_connection is null)
                {
                    SQLite.SQLiteOpenFlags extensiones = SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.SharedCache;

                    if (string.IsNullOrEmpty(FileSystem.AppDataDirectory))
                    {
                        return;
                    }

                    _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, "ShoppingCartDB"), extensiones);

                    var creacion = await _connection.CreateTableAsync<Modelos.ShoppingCartItem>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Init(): {ex.Message}");
            }
        }

        //Create
        public async Task<bool> storeProducto(ShoppingCartItem producto)
        {
            await Init();

            // Check if the product already exists
            var existingProduct = await _connection.Table<ShoppingCartItem>().Where(i => i.idproducto == producto.idproducto).FirstOrDefaultAsync();

            if (existingProduct != null)
            {
                // Product already exists, return false
                return false;
            }

            if (producto.Id == 0)
            {
                await _connection.InsertAsync(producto);
            }
            else
            {
                await _connection.UpdateAsync(producto);
            }

            // Product added successfully, return true
            return true;
        }

        //Update
        public async Task<int> updateProducto(ShoppingCartItem producto)
        {
            await Init();
            return await _connection.UpdateAsync(producto);
        }

        //Read
        public async Task<List<Modelos.ShoppingCartItem>> getListProductos()
        {
            await Init();
            return await _connection.Table<ShoppingCartItem>().ToListAsync();
        }

        //Read Element
        public async Task<Modelos.ShoppingCartItem> getProducto(int pid)
        {
            await Init();
            return await _connection.Table<ShoppingCartItem>().Where(i => i.Id == pid).FirstOrDefaultAsync();
        }

        //Delete
        public async Task<int> deleteProducto(int itemID)
        {
            await Init();
            var itemToDelete = await getProducto(itemID);

            if (itemToDelete != null)
            {
                return await _connection.DeleteAsync(itemToDelete);
            }

            return 0;
        }

        public async Task<int> DeleteAllProductos()
        {
            await Init();
            return await _connection.DeleteAllAsync<ShoppingCartItem>();
        }
    }
}
