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
    [DefaultProperty("Modelo")]
    public class ModeloProducto : BaseObject
    { 
        public ModeloProducto(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        ModeloProductoEntrega entrega;
        string modelo;
        byte[] imagen;


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Modelo
        {
            get => modelo;
            set => SetPropertyValue(nameof(Modelo), ref modelo, value);
        }

        [VisibleInListView(true)]
        [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit, DetailViewImageEditorMode = ImageEditorMode.PictureEdit, ListViewImageEditorCustomHeight = 40)]
        public byte[] Imagen
        {
            get => imagen;
            set => SetPropertyValue(nameof(Imagen), ref imagen, value);
        }

        [PersistentAlias("Iif(IsNull(ToDecimal(Planes[CondicionPago='CDO'].Max(Importe))), ToDecimal(0), ToDecimal(Planes[CondicionPago='CDO'].Max(Importe)))")]
        public decimal PrecioContado => (decimal)EvaluateAlias(nameof(PrecioContado));

        [Association("ModeloProducto-Planes"), DevExpress.Xpo.Aggregated]
        public XPCollection<Planes> Planes => GetCollection<Planes>(nameof(Planes));

        
        [VisibleInDetailView(false)]
        [Association("ModeloProducto-Entregas"), DevExpress.Xpo.Aggregated]
        public XPCollection<ModeloProductoEntrega> Entregas
        {
            get
            {
                return GetCollection<ModeloProductoEntrega>(nameof(Entregas));
            }
        }

        [VisibleInDetailView(false)]
        [Association("ModeloProducto-Plazos"), DevExpress.Xpo.Aggregated]
        public XPCollection<ModeloProductoPlazo> Plazos
        {
            get
            {
                return GetCollection<ModeloProductoPlazo>(nameof(Plazos));
            }
        }


    }

    [XafDefaultProperty("EntregaDisplay")]
    public class ModeloProductoEntrega : BaseObject
    { 
        public ModeloProductoEntrega(Session session) : base(session)  {}
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        long entrega;
        ModeloProducto modeloProducto;

        [Association("ModeloProducto-Entregas")]
        public ModeloProducto ModeloProducto
        {
            get => modeloProducto;
            set => SetPropertyValue(nameof(ModeloProducto), ref modeloProducto, value);
        }

        public string EntregaDisplay
        {
            get
            {
                return entrega.ToString("N0").PadLeft(10);
            }
        }
        
        public long Entrega
        {
            get => entrega;
            set => SetPropertyValue(nameof(Entrega), ref entrega, value);
        }


    }

    [XafDefaultProperty("PlazoDisplay")]
    public class ModeloProductoPlazo : BaseObject
    { 
        public ModeloProductoPlazo(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        long plazo;
        ModeloProducto modeloProducto;

        [Association("ModeloProducto-Plazos")]
        public ModeloProducto ModeloProducto
        {
            get => modeloProducto;
            set => SetPropertyValue(nameof(ModeloProducto), ref modeloProducto, value);
        }

        public string PlazoDisplay
        {
            get
            {
                return plazo.ToString("N0").PadLeft(2, '0');
            }
        }
        
        public long Plazo
        {
            get => plazo;
            set => SetPropertyValue(nameof(Plazo), ref plazo, value);
        }

    }
}