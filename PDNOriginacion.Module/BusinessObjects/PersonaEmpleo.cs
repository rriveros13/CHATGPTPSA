using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using PDNOriginacion.Module.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Persona")]
    public class PersonaEmpleo : BaseObject
    {
        int numeroEmpleo;
        int diaAntiguedad;
        int mesAntiguedad;
        int añoAntiguedad;
        static FieldsClass _Fields;
        private bool _actual;
        private int _antiguedad;
        private string _cargo;
        private Ciudad _ciudad;
        private string _direccion;
        private string _empresa;
        private Persona _persona;
        private double _salario;
        private string _telefono;
        private DateTime _fechaIngreso;
        private DateTime _fechaSalida;

        public PersonaEmpleo(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            _actual = true;
            numeroEmpleo = 0;
        }

        public int NumeroEmpleo
        {
            get => numeroEmpleo;
            set => SetPropertyValue(nameof(NumeroEmpleo), ref numeroEmpleo, value);
        }

        public bool Actual
        {
            get => _actual;
            //set => SetPropertyValue(nameof(Actual), ref _actual, value);
            set
            {
                bool cambio = SetPropertyValue(nameof(Actual), ref _actual, value);
                if (IsLoading || IsSaving || !cambio) return;
                this.Persona.Salario = SumarizarSalarios();
                //this.DiaAntiguedad = Varios.CalcularDiasDiferencia(_fechaIngreso, value);
                OnChanged(nameof(Persona));
            }
        }

        public int Antiguedad
        {
            get => _antiguedad;
            set => SetPropertyValue(nameof(Antiguedad), ref _antiguedad, value);
        }

        public int AñoAntiguedad
        {
            get => añoAntiguedad;
            set => SetPropertyValue(nameof(AñoAntiguedad), ref añoAntiguedad, value);
        }

        public int MesAntiguedad
        {
            get => mesAntiguedad;
            set => SetPropertyValue(nameof(MesAntiguedad), ref mesAntiguedad, value);
        }
        
        public int DiaAntiguedad
        {
            get => diaAntiguedad;
            set => SetPropertyValue(nameof(DiaAntiguedad), ref diaAntiguedad, value);
        }

        public Ciudad Ciudad
        {
            get => _ciudad;
            set => SetPropertyValue(nameof(Ciudad), ref _ciudad, value);
        }

        public string Cargo
        {
            get => _cargo;
            set => SetPropertyValue(nameof(Cargo), ref _cargo, value);
        }


        public string Direccion
        {
            get => _direccion;
            set => SetPropertyValue(nameof(Direccion), ref _direccion, value);
        }

        public string Empresa
        {
            get => _empresa;
            set => SetPropertyValue(nameof(Empresa), ref _empresa, value);
        }

        [ImmediatePostData]
        public DateTime FechaIngreso
        {
            get => _fechaIngreso;
            set
            {
                if (value != null) value = value.Date;
                bool cambio = SetPropertyValue(nameof(FechaIngreso), ref _fechaIngreso, value);
                if (IsLoading || IsSaving || !cambio) return;
                this.AñoAntiguedad = Varios.CalcularAñosDiferencia(value, DateTime.Today);
                OnChanged(nameof(AñoAntiguedad));
                this.MesAntiguedad = Varios.CalcularMesesDiferencia(value, DateTime.Today);
                OnChanged(nameof(MesAntiguedad));
                this.DiaAntiguedad = Varios.CalcularDiasDiferencia(value, DateTime.Today);
                OnChanged(nameof(DiaAntiguedad));
            }
        }

        [ImmediatePostData]
        public DateTime FechaSalida
        {
            get => _fechaSalida;
            set
            {
                if (value != null) value = value.Date;
                bool cambio = SetPropertyValue(nameof(FechaSalida), ref _fechaSalida, value);
                if (IsLoading || IsSaving || !cambio) return;
                this.AñoAntiguedad = Varios.CalcularAñosDiferencia(_fechaIngreso, value);
                OnChanged(nameof(AñoAntiguedad));
                this.MesAntiguedad = Varios.CalcularMesesDiferencia(_fechaIngreso, value);
                OnChanged(nameof(MesAntiguedad));
                this.DiaAntiguedad = Varios.CalcularDiasDiferencia(_fechaIngreso, value);
                OnChanged(nameof(DiaAntiguedad));
            }
        }

        private XPCollection<AuditDataItemPersistent> auditoria;
        public XPCollection<AuditDataItemPersistent> Auditoria
        {
            get
            {
                if (auditoria == null)
                {
                    auditoria = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return auditoria;
            }
        }

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

        [Association("Persona-PersonaEmpleo")]
        public Persona Persona
        {
            get => _persona;
            set => SetPropertyValue(nameof(Persona), ref _persona, value);
        }

        public double Salario
        {
            get => _salario;
            set => SetPropertyValue(nameof(Salario), ref _salario, value);
/*            set
            {
                bool cambio = SetPropertyValue(nameof(Salario), ref _salario, value);
                if (IsLoading || IsSaving || !cambio) return;
                this.Persona.Salario = SumarizarSalarios();
                //this.DiaAntiguedad = Varios.CalcularDiasDiferencia(_fechaIngreso, value);
                OnChanged(nameof(Persona));
            }*/
        }

        public string Telefono
        {
            get => _telefono;
            set => SetPropertyValue(nameof(Telefono), ref _telefono, value);
        }

        private decimal SumarizarSalarios()
        {
            if (Persona.Salario > 0)
            {
                Persona.Salario = 0;
            }

            double totalSalarios = Persona.Empleos.Where(p => p.Actual).Sum(e => e.Salario);
            decimal total = (decimal)totalSalarios;
            return total;
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Actual
            {
                get
                {
                    return new OperandProperty(GetNestedName("Actual"));
                }
            }

            public OperandProperty Antiguedad
            {
                get
                {
                    return new OperandProperty(GetNestedName("Antiguedad"));
                }
            }

            public OperandProperty Cargo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Cargo"));
                }
            }

            public OperandProperty Direccion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Direccion"));
                }
            }

            public OperandProperty Empresa
            {
                get
                {
                    return new OperandProperty(GetNestedName("Empresa"));
                }
            }

            public Persona.FieldsClass Persona
            {
                get
                {
                    return new Persona.FieldsClass(GetNestedName("Persona"));
                }
            }

            public OperandProperty Salario
            {
                get
                {
                    return new OperandProperty(GetNestedName("Salario"));
                }
            }

            public OperandProperty Telefono
            {
                get
                {
                    return new OperandProperty(GetNestedName("Telefono"));
                }
            }
        }
    }
}
