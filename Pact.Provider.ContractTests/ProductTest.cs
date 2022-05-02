using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Pact.Provider.ContractTests.XUnitHelpers;
using PactNet;
using PactNet.Infrastructure.Outputters;
using PactNet.Native;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Pact.Provider.ContractTests
{
    public class ProductTest
    {
        private string _pactServiceUri = "http://127.0.0.1:9001";
        private ITestOutputHelper _outputHelper { get; }

        public ProductTest(ITestOutputHelper output)
        {
            _outputHelper = output;
        }

        [Fact]
        public void EnsureProviderApiHonoursPactWithConsumer()
        {
            // Arrange
            var config = new PactVerifierConfig
            {
                // NOTE: We default to using a ConsoleOutput, however xUnit 2 does not capture the console output,
                // so a custom outputter is required.
                Outputters = new List<IOutput>
                {
                    new XUnitOutput(_outputHelper)
                }
            };

            using (var _webHost = WebHost.CreateDefaultBuilder().UseStartup<TestStartup>().UseUrls(_pactServiceUri).Build())
            {
                _webHost.Start();

                //Act / Assert
                IPactVerifier pactVerifier = new PactVerifier(config);
                var pactFile = new FileInfo(Path.Join("c:", "pacts", "ApiClient-ProductService.json"));
                var providerStateUrl = new Uri($"{_pactServiceUri}/provider-states");
                var pactUrl = new Uri(_pactServiceUri);

                pactVerifier.FromPactFile(pactFile)
                    .WithProviderStateUrl(providerStateUrl)
                    .ServiceProvider("ProductService", pactUrl)
                    .HonoursPactWith("ApiClient")
                    .Verify();
            }
        }
    }
}