using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using TransferDesk.DAL.Manuscript.Repositories;
using TransferDesk.Services.Manuscript;
using TransferDesk.Services.Manuscript.ViewModel;

namespace TransferDesk.MS.Web.Controllers
{
    public class AssociateDashboardController : Controller
    {
        //
        // GET: /AssociateDashboard/


        private readonly ManuscriptDBRepositoryReadSide _manuscriptDbRepositoryReadSide;
        private AssociateDashboardVM associateDasboardVM;
        private AssociateDashBoardReposistory _associateDashBoardReposistory;

        public AssociateDashboardController()
        {
            var conString = Convert.ToString(ConfigurationManager.AppSettings["dbTransferDeskService"]);
            _manuscriptDbRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);
            associateDasboardVM = new AssociateDashboardVM();
            _associateDashBoardReposistory = new AssociateDashBoardReposistory(conString);
        }

        [HttpGet]
        public ActionResult AssociateDashboard()
        {

            var userId = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            //var roleIds = _manuscriptDbRepositoryReadSide.GetUserRoles(userId);
            //if (roleIds.GetValue(0) == "1")
            //{
            //    TempData["msg"] = "<script>alert('No Record Found');</script>";
            //    return View(associateDasboardVM);
            //}
            associateDasboardVM.specificAssociatedetails = _associateDashBoardReposistory.pr_GetAllAssociatesAssignedJobs(userId);
            return View(associateDasboardVM);


        }
    }
}