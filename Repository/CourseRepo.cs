using Prunedge_User_Administration.Data.Entities;
using Prunedge_User_Administration_Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prunedge_User_Administration.Repository
{
    public class CourseRepo: ICourseRepo
    {
        private readonly AdminstrationDbContext _context;

        public CourseRepo(AdminstrationDbContext context)
        {
            _context = context;
        }

        public async Task<string> Delete(int id)
        {
            var response =  _context.Courses.FindAsync(id);
            if (response == null) return ("User not found");
            _context.Remove(response);
            return "";

        }

        public async Task<Courses> Get(int id)
        {
            return   _context.Courses.FirstOrDefault(obj => obj.id == id);
        }

        public List<Courses> GetAll()
        {
            return _context.Courses.ToList();
        }

        public void Post(Courses obj)
        {
             _context.Courses.Add(obj);       
        }
    }
}
