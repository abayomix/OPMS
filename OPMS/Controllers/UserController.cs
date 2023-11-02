﻿using Microsoft.AspNetCore.Mvc;

namespace OPMS.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("IsUserValid") != "True")
            {
                return RedirectToAction("Login", "LogIn");
            }
            else
            {

                return View();
            }

        }
     
    }
}
