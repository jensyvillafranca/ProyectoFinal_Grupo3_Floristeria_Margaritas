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

        public DetalleProductoViewModel(ProductoModel selectedProduct) 
        {
            _selectedProduct = selectedProduct;
        }

        public string ImageSource => _selectedProduct.enlacefoto;
        public string NombreProducto => _selectedProduct.nombreproducto;
        public string Descripcion => _selectedProduct.descripcion;
        public string Precio => _selectedProduct.precioventa;

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
