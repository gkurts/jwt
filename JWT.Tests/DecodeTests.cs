using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using FluentAssertions;

namespace JWT.Tests
{
    [TestClass]
    public class DecodeTests
    {
        private static readonly Customer customer = new Customer { FirstName = "Bob", Age = 37 };

        private const string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJGaXJzdE5hbWUiOiJCb2IiLCJBZ2UiOjM3fQ.cr0xw8c_HKzhFBMQrseSPGoJ0NPlRp_3BKzP96jwBdY";
        private const string malformedtoken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9eyJGaXJzdE5hbWUiOiJCb2IiLCJBZ2UiOjM3fQ.cr0xw8c_HKzhFBMQrseSPGoJ0NPlRp_3BKzP96jwBdY";

        private static readonly IDictionary<string, object> dictionaryPayload = new Dictionary<string, object>
        { 
            { "FirstName", "Bob" },
            { "Age", 37 }
        };

        private JsonWebToken defaultSerializer;
        private JsonWebToken newtonsoftSerializer;
        private JsonWebToken serviceStackSerializer;

        public DecodeTests()
        {
            defaultSerializer = new JsonWebToken(SerializerType.DefaultSerializer);
            newtonsoftSerializer = new JsonWebToken(SerializerType.NewtonsoftJsonSerializer);
            serviceStackSerializer = new JsonWebToken(SerializerType.ServiceStackJsonSerializer);
        }


        [TestMethod]
        public void Should_Decode_Token_To_Json_Encoded_String()
        {
            var jsonSerializer = new JavaScriptSerializer();
            var expectedPayload = jsonSerializer.Serialize(customer);

            string decodedPayload = defaultSerializer.Decode(token, "ABC", false);

            Assert.AreEqual(expectedPayload, decodedPayload);
        }

        [TestMethod]
        public void Should_Decode_Token_To_Dictionary()
        {
            object decodedPayload = defaultSerializer.DecodeToObject(token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(dictionaryPayload, options => options.IncludingAllRuntimeProperties());
        }

        [TestMethod]
        public void Should_Decode_Token_To_Dictionary_With_ServiceStack()
        {
            object decodedPayload = serviceStackSerializer.DecodeToObject(token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(dictionaryPayload, options => options.IncludingAllRuntimeProperties());
        }

        [TestMethod]
        public void Should_Decode_Token_To_Dictionary_With_Newtonsoft()
        {
            object decodedPayload = newtonsoftSerializer.DecodeToObject(token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(dictionaryPayload, options => options.IncludingAllRuntimeProperties());
        }

        [TestMethod]
        public void Should_Decode_Token_To_Generic_Type()
        {
            Customer decodedPayload = defaultSerializer.DecodeToObject<Customer>(token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(customer);
        }

        [TestMethod]
        public void Should_Decode_Token_To_Generic_Type_With_ServiceStack()
        {
            Customer decodedPayload = serviceStackSerializer.DecodeToObject<Customer>(token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(customer);
        }

        [TestMethod]
        public void Should_Decode_Token_To_Generic_Type_With_Newtonsoft()
        {
            Customer decodedPayload = newtonsoftSerializer.DecodeToObject<Customer>(token, "ABC", false);

            decodedPayload.ShouldBeEquivalentTo(customer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Should_Throw_On_Malformed_Token()
        {
            defaultSerializer.DecodeToObject<Customer>(malformedtoken, "ABC", false);
        }

        [TestMethod]
        [ExpectedException(typeof(SignatureVerificationException))]
        public void Should_Throw_On_Invalid_Key()
        {
            string invalidkey = "XYZ";

            defaultSerializer.DecodeToObject<Customer>(token, invalidkey, true);
        }

        [TestMethod]
        [ExpectedException(typeof(SignatureVerificationException))]
        public void Should_Throw_On_Invalid_Expiration_Claim()
        {
            var invalidexptoken = defaultSerializer.Encode(new { exp = "asdsad" }, "ABC", JwtHashAlgorithm.HS256);

            defaultSerializer.DecodeToObject<Customer>(invalidexptoken, "ABC", true);
        }

        [TestMethod]
        [ExpectedException(typeof(SignatureVerificationException))]
        public void Should_Throw_On_Expired_Token()
        {
            var anHourAgoUtc = DateTime.UtcNow.Subtract(new TimeSpan(1, 0, 0));
            Int32 unixTimestamp = (Int32)(anHourAgoUtc.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            var invalidexptoken = defaultSerializer.Encode(new { exp = unixTimestamp }, "ABC", JwtHashAlgorithm.HS256);

            defaultSerializer.DecodeToObject<Customer>(invalidexptoken, "ABC", true);
        }
    }


}