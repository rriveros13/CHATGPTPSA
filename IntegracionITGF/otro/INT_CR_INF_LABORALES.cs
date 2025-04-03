using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace IntegracionITGF.ITGFDataModel
{

    public partial class INT_CR_INF_LABORALES
    {
        public INT_CR_INF_LABORALES(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
