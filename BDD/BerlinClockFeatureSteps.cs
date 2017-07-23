using System;
using TechTalk.SpecFlow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace BerlinClock
{
    [Binding]
    public class TheBerlinClockSteps
    {
        private ITimeConverter berlinClock = new TimeConverter();
        private String theTime;

        [When(@"the time is ""(.*)""")]
        public void WhenTheTimeIs(string time)
        {
            theTime = time;
        }

        [Then(@"the clock should look like")]
        public void ThenTheClockShouldLookLike(string theExpectedBerlinClockOutput)
        {
            //I've switched parameters, to have first expected and than actual as method signature suggest
            Assert.AreEqual(theExpectedBerlinClockOutput, berlinClock.ConvertTime(theTime));
        }

        [Then(@"the exception should be thrown")]
        public void ThenExceptionShouldBeThrown(string expectedExceptionMessage)
        {
            try
            {
                berlinClock.ConvertTime(theTime);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(expectedExceptionMessage, ex.Message);
            }
        }

    }
}
