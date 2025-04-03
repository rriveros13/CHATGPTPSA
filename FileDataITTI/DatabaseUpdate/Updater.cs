using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using System;

namespace ITTI.DatabaseUpdate
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppUpdatingModuleUpdatertopic.aspx
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion)
        {
        }

        public override void UpdateDatabaseAfterUpdateSchema() => base.UpdateDatabaseAfterUpdateSchema();

        public override void UpdateDatabaseBeforeUpdateSchema() => base.UpdateDatabaseBeforeUpdateSchema();
    }
}
