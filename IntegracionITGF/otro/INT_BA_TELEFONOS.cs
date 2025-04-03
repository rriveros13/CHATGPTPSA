using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace IntegracionITGF.ITGFDataModel
{

    public partial class INT_BA_TELEFONOS
    {
        public INT_BA_TELEFONOS(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
