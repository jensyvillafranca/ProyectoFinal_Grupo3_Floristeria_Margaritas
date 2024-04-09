/*
 * Descripción:
 * Este código define el ViewModel para la vista de detalle de producto en la aplicación Floristeria Margaritas.
 * Proporciona datos para mostrar la información detallada de un producto, incluyendo su imagen, nombre, descripción,
 * precio con descuento (si aplica) y precio original.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos;
using System.ComponentModel;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel
{
    public class DetalleProductoViewModel : INotifyPropertyChanged
    {
        private ProductoModel _selectedProduct;
        private double precioProducto = 0;
        private double discountPercentage = 0;
        private double discountedPrice = 0;

        public DetalleProductoViewModel(ProductoModel selectedProduct)
        {
            _selectedProduct = selectedProduct;
            precioProducto = Double.Parse(_selectedProduct.precioventa);
            discountPercentage = Double.Parse(_selectedProduct.descuento) / 100.0;
            discountedPrice = Math.Round(precioProducto - (precioProducto * discountPercentage), 2);
        }

        public string ImageSource => _selectedProduct.enlacefoto;
        public string NombreProducto => _selectedProduct.nombreproducto;
        public string Descripcion => _selectedProduct.descripcion;
        public string Precio => $"L {discountedPrice.ToString("N2")}";
        public string LabelDescuento => _selectedProduct.descuento;
        public string LabelPrecioOriginal => $"L {_selectedProduct.precioventa}";

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
