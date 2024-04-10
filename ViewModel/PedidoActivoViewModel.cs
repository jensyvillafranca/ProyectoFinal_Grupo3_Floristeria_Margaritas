using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.ViewModel
{
    public class PedidoActivoViewModel : INotifyPropertyChanged
    {
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

        private string? _nombreSucursal;
        public string? NombreSucursal
        {
            get { return _nombreSucursal; }
            set
            {
                if (_nombreSucursal != value)
                {
                    _nombreSucursal = value;
                    OnPropertyChanged(nameof(NombreSucursal));
                }
            }
        }

        private int _idEstadoPedido;
        public int IdEstadoPedido
        {
            get { return _idEstadoPedido; }
            set
            {
                if (_idEstadoPedido != value)
                {
                    _idEstadoPedido = value;
                    OnPropertyChanged(nameof(IdEstadoPedido));
                }
            }
        }

        private string? _enlaceFoto;
        public string? EnlaceFoto
        {
            get { return _enlaceFoto; }
            set
            {
                if (_enlaceFoto != value)
                {
                    _enlaceFoto = value;
                    OnPropertyChanged(nameof(EnlaceFoto));
                }
            }
        }

        private string? _nombreCompleto;
        public string? NombreCompleto
        {
            get { return _nombreCompleto; }
            set
            {
                if (_nombreCompleto != value)
                {
                    _nombreCompleto = value;
                    OnPropertyChanged(nameof(NombreCompleto));
                }
            }
        }

        private int _idPedido;
        public int IdPedido
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

        public ICommand? TappedCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
