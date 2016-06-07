using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using TransferDesk.Contracts.Manuscript.ComplexTypes.UserRole;
using TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.DAL.Manuscript.Repositories;
using TransferDesk.Services.Manuscript;
using TransferDesk.Services.Manuscript.ViewModel;

namespace TransferDesk.MS.Web.Controllers
{
    public class ManuscriptLoginController : Controller
    {
        private readonly ManuscriptDBRepositoryReadSide _manuscriptDBRepositoryReadSide;
        private ManuscriptLoginDBRepositoryReadSide ManuscriptLoginDbRepositoryReadSide { get; set; }
        private readonly ManuscriptLoginService _manuscriptLoginService;
        private readonly string _conString;
        private string _errormsg = String.Empty;
        public ManuscriptLoginController()
        {
            _conString = Convert.ToString(ConfigurationManager.AppSettings["dbTransferDeskService"]);
            _manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(_conString);
            ManuscriptLoginDbRepositoryReadSide = new ManuscriptLoginDBRepositoryReadSide(_conString);
            _manuscriptLoginService = new ManuscriptLoginService(_conString);
        }

        private void ManuscriptLoginVmDetails(ManuscriptLoginVM manuscriptLoginVm, int crestId)
        {
            ManuscriptLogin manuscriptLogin;

            var manuscriptLoginDetails = new ManuscriptLoginDetails();
            manuscriptLogin = ManuscriptLoginDbRepositoryReadSide.GetManuscriptByCrestID(crestId);
            manuscriptLoginVm.CrestId = manuscriptLogin.CrestId;
            manuscriptLoginVm.SpecialInstruction = manuscriptLogin.SpecialInstruction;
            //change as per id
            if (manuscriptLogin.ServiceTypeStatusId == 9)
            {
                manuscriptLoginDetails = ManuscriptLoginDbRepositoryReadSide.GetManuscriptLoginDetails(manuscriptLogin.CrestId, ManuscriptLoginDbRepositoryReadSide.MSServiceTypeID());
            }
            else
            {
                manuscriptLoginDetails = ManuscriptLoginDbRepositoryReadSide.GetManuscriptLoginDetails(manuscriptLogin.CrestId, manuscriptLogin.ServiceTypeStatusId);
            }
            if (manuscriptLoginDetails != null)
            {
                if (manuscriptLoginDetails.UserRoleId != null && manuscriptLoginDetails.UserRoleId != 0)
                {
                    var usernameID = ManuscriptLoginDbRepositoryReadSide.GetUserID(Convert.ToInt32(manuscriptLoginDetails.UserRoleId)).UserID;
                    manuscriptLoginVm.Associate = _manuscriptDBRepositoryReadSide.EmployeeName(usernameID);
                }
            }
            manuscriptLoginVm.InitialSubmissionDate = manuscriptLogin.InitialSubmissionDate;
            manuscriptLoginVm.ManuscriptFilePath = manuscriptLogin.ManuscriptFilePath;
            manuscriptLoginVm.ServiceTypeID = manuscriptLogin.ServiceTypeStatusId;
            manuscriptLoginVm.ArticleTypeID = manuscriptLogin.ArticleTypeId;
            manuscriptLoginVm.JournalID = manuscriptLogin.JournalId;
            manuscriptLoginVm.MSID = manuscriptLogin.MSID;
            manuscriptLoginVm.SectionID = manuscriptLogin.SectionId;
            manuscriptLoginVm.ArticleTitle = manuscriptLogin.ArticleTitle;
            manuscriptLoginVm.ReceivedDate = manuscriptLogin.ReceivedDate;
            manuscriptLoginVm.TaskID = manuscriptLogin.TaskID;
        }

        private void ManuscriptBookLoginVmDetails(ManuscriptBookLoginVM manuscriptBookLoginVm, int crestId)
        {
            ManuscriptBookLogin manuscriptBookLogin;
            var manuscriptLoginDetails = new ManuscriptBookLoginDetails();
            try
            {
                manuscriptBookLogin = ManuscriptLoginDbRepositoryReadSide.GetManuscriptBookLoginByCrestID(crestId);
                manuscriptLoginDetails = ManuscriptLoginDbRepositoryReadSide.GetManuscriptBookLoginDetails(manuscriptBookLogin.ID, manuscriptBookLogin.ServiceTypeID);
                if (manuscriptLoginDetails != null)
                {
                    if (manuscriptLoginDetails.UserRoleId != null && manuscriptLoginDetails.UserRoleId != 0)
                    {
                        var usernameID = ManuscriptLoginDbRepositoryReadSide.GetUserID(Convert.ToInt32(manuscriptLoginDetails.UserRoleId)).UserID;
                        manuscriptBookLoginVm.AssociateName = _manuscriptDBRepositoryReadSide.EmployeeName(usernameID);
                    }
                }
                var JsBookLinkAndGPUInformation = GetBookLink(manuscriptBookLogin.BookMasterID);

                manuscriptBookLoginVm.BookMasterId = manuscriptBookLogin.BookMasterID;
                manuscriptBookLoginVm.ChapterNumber = manuscriptBookLogin.ChapterNumber;
                manuscriptBookLoginVm.FTPLink = ((BookMaster)(JsBookLinkAndGPUInformation.Data)).FTPLink;
                manuscriptBookLoginVm.GPUInformation = ((BookMaster)(JsBookLinkAndGPUInformation.Data)).GPUInformation;
                manuscriptBookLoginVm.ChapterTitle = manuscriptBookLogin.ChapterTitle;
                manuscriptBookLoginVm.PageCount = manuscriptBookLogin.PageCount;
                manuscriptBookLoginVm.ReceivedDate = manuscriptBookLogin.ReceivedDate;
                manuscriptBookLoginVm.RequesterName = manuscriptBookLogin.RequesterName;
                manuscriptBookLoginVm.SpecialInstruction = manuscriptBookLogin.SpecialInstruction;
                manuscriptBookLoginVm.ServiceTypeID = manuscriptBookLogin.ServiceTypeID;
                manuscriptBookLoginVm.StatusMasterTaskID = manuscriptBookLogin.StatusMasterTaskID;
            }
            catch (Exception ex) { }
        }

        [HttpPost]
        public ActionResult BookLogin(ManuscriptBookLoginVM manuscriptBookLoginVM)
        {
            manuscriptBookLoginVM.userId = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            if (manuscriptBookLoginVM.ID != 0)
            {
                if(ManuscriptLoginDbRepositoryReadSide.IsBookCrestIDLogin(manuscriptBookLoginVM.ServiceTypeID, manuscriptBookLoginVM.BookMasterId, manuscriptBookLoginVM.ChapterNumber, manuscriptBookLoginVM.ChapterTitle))
                    TempData["msg"] = "<script>alert('Job is already loggedin');</script>";
            }
            IDictionary<string, string> dataErrors = new Dictionary<string, string>();
            if (manuscriptBookLoginVM.ID == 0)
            {
                AddManuscriptBookLoginInfo(manuscriptBookLoginVM, dataErrors);
                TempData["msg"] = "<script>alert('Record added succesfully');</script>";
            }
            else
            {
                AddManuscriptBookLoginInfo(manuscriptBookLoginVM, dataErrors);
                TempData["msg"] = "<script>alert('Record updated succesfully');</script>";
            }

            return RedirectToAction("BookLogin", new { id = 0 });
        }
        private void AddManuscriptBookLoginInfo(ManuscriptBookLoginVM manuscriptBookLoginVM, IDictionary<string, string> dataErrors)
        {
            //code to add record
            var manuscriptBookLogin = new ManuscriptBookLogin();

            _manuscriptLoginService.SaveManuscriptBookLoginVM(dataErrors, manuscriptBookLoginVM, manuscriptBookLogin);
            //TempData["msg"] = "<script>alert('Record updated succesfully');</script>";
        }

        [HttpPost]
        public ActionResult ManuscriptLogin(ManuscriptLoginVM manuscriptLoginVm, HttpPostedFileBase manuscriptFilePath)
        {
            manuscriptLoginVm.userID = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            manuscriptLoginVm.MSID = manuscriptLoginVm.MSID.Trim();
            IDictionary<string, string> dataErrors = new Dictionary<string, string>();
            if (!String.IsNullOrEmpty(manuscriptLoginVm.Associate))
            {
                var empInfo = _manuscriptDBRepositoryReadSide.GetAssociateName(manuscriptLoginVm.Associate);
                if (empInfo.Count() > 0)
                    manuscriptLoginVm.userID = empInfo.FirstOrDefault().UserID;
            }
            if (manuscriptLoginVm.CrestId == 0)
            {
                if (manuscriptLoginVm.IsRevision)
                {
                    AddManuscriptLoginInfo(manuscriptLoginVm, dataErrors);
                }
                else
                {
                    if (!ManuscriptLoginDbRepositoryReadSide.IsMSIDAvailable(manuscriptLoginVm.MSID, manuscriptLoginVm.CrestId, manuscriptLoginVm.ServiceTypeID))
                        TempData["MSIDError"] = "<script>alert('Manuscript Number is already present.');</script>";
                    else
                    {
                        AddManuscriptLoginInfo(manuscriptLoginVm, dataErrors);
                    }
                }
            }
            else
            {
                if (manuscriptLoginVm.IsRevision)
                {
                    AddManuscriptLoginInfo(manuscriptLoginVm, dataErrors);
                }
                else
                {
                    if (ManuscriptLoginDbRepositoryReadSide.IsMSIDAvailable(manuscriptLoginVm.MSID, manuscriptLoginVm.CrestId, manuscriptLoginVm.ServiceTypeID))
                        TempData["MSIDError"] = "<script>alert('Manuscript Number is already present.');</script>";
                    else
                    {
                        var manuscriptLogin = new ManuscriptLogin();
                        manuscriptLogin = ManuscriptLoginDbRepositoryReadSide.GetManuscriptByCrestID(manuscriptLoginVm.CrestId);
                        //code to updated record
                        _manuscriptLoginService.SaveManuscriptLoginVM(dataErrors, manuscriptLoginVm, manuscriptLogin);
                        TempData["msg"] = "<script>alert('Record updated succesfully');</script>";
                    }
                }
            }
            return RedirectToAction("JournalLogin", new { id = 0 });
        }

        private void AddManuscriptLoginInfo(ManuscriptLoginVM manuscriptLoginVm, IDictionary<string, string> dataErrors)
        {
            //code to add record
            var manuscriptLogin = new ManuscriptLogin();
            //if new record or revision then add entry into db
            manuscriptLoginVm.CrestId = 0;
            _manuscriptLoginService.SaveManuscriptLoginVM(dataErrors, manuscriptLoginVm, manuscriptLogin);
            TempData["msg"] = "<script>alert('Record added succesfully');</script>";
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetAssociateName(string searchText)
        {
            return this.Json(_manuscriptDBRepositoryReadSide.GetAssociateName(searchText), JsonRequestBehavior.AllowGet);
        }

        public void SendMail(Dictionary<string, string> dicReplace, string emailTemplatePath, string emailSubject, string emailFrom, string emailTo, string emailCC, string emailBCC)
        {
            var reviewerService = new ReviewerService(_conString, _conString);
            emailTemplatePath = emailTemplatePath;
            var emailBody = new StringBuilder(System.IO.File.ReadAllText(emailTemplatePath));
            foreach (var kvp in dicReplace)
            {
                emailBody.Replace(kvp.Key, kvp.Value);
            }
            var objEmail = new Email();
            objEmail.SendEmail(emailTo, emailFrom, emailCC, emailBCC, emailSubject, Convert.ToString(emailBody));

            //save mail details
            reviewerService.SaveMailDetails(dicReplace, emailTo, emailFrom, emailCC, emailBCC, emailSubject, Convert.ToString(emailBody));
        }

        public bool ValidateMsidIsOpen(string msid)
        {
            return ManuscriptLoginDbRepositoryReadSide.IsMsidOpen(msid);
        }

        public bool IsMsidAvaialable(string msid, int serviceTypeStatusId)
        {
            return ManuscriptLoginDbRepositoryReadSide.IsMSIDAvailable(msid, 0, serviceTypeStatusId);
        }

        public string GetJournalLink(int journalId)
        {
            return _manuscriptDBRepositoryReadSide.GetJournalList().Find(x => x.ID == journalId).Link;
        }

        public JsonResult GetBookLink(int bookTitleId)
        {
            return this.Json(_manuscriptDBRepositoryReadSide.GetManuscriptBookTitle().Find(x => x.ID == bookTitleId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManuscriptLoginExportResult(string FromDate, string ToDate)
        {
            if (FromDate == "" || ToDate == "")
            {
                TempData["msg"] = "<script>alert('Please select Date');</script>";
                return RedirectToAction("ManuscriptLogin");
            }

            var grid = new GridView();
            var manuscriptLoginExportJobs = ManuscriptLoginDbRepositoryReadSide.GetManuscriptLoginJobsDetailsForExcel(FromDate, ToDate);
            var countData = manuscriptLoginExportJobs.Count();
            if (countData > 0)
            {
                grid.DataSource = manuscriptLoginExportJobs;
                grid.DataBind();
                grid.HeaderStyle.Font.Bold = true;
                grid.HeaderRow.BackColor = System.Drawing.Color.LightGray;
                grid.HeaderRow.Cells[0].Text = "Crest ID";
                grid.HeaderRow.Cells[1].Text = "Service Type";
                grid.HeaderRow.Cells[2].Text = "Journal Title";
                grid.HeaderRow.Cells[3].Text = "Manuscript Number";
                grid.HeaderRow.Cells[4].Text = "Article Type";
                grid.HeaderRow.Cells[5].Text = "Section";
                grid.HeaderRow.Cells[6].Text = "Journal Link";
                grid.HeaderRow.Cells[7].Text = "Article Title";
                grid.HeaderRow.Cells[8].Text = "Special Instruction";
                grid.HeaderRow.Cells[9].Text = "Associate";
                grid.HeaderRow.Cells[10].Text = "Initial Submission Date";
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ManuscriptLogin" + DateTime.Now.ToShortDateString() + ".xls"));
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
                return RedirectToAction("ManuscriptLogin");
            }

        }

        public ActionResult ManuscriptBookLoginExportResult(string FromDate, string ToDate)
        {
            if (FromDate == "" || ToDate == "")
            {
                TempData["msg"] = "<script>alert('Please select Date');</script>";
                return RedirectToAction("BookLogin");
            }

            var grid = new GridView();
            var manuscriptBookLoginExportJobs = ManuscriptLoginDbRepositoryReadSide.GetManuscriptBookLoginJobsDetailsForExcel(FromDate, ToDate);
            var countData = manuscriptBookLoginExportJobs.Count();
            if (countData > 0)
            {
                grid.DataSource = manuscriptBookLoginExportJobs;
                grid.DataBind();
                grid.HeaderStyle.Font.Bold = true;
                grid.HeaderRow.BackColor = System.Drawing.Color.LightGray;
                grid.HeaderRow.Cells[0].Text = "Book Title";
                grid.HeaderRow.Cells[1].Text = "Crest ID";
                grid.HeaderRow.Cells[2].Text = "Chapter Number";
                grid.HeaderRow.Cells[3].Text = "FTP Path";
                grid.HeaderRow.Cells[4].Text = "GPU Infromation";
                grid.HeaderRow.Cells[5].Text = "Chapter Title";
                grid.HeaderRow.Cells[6].Text = "Page Count";
                grid.HeaderRow.Cells[7].Text = "Received Date";
                grid.HeaderRow.Cells[8].Text = "Requester Name";
                grid.HeaderRow.Cells[9].Text = "Associate Name";
                grid.HeaderRow.Cells[10].Text = "Special Instruction";
                grid.HeaderRow.Cells[11].Text = "Service Type";
                grid.HeaderRow.Cells[12].Text = "Task";
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ManuscriptBookLogin" + DateTime.Now.ToShortDateString() + ".xls"));
                Response.ContentType = "application/ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                grid.RenderControl(htw);
                Response.Write(sw.ToString());
                Response.Flush();
                Response.End();
                return View("BookLogin");
            }
            else
            {
                TempData["msg"] = "<script>alert('No Record Found');</script>";
                return RedirectToAction("BookLogin");
            }

        }

        public JsonResult GetManuscriptLoginedJobByMsid(string msid)
        {
            return this.Json(ManuscriptLoginDbRepositoryReadSide.GetManuscriptDetailsByMsid(msid), JsonRequestBehavior.AllowGet);
        }

        public ActionResult BookLogin(int? id, string jobtype)
        {
            var manuscriptBookLoginVm = new ManuscriptBookLoginVM();
            var crestId = Convert.ToInt32(id);
            if (id != null && id != 0 && jobtype == "book")
            {
                ManuscriptBookLoginVmDetails(manuscriptBookLoginVm, crestId);
            }
            manuscriptBookLoginVm.TaskList = _manuscriptDBRepositoryReadSide.GetTaskType();
            manuscriptBookLoginVm.ServiceTypes = _manuscriptDBRepositoryReadSide.GetServiceType();
            manuscriptBookLoginVm.BookTitleList = _manuscriptDBRepositoryReadSide.GetManuscriptBookTitle();
            manuscriptBookLoginVm.ManuscriptBookLoginedJobs = ManuscriptLoginDbRepositoryReadSide.GetManuscriptBookLoignedJobs();
            return View("BookLogin", manuscriptBookLoginVm);
        }

        public ActionResult JournalLogin(int? id, string jobtype)
        {
            var manuscriptLoginVm = new ManuscriptLoginVM();
            var userId = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            manuscriptLoginVm.Journal = _manuscriptDBRepositoryReadSide.GetJournalList();
            manuscriptLoginVm.ArticleType = _manuscriptDBRepositoryReadSide.GetArticleList(Convert.ToInt32(manuscriptLoginVm.JournalID));
            manuscriptLoginVm.Section = _manuscriptDBRepositoryReadSide.GetSectionList(Convert.ToInt32(manuscriptLoginVm.JournalID));
            manuscriptLoginVm.TaskList = _manuscriptDBRepositoryReadSide.GetTaskType();
            manuscriptLoginVm.ServiceType = _manuscriptDBRepositoryReadSide.GetServiceType();
            manuscriptLoginVm.EmployeeName = _manuscriptDBRepositoryReadSide.EmployeeName(userId);
            manuscriptLoginVm.ManuscriptLoginedJobs = ManuscriptLoginDbRepositoryReadSide.GetManuscriptLoginJobs();
            var crestId = Convert.ToInt32(id);
            if (id != null && id != 0)
            {
                var serviceTypeid = _manuscriptDBRepositoryReadSide.GetServiceID(crestId);
                var jobstatusisOnHold = _manuscriptDBRepositoryReadSide.CheckJobStatusForHold(crestId, serviceTypeid);
                if (jobstatusisOnHold == false)
                {

                    _errormsg = "This job is on hold and is not editable.";
                    TempData["msg1"] = "<script>alert(\"" + _errormsg + "\");</script>";
                    return PartialView("JournalLogin", manuscriptLoginVm);
                }
                ManuscriptLoginVmDetails(manuscriptLoginVm, crestId);
            }
            return PartialView("JournalLogin", manuscriptLoginVm);
        }

    }
}