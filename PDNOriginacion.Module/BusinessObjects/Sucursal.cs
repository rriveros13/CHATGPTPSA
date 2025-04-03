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
    [DefaultProperty(nameof(Nombre))]
    public class Sucursal : BaseObject
    {
        public Sucursal(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        Direccion direccion;
        string descripcion;
        string nombre;
        string codigo;

        [Size(5)]
        //[RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue]
        [XafDisplayName("Código")]
        [ToolTip("Código de Sucursal")]
        public string Codigo
        {
            get => codigo;
            set => SetPropertyValue(nameof(Codigo), ref codigo, value);
        }

        [Size(100)]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [Size(250)]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        public Direccion Direccion
        {
            get => direccion;
            set => SetPropertyValue(nameof(Direccion), ref direccion, value);
        }
        [Association("Sucursal-SucursalTelefono")]
        public XPCollection<SucursalTelefono> Telefonos
        {
            get
            {
                return GetCollection<SucursalTelefono>(nameof(Telefonos));
            }
        }

        [Association("Sucursal-Usuario", typeof(Usuario))]
        public XPCollection Usuarios => GetCollection(nameof(Usuarios));
    }
}