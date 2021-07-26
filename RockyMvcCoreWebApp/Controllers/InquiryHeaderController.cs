using Microsoft.AspNetCore.Mvc;
using RockyMvcCoreWebApp_DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockyMvcCoreWebApp.Controllers
{
    public class InquiryHeaderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public InquiryHeaderController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
