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
    [DefaultProperty(nameof(Numero))]
    public class SucursalTelefono : BaseObject
    {
        public SucursalTelefono(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        Sucursal sucursal;
        GrupoTelefono tipo;
        Prefijo prefijo;
        string numero;

        [Size(7)]
        public string Numero
        {
            get => numero;
            set => SetPropertyValue(nameof(Numero), ref numero, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [DataSourceCriteria("Tipo = '@This.Tipo'")]
        public Prefijo Prefijo
        {
            get => prefijo;
            set => SetPropertyValue(nameof(Prefijo), ref prefijo, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [ImmediatePostData]
        public GrupoTelefono Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        [Association("Sucursal-SucursalTelefono")]
        public Sucursal Sucursal
        {
            get => sucursal;
            set => SetPropertyValue(nameof(Sucursal), ref sucursal, value);
        }

        private XPCollection<AuditDataItemPersistent> auditoria;
        public XPCollection<AuditDataItemPersistent> Auditoria
        {
            get
            {
                if (auditoria == null)
                {
                    auditoria = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return auditoria;
            }
        }
    }
}