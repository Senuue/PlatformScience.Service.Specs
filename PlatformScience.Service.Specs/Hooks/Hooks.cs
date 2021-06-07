using PlatformScience.Service.Specs.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using TechTalk.SpecFlow;

namespace PlatformScience.Service.Specs.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        private static  Process process;

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            if(scenarioContext.ScenarioInfo.Tags.Contains("RestartService") && IsServiceRunning())
            {
                process.Kill();
            }

            if (!IsServiceRunning())
            {
                var args = "-Dproperties.location=/application.properties -Xdebug -Xrunjdwp:server=y,transport=dt_socket,address=4000,suspend=n -jar Resources/sdet-assignment-service-0.0.1-SNAPSHOT.jar";
                process = new Process();
                process.StartInfo.FileName = "java.exe";
                process.StartInfo.Arguments = args;
                process.Start();

                var checkStartupAttempts = 0;

                while (!IsServiceRunning() && checkStartupAttempts < 10)
                {
                    checkStartupAttempts++;
                    Console.WriteLine("Startup attempt: " + checkStartupAttempts);
                    Thread.Sleep(1000);
                }
            }
        }

        [AfterScenario("RestartService")]
        public void AfterScenario()
        {
            KillProcess();
        }
        
        [AfterTestRun]
        public static void AfterTestRun()
        {
            KillProcess();
        }

        private static void KillProcess()
        {
            if(process != null)
            {
                process.Kill();
            }
        }
        private bool IsServiceRunning()
        {
            try
            {
                RestTester.GetApiWebResponse(Verb.Post, "http://localhost:8080/v1/cleaning-sessions", new RequestBody());
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
