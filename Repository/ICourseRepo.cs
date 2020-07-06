using Prunedge_User_Administration_Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prunedge_User_Administration.Repository
{
    public interface ICourseRepo
    {
        List<Courses> GetAll();
        Task<Courses> Get(int id);
        Task<string> Delete(int id);
        void Post(Courses obj);
    }
}
