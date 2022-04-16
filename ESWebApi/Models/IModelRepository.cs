using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESWebApi.Models
{
    interface IModelRepository
    {
       bool Login(LoginModel login);
    }
}
