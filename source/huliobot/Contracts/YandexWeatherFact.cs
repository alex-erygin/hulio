namespace huliobot.Contracts
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://weather.yandex.ru/forecast")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://weather.yandex.ru/forecast", IsNullable = false)]
    public partial class fact
    {

        private factStation[] stationField;

        private System.DateTime observation_timeField;

        private System.DateTime uptimeField;

        private factTemperature temperatureField;

        private factWeather_condition weather_conditionField;

        private factImage imageField;

        private factImagev2 imagev2Field;

        private factImagev3 imagev3Field;

        private string weather_typeField;

        private string weather_type_ttField;

        private string weather_type_trField;

        private string weather_type_kzField;

        private string weather_type_uaField;

        private string weather_type_byField;

        private string wind_directionField;

        private decimal wind_speedField;

        private byte humidityField;

        private factPressure pressureField;

        private factMslp_pressure mslp_pressureField;

        private string daytimeField;

        private factSeason seasonField;

        private string ipad_imageField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("station")]
        public factStation[] station
        {
            get
            {
                return this.stationField;
            }
            set
            {
                this.stationField = value;
            }
        }

        /// <remarks/>
        public System.DateTime observation_time
        {
            get
            {
                return this.observation_timeField;
            }
            set
            {
                this.observation_timeField = value;
            }
        }

        /// <remarks/>
        public System.DateTime uptime
        {
            get
            {
                return this.uptimeField;
            }
            set
            {
                this.uptimeField = value;
            }
        }

        /// <remarks/>
        public factTemperature temperature
        {
            get
            {
                return this.temperatureField;
            }
            set
            {
                this.temperatureField = value;
            }
        }

        /// <remarks/>
        public factWeather_condition weather_condition
        {
            get
            {
                return this.weather_conditionField;
            }
            set
            {
                this.weather_conditionField = value;
            }
        }

        /// <remarks/>
        public factImage image
        {
            get
            {
                return this.imageField;
            }
            set
            {
                this.imageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("image-v2")]
        public factImagev2 imagev2
        {
            get
            {
                return this.imagev2Field;
            }
            set
            {
                this.imagev2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("image-v3")]
        public factImagev3 imagev3
        {
            get
            {
                return this.imagev3Field;
            }
            set
            {
                this.imagev3Field = value;
            }
        }

        /// <remarks/>
        public string weather_type
        {
            get
            {
                return this.weather_typeField;
            }
            set
            {
                this.weather_typeField = value;
            }
        }

        /// <remarks/>
        public string weather_type_tt
        {
            get
            {
                return this.weather_type_ttField;
            }
            set
            {
                this.weather_type_ttField = value;
            }
        }

        /// <remarks/>
        public string weather_type_tr
        {
            get
            {
                return this.weather_type_trField;
            }
            set
            {
                this.weather_type_trField = value;
            }
        }

        /// <remarks/>
        public string weather_type_kz
        {
            get
            {
                return this.weather_type_kzField;
            }
            set
            {
                this.weather_type_kzField = value;
            }
        }

        /// <remarks/>
        public string weather_type_ua
        {
            get
            {
                return this.weather_type_uaField;
            }
            set
            {
                this.weather_type_uaField = value;
            }
        }

        /// <remarks/>
        public string weather_type_by
        {
            get
            {
                return this.weather_type_byField;
            }
            set
            {
                this.weather_type_byField = value;
            }
        }

        /// <remarks/>
        public string wind_direction
        {
            get
            {
                return this.wind_directionField;
            }
            set
            {
                this.wind_directionField = value;
            }
        }

        /// <remarks/>
        public decimal wind_speed
        {
            get
            {
                return this.wind_speedField;
            }
            set
            {
                this.wind_speedField = value;
            }
        }

        /// <remarks/>
        public byte humidity
        {
            get
            {
                return this.humidityField;
            }
            set
            {
                this.humidityField = value;
            }
        }

        /// <remarks/>
        public factPressure pressure
        {
            get
            {
                return this.pressureField;
            }
            set
            {
                this.pressureField = value;
            }
        }

        /// <remarks/>
        public factMslp_pressure mslp_pressure
        {
            get
            {
                return this.mslp_pressureField;
            }
            set
            {
                this.mslp_pressureField = value;
            }
        }

        /// <remarks/>
        public string daytime
        {
            get
            {
                return this.daytimeField;
            }
            set
            {
                this.daytimeField = value;
            }
        }

        /// <remarks/>
        public factSeason season
        {
            get
            {
                return this.seasonField;
            }
            set
            {
                this.seasonField = value;
            }
        }

        /// <remarks/>
        public string ipad_image
        {
            get
            {
                return this.ipad_imageField;
            }
            set
            {
                this.ipad_imageField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://weather.yandex.ru/forecast")]
    public partial class factStation
    {

        private string langField;

        private byte distanceField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string lang
        {
            get
            {
                return this.langField;
            }
            set
            {
                this.langField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte distance
        {
            get
            {
                return this.distanceField;
            }
            set
            {
                this.distanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://weather.yandex.ru/forecast")]
    public partial class factTemperature
    {

        private string colorField;

        private string plateField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string color
        {
            get
            {
                return this.colorField;
            }
            set
            {
                this.colorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string plate
        {
            get
            {
                return this.plateField;
            }
            set
            {
                this.plateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public byte Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://weather.yandex.ru/forecast")]
    public partial class factWeather_condition
    {

        private string codeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://weather.yandex.ru/forecast")]
    public partial class factImage
    {

        private byte typeField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public byte Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://weather.yandex.ru/forecast")]
    public partial class factImagev2
    {

        private string colorField;

        private string typeField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string color
        {
            get
            {
                return this.colorField;
            }
            set
            {
                this.colorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://weather.yandex.ru/forecast")]
    public partial class factImagev3
    {

        private string typeField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://weather.yandex.ru/forecast")]
    public partial class factPressure
    {

        private string unitsField;

        private ushort valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string units
        {
            get
            {
                return this.unitsField;
            }
            set
            {
                this.unitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public ushort Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://weather.yandex.ru/forecast")]
    public partial class factMslp_pressure
    {

        private string unitsField;

        private ushort valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string units
        {
            get
            {
                return this.unitsField;
            }
            set
            {
                this.unitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public ushort Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://weather.yandex.ru/forecast")]
    public partial class factSeason
    {

        private string typeField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }


}