using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using virtual_diary.Models;
using virtual_diary.Models.DTO;

namespace virtual_diary.Services
{
    public interface IParentService
    {
        IEnumerable<Parent> GetParents();       
        Parent GetParentById(string id);
        Parent GetParentByUserName(string username);       
        Task<IdentityResult> RegisterParent(ParentDTO parent);
        Parent PutParent(string username, Parent parent);
        bool DeleteParent(string username);
        ParentStudentDTO GetStudentsFromParent(string parentUsername);
    }
}
