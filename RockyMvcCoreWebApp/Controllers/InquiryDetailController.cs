using Microsoft.AspNetCore.Mvc;
using RockyMvcCoreWebApp_DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockyMvcCoreWebApp.Controllers
{
    public class InquiryDetailController : Controller
    {

        private readonly ApplicationDbContext _db;

        public InquiryDetailController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
