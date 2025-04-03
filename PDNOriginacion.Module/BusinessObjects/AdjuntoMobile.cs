using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultProperty("Descripcion")]
    [ImageName("BO_Note")]
    public class AdjuntoMobile : BaseObject
    {
        static FieldsClass _Fields;
        private bool _adjuntado;
        private byte[] _archivo;
        private string _descripcion;
        private DateTime _fecha = DateTime.Now;
        private NuevaConsulta _nuevaconsulta;

        public AdjuntoMobile(Session session) : base(session)
        {
        }

        protected override void OnSaving()
        {
            //    try
            //    {
            //        if (ConfigurationManager.AppSettings["Prediction_validar"].Equals("S"))
            //        {
            //            PredictionEndpoint endpoint = new PredictionEndpoint() { ApiKey = ConfigurationManager.AppSettings["Prediction_apiKey"] };
            //            Guid projectID = Guid.Parse(ConfigurationManager.AppSettings["Prediction_projectID"]);

            //            System.IO.Stream stream = new MemoryStream();
            //            this.Archivo.SaveToStream(stream);
            //            stream.Position = 0;

            //            var result = endpoint.PredictImage(projectID, stream);

            //            foreach (ImageTagPredictionModel c in result.Predictions)
            //            {
            //                if (this.Descripcion == c.Tag)
            //                {
            //                    if (c.Probability * 100 > 98)
            //                    {
            //                        this.Adjuntado = this.Archivo != null;
            //                        base.OnSaving();
            //                    }
            //                    else
            //                        throw new Exception("No se cumplieron los requerimientos mínimos de la imagen.");
            //                }
            //            }
            //        }
            //        else
            //        {
            //            this.Adjuntado = this.Archivo != null;
            //            base.OnSaving();
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        throw e;
            //    }
        }

        public override void AfterConstruction() => base.AfterConstruction();

        [Appearance("Adjunto-Adjuntado", AppearanceItemType = nameof(ViewItem), TargetItems = nameof(Adjuntado), Context = "DetailView", Visibility = ViewItemVisibility.Hide)]
        public bool Adjuntado
        {
            get => _adjuntado;
            set => SetPropertyValue(nameof(Adjuntado), ref _adjuntado, value);
        }

        public byte[] Archivo
        {
            get => _archivo;
            set => SetPropertyValue(nameof(Archivo), ref _archivo, value);
        }

        [Size(250)]
        public string Descripcion
        {
            get => _descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref _descripcion, value);
        }

        [Appearance("Adjunto-Fecha", AppearanceItemType = nameof(ViewItem), TargetItems = nameof(Fecha), Context = "DetailView", Enabled = false)]
        public DateTime Fecha
        {
            get => _fecha;
            set => SetPropertyValue(nameof(Fecha), ref _fecha, value);
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

        [Association("NuevaConsulta-AdjuntosMobile")]
        public NuevaConsulta NuevaConsulta
        {
            get => _nuevaconsulta;
            set => SetPropertyValue(nameof(Consulta), ref _nuevaconsulta, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Adjuntado
            {
                get
                {
                    return new OperandProperty(GetNestedName("Adjuntado"));
                }
            }

            public OperandProperty Archivo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Archivo"));
                }
            }

            public OperandProperty Descripcion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Descripcion"));
                }
            }

            public OperandProperty Fecha
            {
                get
                {
                    return new OperandProperty(GetNestedName("Fecha"));
                }
            }

            public NuevaConsulta.FieldsClass NuevaConsulta
            {
                get
                {
                    return new NuevaConsulta.FieldsClass(GetNestedName("NuevaConsulta"));
                }
            }
        }
    }
}
