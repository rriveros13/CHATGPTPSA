using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;

namespace PDNOriginacion.Module.BusinessObjects
{
    public class LPassword : BaseObject
    {
        static FieldsClass _Fields;
        private string lastPaswword;
        private DateTime pwChangeDate;
        private Usuario usuario;

        public LPassword(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
        public static bool ComparePassword(string storedPassword, string password) => new PasswordCryptographer().AreEqual(storedPassword,
                                                                                                                           password);

        public new static FieldsClass Fields
        {
            get
            {
                if(ReferenceEquals(_Fields, null))
                {
                    _Fields = new FieldsClass();
                }

                return _Fields;
            }
        }

        public string LastPassword
        {
            get => lastPaswword;
            set => SetPropertyValue(nameof(LastPassword), ref lastPaswword, value);
        }

        public DateTime PWChangeDate
        {
            get => pwChangeDate;
            set => SetPropertyValue(nameof(PWChangeDate), ref pwChangeDate, value);
        }

        [Association("Usuario-LPasswords")]
        public Usuario Usuario
        {
            get => usuario;
            set => SetPropertyValue(nameof(Usuario), ref usuario, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty LastPassword
            {
                get
                {
                    return new OperandProperty(GetNestedName("LastPassword"));
                }
            }

            public OperandProperty PWChangeDate
            {
                get
                {
                    return new OperandProperty(GetNestedName("PWChangeDate"));
                }
            }

            public Usuario.FieldsClass Usuario
            {
                get
                {
                    return new Usuario.FieldsClass(GetNestedName("Usuario"));
                }
            }
        }
    }
}