/*
 * Descripción:
 * Este código define el ViewModel para las tarjetas en la aplicación Floristeria Margaritas.
 * Proporciona propiedades para almacenar información sobre cada tarjeta, como el ID de la tarjeta,
 * el número de la tarjeta, la fecha de vencimiento, el ID del cliente, el nombre y la descripción,
 * el color de fondo del marco y maneja los eventos relacionados con PropertyChanged.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel
{
    public class TarjetasViewModel : INotifyPropertyChanged
    {
        private int _idTarjeta;
        public int IdTarjeta
        {
            get { return _idTarjeta; }
            set
            {
                if (_idTarjeta != value)
                {
                    _idTarjeta = value;
                    OnPropertyChanged(nameof(IdTarjeta));
                }
            }
        }

        private string? _NumeroTarjeta;
            public string? NumeroTarjeta
            {
                get { return _NumeroTarjeta; }
                set
                {
                    if (_NumeroTarjeta != value)
                    {
                        _NumeroTarjeta = value;
                        OnPropertyChanged(nameof(NumeroTarjeta));
                    }
                }
            }

        private string? _FechaVencimiento;
        public string? FechaVencimiento
        {
            get { return _FechaVencimiento; }
            set
            {
                if (_FechaVencimiento != value)
                {
                    _FechaVencimiento = value;
                    OnPropertyChanged(nameof(FechaVencimiento));
                }
            }
        }

        private int _idCliente;
        public int IdCliente
        {
            get { return _idCliente; }
            set
            {
                if (_idCliente != value)
                {
                    _idCliente = value;
                    OnPropertyChanged(nameof(IdCliente));
                }
            }
        }

        private string? _Nombre;
        public string? Nombre
        {
            get { return _Nombre; }
            set
            {
                if (_Nombre != value)
                {
                    _Nombre = value;
                    OnPropertyChanged(nameof(Nombre));
                }
            }
        }

        private string? _Descripcion;
        public string? Descripcion
        {
            get { return _Descripcion; }
            set
            {
                if (_Descripcion != value)
                {
                    _Descripcion = value;
                    OnPropertyChanged(nameof(Descripcion));
                }
            }
        }

        private Color? _frameBackgroundColor;
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

        public ICommand? TappedCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    
}
