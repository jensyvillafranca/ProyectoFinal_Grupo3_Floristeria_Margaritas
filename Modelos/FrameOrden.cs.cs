using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class FrameOrden : INotifyPropertyChanged
    {
        private string _imageSource;
        public string ImageSource
        {
            get { return _imageSource; }
            set
            {
                if (_imageSource != value)
                {
                    _imageSource = value;
                    OnPropertyChanged(nameof(ImageSource));
                }
            }
        }

        private string _labelNombreProducto;
        public string LabelNombreProducto
        {
            get { return _labelNombreProducto; }
            set
            {
                if (_labelNombreProducto != value)
                {
                    _labelNombreProducto = value;
                    OnPropertyChanged(nameof(LabelNombreProducto));
                }
            }
        }

        private string _labelPrecioTotal;
        public string LabelPrecioTotal
        {
            get { return _labelPrecioTotal; }
            set
            {
                if (_labelPrecioTotal != value)
                {
                    _labelPrecioTotal = value;
                    OnPropertyChanged(nameof(LabelPrecioTotal));
                }
            }
        }

        private double _precioProducto;
        public double PrecioProducto
        {
            get { return _precioProducto; }
            set
            {
                if (_precioProducto != value)
                {
                    _precioProducto = value;
                    OnPropertyChanged(nameof(PrecioProducto));
                }
            }
        }

        private int _entryQuantity;
        public int EntryQuantity
        {
            get { return _entryQuantity; }
            set
            {
                if (_entryQuantity != value)
                {
                    _entryQuantity = value;
                    OnPropertyChanged(nameof(EntryQuantity));
                }
            }
        }

        public ICommand TappedCommand { get; set; }
        public ICommand substractCommand { get; set; }
        public ICommand addCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
