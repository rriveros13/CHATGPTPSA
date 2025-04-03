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
    [DefaultProperty("Tipo")]
    public class TipoIngreso : BaseObject
    {
        public TipoIngreso(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        string descripcion;
        string tipo;

        [Size(60)]
        //[RuleRequiredField(DefaultContexts.Save)]
        public string Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }
        
        [Size(200)]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }
    }
}