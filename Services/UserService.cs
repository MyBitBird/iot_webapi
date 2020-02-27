using IOT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IOT.Helper;
using System.Security.Authentication;

namespace IOT.Services
{

    public class UserService
    {
        private readonly IOTContext _context;

        public UserService(IOTContext context)
        {
            _context = context;
        }

        public async Task<Users> GetById(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Users> GetByIdAndParentId(Guid id, Guid parentId)
        {
            var user = await _context.Users.AsNoTracking()
                .Include(i => i.ServiceUsers)
                .FirstOrDefaultAsync(x => x.Id == id &&
                                          x.ParentUserId == parentId &&
                                          x.Status == MyEnums.UserStatus.Active);

            user.ServiceUsers = user.ServiceUsers.Where(x => x.Deleted == false).ToArray();
            return user;
        }

        public async Task<Users[]> GetSubUsers(Guid parentId)
        {
            return await _context.Users.Include(i => i.ServiceUsers).Where(x => x.ParentUserId == parentId && x.Status == MyEnums.UserStatus.Active).ToArrayAsync();
        }

        public Users Authenticate(string username, string password)
        {
            return _context.Users.FirstOrDefault(x => x.Username == username &&
                                                      x.Password == password &&
                                                      x.Status == MyEnums.UserStatus.Active);//TODO #warning always encrypt your password!
        }

        public async Task<Users> SignUp(Users user)
        {
            user = InitUserData(user, MyEnums.UserTypes.Admin);
            user.ServiceUsers = null;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;

        }

        public async Task<Users> AddDeviceUser(Users user, Guid parentUserId, Models.Services[] validServices)
        {
            user = InitUserData(user, MyEnums.UserTypes.Device, parentUserId);

            if (HasInvalidServices(user.ServiceUsers, validServices))
                throw new InvalidOperationException();

            //TODO user.Password = hash user.Password
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;

        }
        private static Users InitUserData(Users user, MyEnums.UserTypes userType, Guid? parentUserId = null)
        {
            user.RegisterDate = DateTime.Now;
            user.Type = userType;
            user.Status = MyEnums.UserStatus.Active;
            user.Username = user.Username.ToLower();
            user.ParentUserId = parentUserId;
            return user;
        }
        private static bool HasInvalidServices(ICollection<ServiceUsers> serviceUsers, Models.Services[] validServices)
        {
            return serviceUsers.Any(service => validServices.All(x => x.Id != service.ServiceId));
        }

        public async Task<bool> EditProfile(Guid userId, Users user, string reOldPassword)
        {
            var oldUser = await GetById(userId);
            oldUser.Username = user.Username;
            oldUser.Name = user.Name;
            oldUser.Family = user.Family;
            try
            {
                oldUser.Password = CheckPasswords(user.Password, oldUser.Password, reOldPassword);
            }
            catch
            {
                return false;
            }

            _context.Users.Update(oldUser);
            await _context.SaveChangesAsync();
            return true;

        }

        private static string CheckPasswords(string newPassword, string oldPassword, string reOldPassword)
        {
            if (newPassword.Trim().Equals(string.Empty)) return oldPassword;
            return oldPassword.Equals(reOldPassword) ? newPassword : throw new AuthenticationException();
        }

        public async Task<bool> UpdateDeviceSubUser(Guid id, Guid parentId, Users user, Models.Services[] validServices)
        {
            var oldUser = await GetByIdAndParentId(id, parentId);
            if (oldUser == null)
                return false;

            oldUser = InitDeviceSubUserData(user, oldUser);

            if (HasInvalidServices(user.ServiceUsers, validServices))
                throw new InvalidOperationException();

            var oldServices = oldUser.ServiceUsers.ToList();
            var newServices = user.ServiceUsers.ToList();

            oldUser.ServiceUsers = null;

            RemoveOldServices(oldServices);
            await AddNewServices(id, newServices);

            _context.Users.Update(oldUser);
            await _context.SaveChangesAsync();
            return true;

        }

        private static Users InitDeviceSubUserData(Users user, Users oldUser)
        {
            oldUser.Username = user.Username;
            oldUser.Name = user.Name;
            oldUser.Family = user.Family;
            oldUser.Password =
                CheckPasswords(user.Password, oldUser.Password,
                    oldUser.Password); //for subUser is not require to check old password
            return oldUser;
        }

        private async Task AddNewServices(Guid userId, IEnumerable<ServiceUsers> services)
        {
            foreach (var service in services)
            {
                service.RegisterDate = DateTime.Now;
                service.UserId = userId;
                await _context.ServiceUsers.AddAsync(service);
            }
        }

        private void RemoveOldServices(IEnumerable<ServiceUsers> oldServices)
        {
            foreach (var service in oldServices)
            {
                service.Deleted = true;
                _context.ServiceUsers.Update(service);
            }
        }

        public async Task<bool> DeleteSubUser(Guid id, Guid parentId)
        {
            var oldUser = await GetByIdAndParentId(id, parentId);
            if (oldUser == null) return false;

            oldUser.Status = MyEnums.UserStatus.Deleted;
            _context.Users.Update(oldUser);
            _context.SaveChanges();
            return true;

        }
    }
}