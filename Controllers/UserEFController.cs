using AutoMapper;
using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class UserEFController : ControllerBase
{
    DataContextEF _entityframework;
    IMapper _mapper;
    public UserEFController(IConfiguration config)
    {
        _entityframework = new DataContextEF(config);

        _mapper = new Mapper(new MapperConfiguration(cfg => {
            cfg.CreateMap<UserToAddDto, User>();
        }));
    }

    [HttpGet("GetUsers")]

    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _entityframework.Users.ToList<User>();
    return users;
    }

    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        User? userSingle = _entityframework.Users
            .Where(u => u.UserId == userId)
            .FirstOrDefault<User>();

        if (userSingle != null)
        {
        return userSingle;
        }

        throw new Exception("Failed to get user");
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        User? userDb = _entityframework.Users
            .Where(u => u.UserId == user.UserId)
            .FirstOrDefault<User>();

        if (userDb != null)
        {
            userDb.Active = user.Active;
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            if(_entityframework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Failed to update user");
        }

        throw new Exception("Failed to get user");
    } 


    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto user)
    {
        User? userDb = _mapper.Map<User>(user);

        _entityframework.Add(userDb);
        if(_entityframework.SaveChanges() > 0)
        {
            return Ok();
        }
        throw new Exception("Failed to add user");
        
    }


    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _entityframework.Users
            .Where(u => u.UserId == userId)
            .FirstOrDefault<User>();

        if (userDb != null)
        {
            _entityframework.Users.Remove(userDb);
            if(_entityframework.SaveChanges() > 0)
            {
                return Ok();
            }
            throw new Exception("Failed to delete user");
        }

        throw new Exception("Failed to get user");
    }


    // // USER JOB INFO ROUTES
    // [HttpGet("GetUserJob")]

    // public IEnumerable<User> GetUserJob()
    // {
    //     IEnumerable<User> users = _entityframework.Users.ToList<User>();
    // return users;
    // }
}