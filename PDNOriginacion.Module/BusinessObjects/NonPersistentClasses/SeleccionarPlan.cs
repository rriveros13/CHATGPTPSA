using System;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace PDNOriginacion.Module.BusinessObjects.NonPersistentClasses
{
    [NonPersistent]
    public class SeleccionarPlan : BaseObject
    {
        public SeleccionarPlan() : base()
        {
        }

        public SeleccionarPlan(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        bool planInicial;
        string documento;
        ModeloProductoPlazo plazo;
        ModeloProductoEntrega entrega;
        Planes plan;
        ModeloProducto modelo;

        [ImmediatePostData(true)]
        public ModeloProducto Modelo
        {
            get => modelo;
            set
            {
                bool cambio = SetPropertyValue(nameof(Modelo), ref modelo, value);
                if (cambio && !IsSaving && !IsLoading)
                {
                    plan = null;
                    OnChanged("Plan");
                }
            }
        }

        [ImmediatePostData(true)]
        [ModelDefault("LookupEditorMode", "AllItems")]
        [DataSourceProperty("Modelo.Entregas")]
        public ModeloProductoEntrega Entrega
        {
            get => entrega;
            set
            {
                bool cambio = SetPropertyValue(nameof(Entrega), ref entrega, value);
                if (cambio && !IsLoading && !IsSaving)
                {
                    if (entrega != null && plazo != null && modelo != null)
                    {
                        plan = Session.FindObject<Planes>(CriteriaOperator.Parse("ModeloProducto.Oid=? and Entrega=? and CantidadPagos=?", modelo.Oid, entrega.Entrega, plazo.Plazo));
                        OnChanged("Plan");
                    }
                }
            }
        }

        [ImmediatePostData(true)]
        [ModelDefault("LookupEditorMode", "AllItems")]
        [DataSourceProperty("Modelo.Plazos")]
        public ModeloProductoPlazo Plazo
        {
            get => plazo;
            set
            {
                bool cambio = SetPropertyValue(nameof(Plazo), ref plazo, value);
                if (cambio && !IsLoading && !IsSaving)
                {
                    if (entrega != null && plazo != null && modelo != null)
                    {
                        plan = Session.FindObject<Planes>(CriteriaOperator.Parse("ModeloProducto.Oid=? and Entrega=? and CantidadPagos=?", Modelo.Oid, Entrega.Entrega, Plazo.Plazo));
                        OnChanged("Plan");
                    }
                }
            }
        }

        [ModelDefault("AllowEdit", "false")]
        public Planes Plan
        {
            get => plan;
            set => SetPropertyValue(nameof(Plan), ref plan, value);
        }

        [VisibleInDetailView(false)]
        public bool PlanInicial
        {
            get => planInicial;
            set => SetPropertyValue(nameof(PlanInicial), ref planInicial, value);
        }


        public decimal? Importe
        {
            get
            {
                if (plan != null)
                {
                    return plan.Importe;
                }
                else
                {
                    return null;
                }

            }
        }
        
        [Appearance("SeleccionarPlanDoc", TargetItems = nameof(Documento), AppearanceItemType = nameof(ViewItem), Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "!PlanInicial")]
        [Size(20)]
        public string Documento
        {
            get => documento;
            set => SetPropertyValue(nameof(Documento), ref documento, value);
        }

    }

}