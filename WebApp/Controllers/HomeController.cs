using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using webApp.model;

namespace webApp.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class HomeController : Controller
    {
        public HomeController() { }
        [HttpGet("getuser")]
        public IActionResult getuser()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "jsonfile.json");
            if (System.IO.File.Exists(filePath))
            {
                string json = System.IO.File.ReadAllText(filePath);
                var people = JsonConvert.DeserializeObject<List<person>>(json);
                return Ok(people);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, "file doesnt exist");
            }
        }
        [HttpGet("user/{id}")]
        public IActionResult getUserById(int id)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "jsonfile.json");
            List<person> people = new List<person>();
            if (System.IO.File.Exists(filePath))
            {
                string json = System.IO.File.ReadAllText(filePath);
                people = JsonConvert.DeserializeObject<List<person>>(json);

            }
            var user = people.FirstOrDefault(person => person.Id == id);
            if (user == null)
            {
                return BadRequest("person not found");
            }
            return Ok(user);
        }
        [HttpGet("FilterUser")]
        public IActionResult FilterUser([FromQuery] person person)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "jsonfile.json");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found");
            }

            string json = System.IO.File.ReadAllText(filePath);
            var users = JsonConvert.DeserializeObject<List<person>>(json);

            var filteredUsers = users.Where(x => x.Salary < person.Salary && x.PersonAddress.City.Contains(person.PersonAddress.City)).ToList();

            if (filteredUsers.Count == 0)
            {
                return BadRequest("No users match the filter criteria");
            }

            return Ok(filteredUsers);
        }
        [HttpPost("addPerson")]
        public IActionResult addPerson(person person)
        {
            var validator = new userValidator();
            var valid = validator.Validate(person);
            var errorMessage = "";
            if (!valid.IsValid)
            {
                foreach (var item in valid.Errors)
                {
                    errorMessage += item.ErrorMessage + " , ";
                }
                return BadRequest(errorMessage);
            }
            try
            {
                var userList = new List<person>();
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "jsonfile.json");
                if (System.IO.File.Exists(filePath))
                {
                    string json = System.IO.File.ReadAllText(filePath);
                    userList = JsonConvert.DeserializeObject<List<person>>(json);
                }
                person.Id = userList.Count + 1;
                person.CreateDate = DateTime.Now;
                userList.Add(person);
                string updateJson = JsonConvert.SerializeObject(userList, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(filePath, updateJson);
                return Ok(userList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"person cant be added,{ex.Message}");
            }
        }
        [HttpPut("updatePerson/{id}")]
        public IActionResult updatePerson(int id, [FromBody] person person)
        {
            var user = new List<person>();
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "jsonfile.json");
            if (System.IO.File.Exists(filePath))
            {
                string json = System.IO.File.ReadAllText(filePath);
                user = JsonConvert.DeserializeObject<List<person>>(json);
            }
            else if (!System.IO.File.Exists(filePath) || id < 0 || id >= user.Count)
            {
                return NotFound();
            }
            var validator = new userValidator();
            var validatorResult = validator.Validate(person);
            var errormessage = "";
            if (!validatorResult.IsValid)
            {
                foreach (var item in validatorResult.Errors)
                {
                    errormessage += item.ErrorMessage + " , ";
                }
                return BadRequest(errormessage);
            }
            person.CreateDate = DateTime.Now;
            user[id] = person;
            var serialized = JsonConvert.SerializeObject(user, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(filePath, serialized);
            return Ok(user);
        }
        [HttpDelete("deletePerson")]
        public IActionResult deletePerson(int id, person person)
        {
            var user = new List<person>();
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "jsonfile.json");
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            string json = System.IO.File.ReadAllText(filePath);
            user = JsonConvert.DeserializeObject<List<person>>(json);
            if (id < 0 || user.Count <= id)
            {
                return NotFound();
            }
            else
            {
                user.RemoveAt(id);
                person.CreateDate = DateTime.Now;
                var serialized = JsonConvert.SerializeObject(user, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(filePath, serialized);
                return Ok(user);
            }
        }
    }
}
