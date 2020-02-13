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
            Users user= await _context.Users.AsNoTracking().Include(i=>i.ServiceUsers).FirstOrDefaultAsync(x => x.Id == id && x.ParentUserId == parentId && x.Status == MyEnums.UserStatus.ACTIVE);
            user.ServiceUsers = user.ServiceUsers.Where(x => x.Deleted == false).ToArray();
            return user;
        }

        public async Task<Users[]> GetSubUsers(Guid parentId)
        {
            return await _context.Users.Include(i => i.ServiceUsers).Where(x => x.ParentUserId == parentId && x.Status == MyEnums.UserStatus.ACTIVE).ToArrayAsync();
        }

        public Users Authenticate(string username, string password)
        {
            return _context.Users.FirstOrDefault(x => x.Username == username && x.Password == password && x.Status == MyEnums.UserStatus.ACTIVE);//#warning always encrypt your password!
        }

        public async Task<Users> SignUp(Users user)
        {
            user.RegisterDate = DateTime.Now;
            user.Type = MyEnums.UserTypes.ADMIN; ;
            user.Status = MyEnums.UserStatus.ACTIVE;
            user.Username = user.Username.ToLower();
            user.ServiceUsers = null;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;

        }

        public async Task<Users> AddUser(Users user, Guid parentUserId, Models.Services[] validServices)
        {
            user.RegisterDate = DateTime.Now;
            user.Type = MyEnums.UserTypes.DEVICE;
            user.Status = MyEnums.UserStatus.ACTIVE;
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
            if (oldUser == null) return false;

            oldUser.Username = user.Username;
            oldUser.Name = user.Name;
            oldUser.Family = user.Family;
            oldUser.Password =  user.Password.Trim().Equals("") ?  oldUser.Password : user.Password;

            List<ServiceUsers> oldServices = oldUser.ServiceUsers.ToList();

            List<ServiceUsers> newServices = user.ServiceUsers.ToList();

            oldUser.ServiceUsers = null ; //to get ride of exception collection was of a fixed size error

            foreach(var service in oldServices)
            {
                service.Deleted=true;
                _context.ServiceUsers.Update(service);
            }

            foreach (var service in newServices)
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

        public async Task<bool> DeleteSubUser(Guid id,Guid parentId)
        {
            Users oldUser = await GetByIdAndParentId(id, parentId);
            if(oldUser == null) return false;

            oldUser.Status = MyEnums.UserStatus.DELETED;
            _context.Users.Update(oldUser);
            _context.SaveChanges();
            return true;

        }

    }
}