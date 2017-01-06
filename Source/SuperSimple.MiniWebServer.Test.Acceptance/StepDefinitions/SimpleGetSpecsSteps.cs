﻿using FluentAssertions;
using System;
using System.Net.Http;
using TechTalk.SpecFlow;

namespace SuperSimple.MiniWebServer.Test.Acceptance.StepDefinitions
{
    using System.Net;

    [Binding]
    public class SimpleGetSpecsSteps
    {
        private HttpClient client = new HttpClient();
        private string payload;

        private HttpResponseMessage response;

        public SimpleGetSpecsSteps()
        {
            payload = Guid.NewGuid().ToString();
            client.BaseAddress = new Uri("http://localhost:8182");
        }

        [Given(@"I post to resource (.*) with header (.*):(.*)")]
        [When(@"I post to resource (.*) with header (.*):(.*)")]
        public void GivenIPostToResourceWithHeader(string resource, string header, string headerValue)
        {
            var content = new StringContent(payload);
            content.Headers.Add(header, headerValue);
            client.PostAsync(resource, content).Wait();
        }
        
        [When(@"I attempt a get on resource (.*)")]
        public void WhenIAttemptAGetOnResourceMyResourceResourceId(string resource)
        {
            response = client.GetAsync(resource).Result;
        }
        
        [Then(@"I should get back exactly what I set up")]
        public void ThenIShouldGetBackExactlyWhatISetUp()
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            response.Content.ReadAsStringAsync().Result.Trim().Should().Be(payload);
        }

        [Then(@"a get on (.*) should return status 404")]
        public void ThenAGetOnMyResourceResourceIdShouldReturnStatus(string resource)
        {
            var response = client.GetAsync(resource).Result;

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Then(@"a get on (.*) should get back exactly what I set up")]
        public void ThenAGetOnMyResourceResourceIdShouldGetBackExactlyWhatISetUp(string resource)
        {
            var response = client.GetAsync(resource).Result;

            response.IsSuccessStatusCode.Should().BeTrue();
            response.Content.ReadAsStringAsync().Result.Trim().Should().Be(payload);
        }
    }
}
