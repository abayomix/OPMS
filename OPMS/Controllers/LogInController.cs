using Business_PMS.Abstract;
using Business_PMS.Logics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using OPMS.Models;
using Repo_PMS.Models;

namespace OPMS.Controllers
{
    public class LogInController : Controller
    {
        IBusiness _Brepo = new Logic_PMS();
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login Login, string Btn_click)
        {
            if (!ModelState.IsValid)
            {
                return View(Login);
            }
            else
            {
                user u = _Brepo.validateuser(Login, Btn_click);

                if (u.response == 200)
                {
                    if (u.userid != null)
                    {
                        if (u.IsVAliduser == "True")
                        {
                            HttpContext.Session.SetString("IsUserValid", "True");
                            HttpContext.Session.SetString("uid", u.userid);
                            HttpContext.Session.SetString("EmpID", u.empId);
                            HttpContext.Session.SetString("PwdChgStatus", u.pwdchgstatus);
                            HttpContext.Session.SetString("URole", u.role);
                            HttpContext.Session.SetString("UDeg", u.udeg);
                            HttpContext.Session.SetString("UDept", u.udept);
                            HttpContext.Session.SetString("Utype", u.utype);
                            

                            return RedirectToAction("USerRedirection", "LogIn");
                        }
                        else
                        {
                            TempData["Message"] = "InCorrect Password ";
                            return View(Login);
                        }

                    }
                    else
                    {
                        TempData["Message"] = "User Not found You Tried With :" + Btn_click;
                        return View(Login);
                    }
                }
                else
                {
                    TempData["Message"] = "Internal Error Server Not Responded";
                    return View(Login);
                }

            }

        }

        public IActionResult USerRedirection()
        {
            if (HttpContext.Session.GetString("IsUserValid") != "True")
            {
                return RedirectToAction("Login", "LogIn");
            }
            else
            {
                string utype = HttpContext.Session.GetString("Utype");
                string changePwdSts = HttpContext.Session.GetString("PwdChgStatus");

                if (utype == "Self")
                {
                    if (changePwdSts == "True")
                    {
                        string getrole = HttpContext.Session.GetString("URole");

                        if (getrole == "Admin" || getrole == "HR")
                        {

                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {

                            return RedirectToAction("Index", "User");
                        }
                    }
                    else
                    {
                        return RedirectToAction("ChangePassword");
                    }
                }

                else
                {
                    string getrole = HttpContext.Session.GetString("URole");

                    if (getrole == "Admin" || getrole == "HR")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "User");
                    }

                }

            }

        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (HttpContext.Session.GetString("IsUserValid") == "True")
            {
                string Utype = HttpContext.Session.GetString("Utype");
                if (Utype == "Self")
                {
                    string uid = HttpContext.Session.GetString("uid");
                    ChangePasswordVM CVM = new ChangePasswordVM();
                    CVM.UserId = uid;
                    return View(CVM);
                }
                else
                {
                    TempData["Messege"] = "Change Password allowed for OPMS User only";

                    return RedirectToAction("USerRedirectToAction", "LogIn");



                }
            }
            else
            {
                return RedirectToAction("LogIn", "LogIn");
            }
            

        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordVM CVM) 
        { 
            string response = _Brepo.Changepassword(CVM);   
            if(response == "success")
            {
                HttpContext.Session.SetString("pwdChgStatus", "True");
                TempData["Message"] = "Password Changed Successfully";
                return RedirectToAction("USerReDirectToAction", "LogIn");

            }
            else
            {
                return View(CVM);   
            }
        }

       

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "LogIn");

        }
    }

 
}
