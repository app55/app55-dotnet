using System;
using System.Collections.Generic;
using System.Text;

namespace App55 {
    internal interface IConverter {
        bool CanConvert(Type t);
        String Convert(object o);
        object Convert(String s, Type t);
    }

    internal class Converter : IConverter {
        private IList<IConverter> converters;

        private Converter() {
            converters = new List<IConverter>();
            Register(new PrimitiveConverter());
        }

        private static Converter instance;
        public static Converter Instance {
            get {
                if(instance == null) instance = new Converter();
                return instance;
            }
        }

        public void Register(IConverter converter) {
            if(converter == this) throw new ArgumentException();
            converters.Add(converter);
        }

        public bool CanConvert(Type t) {
            foreach(IConverter converter in converters)
                if(converter.CanConvert(t)) return true;

            return false;
        }

        public string Convert(object o) {
            if(o == null) return null;

            foreach(IConverter converter in converters)
                if(converter.CanConvert(o.GetType())) return converter.Convert(o);

            return null;
        }

        public object Convert(string s, Type t) {
            foreach(IConverter converter in converters)
                if(converter.CanConvert(t)) return converter.Convert(s, t);

            return null;
        }
    }

    internal class PrimitiveConverter : IConverter {
        public bool CanConvert(Type t) {
            if(t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>)) {
                return t.GetGenericArguments()[0].IsPrimitive;
            }

            return t.IsPrimitive || t == typeof(string);
        }

        public string Convert(object o) {
            if(!CanConvert(o.GetType())) return null;
            if(o == null) return null;

            if(o.GetType() == typeof(bool)) return o.ToString().ToLower();

            return o.ToString();
        }

        public object Convert(string s, Type t) {
            if(s == null) return null;

            if(t == typeof(bool) || t == typeof(bool?)) return Boolean.Parse(s);
            if(t == typeof(byte) || t == typeof(byte?)) return Byte.Parse(s);
            if(t == typeof(sbyte) || t == typeof(sbyte?)) return SByte.Parse(s);
            if(t == typeof(short) || t == typeof(short?)) return Int16.Parse(s);
            if(t == typeof(ushort) || t == typeof(ushort?)) return UInt16.Parse(s);
            if(t == typeof(int) || t == typeof(int?)) return Int32.Parse(s);
            if(t == typeof(uint) || t == typeof(uint?)) return UInt32.Parse(s);
            if(t == typeof(long) || t == typeof(long?)) return Int64.Parse(s);
            if(t == typeof(ulong) || t == typeof(ulong?)) return UInt64.Parse(s);
            if(t == typeof(char) || t == typeof(char?)) return Char.Parse(s);
            if(t == typeof(float) || t == typeof(float?)) return Single.Parse(s);
            if(t == typeof(double) || t == typeof(double?)) return Double.Parse(s);
            if(t == typeof(string)) return s;

            return null;
        }
    }
}
