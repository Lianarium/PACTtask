using Newtonsoft.Json;
using PactNet;
using PactNet.Mocks.MockHttpService;
using PactNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PACTTest
{
   public class ConsumerApiPact : IDisposable
    {
        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }

        public int MockServerPort { get { return 8080; } }
        public string MockProviderServiceBaseUri { get { return String.Format("http://localhost:{0}", MockServerPort); } }

        public ConsumerApiPact()
        {

            PactBuilder = new PactBuilder(); //Defaults to specification version 1.1.0, uses default directories. PactDir: ..\..\pacts and LogDir: ..\..\logs
           
            PactBuilder
              .ServiceConsumer("Consumer")
              .HasPactWith("Some API");  
                                                                         
            MockProviderService = PactBuilder.MockService(MockServerPort, false); //By passing true as the second param, you can enabled SSL. A self signed SSL cert will be provisioned by default.
                                                                                 //or
        }

        public void Dispose()
        {
            PactBuilder.Build(); //NOTE: Will save the pact file once finished
        }
    }
}
