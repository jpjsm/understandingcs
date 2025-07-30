using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace ServiceStub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RandomNumberController : ControllerBase
    {
        private static Random rnd = new Random();

        [HttpGet()]
        public int Get()
        {
            int randomNumber = rnd.Next();
            int waitTime = (randomNumber % 997) + 2000;
            Thread.Sleep(waitTime);
            Console.WriteLine(
                $"Request received: GET /RandomNumber. Returning random number {randomNumber}, after waiting at least {waitTime} milliseconds."
            );
            return randomNumber;
        }
    }
}
