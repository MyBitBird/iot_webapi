using IOT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using IOT.Helper;
using System.Data;

namespace IOT.Services
{

public class UserService
{
    IOTContext _context;

    public UserService(IOTContext context)
    {
        _context=context;
    }

    public Users Authenticate(string username,string password)
    {
        return _context.Users.FirstOrDefault(x=>x.Username==username && x.Password==password);//#warning always encrypt your password!
    }

    public async Task<Users> SignUp(Users user)
    {
        user.RegisterDate=DateTime.Now;
        user.Type=(byte)MyEnums.UserTypes.ADMIN;;
        user.Status=(byte)MyEnums.UserStatus.ACTIVE;
        user.Username=user.Username.ToLower();
        user.ServiceUsers=null;
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;

    }

        public async Task<Users> AddUser(Users user, Models.Services[] validServices)
        {
            user.RegisterDate = DateTime.Now;
            user.Type = (byte)MyEnums.UserTypes.DEVICE;
            user.Status = (byte)MyEnums.UserStatus.ACTIVE;
            user.Username = user.Username.ToLower();
            foreach (var service in user.ServiceUsers)
            {
                service.RegisterDate=DateTime.Now;
               if(!validServices.Any(x=>x.Id==service.ServiceId)) return null;
            }

            //user.Password = hash user.Password
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;

        }

    
    
}
}