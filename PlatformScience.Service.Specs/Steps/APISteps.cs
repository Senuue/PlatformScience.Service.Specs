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
            if (response.StatusCode.IsSuccess())
            {
                responseModel = response.GetResponse<ResponseModel>();
            }
        }

        [Then(@"I should get the following ending coords (.*) and (.*)")]
        public void ThenIShouldGetTheFollowingEndingCoordsAnd(int endX, int endY)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
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

        [Given(@"I create instructions to traverse the entire grid from (.*) and (.*)")]
        public void GivenICreateInstructionsToTraverseTheEntireGridFromAnd(int dimensionX, int dimensionY)
        {
            var instructions = TraverseEntireGridInstructions(dimensionX, dimensionY);
            requestModel.Instructions = instructions;
        }

        [Then(@"I should get the expected end coordinates from traversing a (.*) by (.*) grid")]
        public void ThenIShouldGetTheExpectedEndCoordinatesFromTraversingAByGrid(int dimensionX, int dimensionY)
        {
            var expectedX = dimensionX -1;
            var expectedY = dimensionX % 2 == 0 ? 0 : dimensionY -1;
            
            Assert.That(responseModel.Coords[0], Is.EqualTo(expectedX));
            Assert.That(responseModel.Coords[1], Is.EqualTo(expectedY));
        }

        private string TraverseEntireGridInstructions(int dimensionX, int dimensionY)
        {
            var instructions = "";
            var isSouth = false;

            for(var i = 0; i < dimensionX; i++)
            {
                for(var j = 0; j < dimensionY; j++)
                {
                    instructions += isSouth ? "S" : "N";
                }
                instructions += "E";
                isSouth = !isSouth;
            }

            return instructions;
        }
    }
}
