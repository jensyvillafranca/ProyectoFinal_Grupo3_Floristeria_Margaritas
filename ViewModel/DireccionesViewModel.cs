/*
 * Descripción:
 * Este código define el ViewModel para la gestión de direcciones en la aplicación Floristeria Margaritas.
 * Proporciona propiedades para almacenar información sobre una dirección, como su ID, dirección, ciudad,
 * departamento, longitud, latitud, referencia y color de fondo del marco.
 * También incluye un ICommand opcional para manejar eventos de toque en la dirección.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel
{
    public class DireccionesViewModel : INotifyPropertyChanged
    {
        private int _idDireccion;
        public int IdDireccion
        {
            get { return _idDireccion; }
            set
            {
                if (_idDireccion != value)
                {
                    _idDireccion = value;
                    OnPropertyChanged(nameof(IdDireccion));
                }
            }
        }

        private string? _Direccion;
        public string? Direccion
        {
            get { return _Direccion; }
            set
            {
                if (_Direccion != value)
                {
                    _Direccion = value;
                    OnPropertyChanged(nameof(Direccion));
                }
            }
        }

        private string? _Ciudad;
        public string? Ciudad
        {
            get { return _Ciudad; }
            set
            {
                if (_Ciudad != value)
                {
                    _Ciudad = value;
                    OnPropertyChanged(nameof(Ciudad));
                }
            }
        }

        private string? _Departamento;
        public string? Departamento
        {
            get { return _Departamento; }
            set
            {
                if (_Departamento != value)
                {
                    _Departamento = value;
                    OnPropertyChanged(nameof(Departamento));
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

        private string? _Longitud;
        public string? Longitud
        {
            get { return _Longitud; }
            set
            {
                if (_Longitud != value)
                {
                    _Longitud = value;
                    OnPropertyChanged(nameof(Longitud));
                }
            }
        }

        private string? _Latitud;
        public string? Latitud
        {
            get { return _Latitud; }
            set
            {
                if (_Latitud != value)
                {
                    _Latitud = value;
                    OnPropertyChanged(nameof(Latitud));
                }
            }
        }

        private string? _Referencia;
        public string? Referencia
        {
            get { return _Referencia; }
            set
            {
                if (_Referencia != value)
                {
                    _Referencia = value;
                    OnPropertyChanged(nameof(Referencia));
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


        public ICommand? TappedCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
