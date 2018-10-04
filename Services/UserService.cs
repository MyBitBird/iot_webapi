using IOT.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

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
    
}
}