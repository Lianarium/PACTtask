using NUnit.Framework;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PACTTest
{

    [TestFixture]
    public class ApiConsumerTests //:  ConsumerApiPact
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;
        ConsumerApiPact data1 = new ConsumerApiPact();
     
        
        public ApiConsumerTests()
        {

        }

        [SetUp]
        public void SetUp()
        {
            ApiConsumerTests test = new ApiConsumerTests();
            _mockProviderService = data1.MockProviderService;
            _mockProviderService.ClearInteractions(); //NOTE: Clears any previously registered interactions before the test is run
            _mockProviderServiceBaseUri = "https://jsonplaceholder.typicode.com";
        }

        [Test]
        public void GetSomething_WhenTheTesterSomethingExists_ReturnsTheSomething()
        {
            //Arrange
            _mockProviderService
              .Given("There is a something with id 'tester'")
              .UponReceiving("A POST request to retrieve the something")
              .With(new ProviderServiceRequest
              {
                  Method = HttpVerb.Post,
                  Path = "/posts",

                  Headers = new Dictionary<string, object>
                  {
                      {
                          "Content-Type", "application/json"
                      },

                      {
                          "Accept", "application/json"
                      }
                  },

                  Body = new //NOTE: Note the case sensitivity here, the body will be serialised as per the casing defined
                  {
                      UserId = "123",
                      Body = "postBody",
                      Id = "1234",
                      Title = "postTitle"

                  }
              }
              )
              .WillRespondWith(new ProviderServiceResponse
              {
                  Status = 201,
                  Headers = new Dictionary<string, object>
                {

                { "Content-Type", "application/json; charset=utf-8" }

               } 

              }); //NOTE: WillRespondWith call must come last as it will register the interaction

            //_mockProviderService.VerifyInteractions(); //NOTE: Verifies that interactions registered on the mock provider are called once and only once

            var consumer = new ApiClient(_mockProviderServiceBaseUri);
          
            //Act
            var result = consumer.GetResponse("Created");

            //Assert
            Assert.AreEqual("Created", result.ToString());

           _mockProviderService.VerifyInteractions(); //NOTE: Verifies that interactions registered on the mock provider are called once and only once
        }
 

    }
}
