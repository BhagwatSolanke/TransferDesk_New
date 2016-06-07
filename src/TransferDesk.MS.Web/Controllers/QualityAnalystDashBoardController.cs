using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransferDesk.DAL.Manuscript.Repositories;
using TransferDesk.Services.Manuscript.ViewModel;

namespace TransferDesk.MS.Web.Controllers
{
    public class QualityAnalystDashBoardController : Controller
    {
        //
        // GET: /QualityAnalystDashBoard/

        private readonly ManuscriptDBRepositoryReadSide _manuscriptDbRepositoryReadSide;
        private QualityAnalystDashBoardVM qualityAnalystDashBoardVm;
        private QualityAnalystDashBoardReposistory _qualityanalystreposistory;


        public QualityAnalystDashBoardController()
        {
            var conString = Convert.ToString(ConfigurationManager.AppSettings["dbTransferDeskService"]);
            _manuscriptDbRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);
            qualityAnalystDashBoardVm = new QualityAnalystDashBoardVM();
            _qualityanalystreposistory = new QualityAnalystDashBoardReposistory(conString);    
        }
        [HttpGet]
        public ActionResult QualityAnalystDashboard()
        {

            var userId = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            // var roleIds = _manuscriptDbRepositoryReadSide.GetUserRoles(userId);            
            qualityAnalystDashBoardVm.specificQualityAnalystdetails = _qualityanalystreposistory.pr_GetAllQualityAnalystAssignedJobs(userId);
            return View(qualityAnalystDashBoardVm);


        }
	}
}