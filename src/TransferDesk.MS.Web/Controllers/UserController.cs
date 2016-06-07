using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TransferDesk.DAL.Manuscript;
using TransferDesk.DAL.Manuscript.DataContext;

using TransferDesk.Contracts.Manuscript.Repositories;
using TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.Services.Manuscript.ViewModel;
using System.Configuration;


namespace TransferDesk.MS.Web.Controllers
{
    public class UserController : Controller
    {
        private UserRepository _UserRepository;
        private UserVM userVM;
        public UserController()
        {
            //    string conString = "data source=192.168.84.149;initial catalog=dbTransferDeskService;user id=sa;password=admin@123;MultipleActiveResultSets=True;App=EntityFramework";

            //string conString = "data source=192.168.84.10;initial catalog=dbTransferDeskService;user id=sa;password=WsSe2003R2;MultipleActiveResultSets=True;App=EntityFramework";
            string conString = Convert.ToString(ConfigurationManager.AppSettings["dbTransferDeskService"]);
            //ManuscriptContext  
            this._UserRepository = new UserRepository((new ManuscriptDBContext(conString)));

        }

        // GET: /User/
        public ActionResult Index()
        {
            var users = from user in _UserRepository.GetUsers()
                        select User;
            return View(users);
        }

        public ActionResult UserMaster()
        {
            string userId = System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            userVM = new UserVM();
            userVM.users = _UserRepository.GetUsers();
            return View(userVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserMaster(User user)
        {
            if (ModelState.IsValid)
            {
                //if (user.ID == 0)
                //{
                //    if(!_UserRepository.IsUserAvailable(user.EmpID,user.EmpUserID,user.ID))
                //    {
                //        _UserRepository.AddUser(user);
                //        TempData["msg"] = "<script>alert('Record added succesfully');</script>";
                //    }
                //    else
                //    {
                //        TempData["msg"] = "<script>alert('Employee ID or User ID is already present.');</script>";
                //    }
                //}
                //else
                //{
                //    if (!_UserRepository.IsUserAvailable(user.EmpID, user.EmpUserID, user.ID))
                //    {
                //        _UserRepository.UpdateUser(user);
                //        TempData["msg"] = "<script>alert('Record updated succesfully');</script>";
                //    }
                //    else
                //    {
                //        TempData["msg"] = "<script>alert('Employee ID or User ID is already present.');</script>";
                //    }
                //}
            }
            return RedirectToAction("UserMaster", "User");
        }




    }
}
