using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal_Grupo3_Floristeria_Margaritas.RestApi{
    public class TableJoinQueries{
        //--------------INT----------------
        public int ped_idestadopedido{
            get; set;
        }

        public int pro_stock
        {
            get; set;
        }

        public int ciu_iddepartamento
        {
            get; set;
        }

        public int dep_iddepartamento
        {
            get; set;
        }

        public int ped_idpedido
        {
            get; set;
        }

        public int ped_fk_idrepartidor{
            get; set;
        }

        public int ped_count
        {
            get; set;
        }

        //---------------STRING---------------
        public string? dir_direccion{
            get; set;
        }

        public string? ciu_ciudad
        {
            get; set;
        }

        public string? pro_nombreproducto{
            get; set;
        }

        public string? rep_nombre
        {
            get; set;
        }

        public string? suc_nombresucursal{
            get; set;
        }

        public string? est_estado{
            get; set;
        }

        public string? ped_enlacefoto{
            get; set;
        }

        public string? cli_nombrecompleto{
            get; set;
        }

        public string? enlace_status{
            get; set;
        }

        public string? ped_fechapedido{
            get; set;
        }

        public string? ped_comisionrepartidor{
            get; set;
        }

        public string? ped_total{
            get; set;
        }

        public string? ped_propina{
            get; set;
        }

        public string? ped_sum_totales{
            get; set;
        }

        public string? search_data{
            get; set;
        }

        public string? color{
            get; set;
        }

        public string? ped_fechaestimadaentrega{
            get; set;
        }

        public string? cli_telefono{
            get; set;
        }

        public string? ped_notapedido{
            get; set;
        }

        public string? pro_enlacefoto{
            get; set;
        }

        public string? dep_departamento
        {
            get; set;
        }

        public string? star_date
        {
            get; set;
        }

        public string? end_date
        {
            get; set;
        }

        public bool success{
            get; set;
        }
        public string message{
            get; set;
        }
        public string direccion{
            get; set;
        }



    }
}
