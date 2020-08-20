using System.Collections.Generic;
using proj.Models;

namespace proj.Data
{
    public interface IProjRepo
    {
        bool SaveChanges();
        IEnumerable<Detail> GetAllDetails();
        Detail GetDetailById(int id);
        void CreateDetail(Detail dtl);
        void UpdateDetail(Detail dtl);
        void DeleteDetail(Detail dtl);
        Detail Authenticate(string username, string password);
    }
}