using System;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace SimpleSerialize
{
    public class Person
    {
        // Открытое поле
        [JsonInclude] public bool IsAlive = true;
        // Закрытое поле
        private int PersonAge = 21;
        // Открытое свойство/закрытые данные
        private string _fName = string.Empty;
        public string FirstName
        {
            get => _fName;
            set => _fName = value;
        }
        public override string ToString()
        {
            return $"IsAlive: {IsAlive} FirstName: {FirstName} Age: {PersonAge}";
        }
    }
}
