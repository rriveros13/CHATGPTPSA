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
    public class Profesion : BaseObject
    {
        private string _codigo;
        private string _descripcion;

        public Profesion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        [Size(5)]
        //[RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue]
        [XafDisplayName("Código")]
        [ToolTip("Código de profesión")]
        public string Codigo
        {
            get => _codigo;
            set => SetPropertyValue(nameof(Codigo), ref _codigo, value);
        }

        [Size(100)]
        //[RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Descripción")]
        [ToolTip("Descripción de la profesión")]
        public string Descripcion
        {
            get => _descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref _descripcion, value);
        }
    }
}