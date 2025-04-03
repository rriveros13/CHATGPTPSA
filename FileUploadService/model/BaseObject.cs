using System;
using System.ComponentModel;
using DevExpress.Xpo;

namespace FileUploadService.model
{
    [NonPersistent]
    public abstract class BaseObject : XPCustomObject
    {
        protected BaseObject(Session session) : base(session) { }
        [Persistent("Oid"), Key(true), Browsable(false), MemberDesignTimeVisibility(false)]
        private Guid _Oid = Guid.Empty;
        [PersistentAlias("_Oid"), Browsable(false)]
        public Guid Oid { get { return _Oid; } }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (!(Session is NestedUnitOfWork) && Session.IsNewObject(this))
                _Oid = XpoDefault.NewGuid();

        }
    }
}

