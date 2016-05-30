using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace JWT.Tests
{
    [TestClass]
    public class EncodeTests
    {
        private static readonly Customer customer = new Customer { FirstName = "Bob", Age = 37 };

        private const string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJGaXJzdE5hbWUiOiJCb2IiLCJBZ2UiOjM3fQ.cr0xw8c_HKzhFBMQrseSPGoJ0NPlRp_3BKzP96jwBdY";
        private const string extraheaderstoken = "eyJmb28iOiJiYXIiLCJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJGaXJzdE5hbWUiOiJCb2IiLCJBZ2UiOjM3fQ.slrbXF9VSrlX7LKsV-Umb_zEzWLxQjCfUOjNTbvyr1g";

        private JsonWebToken defaultSerializer;
        private JsonWebToken newtonsoftSerializer;
        private JsonWebToken serviceStackSerializer;

        public EncodeTests()
        {
            defaultSerializer = new JsonWebToken(SerializerType.DefaultSerializer);
            newtonsoftSerializer = new JsonWebToken(SerializerType.NewtonsoftJsonSerializer);
            serviceStackSerializer = new JsonWebToken(SerializerType.ServiceStackJsonSerializer);
        }

        [TestMethod]
        public void Should_Encode_Type()
        {
            string result = defaultSerializer.Encode(customer, "ABC", JwtHashAlgorithm.HS256);

            Assert.AreEqual(token, result);
        }

        [TestMethod]
        public void Should_Encode_Type_With_Extra_Headers()
        {
            var extraheaders = new Dictionary<string, object> { { "foo", "bar" } };

            string result = defaultSerializer.Encode(extraheaders, customer, "ABC", JwtHashAlgorithm.HS256);

            Assert.AreEqual(extraheaderstoken, result);
        }

        [TestMethod]
        public void Should_Encode_Type_With_ServiceStack()
        {
            string result = serviceStackSerializer.Encode(customer, "ABC", JwtHashAlgorithm.HS256);

            Assert.AreEqual(token, result);
        }

        [TestMethod]
        public void Should_Encode_Type_With_ServiceStack_And_Extra_Headers()
        {
            var extraheaders = new Dictionary<string, object> { { "foo", "bar" } };
            string result = serviceStackSerializer.Encode(extraheaders, customer, "ABC", JwtHashAlgorithm.HS256);

            Assert.AreEqual(extraheaderstoken, result);
        }

        [TestMethod]
        public void Should_Encode_Type_With_Newtonsoft_Serializer()
        {
            string result = newtonsoftSerializer.Encode(customer, "ABC", JwtHashAlgorithm.HS256);

            Assert.AreEqual(token, result);
        }

        [TestMethod]
        public void Should_Encode_Type_With_Newtonsoft_Serializer_And_Extra_Headers()
        {
            var extraheaders = new Dictionary<string, object> { { "foo", "bar" } };
            string result = newtonsoftSerializer.Encode(extraheaders, customer, "ABC", JwtHashAlgorithm.HS256);

            Assert.AreEqual(extraheaderstoken, result);
        }
    }
}