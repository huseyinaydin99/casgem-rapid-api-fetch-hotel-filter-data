using Casgem.RapidAPI.Hotel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Casgem.RapidAPI.Hotel.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}