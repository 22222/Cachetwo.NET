using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cachetwo.Distributed
{
    /// <summary>
    /// Provides methods for converting between .NET types and binary data.
    /// </summary>
    public static class BinaryConvert
    {
        /// <summary>
        /// Serializes the specified object to binary data.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <returns>A binary data representation of the object.</returns>
        /// <exception cref="FormatException">if serialization fails</exception>
        public static byte[] Serialize(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var valueType = value.GetType();
            byte[] result;
            if (valueType == typeof(byte[]))
            {
                result = (byte[])value;
            }
            else if (valueType == typeof(string))
            {
                result = Encoding.UTF8.GetBytes((string)value);
            }
            else if (valueType.IsValueType)
            {
                result = SerializeValueType(value, valueType);
            }
            else
            {
                result = SerializeObject(value, valueType);
            }
            return result;
        }

        private static byte[] SerializeValueType(object value, Type valueType)
        {
            var unwrappedValueType = Nullable.GetUnderlyingType(valueType) ?? valueType;

            byte[] result;
            if (unwrappedValueType.IsPrimitive)
            {
                result = SerializePrimitive(value, valueType);
            }
            else if (unwrappedValueType.IsEnum)
            {
                var enumUnderlyingType = Enum.GetUnderlyingType(unwrappedValueType);
                result = SerializePrimitive(value, enumUnderlyingType);
            }
            else if (unwrappedValueType == typeof(decimal))
            {
                var valueStr = ((decimal)value).ToString(CultureInfo.InvariantCulture);
                result = Encoding.UTF8.GetBytes(valueStr);
            }
            else if (unwrappedValueType == typeof(Guid))
            {
                result = ((Guid)value).ToByteArray();
            }
            else if (unwrappedValueType == typeof(DateTime))
            {
                var valueDt = (DateTime)value;
                result = BitConverter.GetBytes(valueDt.ToBinary());
            }
            else if (unwrappedValueType == typeof(DateTimeOffset))
            {
                var valueStr = ((DateTimeOffset)value).ToString("o", CultureInfo.InvariantCulture);
                result = Encoding.UTF8.GetBytes(valueStr);
            }
            else if (unwrappedValueType == typeof(TimeSpan))
            {
                var valueTicks = ((TimeSpan)value).Ticks;
                result = SerializePrimitive(valueTicks, typeof(long));
            }
            else
            {
                result = SerializeObject(value, valueType);
            }
            return result;
        }

        private static byte[] SerializePrimitive(object value, Type valueType)
        {
            var unwrappedValueType = Nullable.GetUnderlyingType(valueType) ?? valueType;

            byte[] result;
            if (unwrappedValueType == typeof(bool))
            {
                result = BitConverter.GetBytes((bool)value);
            }
            else if (unwrappedValueType == typeof(char))
            {
                result = BitConverter.GetBytes((char)value);
            }
            else if (unwrappedValueType == typeof(byte))
            {
                result = new[] { (byte)value };
            }
            else if (unwrappedValueType == typeof(sbyte))
            {
                result = new[] { unchecked((byte)(sbyte)value) };
            }
            else if (unwrappedValueType == typeof(short))
            {
                result = BitConverter.GetBytes((short)value);
            }
            else if (unwrappedValueType == typeof(ushort))
            {
                result = BitConverter.GetBytes((ushort)value);
            }
            else if (unwrappedValueType == typeof(int))
            {
                result = BitConverter.GetBytes((int)value);
            }
            else if (unwrappedValueType == typeof(uint))
            {
                result = BitConverter.GetBytes((uint)value);
            }
            else if (unwrappedValueType == typeof(long))
            {
                result = BitConverter.GetBytes((long)value);
            }
            else if (unwrappedValueType == typeof(ulong))
            {
                result = BitConverter.GetBytes((ulong)value);
            }
            else if (unwrappedValueType == typeof(float))
            {
                result = BitConverter.GetBytes((float)value);
            }
            else if (unwrappedValueType == typeof(double))
            {
                result = BitConverter.GetBytes((double)value);
            }
            else
            {
                throw new FormatException($"Unsupported primitive type: {unwrappedValueType.FullName}");
            }
            return result;
        }

        private static byte[] SerializeObject(object value, Type valueType)
        {
            // Special handling for Task to make it more obvious when an await is missing.
            if (value is System.Threading.Tasks.Task)
            {
                throw new FormatException($"Failed to serialize value of type {valueType.FullName}");
            }

            try
            {
                return RoundTripBsonConvert.SerializeObject(value);
            }
            catch (JsonWriterException ex)
            {
                throw new FormatException($"Failed to write value of type {valueType.FullName}", ex);
            }
            catch (JsonSerializationException ex)
            {
                throw new FormatException($"Failed to serialize value of type {valueType.FullName}", ex);
            }
        }

        /// <summary>
        /// Deserializes the binary data to the specified .NET type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="value">The binary data to deserialize.</param>
        /// <returns>The deserialized object from the binary data.</returns>
        /// <exception cref="FormatException">if deserialization fails</exception>
        public static T Deserialize<T>(byte[] value)
        {
            var valueType = typeof(T);
            var result = Deserialize(value, valueType);
            return (T)result;
        }

        /// <summary>
        /// Deserializes the binary data to the specified .NET type.
        /// </summary>
        /// <param name="value">The binary data to deserialize.</param>
        /// <param name="valueType">The <see cref="Type"/> of object being deserialized.</param>
        /// <returns>The deserialized object from the binary data.</returns>
        /// <exception cref="FormatException">if deserialization fails</exception>
        public static object Deserialize(byte[] value, Type valueType)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            object result;
            if (valueType == typeof(byte[]))
            {
                result = value;
            }
            else if (valueType == typeof(string))
            {
                result = Encoding.UTF8.GetString(value);
            }
            else if (valueType.IsValueType)
            {
                result = DeserializeValueType(value, valueType);
            }
            else
            {
                result = DeserializeObject(value, valueType);
            }
            return result;
        }

        private static object DeserializeValueType(byte[] value, Type valueType)
        {
            var unwrappedValueType = Nullable.GetUnderlyingType(valueType) ?? valueType;

            object result;
            if (unwrappedValueType.IsPrimitive)
            {
                result = DeserializePrimitive(value, valueType);
            }
            else if (unwrappedValueType.IsEnum)
            {
                var enumUnderlyingType = Enum.GetUnderlyingType(unwrappedValueType);
                var underlyingResult = DeserializePrimitive(value, enumUnderlyingType);
                result = Enum.ToObject(unwrappedValueType, underlyingResult);
            }
            else if (unwrappedValueType == typeof(decimal))
            {
                var valueStr = Encoding.UTF8.GetString(value);
                result = decimal.Parse(valueStr, CultureInfo.InvariantCulture);
            }
            else if (unwrappedValueType == typeof(Guid))
            {
                result = new Guid(value);
            }
            else if (unwrappedValueType == typeof(DateTime))
            {
                result = DateTime.FromBinary(BitConverter.ToInt64(value, 0));
            }
            else if (unwrappedValueType == typeof(DateTimeOffset))
            {
                var valueStr = Encoding.UTF8.GetString(value);
                result = DateTimeOffset.Parse(valueStr, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            }
            else if (unwrappedValueType == typeof(TimeSpan))
            {
                var valueTicks = (long)DeserializePrimitive(value, typeof(long));
                result = TimeSpan.FromTicks(valueTicks);
            }
            else
            {
                result = DeserializeObject(value, valueType);
            }
            return result;
        }

        private static object DeserializePrimitive(byte[] value, Type valueType)
        {
            var unwrappedValueType = Nullable.GetUnderlyingType(valueType) ?? valueType;

            object result;
            if (unwrappedValueType == typeof(bool))
            {
                result = BitConverter.ToBoolean(value, 0);
            }
            else if (unwrappedValueType == typeof(char))
            {
                result = BitConverter.ToChar(value, 0);
            }
            else if (unwrappedValueType == typeof(byte))
            {
                result = value.Single();
            }
            else if (unwrappedValueType == typeof(sbyte))
            {
                result = unchecked((sbyte)value.Single());
            }
            else if (unwrappedValueType == typeof(short))
            {
                result = BitConverter.ToInt16(value, 0);
            }
            else if (unwrappedValueType == typeof(ushort))
            {
                result = BitConverter.ToUInt16(value, 0);
            }
            else if (unwrappedValueType == typeof(int))
            {
                result = BitConverter.ToInt32(value, 0);
            }
            else if (unwrappedValueType == typeof(uint))
            {
                result = BitConverter.ToUInt32(value, 0);
            }
            else if (unwrappedValueType == typeof(long))
            {
                result = BitConverter.ToInt64(value, 0);
            }
            else if (unwrappedValueType == typeof(ulong))
            {
                result = BitConverter.ToUInt64(value, 0);
            }
            else if (unwrappedValueType == typeof(float))
            {
                result = BitConverter.ToSingle(value, 0);
            }
            else if (unwrappedValueType == typeof(double))
            {
                result = BitConverter.ToDouble(value, 0);
            }
            else
            {
                throw new FormatException($"Unsupported primitive type: {unwrappedValueType.FullName}");
            }
            return result;
        }

        private static object DeserializeObject(byte[] value, Type valueType)
        {
            var readRootValueAsArray = IsArrayType(valueType);
            try
            {
                return RoundTripBsonConvert.DeserializeObject(value, valueType, readRootValueAsArray: readRootValueAsArray);
            }
            catch (JsonSerializationException ex)
            {
                throw new FormatException($"Failed to deserialize value to type {valueType.FullName}", ex);
            }
        }

        private static bool IsArrayType(Type valueType)
        {
            bool isEnumerable = typeof(System.Collections.IEnumerable).IsAssignableFrom(valueType);
            if (!isEnumerable)
            {
                return false;
            }

            // Strings are enumerable but not serialized as arrays
            if (valueType == typeof(string))
            {
                return false;
            }

            // Dictionaries are enumerable but serialized as objects
            bool isDictionary = typeof(System.Collections.IDictionary).IsAssignableFrom(valueType)
                || ImplementsGenericInterface(valueType, typeof(IDictionary<,>))
                || ImplementsGenericInterface(valueType, typeof(IReadOnlyDictionary<,>));
            if (isDictionary)
            {
                return false;
            }

            return true;
        }

        private static bool ImplementsGenericInterface(Type valueType, Type interfaceGenericTypeDefinition)
        {
            IEnumerable<Type> interfaces = valueType.GetInterfaces();
            if (valueType.IsInterface)
            {
                interfaces = new[] { valueType }.Concat(interfaces);
            }
            return interfaces.Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == interfaceGenericTypeDefinition);
        }

        /// <summary>
        /// Like <see cref="JsonConvert"/>, but for BSON data for a round trip of serialization and deserialization.
        /// </summary>
        private static class RoundTripBsonConvert
        {
            public static byte[] SerializeObject(object value)
            {
                if (value == null) throw new ArgumentNullException(nameof(value));

                var bsonStream = new MemoryStream();
                using (bsonStream)
                using (var writer = new BsonDataWriter(bsonStream))
                {
                    CreateJsonSerializer().Serialize(writer, value);
                }
                return bsonStream.ToArray();
            }

            public static object DeserializeObject(byte[] value, Type valueType, bool readRootValueAsArray)
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                if (valueType == null) throw new ArgumentNullException(nameof(valueType));

                using (var bsonStream = new MemoryStream(value))
                using (var reader = new BsonDataReader(bsonStream, readRootValueAsArray: readRootValueAsArray, dateTimeKindHandling: DateTimeKind.Unspecified))
                {
                    return CreateJsonSerializer().Deserialize(reader, valueType);
                }
            }

            private static JsonSerializer CreateJsonSerializer()
            {
                var serializer = new JsonSerializer()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Ignore,
                    ContractResolver = new RoundTripContractResolver(),
                };
                serializer.Converters.Add(new IsoDateTimeConverter());
                return serializer;
            }

            /// <summary>
            /// A custom version of <see cref="DateTimeConverterBase"/> that serializes <see cref="DateTime"/> and <see cref="DateTimeOffset"/>
            /// values for a round trip of serialization and deserialization.
            /// </summary>
            private class IsoDateTimeConverter : DateTimeConverterBase
            {
                private const string DateTimeFormat = "o";

                private static DateTimeStyles DateTimeStyles => DateTimeStyles.RoundtripKind;

                private static CultureInfo Culture => CultureInfo.InvariantCulture;

                public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
                {
                    string text;
                    if (value is DateTime)
                    {
                        DateTime dateTime = (DateTime)value;
                        text = dateTime.ToString(DateTimeFormat, Culture);
                    }
                    else if (value is DateTimeOffset)
                    {
                        DateTimeOffset dateTimeOffset = (DateTimeOffset)value;
                        text = dateTimeOffset.ToString(DateTimeFormat, Culture);
                    }
                    else
                    {
                        throw new JsonSerializationException($"Unexpected value when converting date. Expected {nameof(DateTime)} or  {nameof(DateTimeOffset)}, got {value?.GetType().FullName}.");
                    }
                    writer.WriteValue(text);
                }

                public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
                {
                    bool isNullable = !objectType.IsValueType || Nullable.GetUnderlyingType(objectType) != null;
                    if (reader.TokenType == JsonToken.Null)
                    {
                        if (!isNullable)
                        {
                            throw new JsonSerializationException($"Cannot convert null value to {objectType.FullName}");
                        }
                        return null;
                    }

                    Type t = isNullable
                        ? Nullable.GetUnderlyingType(objectType) ?? objectType
                        : objectType;
                    if (reader.TokenType == JsonToken.Date)
                    {
                        if (t == typeof(DateTimeOffset))
                        {
                            return (reader.Value is DateTimeOffset) ? reader.Value : new DateTimeOffset((DateTime)reader.Value);
                        }
                        if (reader.Value is DateTimeOffset)
                        {
                            return ((DateTimeOffset)reader.Value).DateTime;
                        }
                        return reader.Value;
                    }

                    if (reader.TokenType != JsonToken.String)
                    {
                        throw new JsonSerializationException($"Unexpected token type for parsing date. Expected {nameof(JsonToken.String)}, got {Enum.GetName(typeof(JsonToken), reader.TokenType)}.");
                    }

                    string dateText = reader.Value.ToString();
                    if (string.IsNullOrEmpty(dateText) && isNullable)
                    {
                        return null;
                    }

                    if (t == typeof(DateTimeOffset))
                    {
                        return DateTimeOffset.Parse(dateText, Culture, DateTimeStyles);
                    }
                    return DateTime.Parse(dateText, Culture, DateTimeStyles);
                }
            }

            /// <summary>
            /// A custom version of <see cref="DefaultContractResolver"/> that only serializes mutable properties.
            /// </summary>
            private class RoundTripContractResolver : DefaultContractResolver
            {
                protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
                {
                    var property = base.CreateProperty(member, memberSerialization);
                    property.PropertyName = property.UnderlyingName;
                    property.Ignored = !property.Writable;
                    return property;
                }
            }
        }
    }
}