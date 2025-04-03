using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Descripcion")]
    public class Planes : BaseObject
    { 
        public Planes(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        ModeloProducto modeloProducto;
        DateTime? fechaFin;
        DateTime fechaInicio;
        decimal importe;
        long entrega;
        long cantidadPagos;
        string descripcion;
        string condicionPago;
        string modelo;

        [Association("ModeloProducto-Planes")]
        public ModeloProducto ModeloProducto
        {
            get => modeloProducto;
            set => SetPropertyValue(nameof(ModeloProducto), ref modeloProducto, value);
        }

        //public string Display
        //{
        //    get
        //    {
        //        return string.Concat(descripcion, " - ENT.: ", entrega.ToString("N0"), " - CUOTA: ", importe.ToString("N0"));
        //    }
        //}

        [Size(200)]
        public string Modelo
        {
            get => modelo;
            set => SetPropertyValue(nameof(Modelo), ref modelo, value);
        }

        [Size(4)]
        public string CondicionPago
        {
            get => condicionPago;
            set => SetPropertyValue(nameof(CondicionPago), ref condicionPago, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        public long CantidadPagos
        {
            get => cantidadPagos;
            set => SetPropertyValue(nameof(CantidadPagos), ref cantidadPagos, value);
        }

        public long Entrega
        {
            get => entrega;
            set => SetPropertyValue(nameof(Entrega), ref entrega, value);
        }

        public decimal Importe
        {
            get => importe;
            set => SetPropertyValue(nameof(Importe), ref importe, value);
        }

        public DateTime FechaInicio
        {
            get => fechaInicio;
            set => SetPropertyValue(nameof(FechaInicio), ref fechaInicio, value);
        }
        
        public DateTime? FechaFin
        {
            get => fechaFin;
            set => SetPropertyValue(nameof(FechaFin), ref fechaFin, value);
        }
    }
}