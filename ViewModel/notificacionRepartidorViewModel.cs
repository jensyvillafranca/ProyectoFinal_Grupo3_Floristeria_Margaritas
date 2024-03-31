/*
 * Descripción:
 * Este código define el ViewModel para las notificaciones del repartidor en la aplicación Floristeria Margaritas.
 * Proporciona propiedades para almacenar información sobre cada notificación, como el ID del ítem, la fuente de la imagen,
 * el título, el cuerpo, la fecha, el estado de lectura, el color de fondo del marco, el color del texto, y maneja los eventos
 * relacionados con PropertyChanged.
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
    public class notificacionRepartidorViewModel : INotifyPropertyChanged
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

        private string? _titulo;
        public string? Titulo
        {
            get { return _titulo; }
            set
            {
                if (_titulo != value)
                {
                    _titulo = value;
                    OnPropertyChanged(nameof(Titulo));
                }
            }
        }

        private string? _cuerpo;
        public string? Cuerpo
        {
            get { return _cuerpo; }
            set
            {
                if (_cuerpo != value)
                {
                    _cuerpo = value;
                    OnPropertyChanged(nameof(Cuerpo));
                }
            }
        }

        private string? _fecha;
        public string? Fecha
        {
            get { return _fecha; }
            set
            {
                if (_fecha != value)
                {
                    _fecha = value;
                    OnPropertyChanged(nameof(Fecha));
                }
            }
        }

        private string? _lectura;
        public string? Lectura
        {
            get { return _lectura; }
            set
            {
                if (_lectura != value)
                {
                    _lectura = value;
                    OnPropertyChanged(nameof(Lectura));
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

        public ICommand? TappedCommand { get; set; }
        public ICommand? EliminarCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
