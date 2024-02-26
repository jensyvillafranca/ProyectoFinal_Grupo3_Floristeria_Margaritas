using System.ComponentModel;
using System.Windows.Input;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.Modelos
{
    public class FrameOrden : INotifyPropertyChanged
    {

        private int _idItem;
        public int IdItem
        {
            get { return _idItem; }
            set
            {
                if (_idItem != value)
                {
                    _idItem = value;
                    OnPropertyChanged(nameof(IdItem));
                }
            }
        }

        private int _idProducto;
        public int IdProducto
        {
            get { return _idProducto; }
            set
            {
                if (_idProducto != value)
                {
                    _idProducto = value;
                    OnPropertyChanged(nameof(IdProducto));
                }
            }
        }

        private string? _imageSource;
        public string? ImageSource
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

        private string? _labelNombreProducto;
        public string? LabelNombreProducto
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

        private string? _labelPrecioTotal;
        public string? LabelPrecioTotal
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

        private string? _Descuento;
        public string? Descuento
        {
            get { return _Descuento; }
            set
            {
                if (_Descuento != value)
                {
                    _Descuento = value;
                    OnPropertyChanged(nameof(Descuento));
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

        private int _stockQuantity;
        public int StockQuantity
        {
            get { return _stockQuantity; }
            set
            {
                if (_stockQuantity != value)
                {
                    _stockQuantity = value;
                    OnPropertyChanged(nameof(StockQuantity));
                }
            }
        }

        public ICommand? TappedCommand { get; set; }
        public ICommand? substractCommand { get; set; }
        public ICommand? addCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
