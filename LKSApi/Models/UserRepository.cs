using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LKSApi.Models
{
    public class UserRepository : IDisposable
    {
        lksEntities context = new lksEntities();

        public tbluser ValidateUser(string username, string password)
        {
            return context.tblusers.FirstOrDefault(e => e.username.Equals(username, StringComparison.OrdinalIgnoreCase) && e.pass == password);
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}