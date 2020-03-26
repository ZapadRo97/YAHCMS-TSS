using System;
using System.Collections.Generic;
using YAHCMS.UserService.Models;

namespace YAHCMS.UserService.Persistence {
    public interface IUserRepository 
    {
        IEnumerable<User> GetAll();
        User Get(string userID);
        User Add(User user);
        User Delete(string userID);
        User Update(User user);
    }
}