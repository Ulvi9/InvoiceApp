using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ulvi.Controllers
{
    public class BioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
