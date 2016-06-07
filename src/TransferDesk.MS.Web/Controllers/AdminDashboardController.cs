using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransferDesk.Services.Manuscript;
using TransferDesk.Services.Manuscript.ViewModel;
using TransferDesk.Contracts.Manuscript.Repositories;
using TransferDesk.DAL.Manuscript.Repositories;
using System;
using TransferDesk.Contracts.Manuscript.Entities;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics.PerformanceData;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Web.UI;
using DTOs = TransferDesk.Contracts.Manuscript.DTO;
using System.Web.UI.WebControls;
namespace TransferDesk.MS.Web.Controllers
{
    public class AdminDashboardController : Controller
    {

        private AdminDasboardVM adminDasboardVM;
        private AdminDashBoardService adminDashBoardService;
        private AdminDashBoardReposistory _adminDashBoardReposistory;
        private readonly ManuscriptDBRepositoryReadSide _manuscriptDBRepositoryReadSide;

        public AdminDashboardController()
        {
            string conString = string.Empty;
            conString = Convert.ToString(ConfigurationManager.AppSettings["dbTransferDeskService"]);
            adminDasboardVM = new AdminDasboardVM();
            adminDashBoardService = new AdminDashBoardService(conString);
            _adminDashBoardReposistory = new AdminDashBoardReposistory(conString);
            _manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);
        }

        [HttpGet]
        public ActionResult AdminDashBoard()
        {
            adminDasboardVM.jobsdetails = _adminDashBoardReposistory.pr_GetAllJobsDetails();
            return View(adminDasboardVM);
        }


        public bool AdminActionResult(string AssociateNameVM, int CrestIdVM, string ServiceTypeVM, string JobProcessingStatusVM, string RoleVM)
        {
            var adminDash = new AdminDasboardVM();
            adminDash.AssociateNameVM = AssociateNameVM;
            adminDash.CrestIdVM = CrestIdVM;
            adminDash.ServiceTypeVM = ServiceTypeVM;
            adminDash.JobProcessingStatusVM = JobProcessingStatusVM;
            adminDash.RoleVM = RoleVM;
            return adminDashBoardService.AllocateMSIDToUser(adminDash);
        }

        public bool AdminUnallocateMSID(string AssociateNameVM, int CrestIdVM, string ServiceTypeVM, string JobProcessingStatusVM, string RoleVM)
        {
            var adminDash = new AdminDasboardVM();
            adminDash.AssociateNameVM = AssociateNameVM;
            adminDash.CrestIdVM = CrestIdVM;
            adminDash.ServiceTypeVM = ServiceTypeVM;
            adminDash.JobProcessingStatusVM = JobProcessingStatusVM;
            adminDash.RoleVM = RoleVM;
            return adminDashBoardService.UnallocateMSIDFromUser(adminDash);

        }

        public bool AdminHoldMSID(string AssociateNameVM, int CrestIdVM, string ServiceTypeVM, string JobProcessingStatusVM, string RoleVM)
        {
            var adminDash = new AdminDasboardVM();
            adminDash.CrestIdVM = CrestIdVM;
            adminDash.AssociateNameVM = AssociateNameVM;
            adminDash.ServiceTypeVM = ServiceTypeVM;
            adminDash.JobProcessingStatusVM = JobProcessingStatusVM;
            adminDash.RoleVM = RoleVM;
            return adminDashBoardService.HoldMSID(adminDash);

        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetAssociateName(string searchAssociate,string RoleName)
        {
            return this.Json(_adminDashBoardReposistory.GetAssociateResult(searchAssociate, RoleName), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string GetLastAssociateName(int crestid, string servicetype)
        {
            var LastAssociateName = _adminDashBoardReposistory.GetLastNameOfAssociate_ForHoldJob(crestid, servicetype);
            if (LastAssociateName.Count > 0)
            {
                foreach (var item in LastAssociateName)
                {
                    return item.empname;
                }
            }
            return null;

        }

        public ActionResult AdminDashBoardExportToExcelData(string FromDate, string ToDate)
        
        {

            if (FromDate == "" || ToDate == "")
            {
                TempData["msg"] = "<script>alert('Please select Date');</script>";                
                return RedirectToAction("AdminDashBoard"); 
            }

            var grid = new GridView();
            var countData = _adminDashBoardReposistory.GetAdminDashBoardExportToExcel(FromDate, ToDate).ToList().Count;
            if (countData > 0)
            {
                grid.DataSource = _adminDashBoardReposistory.GetAdminDashBoardExportToExcel(FromDate, ToDate);
                grid.DataBind();
                grid.HeaderStyle.Font.Bold = true;
                grid.HeaderRow.BackColor = System.Drawing.Color.LightGray;
                grid.HeaderRow.Cells[0].Text = "Sr. No.";
                grid.HeaderRow.Cells[1].Text = "Crest ID";
                grid.HeaderRow.Cells[2].Text = "Job Type";
                grid.HeaderRow.Cells[3].Text = "Service Type";
                grid.HeaderRow.Cells[4].Text = "MSID/Chp No.";
                grid.HeaderRow.Cells[5].Text = "Journal Name/Book title";
                grid.HeaderRow.Cells[6].Text = "PageCount";
                grid.HeaderRow.Cells[7].Text = "Name";
                grid.HeaderRow.Cells[8].Text = "Role";
                grid.HeaderRow.Cells[9].Text = "Status";
                grid.HeaderRow.Cells[10].Text = "Task";
                grid.HeaderRow.Cells[11].Text = "Revision No.";
                grid.HeaderRow.Cells[12].Text = "Group No.";
                grid.HeaderRow.Cells[13].Text = "Received Date";
                grid.HeaderRow.Cells[14].Text = "Logged in date/time";
                grid.HeaderRow.Cells[15].Text = "Fetch date/time";
                grid.HeaderRow.Cells[16].Text = "Age (days)";
                grid.HeaderRow.Cells[17].Text = "Handling time"; 
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition",
                    string.Format("attachment; filename={0}", "ManuscriptAdminDashBoard" + DateTime.Now.ToShortDateString() + ".xls"));
                Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grid.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.Flush();
                Response.End();
                return View();
            }
            else
            {
                TempData["msg"] = "<script>alert('No Record Found');</script>";
                return RedirectToAction("AdminDashBoard");
            }
        }
    }
}