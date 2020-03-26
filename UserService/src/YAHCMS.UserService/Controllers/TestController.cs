using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace YAHCMS.UserService.Controllers
{
    [ApiController]
    [Route("hello")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Hello from Kubernetes";
        }
    }
}