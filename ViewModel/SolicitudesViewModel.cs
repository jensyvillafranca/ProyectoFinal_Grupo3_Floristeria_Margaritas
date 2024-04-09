using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel
{
    public class SolicitudesViewModel : INotifyPropertyChanged
    {
        private int _idSolicitud;
        public int IdSolicitud
        {
            get { return _idSolicitud; }
            set
            {
                if (_idSolicitud != value)
                {
                    _idSolicitud = value;
                    OnPropertyChanged(nameof(IdSolicitud));
                }
            }
        }

        private string? _NombreCompleto;
        public string? NombreCompleto
        {
            get { return _NombreCompleto; }
            set
            {
                if (_NombreCompleto != value)
                {
                    _NombreCompleto = value;
                    OnPropertyChanged(nameof(NombreCompleto));
                }
            }
        }

        private string? _Correo;
        public string? Correo
        {
            get { return _Correo; }
            set
            {
                if (_Correo != value)
                {
                    _Correo = value;
                    OnPropertyChanged(nameof(Correo));
                }
            }
        }

        private string? _Telefono;
        public string? Telefono
        {
            get { return _Telefono; }
            set
            {
                if (_Telefono != value)
                {
                    _Telefono = value;
                    OnPropertyChanged(nameof(Telefono));
                }
            }
        }

        private string? _Dni;
        public string? Dni
        {
            get { return _Dni; }
            set
            {
                if (_Dni != value)
                {
                    _Dni = value;
                    OnPropertyChanged(nameof(Dni));
                }
            }
        }

        private string? _FechaNacimiento;
        public string? FechaNacimiento
        {
            get { return _FechaNacimiento; }
            set
            {
                if (_FechaNacimiento != value)
                {
                    _FechaNacimiento = value;
                    OnPropertyChanged(nameof(FechaNacimiento));
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

        private string? _Genero;
        public string? Genero
        {
            get { return _Genero; }
            set
            {
                if (_Genero != value)
                {
                    _Genero = value;
                    OnPropertyChanged(nameof(Genero));
                }
            }
        }

        private bool _Motocicleta;
        public bool Motocicleta
        {
            get { return _Motocicleta; }
            set
            {
                if (_Motocicleta != value)
                {
                    _Motocicleta = value;
                    OnPropertyChanged(nameof(Motocicleta));
                }
            }
        }

        private bool _Pesada;
        public bool Pesada
        {
            get { return _Pesada; }
            set
            {
                if (_Pesada != value)
                {
                    _Pesada = value;
                    OnPropertyChanged(nameof(Pesada));
                }
            }
        }

        private bool _Liviana;
        public bool Liviana
        {
            get { return _Liviana; }
            set
            {
                if (_Liviana != value)
                {
                    _Liviana = value;
                    OnPropertyChanged(nameof(Liviana));
                }
            }
        }

        private string? _FechaSolicitud;
        public string? FechaSolicitud
        {
            get { return _FechaSolicitud; }
            set
            {
                if (_FechaSolicitud != value)
                {
                    _FechaSolicitud = value;
                    OnPropertyChanged(nameof(FechaSolicitud));
                }
            }
        }

        private int _idEstado;
        public int IdEstado
        {
            get { return _idEstado; }
            set
            {
                if (_idEstado != value)
                {
                    _idEstado = value;
                    OnPropertyChanged(nameof(IdEstado));
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

        private bool _EnabledAceptar;
        public bool EnabledAceptar
        {
            get { return _EnabledAceptar; }
            set
            {
                if (_EnabledAceptar != value)
                {
                    _EnabledAceptar = value;
                    OnPropertyChanged(nameof(EnabledAceptar));
                }
            }
        }

        private bool _EnabledDenegar;
        public bool EnabledDenegar
        {
            get { return _EnabledDenegar; }
            set
            {
                if (_EnabledDenegar != value)
                {
                    _EnabledDenegar = value;
                    OnPropertyChanged(nameof(EnabledDenegar));
                }
            }
        }

        private string? _Estado;
        public string? Estado
        {
            get { return _Estado; }
            set
            {
                if (_Estado != value)
                {
                    _Estado = value;
                    OnPropertyChanged(nameof(Estado));
                }
            }
        }

        public ICommand? FotoCommand { get; set; }
        public ICommand? AceptarCommand { get; set; }
        public ICommand? DenegarCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
