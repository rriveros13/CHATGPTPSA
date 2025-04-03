using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class DeviceInfo : BaseObject
    {
        static FieldsClass _Fields;
        private string manufacturer;
        private string model;
        private string name;
        private string platform;
        private string uuid;
        private string version;

        public DeviceInfo(Session session) : base(session)
        {
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

        [Association("DeviceLocation-Locations")]
        public XPCollection<DeviceLocation> Locations => GetCollection<DeviceLocation>(nameof(Locations));

        public string Manufacturer
        {
            get => manufacturer;
            set => SetPropertyValue(nameof(Manufacturer), ref manufacturer, value);
        }

        public string Model
        {
            get => model;
            set => SetPropertyValue(nameof(Model), ref model, value);
        }

        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        public string Platform
        {
            get => platform;
            set => SetPropertyValue(nameof(Platform), ref platform, value);
        }

        public string Uuid
        {
            get => uuid;
            set => SetPropertyValue(nameof(Uuid), ref uuid, value);
        }

        public string Version
        {
            get => version;
            set => SetPropertyValue(nameof(Version), ref version, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Locations
            {
                get
                {
                    return new OperandProperty(GetNestedName("Locations"));
                }
            }

            public OperandProperty Manufacturer
            {
                get
                {
                    return new OperandProperty(GetNestedName("Manufacturer"));
                }
            }

            public OperandProperty Model
            {
                get
                {
                    return new OperandProperty(GetNestedName("Model"));
                }
            }

            public OperandProperty Name
            {
                get
                {
                    return new OperandProperty(GetNestedName("Name"));
                }
            }

            public OperandProperty Platform
            {
                get
                {
                    return new OperandProperty(GetNestedName("Platform"));
                }
            }

            public OperandProperty Uuid
            {
                get
                {
                    return new OperandProperty(GetNestedName("Uuid"));
                }
            }

            public OperandProperty Version
            {
                get
                {
                    return new OperandProperty(GetNestedName("Version"));
                }
            }
        }
    }

    //[DefaultClassOptions]
    public class DeviceLocation : BaseObject, IMapsMarker
    {
        static FieldsClass _Fields;
        private double accuracy;
        private string activityType;
        private DeviceInfo deviceInfo;
        private double latitude;
        private double longitude;
        private double speed;
        private DateTime timeStamp;

        public DeviceLocation(Session session) : base(session)
        {
        }

        public double Accuracy
        {
            get => accuracy;
            set => SetPropertyValue(nameof(Accuracy), ref accuracy, value);
        }

        public string ActivityType
        {
            get => activityType;
            set => SetPropertyValue(nameof(ActivityType), ref activityType, value);
        }

        [Association("DeviceLocation-Locations")]
        public DeviceInfo DeviceInfo
        {
            get => deviceInfo;
            set => SetPropertyValue(nameof(DeviceInfo), ref deviceInfo, value);
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

        public double Latitude
        {
            get => latitude;
            set => SetPropertyValue(nameof(Latitude), ref latitude, value);
        }

        public double Longitude
        {
            get => longitude;
            set => SetPropertyValue(nameof(Longitude), ref longitude, value);
        }

        public double Speed
        {
            get => speed;
            set => SetPropertyValue(nameof(Speed), ref speed, value);
        }

        [ModelDefault("DisplayFormat", "{0: dd/MM/yyyy HH:mm:ss}")]
        public DateTime TimeStamp
        {
            get => timeStamp;
            set => SetPropertyValue(nameof(TimeStamp), ref timeStamp, value);
        }

        public string Title => $"Time: {TimeStamp}<br>Accuracy: {Accuracy}<br>Speed: {Speed}<br>Activity: {ActivityType}";

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Accuracy
            {
                get
                {
                    return new OperandProperty(GetNestedName("Accuracy"));
                }
            }

            public OperandProperty ActivityType
            {
                get
                {
                    return new OperandProperty(GetNestedName("ActivityType"));
                }
            }

            public DeviceInfo.FieldsClass DeviceInfo
            {
                get
                {
                    return new DeviceInfo.FieldsClass(GetNestedName("DeviceInfo"));
                }
            }

            public OperandProperty Latitude
            {
                get
                {
                    return new OperandProperty(GetNestedName("Latitude"));
                }
            }

            public OperandProperty Longitude
            {
                get
                {
                    return new OperandProperty(GetNestedName("Longitude"));
                }
            }

            public OperandProperty Speed
            {
                get
                {
                    return new OperandProperty(GetNestedName("Speed"));
                }
            }

            public OperandProperty TimeStamp
            {
                get
                {
                    return new OperandProperty(GetNestedName("TimeStamp"));
                }
            }

            public OperandProperty Title
            {
                get
                {
                    return new OperandProperty(GetNestedName("Title"));
                }
            }
        }
    }
}
