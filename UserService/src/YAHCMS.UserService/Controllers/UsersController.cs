using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using YAHCMS.UserService.Persistence;
using YAHCMS.UserService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace YAHCMS.UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        IUserRepository repository;
        public UsersController(IUserRepository repo)
        {
            repository = repo;
        }

        [HttpGet("{userID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public ActionResult<User> Get(string userID)
        {
            User u = repository.Get(userID);
            if(u == null)
                return NotFound();

            return Ok(u);
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAll()
        {
            var users = repository.GetAll().OrderByDescending(user => user.Score).Take(10).ToList();
            return Ok(users);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create(User user)
        {
            User u = repository.Add(user);
            return Created($"{u.UID}", user);
        }


    }

}