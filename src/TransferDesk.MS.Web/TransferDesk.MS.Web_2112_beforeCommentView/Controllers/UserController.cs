using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TransferDesk.DAL.Manuscript;
using TransferDesk.DAL.Manuscript.DataContext;

using TransferDesk.Contracts.Manuscript.Repositories;
using TransferDesk.Contracts.Manuscript.Entities;


namespace TransferDesk.MS.Web.Controllers
{
    public class UserController : Controller
    {
        private IUserRepository _UserRepository;

        public UserController()
        {
            string conString = "data source=192.168.84.149;initial catalog=dbTransferDeskService;user id=sa;password=admin@123;MultipleActiveResultSets=True;App=EntityFramework";
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

            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult UserMaster(UserRoleViewModel userRoleViewModel)
        //{
        //    return View();
        //}

        //

      
    }
}
