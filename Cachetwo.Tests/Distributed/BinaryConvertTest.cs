using LatticeObjectTree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cachetwo.Distributed
{
    public class BinaryConvertTest
    {
        [Fact]
        public void Serialize_Null()
        {
            Assert.Throws<ArgumentNullException>(() => BinaryConvert.Serialize(null));
        }

        [Fact]
        public void Serialize_EmptyString()
        {
            var input = string.Empty;
            var actual = BinaryConvert.Serialize(input);
            var expected = new byte[0];
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Serialize_EmptyArray()
        {
            var input = new string[0];
            var actual = BinaryConvert.Serialize(input);
            var expected = new byte[] { 5, 0, 0, 0, 0 };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Serialize_StringComparisonMetadata()
        {
            var input = new StringComparisonMetadata
            {
                StringComparisonId = (int)StringComparison.OrdinalIgnoreCase,
            };
            var serialized = BinaryConvert.Serialize(input);
            var actual = Convert.ToBase64String(serialized);
            const string expected = @"HQAAABBTdHJpbmdDb21wYXJpc29uSWQABQAAAAA=";
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Serialize_Type()
        {
            var input = typeof(string);
            Assert.Throws<FormatException>(() => BinaryConvert.Serialize(input));
        }

        [Fact]
        public void Serialize_SelfReferencingClass()
        {
            var input = new SelfReferencingClass();
            input.Self = input;
            Assert.Throws<FormatException>(() => BinaryConvert.Serialize(input));
        }

        [Fact]
        public void Serialize_CircularParentChildClass()
        {
            var input = new CircularParentClass();
            input.Child = new CircularChildClass();
            input.Child.Parent = input;
            Assert.Throws<FormatException>(() => BinaryConvert.Serialize(input));
        }

        [Fact]
        public void RoundTrip_Task()
        {
            Task<int> input = Task.FromResult(2);
            Assert.Throws<FormatException>(() => BinaryConvert.Serialize(input));
        }

        [Fact]
        public void RoundTrip_CancellationToken()
        {
            var input = new CancellationToken(true);
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<CancellationToken>(serialized);
            Assert.Equal(default(CancellationToken), deserialized);
        }

        [Fact]
        public void Deserialize_Null()
        {
            Assert.Throws<ArgumentNullException>(() => BinaryConvert.Deserialize<IdNamePair>(null));
        }

        [Fact]
        public void Deserialize_EmptyString()
        {
            var actual = BinaryConvert.Deserialize<string>(new byte[0]);
            Assert.Equal(string.Empty, actual);
        }

        [Fact]
        public void Deserialize_EmptyBytesAsIdNamePair()
        {
            var actual = BinaryConvert.Deserialize<IdNamePair>(new byte[0]);
            Assert.Null(actual);
        }

        [Fact]
        public void Deserialize_InvalidBytes()
        {
            Assert.Throws<FormatException>(() => BinaryConvert.Deserialize<IdNamePair>(new byte[] { 1, 2, 3, 4 }));
        }

        [Fact]
        public void RoundTrip_IdNamePair()
        {
            var input = new IdNamePair
            {
                Id = 2,
                Name = "hello",
            };
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<IdNamePair>(serialized);
            ObjectTreeAssert.AreEqual(input, deserialized);
        }

        [Fact]
        public void RoundTrip_ImmutableIdNamePair()
        {
            var input = new ImmutableIdNamePair(2, "hello");
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<ImmutableIdNamePair>(serialized);
            ObjectTreeAssert.AreEqual(new ImmutableIdNamePair(0, null), deserialized);
        }

        [Fact]
        public void RoundTrip_ImmutableIdNamePair_ToIdNamePair()
        {
            var input = new ImmutableIdNamePair(2, "hello");
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<IdNamePair>(serialized);
            ObjectTreeAssert.AreEqual(new IdNamePair(), deserialized);
        }

        [Fact]
        public void RoundTrip_NoIdSetterIdNamePair()
        {
            var input = new NoIdSetterIdNamePair { Name = "hello" };
            input.SetId(1);

            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<NoIdSetterIdNamePair>(serialized);

            var expected = new NoIdSetterIdNamePair { Name = "hello" };
            ObjectTreeAssert.AreEqual(expected, deserialized);
        }

        [Fact]
        public void RoundTrip_NoIdSetterIdNamePair_ToIdNamePair()
        {
            var input = new NoIdSetterIdNamePair { Name = "hello" };
            input.SetId(1);

            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<IdNamePair>(serialized);

            var expected = new IdNamePair { Name = "hello" };
            ObjectTreeAssert.AreEqual(expected, deserialized);
        }

        [Fact]
        public void RoundTrip_NoIdGetterIdNamePair()
        {
            var input = new NoIdGetterIdNamePair { Id = 2, Name = "hello", };
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<NoIdGetterIdNamePair>(serialized);

            var expected = new NoIdGetterIdNamePair { Name = "hello" };
            ObjectTreeAssert.AreEqual(expected, deserialized);
            Assert.Equal(0, deserialized.GetId());
        }

        [Fact]
        public void RoundTrip_NoIdGetterIdNamePair_ToIdNamePair()
        {
            var input = new NoIdGetterIdNamePair { Id = 2, Name = "hello", };
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<IdNamePair>(serialized);

            var expected = new IdNamePair { Name = "hello" };
            ObjectTreeAssert.AreEqual(expected, deserialized);
        }

        [Fact]
        public void RoundTrip_StringComparisonMetadata()
        {
            var input = new StringComparisonMetadata
            {
                StringComparisonId = (int)StringComparison.OrdinalIgnoreCase,
            };
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<StringComparisonMetadata>(serialized);
            ObjectTreeAssert.AreEqual(input, deserialized);
        }

        [Fact]
        public void RoundTrip_TimeParty()
        {
            var input = new TimeParty
            {
                UnixDateTimeSeconds = 2,
                UnitDateTimeSecondsOrNull = 2,
                DateTimeUtc = new DateTime(2002, 2, 2, 12, 1, 2, 123, DateTimeKind.Utc),
                DateTimeUtcOrNull = new DateTime(2002, 2, 2, 12, 1, 2, 123, DateTimeKind.Utc),
                DateTimeLocal = new DateTime(2002, 2, 2, 12, 1, 2, 123, DateTimeKind.Local),
                DateTimeLocalOrNull = new DateTime(2002, 2, 2, 12, 1, 2, 123, DateTimeKind.Local),
                DateTimeUnspecified = new DateTime(2002, 2, 2, 12, 1, 2, 123, DateTimeKind.Unspecified),
                DateTimeUnspecifiedOrNull = new DateTime(2002, 2, 2, 12, 1, 2, 123, DateTimeKind.Unspecified),
                DateTimeOffsetUtc = new DateTimeOffset(2002, 2, 2, 12, 1, 2, TimeSpan.FromHours(0)),
                DateTimeOffsetUtcOrNull = new DateTimeOffset(2002, 2, 2, 12, 1, 2, TimeSpan.FromHours(0)),
                DateTimeOffsetLocal = new DateTimeOffset(2002, 2, 2, 12, 1, 2, TimeSpan.FromHours(-5)),
                DateTimeOffsetLocalOrNull = new DateTimeOffset(2002, 2, 2, 12, 1, 2, TimeSpan.FromHours(-5)),
            };

            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<TimeParty>(serialized);
            ObjectTreeAssert.AreEqual(input, deserialized);

            Assert.Equal(DateTimeKind.Utc, deserialized.DateTimeUtc.Kind);
            Assert.Equal(DateTimeKind.Utc, deserialized.DateTimeUtcOrNull.Value.Kind);
            Assert.Equal(DateTimeKind.Local, deserialized.DateTimeLocal.Kind);
            Assert.Equal(DateTimeKind.Local, deserialized.DateTimeLocalOrNull.Value.Kind);
            Assert.Equal(DateTimeKind.Unspecified, deserialized.DateTimeUnspecified.Kind);
            Assert.Equal(DateTimeKind.Unspecified, deserialized.DateTimeUnspecifiedOrNull.Value.Kind);
        }

        [Fact]
        public void RoundTrip_TimeParty_Defaults()
        {
            var input = new TimeParty();

            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<TimeParty>(serialized);
            ObjectTreeAssert.AreEqual(input, deserialized);
        }

        [Fact]
        public void RoundTrip_CustomJsonAttributeClass()
        {
            var input = new CustomJsonAttributeClass
            {
                Id = 1,
                AlternateId = 2,
                Ignored = 3,
                Name = "4",
            };

            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<CustomJsonAttributeClass>(serialized);
            ObjectTreeAssert.AreEqual(input, deserialized);
        }

        [Fact]
        public void RoundTrip_CustomDataContractClass()
        {
            var input = new CustomDataContractClass
            {
                Id = 1,
                AlternateId = 2,
                NotMember = 3,
                Ignored = 4,
                Name = "5",
            };

            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<CustomDataContractClass>(serialized);
            ObjectTreeAssert.AreEqual(input, deserialized);
        }

        [Fact]
        public void RoundTrip_ArrayOfIntegers()
        {
            int[] input = new int[] { 1, 2, 3 };
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<int[]>(serialized);
            Assert.Equal(input, deserialized);
        }

        [Fact]
        public void RoundTrip_EmptyArray()
        {
            int[] input = new int[0];
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<int[]>(serialized);
            Assert.NotNull(deserialized);
            Assert.Empty(deserialized);
        }

        [Fact]
        public void RoundTrip_ArrayOfObjects()
        {
            TimeParty[] input = new TimeParty[]
            {
                new TimeParty
                {
                    UnixDateTimeSeconds = 2,
                    DateTimeUtc = new DateTime(2002, 2, 2, 12, 1, 2, 123, DateTimeKind.Utc),
                    DateTimeLocal = new DateTime(2002, 2, 2, 12, 1, 2, 123, DateTimeKind.Local),
                    DateTimeUnspecified = new DateTime(2002, 2, 2, 12, 1, 2, 123, DateTimeKind.Unspecified),
                    DateTimeOffsetUtc = new DateTimeOffset(2002, 2, 2, 12, 1, 2, TimeSpan.FromHours(0)),
                    DateTimeOffsetLocal = new DateTimeOffset(2002, 2, 2, 12, 1, 2, TimeSpan.FromHours(-5)),
                },
                new TimeParty(),
            };
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<TimeParty[]>(serialized);
            ObjectTreeAssert.AreEqual(input, deserialized);
        }

        [Fact]
        public void RoundTrip_ListOfIntegers()
        {
            List<int> input = new List<int> { 1, 2, 3 };
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<List<int>>(serialized);
            Assert.Equal(input, deserialized);
        }

        [Fact]
        public void RoundTrip_EmptyList()
        {
            List<int> input = new List<int>(0);
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<List<int>>(serialized);
            Assert.NotNull(deserialized);
            Assert.Empty(deserialized);
        }

        [Fact]
        public void RoundTrip_ListOfObjects()
        {
            TimeParty[] input = new TimeParty[]
            {
                new TimeParty
                {
                    UnixDateTimeSeconds = 2,
                    DateTimeUtc = new DateTime(2002, 2, 2, 12, 1, 2, 123, DateTimeKind.Utc),
                    DateTimeLocal = new DateTime(2002, 2, 2, 12, 1, 2, 123, DateTimeKind.Local),
                    DateTimeUnspecified = new DateTime(2002, 2, 2, 12, 1, 2, 123, DateTimeKind.Unspecified),
                    DateTimeOffsetUtc = new DateTimeOffset(2002, 2, 2, 12, 1, 2, TimeSpan.FromHours(0)),
                    DateTimeOffsetLocal = new DateTimeOffset(2002, 2, 2, 12, 1, 2, TimeSpan.FromHours(-5)),
                },
                new TimeParty(),
            };
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<List<TimeParty>>(serialized);
            ObjectTreeAssert.AreEqual(input, deserialized);
        }

        [Fact]
        public void RoundTrip_ListToArray()
        {
            List<int> input = new List<int> { 1, 2, 3 };
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<int[]>(serialized);
            Assert.Equal(input, deserialized);
        }

        [Fact]
        public void RoundTrip_ListToIList()
        {
            List<int> input = new List<int> { 1, 2, 3 };
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<IList<int>>(serialized);
            Assert.Equal(input, deserialized);
        }

        [Fact]
        public void RoundTrip_DictionaryOfIntegers()
        {
            Dictionary<string, int> input = new Dictionary<string, int>
            {
                ["one"] = 1,
                ["two"] = 2,
            };
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<Dictionary<string, int>>(serialized);
            Assert.Equal(input.OrderBy(x => x.Key), deserialized.OrderBy(x => x.Key));
        }

        [Fact]
        public void RoundTrip_DictionaryOfStrings()
        {
            Dictionary<string, string> input = new Dictionary<string, string>
            {
                ["one"] = "1",
                ["two"] = "2",
            };
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<Dictionary<string, string>>(serialized);
            Assert.Equal(input.OrderBy(x => x.Key), deserialized.OrderBy(x => x.Key));
        }

        [Fact]
        public void RoundTrip_DictionaryOfObjects()
        {
            Dictionary<string, TimeParty> input = new Dictionary<string, TimeParty>
            {
                ["one"] = new TimeParty(),
                ["two"] = new TimeParty
                {
                    UnixDateTimeSeconds = 2,
                    DateTimeUtc = new DateTime(2002, 2, 2, 12, 1, 2, 123, DateTimeKind.Utc),
                    DateTimeLocal = new DateTime(2002, 2, 2, 12, 1, 2, 123, DateTimeKind.Local),
                    DateTimeUnspecified = new DateTime(2002, 2, 2, 12, 1, 2, 123, DateTimeKind.Unspecified),
                    DateTimeOffsetUtc = new DateTimeOffset(2002, 2, 2, 12, 1, 2, TimeSpan.FromHours(0)),
                    DateTimeOffsetLocal = new DateTimeOffset(2002, 2, 2, 12, 1, 2, TimeSpan.FromHours(-5)),
                },
                ["three"] = null
            };
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<Dictionary<string, TimeParty>>(serialized);
            Assert.Equal(input.Keys.OrderBy(x => x), deserialized.Keys.OrderBy(x => x));
            ObjectTreeAssert.AreEqual(input["one"], deserialized["one"]);
            ObjectTreeAssert.AreEqual(input["two"], deserialized["two"]);
            Assert.Null(deserialized["three"]);
        }

        [Fact]
        public void RoundTrip_DictionaryToIDictionary()
        {
            Dictionary<string, int> input = new Dictionary<string, int>
            {
                ["one"] = 1,
                ["two"] = 2,
            };
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<IDictionary>(serialized);
            Assert.Equal(input.Keys.OrderBy(x => x), deserialized.Keys.Cast<string>().OrderBy(x => x));
        }

        [Fact]
        public void RoundTrip_DictionaryToIDictionaryGeneric()
        {
            Dictionary<string, int> input = new Dictionary<string, int>
            {
                ["one"] = 1,
                ["two"] = 2,
            };
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<IDictionary<string, int>>(serialized);
            Assert.Equal(input.OrderBy(x => x.Key), deserialized.OrderBy(x => x.Key));
        }

        [Fact]
        public void RoundTrip_DictionaryToIReadOnlyDictionary()
        {
            Dictionary<string, int> input = new Dictionary<string, int>
            {
                ["one"] = 1,
                ["two"] = 2,
            };
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<IReadOnlyDictionary<string, int>>(serialized);
            Assert.Equal(input.OrderBy(x => x.Key), deserialized.OrderBy(x => x.Key));
        }

        [Fact]
        public void RoundTrip_CustomReadOnlyDictionary()
        {
            IReadOnlyDictionary<string, int> input = new CustomReadOnlyDictionary<string, int>(new Dictionary<string, int>
            {
                ["one"] = 1,
                ["two"] = 2,
            });
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<CustomReadOnlyDictionary<string, int>>(serialized);
            Assert.Equal(input.OrderBy(x => x.Key), deserialized.OrderBy(x => x.Key));
        }

        [Fact]
        public void RoundTrip_CustomDictionary()
        {
            IDictionary<string, int> input = new CustomDictionary<string, int>
            {
                ["one"] = 1,
                ["two"] = 2,
            };
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize<CustomDictionary<string, int>>(serialized);
            Assert.Equal(input.OrderBy(x => x.Key), deserialized.OrderBy(x => x.Key));
        }

        [Theory]
        [MemberData(nameof(TestData.PrimitiveValueCases), MemberType = typeof(TestData))]
        public void RoundTrip_Primitives(object input)
        {
            var inputType = input.GetType();
            var serialized = BinaryConvert.Serialize(input);
            var deserialized = BinaryConvert.Deserialize(serialized, inputType);
            Assert.Equal(input, deserialized);
        }
    }
}