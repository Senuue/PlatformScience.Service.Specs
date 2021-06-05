using NUnit.Framework;
using PlatformScience.Service.Specs.Helpers;
using PlatformScience.Service.Specs.Models;
using System;
using System.Collections.Generic;
using System.Net;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace PlatformScience.Service.Specs.Steps
{
    [Binding]
    public class APISteps
    {
        public RequestModel requestModel { get; set; }
        public ResponseModel responseModel { get; set; }
        public HttpWebResponse response { get; set; }

        [Given(@"I set the room size to (.*) by (.*)")]
        public void GivenISetTheRoomSizeToBy(int dimensionX, int dimensionY)
        {
            requestModel = new RequestModel();
            requestModel.RoomSize[0] = dimensionX;
            requestModel.RoomSize[1] = dimensionY;
        }

        [Given(@"the starting coordinates to (.*) and (.*)")]
        public void GivenTheStartingCoordinatesToAnd(int startX, int startY)
        {
            requestModel.Coords[0] = startX;
            requestModel.Coords[1] = startY;
        }

        [Given(@"I set dirt patches at")]
        public void GivenISetDirtPatchesAt(Table table)
        {
            foreach(var row in table.Rows)
            {
                var x = int.Parse(row[0]);
                var y = int.Parse(row[1]);
                requestModel.Patches.Add(new int[2] { x, y });
            }
        }

        [Given(@"I set the following instructions (.*)")]
        public void GivenISetTheFollowingInstructions(string instructions)
        {
            requestModel.Instructions = instructions;
        }

        [When(@"I call the cleaning-sessions endpoint")]
        public void WhenICallTheCleaning_SessionsEndpoint()
        {
            response = RestTester.GetApiWebResponse(Verb.Post, "http://localhost:8080/v1/cleaning-sessions", requestModel);
        }

        [Then(@"I should get the following ending coords (.*) and (.*)")]
        public void ThenIShouldGetTheFollowingEndingCoordsAnd(int endX, int endY)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            responseModel = response.GetResponse<ResponseModel>();
            Assert.That(responseModel.Coords[0], Is.EqualTo(endX));
            Assert.That(responseModel.Coords[1], Is.EqualTo(endY));
        }

        [Then(@"I should have (.*) patches")]
        public void ThenIShouldHavePatches(int patches)
        {
            Assert.That(responseModel.Patches, Is.EqualTo(patches));
        }

        [Then(@"I should receive a (.*) result")]
        public void ThenIShouldReceiveAResult(HttpStatusCode status)
        {
            Assert.That(response.StatusCode, Is.EqualTo(status));
        }

    }
}
