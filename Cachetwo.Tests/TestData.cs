using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cachetwo
{
    public static class TestData
    {
        public static IEnumerable<object[]> PrimitiveValueCases => NonNullablePrimitiveValueCases.Concat(NullablePrimitiveValueCases);

        public static IEnumerable<object[]> NonNullablePrimitiveValueCases => new[]
        {
            new object[] { true },
            new object[] { false },
            new object[] { byte.MinValue },
            new object[] { default(byte) },
            new object[] { byte.MaxValue },
            new object[] { sbyte.MinValue },
            new object[] { default(sbyte) },
            new object[] { sbyte.MaxValue },
            new object[] { short.MinValue },
            new object[] { default(short) },
            new object[] { short.MaxValue },
            new object[] { ushort.MinValue },
            new object[] { default(ushort) },
            new object[] { ushort.MaxValue },
            new object[] { int.MinValue },
            new object[] { default(int) },
            new object[] { int.MaxValue },
            new object[] { uint.MinValue },
            new object[] { default(uint) },
            new object[] { uint.MaxValue },
            new object[] { long.MinValue },
            new object[] { default(long) },
            new object[] { long.MaxValue },
            new object[] { ulong.MinValue },
            new object[] { default(ulong) },
            new object[] { ulong.MaxValue },
            new object[] { float.MinValue },
            new object[] { default(float) },
            new object[] { float.Epsilon },
            new object[] { float.MaxValue },
            new object[] { double.MinValue },
            new object[] { default(double) },
            new object[] { double.Epsilon },
            new object[] { double.MaxValue },
            new object[] { decimal.MinValue },
            new object[] { default(decimal) },
            new object[] { decimal.MaxValue },
            new object[] { DateTime.MinValue },
            new object[] { default(DateTime) },
            new object[] { new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new object[] { new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local) },
            new object[] { new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified) },
            new object[] { DateTime.MaxValue },
            new object[] { DateTimeOffset.MinValue },
            new object[] { default(DateTimeOffset) },
            new object[] { new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromHours(0)) },
            new object[] { new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.FromHours(-5)) },
            new object[] { DateTimeOffset.MaxValue },
            new object[] { default(TimeSpan) },
            new object[] { new TimeSpan(long.MinValue) },
            new object[] { TimeSpan.FromHours(2) },
            new object[] { new TimeSpan(long.MaxValue) },
            new object[] { '2' },
            new object[] { '☃' },
            new object[] { "Hello World!" },
            new object[] { "Hello ☃!" },
            new object[] { Guid.Empty },
            new object[] { new Guid("42A252CC-1A7F-4C2A-B817-3B5BCA4660A0") },
            new object[] { new byte[] { 1, 2, 3 } },
            new object[] { default(StringComparison) },
            new object[] { StringComparison.OrdinalIgnoreCase },
            new object[] { LongEnum.MinLong },
            new object[] { LongEnum.ZeroLong },
            new object[] { LongEnum.MaxLong },
            new object[] { ShortEnum.MinShort },
            new object[] { ShortEnum.ZeroShort },
            new object[] { ShortEnum.MaxShort },
            new object[] { default(KeyValuePair<string, int>) },
            new object[] { new KeyValuePair<string, int>("a", 1) },
            new object[] { CancellationToken.None },
        };

        public static IEnumerable<object[]> NullablePrimitiveValueCases => NonNullablePrimitiveValueCases
            .Select(parameters =>
            {
                var value = parameters.Single();
                var valueType = value.GetType();
                if (!valueType.IsValueType)
                {
                    return null;
                }
                var nullableValueType = typeof(Nullable<>).MakeGenericType(valueType);
                var nullableValue = Activator.CreateInstance(nullableValueType, new object[] { value });
                return new object[] { nullableValue };
            })
            .Where(parameters => parameters != null && parameters.Any() && parameters.First() != null);
    }
}
