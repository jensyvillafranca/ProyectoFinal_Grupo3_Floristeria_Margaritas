using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.RestApi{
    public class RestApiData{
        public const string url_bd = "https://phpclusters-164276-0.cloudclusters.net/";
        public const string select_pedidos = url_bd+"select_pedidos_entrantes.php";
        public const string select_ingresos_repartidor = url_bd+"select_ingresos_repartidor.php";
        public const string select_total_repartidor = url_bd+"select_total_repartidor.php";
        public const string select_search_ingresos_pedidos = url_bd+"select_search_ingresos_pedidos.php";
        public const string select_detalle_pedido = url_bd+"select_detalle_pedido.php";
        public const string select_direccion = url_bd+"select_direccion.php";
        public const string select_departments = url_bd+"select_departments.php";
        public const string select_repartidores = url_bd+"select_repartidores.php";
        public const string select_city = url_bd+"select_city.php";
        public const string select_stock = url_bd+"select_stock.php";
        public const string select_status_pedidos = url_bd+"select_status_pedidos.php";
        public const string buscar_sucursal = url_bd+"/buscarSucursalPedido.php";
        public const string estado_pedido = url_bd+"/validarCampoEstado.php";
        public const string update_pedido = url_bd+"/update_pedido.php";
        public const string update_pedido2 = url_bd + "/update_pedido2.php";
        public const string update_pedido3 = url_bd + "/update_pedido3.php";
    }
}
