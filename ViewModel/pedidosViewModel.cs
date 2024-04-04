﻿/*
 * Descripción:
 * Este código define el ViewModel para los pedidos en la aplicación Floristeria Margaritas.
 * Proporciona propiedades para almacenar información sobre cada pedido, como el ID del pedido,
 * la fuente de la imagen, el estado del pedido, la dirección de entrega, la hora de entrega,
 * la fecha del pedido, el total del pedido, el color de fondo del marco, el color del texto,
 * la visibilidad y maneja los eventos relacionados con PropertyChanged.
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
    public class pedidosViewModel : INotifyPropertyChanged
    {
        private string _idPedido;
        public string IdPedido
        {
            get { return _idPedido; }
            set
            {
                if (_idPedido != value)
                {
                    _idPedido = value;
                    OnPropertyChanged(nameof(IdPedido));
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

        private string? _direccionEntrega;
        public string? DireccionEntrega
        {
            get { return _direccionEntrega; }
            set
            {
                if (_direccionEntrega != value)
                {
                    _direccionEntrega = value;
                    OnPropertyChanged(nameof(DireccionEntrega));
                }
            }
        }

        private string? _horaEntrega;
        public string? HoraEntrega
        {
            get { return _horaEntrega; }
            set
            {
                if (_horaEntrega != value)
                {
                    _horaEntrega = value;
                    OnPropertyChanged(nameof(HoraEntrega));
                }
            }
        }

        private string? _fechaPedido;
        public string? FechaPedido
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

        private double _totalPedido;
        public double TotalPedido
        {
            get { return _totalPedido; }
            set
            {
                if (_totalPedido != value)
                {
                    _totalPedido = value;
                    OnPropertyChanged(nameof(TotalPedido));
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

        public ICommand? TappedCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
