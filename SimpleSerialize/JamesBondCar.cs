using System;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace SimpleSerialize
{
    public class JamesBondCar : Car
    {
        [JsonInclude] [XmlAttribute] public bool CanFly;
        [JsonInclude] [XmlAttribute] public bool CanSubmerge;
        public override string ToString()
        {
            return $"CanFly: {CanFly} CanSubmerge: {CanSubmerge} {base.ToString()}";
        }
    }
}
