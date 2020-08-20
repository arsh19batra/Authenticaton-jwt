using System;
using System.Collections.Generic;
using System.Linq;
using proj.Models;

namespace proj.Data
{
    public class SqlProjRepo : IProjRepo
    {
        private readonly ProjContext _context;

        public SqlProjRepo(ProjContext context)
        {
            _context=context;
        }

        public void CreateDetail(Detail dtl)
        {
            if(dtl == null)
            {
                throw new ArgumentNullException(nameof(dtl));
            }
            _context.Details.Add(dtl);
        }

        public void DeleteDetail(Detail dtl)
        {
            if(dtl == null)
            {
                throw new ArgumentNullException(nameof(dtl));
            }
            _context.Details.Remove(dtl);
        }

        public IEnumerable<Detail> GetAllDetails()
        {
            return _context.Details.ToList();
        }

        public Detail GetDetailById(int id)
        {
            return _context.Details.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges()>=0);
        }

        public void UpdateDetail(Detail dtl)
        {
            
        }
        public Detail Authenticate(string username, string password)
        {
            if(string.IsNullOrEmpty(username)||string.IsNullOrEmpty(password))
            {
                return null;
            }
            var user = _context.Details.SingleOrDefault(x=>x.uname==username);
            if(user==null)
            {
                return null;
            }
            if(password!=user.password)
            {
                return null; 
            }
            return user;
        }
    }
}