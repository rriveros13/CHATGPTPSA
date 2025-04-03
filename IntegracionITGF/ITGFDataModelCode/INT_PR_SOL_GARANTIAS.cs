using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
namespace IntegracionITGF.ITGFDataModel
{

    public partial class INT_PR_SOL_GARANTIAS
    {
        public INT_PR_SOL_GARANTIAS(Session session) : base(session) { }
        public override void AfterConstruction() { base.AfterConstruction(); }
    }

}
