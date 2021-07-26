using Microsoft.AspNetCore.Mvc.Rendering;
using RockyMvcCoreWebApp_DataAccess.Data;
using RockyMvcCoreWebApp_DataAccess.Repository.IRepository;
using RockyMvcCoreWebApp_Models;
using RockyMvcCoreWebApp_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockyMvcCoreWebApp_DataAccess.Repository
{
    public class InquiryDetailRepository : Repository<InquiryDetail>, IInquiryDetailRepository
    {
        private readonly ApplicationDbContext _db;
        public InquiryDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(InquiryDetail obj)
        {
            _db.InquiryDetail.Update(obj);
        }
    }
}
