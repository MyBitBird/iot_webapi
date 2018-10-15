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
            _context = context;
        }

        public async Task<Users> GetById(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(x=>x.Id==id);
        }
        public async Task<Users> GetByIdAndParentId(Guid id,Guid parentId)
        {
            return await _context.Users.Include(i=>i.ServiceUsers).FirstOrDefaultAsync(x => x.Id == id && x.ParentUserId==parentId);
        }

        public Users Authenticate(string username, string password)
        {
            return _context.Users.FirstOrDefault(x => x.Username == username && x.Password == password);//#warning always encrypt your password!
        }

        public async Task<Users> SignUp(Users user)
        {
            user.RegisterDate = DateTime.Now;
            user.Type = (byte)MyEnums.UserTypes.ADMIN; ;
            user.Status = (byte)MyEnums.UserStatus.ACTIVE;
            user.Username = user.Username.ToLower();
            user.ServiceUsers = null;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;

        }

        public async Task<Users> AddUser(Users user, Guid parentUserId, Models.Services[] validServices)
        {
            user.RegisterDate = DateTime.Now;
            user.Type = (byte)MyEnums.UserTypes.DEVICE;
            user.Status = (byte)MyEnums.UserStatus.ACTIVE;
            user.Username = user.Username.ToLower();
            user.ParentUserId = parentUserId;
            foreach (var service in user.ServiceUsers)
            {
                service.RegisterDate = DateTime.Now;
                if (!validServices.Any(x => x.Id == service.ServiceId)) return null;
            }

            //user.Password = hash user.Password
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;

        }

        public async Task<bool> EditProfile(Guid userId,Users user,String oldPassword)
        {
            Users oldUser= await GetById(userId);
            oldUser.Username = user.Username;
            oldUser.Name=user.Name;
            oldUser.Family=user.Family;
            if(!user.Password.Trim().Equals(""))
            {
                if(!oldUser.Password.Equals(oldPassword))
                    return false;
                oldUser.Password=user.Password;    
            }
            _context.Users.Update(oldUser);
            await _context.SaveChangesAsync();
            return true;
            
        }

        public async Task<bool> UpdateSubUsers(Guid id,Guid parentId,Users user,Models.Services[] validServices)
        {
            Users oldUser = await GetByIdAndParentId(id,parentId);
            oldUser.Username = user.Username;
            oldUser.Name = user.Name;
            oldUser.Family = user.Family;
            oldUser.Password =  user.Password.Trim().Equals("") ?  oldUser.Password : user.Password;
            
            foreach(var service in oldUser.ServiceUsers)
            {
                service.Deleted=true;
                _context.ServiceUsers.Update(service);
            }

            foreach (var service in user.ServiceUsers)
            {
                service.RegisterDate = DateTime.Now;
                service.UserId=id;
                if (!validServices.Any(x => x.Id == service.ServiceId)) return false;
                await _context.ServiceUsers.AddAsync(service);
            }

            _context.Users.Update(oldUser);
            await _context.SaveChangesAsync();
            return true;

        }



    }
}