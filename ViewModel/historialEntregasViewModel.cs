using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel
{
    public class historialEntregasViewModel : INotifyPropertyChanged
    {
        private string? _idItem;
        public string? IdItem
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

        private DateTime _fechaPedido;
        public DateTime FechaPedido
        {
            get { return _fechaPedido; }
            set
            {
                if (_fechaPedido != value)
                {
                    _fechaPedido = value;
                    OnPropertyChanged(nameof(FechaPedido));
                }
            }
        }

        private string? _nombresCliente;
        public string? NombresCliente
        {
            get { return _nombresCliente; }
            set
            {
                if (_nombresCliente != value)
                {
                    _nombresCliente = value;
                    OnPropertyChanged(nameof(NombresCliente));
                }
            }
        }

        private double _ganancia;
        public double Ganancia
        {
            get { return _ganancia; }
            set
            {
                if (_ganancia != value)
                {
                    _ganancia = value;
                    OnPropertyChanged(nameof(Ganancia));
                }
            }
        }

        private string? _estadoPedido;
        public string? EstadoPedido
        {
            get { return _estadoPedido; }
            set
            {
                if (_estadoPedido != value)
                {
                    _estadoPedido = value;
                    OnPropertyChanged(nameof(EstadoPedido));
                }
            }
        }

        private int _calificacion;
        public int Calificacion
        {
            get { return _calificacion; }
            set
            {
                if (_calificacion != value)
                {
                    _calificacion = value;
                    OnPropertyChanged(nameof(Calificacion));
                }
            }
        }

        private string? _motivoCalificacion;
        public string? MotivoCalificacion
        {
            get { return _motivoCalificacion; }
            set
            {
                if (_motivoCalificacion != value)
                {
                    _motivoCalificacion = value;
                    OnPropertyChanged(nameof(MotivoCalificacion));
                }
            }
        }

        private string? _fechaPedidoFormateada;
        public string? FechaPedidoFormateada
        {
            get { return _fechaPedidoFormateada; }
            set
            {
                if (_fechaPedidoFormateada != value)
                {
                    _fechaPedidoFormateada = value;
                    OnPropertyChanged(nameof(FechaPedidoFormateada));
                }
            }
        }

        private Color _frameBackgroundColor;
        public Color FrameBackgroundColor
        {
            get { return _frameBackgroundColor; }
            set
            {
                if (_frameBackgroundColor != value)
                {
                    _frameBackgroundColor = value;
                    OnPropertyChanged(nameof(FrameBackgroundColor));
                }
            }
        }

        private Color _textColor;
        public Color TextColor
        {
            get { return _textColor; }
            set
            {
                if (_textColor != value)
                {
                    _textColor = value;
                    OnPropertyChanged(nameof(TextColor));
                }
            }
        }

        private string? _ImageSource;
        public string? ImageSource
        {
            get { return _ImageSource; }
            set
            {
                if (_ImageSource != value)
                {
                    _ImageSource = value;
                    OnPropertyChanged(nameof(ImageSource));
                }
            }
        }

        private string? _ImageStar1;
        public string? ImageStar1
        {
            get { return _ImageStar1; }
            set
            {
                if (_ImageStar1 != value)
                {
                    _ImageStar1 = value;
                    OnPropertyChanged(nameof(ImageStar1));
                }
            }
        }

        private string? _ImageStar2;
        public string? ImageStar2
        {
            get { return _ImageStar2; }
            set
            {
                if (_ImageStar2 != value)
                {
                    _ImageStar2 = value;
                    OnPropertyChanged(nameof(ImageStar2));
                }
            }
        }

        private string? _ImageStar3;
        public string? ImageStar3
        {
            get { return _ImageStar3; }
            set
            {
                if (_ImageStar3 != value)
                {
                    _ImageStar3 = value;
                    OnPropertyChanged(nameof(ImageStar3));
                }
            }
        }

        private string? _ImageStar4;
        public string? ImageStar4
        {
            get { return _ImageStar4; }
            set
            {
                if (_ImageStar4 != value)
                {
                    _ImageStar4 = value;
                    OnPropertyChanged(nameof(ImageStar4));
                }
            }
        }

        private string? _ImageStar5;
        public string? ImageStar5
        {
            get { return _ImageStar5; }
            set
            {
                if (_ImageStar5 != value)
                {
                    _ImageStar5 = value;
                    OnPropertyChanged(nameof(ImageStar5));
                }
            }
        }

        private bool _visibilidad;
        public bool Visibilidad
        {
            get { return _visibilidad; }
            set
            {
                if (_visibilidad != value)
                {
                    _visibilidad = value;
                    OnPropertyChanged(nameof(Visibilidad));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
