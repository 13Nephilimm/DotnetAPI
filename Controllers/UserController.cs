using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class UserController : ControllerBase
{
    DataContextDapper _dapper;
    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }


    [HttpGet("GetUsers")]

    public IEnumerable<User> GetUsers()
    {
        string sql = @"
            SELECT [UserId],
                        [FirstName],
                        [LastName],
                        [Email],
                        [Gender],
                        [Active] 
            FROM TutorialAppSchema.Users
        ";
        IEnumerable<User> users = _dapper.LoadData<User>(sql);
    return users;
    }

    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        string sql = $@"
        SELECT * FROM TutorialAppSchema.Users WHERE UserId = {userId}
        ";
        User userSingle = _dapper.LoadDataSingle<User>(sql);
        return userSingle;
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        string sql = $@"
        UPDATE TutorialAppSchema.Users
            SET [FirstName] = '{user.FirstName}',
            [LastName] = '{user.LastName}',
            [Email] = '{user.Email}',
            [Gender] = '{user.Gender}',
            [Active] = '{user.Active}'
        WHERE UserId = {user.UserId}
        ";
        Console.WriteLine(sql);

        if(_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to update user");
    } 


    [HttpPost("AddUser")]
    public IActionResult AddUser(UserToAddDto user)
    {
        string sql = $@"
         INSERT INTO TutorialAppSchema.Users(
            [FirstName],
            [LastName],
            [Email],
            [Gender],
            [Active]
        ) VALUES (
            '{user.FirstName}',
            '{user.LastName}',
            '{user.Email}',
            '{user.Gender}',
            '{user.Active}'
        )
        ";

        if(_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to add user");
    }


    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sql = $@"
         DELETE FROM TutorialAppSchema.Users WHERE UserId = {userId}
        ";

        if(_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to delete user");
    }


    /// /////////////////////////////// USER JOB INFO ROUTES
    
     [HttpGet("GetUserJobInfo")]

    public IEnumerable<UserJobInfo> GetUserJobInfo()
    {
        string sql = @"
            SELECT *
            FROM TutorialAppSchema.UserJobInfo
        ";
        IEnumerable<UserJobInfo> userJobInfo = _dapper.LoadData<UserJobInfo>(sql);
    return userJobInfo;
    }


    [HttpPut("EditUserJobInfo")]
    public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
    {
        string sql = $@"
        UPDATE TutorialAppSchema.UserJobInfo
            SET [JobTitle] = '{userJobInfo.JobTitle}',
            [Department] = '{userJobInfo.Department}'
        WHERE UserId = {userJobInfo.UserId}
        ";
        Console.WriteLine(sql);

        if(_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to update user job info");
    } 


    [HttpPost("AddUserJobInfo")]
    public IActionResult AddUserJobInfo(UserJobInfoToAddDto userJobInfo)
    {
        string sql = $@"
         INSERT INTO TutorialAppSchema.UserJobInfo(
            [JobTitle],
            [Department]
        ) VALUES (
            '{userJobInfo.JobTitle}',
            '{userJobInfo.Department}'
        )
        ";

        if(_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Failed to add user");
    }
}