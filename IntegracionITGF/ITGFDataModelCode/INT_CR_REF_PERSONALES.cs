using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace IntegracionITGF.ITGFDataModel
{

    public partial class INT_CR_REF_PERSONALES
    {
        public INT_CR_REF_PERSONALES(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
