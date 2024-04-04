/*
 * Descripción:
 * Este código define una clase llamada ShoppingCartItem dentro del espacio de nombres ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos.
 * Esta clase representa un artículo en el carrito de compras y está mapeada a una tabla de base de datos llamada ShoppingCartDB.
 * La clase contiene propiedades que representan los diferentes atributos de un artículo en el carrito de compras.
 * Se utilizan atributos SQLite para configurar el mapeo de propiedades a la tabla de la base de datos.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    [SQLite.Table("ShoppingCartDB")]
    public class ShoppingCartItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public int idproducto { get; set; }
        [MaxLength(255), NotNull]
        public string? nombreproducto { get; set; }
        [MaxLength(255), NotNull]
        public string? categoria { get; set; }
        [MaxLength(255), NotNull]
        public string? precioventa { get; set; }
        [MaxLength(255)]
        public string? precioventaOutdated { get; set; } = null;
        [MaxLength(255), NotNull]
        public int stock { get; set; }
        [MaxLength(255), NotNull]
        public string? enlacefoto { get; set; }
        [MaxLength(255), NotNull]
        public string? descuento { get; set; }
        [MaxLength(255)]
        public string? descuentoOutdated { get; set; } = null;
        [MaxLength(255), NotNull]
        public string? descripcion { get; set; }
        [NotNull]
        public int cantidad { get; set; }
    }
}
