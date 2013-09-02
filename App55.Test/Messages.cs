using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using App55;
using System.Collections;

namespace App55.Test {
    public class TestResponse : Response {
        public TestResponse(Gateway gateway, string json) : base(gateway, (Hashtable)JSON.Parse(json)) {
        }


    }

    [TestFixture]
    public class MessagesTest {
        [TestCase]
        public void AdditionalPropertiesTest() {
            Gateway gateway = new Gateway(Environment.Development, "API_KEY", "API_SECRET");

            string json = "{\"a\":0,\"b\":{\"c\":{\"d\":0}},\"sig\":\"FwORZsveXVAf1M14JuCiXHy1y7s=\",\"ts\":\"TS\"}";
            Response response = new TestResponse(gateway, json);
            Assert.True(response.IsValidSignature, response.ExpectedSignature);

            json = "{\"a\":0,\"b\":[0,{\"d\":0}],\"sig\":\"Fzib2BP3VJNdTRhX-Jbbgi3zthc=\",\"ts\":\"TS\"}";
            response = new TestResponse(gateway, json);
            Assert.True(response.IsValidSignature, response.ExpectedSignature);
        }
    }
}
