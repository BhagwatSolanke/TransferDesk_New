
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


using TransferDesk.Services.Manuscript;
using TransferDesk.Services.Manuscript.ViewModel;
using TransferDesk.Contracts.Manuscript.Repositories;
using TransferDesk.DAL.Manuscript.Repositories;
using System;
using TransferDesk.Contracts.Manuscript.Entities;
using Newtonsoft.Json;
using System.Configuration;
using System.Text;
using Microsoft.Ajax.Utilities;
using TransferDesk.Services.Manuscript.ReportOutputs;
using TransferDesk.Services.Manuscript.Preview;

namespace TransferDesk.MS.Web.Controllers
{
    public class ManuscriptController : Controller
    {
        private readonly ManuscriptDBRepositoryReadSide _manuscriptDbRepositoryReadSide;
        private readonly ManuscriptService _manuscriptService;

        public ManuscriptController()
        {
            var conString = Convert.ToString(ConfigurationManager.AppSettings["dbTransferDeskService"]);
            _manuscriptService = new ManuscriptService(conString, conString);
            _manuscriptDbRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);
        }

        [HttpGet]
        public ActionResult HomePage(int? id)
        {
            var userId = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            var roleIds = _manuscriptDbRepositoryReadSide.GetUserRoles(userId);
            if (roleIds.Count() > 0)
            {
                ViewBag.RoleList = _manuscriptDbRepositoryReadSide.GetUserRoleList(roleIds);
                ViewBag.SearchList = _manuscriptDbRepositoryReadSide.GetSearchList("j");
                ViewBag.JournalList = _manuscriptDbRepositoryReadSide.GetJournalList();
                ViewBag.JournalStatusList = _manuscriptDbRepositoryReadSide.GetJournalStatusList();
                ViewBag.iThenticateResult = JsonConvert.SerializeObject(_manuscriptDbRepositoryReadSide.GetList(1));
                ViewBag.EnglishLangQuality = JsonConvert.SerializeObject(_manuscriptDbRepositoryReadSide.GetList(2));
                ViewBag.EthicsComplience = JsonConvert.SerializeObject(_manuscriptDbRepositoryReadSide.GetList(3));
                ViewBag.OverallAnalysis = JsonConvert.SerializeObject(_manuscriptDbRepositoryReadSide.GetList(4));
                var manuscriptId = id ?? default(int);

                if (_manuscriptService._manuscriptScreeningBL == null)
                    _manuscriptService._manuscriptScreeningBL = new BAL.Manuscript.ManuscriptScreeningBL();

                if (_manuscriptService._manuscriptScreeningBL._manuscriptDBRepositoryReadSide == null)
                    _manuscriptService._manuscriptScreeningBL._manuscriptDBRepositoryReadSide = _manuscriptDbRepositoryReadSide;

                var manuscriptVm = manuscriptId == 0 ? _manuscriptService.GetManuscriptScreeningDefaultVM() : _manuscriptService.GetManuscriptScreeningVM(manuscriptId);

                var journalId = Convert.ToInt32(manuscriptVm.JournalID);
                ViewBag.ArticleList = _manuscriptDbRepositoryReadSide.GetArticleList(journalId);
                ViewBag.SectionList = _manuscriptDbRepositoryReadSide.GetSectionList(journalId);
                return View("HomePage", manuscriptVm);
            }
            return File("~/Views/Shared/Unauthorised.htm", "text/html");
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult HomePage(ManuscripScreeningVM manuscriptVm, string associateCommand, string qualityCommand)
        {
            try
            {
                manuscriptVm.OverallAnalysis = "Okay to proceed";
                manuscriptVm.MSID = manuscriptVm.MSID.Trim();
                var userId = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
                IDictionary<string, string> dataErrors = new Dictionary<string, string>();
                if (manuscriptVm.ID == 0)
                {
                    if (!_manuscriptDbRepositoryReadSide.IsMSIDAvailable(manuscriptVm.MSID, manuscriptVm.ID))
                        TempData["MSIDError"] = "<script>alert('Manuscript Number is already present.');</script>";
                    else
                    {
                        manuscriptVm.RevisedDate = null;
                        manuscriptVm = IsSaveOrSubmit(manuscriptVm, associateCommand, qualityCommand);
                        _manuscriptService.SaveManuscriptScreeningVM(dataErrors, manuscriptVm);
                        TempData["msg"] = "<script>alert('Record added succesfully');</script>";
                        ModelState.Clear();
                        manuscriptVm.ID = _manuscriptDbRepositoryReadSide.GetManuscriptID(manuscriptVm.MSID);
                        manuscriptVm = _manuscriptService.GetManuscriptScreeningVM(manuscriptVm.ID);
                    }
                }
                else
                {
                    if (_manuscriptDbRepositoryReadSide.IsMSIDAvailable(manuscriptVm.MSID, manuscriptVm.ID))
                    {
                        TempData["MSIDError"] = "<script>alert('Manuscript Number is already present.');</script>";
                    }
                    else
                    {
                        manuscriptVm = IsSaveOrSubmit(manuscriptVm, associateCommand, qualityCommand);
                        if (manuscriptVm.AddedNewRevision == true)
                        {
                            if (!_manuscriptDbRepositoryReadSide.IsMSIDAvailable(manuscriptVm.MSID, 0))
                            {
                                TempData["MSIDError"] = "<script>alert('Manuscript Number is already present.');</script>";
                            }
                            else
                            {
                                _manuscriptService.SaveManuscriptScreeningVM(dataErrors, manuscriptVm);
                                TempData["msg"] = "<script>alert('Record added succesfully');</script>";
                            }
                        }
                        else
                        {
                            _manuscriptService.SaveManuscriptScreeningVM(dataErrors, manuscriptVm);
                            TempData["msg"] = "<script>alert('Record updated succesfully');</script>";
                        }
                        ModelState.Clear();
                        manuscriptVm.ID = _manuscriptDbRepositoryReadSide.GetManuscriptID(manuscriptVm.MSID);
                        manuscriptVm = _manuscriptService.GetManuscriptScreeningVM(manuscriptVm.ID);
                    }
                }
                foreach (var item in dataErrors)
                {
                    ModelState.AddModelError(item.Key, item.Value);
                }

                ViewBag.SearchList = _manuscriptDbRepositoryReadSide.GetSearchList("j");
                ViewBag.JournalList = _manuscriptDbRepositoryReadSide.GetJournalList();
                var roleIds = _manuscriptDbRepositoryReadSide.GetUserRoles(userId);
                ViewBag.RoleList = _manuscriptDbRepositoryReadSide.GetUserRoleList(roleIds);
                ViewBag.JournalStatusList = _manuscriptDbRepositoryReadSide.GetJournalStatusList();
                ViewBag.iThenticateResult = JsonConvert.SerializeObject(_manuscriptDbRepositoryReadSide.GetList(1));
                ViewBag.EnglishLangQuality = JsonConvert.SerializeObject(_manuscriptDbRepositoryReadSide.GetList(2));
                ViewBag.EthicsComplience = JsonConvert.SerializeObject(_manuscriptDbRepositoryReadSide.GetList(3));
                ViewBag.OverallAnalysis = JsonConvert.SerializeObject(_manuscriptDbRepositoryReadSide.GetList(4));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator. [exception] " + ex.Message);
            }
            finally
            {

                var journalId = Convert.ToInt32(manuscriptVm.JournalID);
                ViewBag.ArticleList = _manuscriptDbRepositoryReadSide.GetArticleList(journalId);
                ViewBag.SectionList = _manuscriptDbRepositoryReadSide.GetSectionList(journalId);

                _manuscriptDbRepositoryReadSide.Dispose();

            }
            return View("HomePage", manuscriptVm);
        }

        public ManuscripScreeningVM IsSaveOrSubmit(ManuscripScreeningVM manuscriptVm, string associateCommand, string qualityCommand)
        {
            if (associateCommand != null)
            {
                switch (associateCommand.ToLower())
                {
                    case "save":
                        manuscriptVm.IsAssociateFinalSubmit = false;
                        break;
                    case "submit":
                        manuscriptVm.IsAssociateFinalSubmit = true;
                        manuscriptVm.FinalSubmitDate = System.DateTime.Now;
                        break;
                }
            }
            if (qualityCommand != null)
            {
                switch (qualityCommand.ToLower())
                {
                    case "save":
                        manuscriptVm.IsQualityFinalSubmit = false;
                        break;
                    case "submit":
                        manuscriptVm.IsQualityFinalSubmit = true;
                        manuscriptVm.QualityEndDate = System.DateTime.Now;
                        break;
                }
            }
            return manuscriptVm;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetSearchResult(string selectedValue, string searchBy)
        {
            return this.Json(_manuscriptDbRepositoryReadSide.GetSearchResult(selectedValue, searchBy), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetAssignedEditor(string searchText, string journalId)
        {
            return this.Json(_manuscriptDbRepositoryReadSide.GetAssignedEditor(searchText, journalId), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string GetArticleType(int journalMasterId)
        {
            var articleTypeList = string.Empty;
            if (journalMasterId > 0)
            {
                var articleTypeMaster = _manuscriptDbRepositoryReadSide.GetArticleTypeList(journalMasterId);
                foreach (var articleType in articleTypeMaster)
                {
                    articleTypeList += Convert.ToString(articleType.ID) + "---" +
                                       Convert.ToString(articleType.ArticleTypeName) + "~";
                }
            }
            return articleTypeList;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string GetSectionType(int journalMasterId)
        {
            var sectionTypeList = string.Empty;
            if (journalMasterId > 0)
            {
                var sectionTypeMaster = _manuscriptDbRepositoryReadSide.GetSectionMasterList(journalMasterId);
                foreach (var sectionType in sectionTypeMaster)
                {
                    sectionTypeList += Convert.ToString(sectionType.ID) + "---" + Convert.ToString(sectionType.SectionName) + "~";
                }
            }
            return sectionTypeList;
        }

        private bool disposed = false;


        [HttpGet]
        public FileResult TransferReport(int manuscriptId)
        {
            var manuscriptVm = _manuscriptService.GetManuscriptScreeningVM(manuscriptId);
            var templatePath = Server.MapPath("~/Templates/Docx/TransferReportTemplate.docx");
            var outputPath = Server.MapPath("~/Templates/TransferReport/" + manuscriptVm.MSID + "_TransferReport.docx");
            //todo: Process and return result here
            var transferReportDocX = new TransferReportDocX();
            var downloadPath = transferReportDocX.CreateReport(manuscriptVm, templatePath, outputPath);
            return File("~/Templates/TransferReport/" + manuscriptVm.MSID + "_TransferReport.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", manuscriptVm.MSID + "_TransferReport.docx");
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_manuscriptDbRepositoryReadSide != null)
                {
                    _manuscriptDbRepositoryReadSide.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public string PreviewForm(ManuscripScreeningVM manuscriptVm)
        {
            var journalId = Convert.ToInt32(manuscriptVm.JournalID);
            var articleTypeList = _manuscriptDbRepositoryReadSide.GetArticleList(journalId);
            var sectionList = _manuscriptDbRepositoryReadSide.GetSectionList(journalId);
            var journalList = _manuscriptDbRepositoryReadSide.GetJournalList();
            var journalStatusList = _manuscriptDbRepositoryReadSide.GetJournalStatusList();
            var msHtmlPreview = new ManuscriptScreeningHtmlPreview(manuscriptVm)
            {
                ArticleTypeList = articleTypeList,
                SectionList = sectionList,
                JournalList = journalList,
                JournalStatusList = journalStatusList
            };
            var StyleGroupRow = "StyleGroupRow";
            var StyleRow = "row";
            var htmlRowDataList = new List<HtmlRowData>
            {
                 new HtmlRowData() { LabelText = "MS screening  fields", InnerHtml = "Information", StyleClass = StyleGroupRow},
                new HtmlRowData() { LabelText = "Start Date",           InnerHtml = manuscriptVm.StartDate.ToString("d"), StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Associate ID",         InnerHtml = manuscriptVm.UserID, StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Role",                 InnerHtml =_manuscriptDbRepositoryReadSide.GetRole(Convert.ToInt32(manuscriptVm.RoleID)), StyleClass = StyleRow },

                new HtmlRowData() { LabelText = "Manuscript details",   InnerHtml = string.Empty, StyleClass = StyleGroupRow },
                new HtmlRowData() { LabelText = "Journal title*",InnerHtml = msHtmlPreview.GetJournal(manuscriptVm.JournalID), StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Manuscript Number*",                 InnerHtml = manuscriptVm.MSID.ToString(), StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Article type*",        InnerHtml = msHtmlPreview.GetArticleType(manuscriptVm.ArticleTypeID), StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Article title*",       InnerHtml = manuscriptVm.ArticleTitle, StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Section",              InnerHtml = msHtmlPreview.GetSection(manuscriptVm.SectionID), StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Initial date submitted*",      InnerHtml = manuscriptVm.InitialSubmissionDate.ToString("d"), StyleClass = StyleRow },

                 new HtmlRowData() { LabelText = "Status",InnerHtml =msHtmlPreview.GetJournalStatus(manuscriptVm.JournalStatusID), StyleClass = StyleRow },

                new HtmlRowData() { LabelText = "Author(s)/ Editor(s) details", InnerHtml = string.Empty, StyleClass = StyleGroupRow },
                new HtmlRowData() { LabelText = "Corresponding author*",        InnerHtml = manuscriptVm.CorrespondingAuthor, StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Corresponding author email*",  InnerHtml = manuscriptVm.CorrespondingAuthorEmail, StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Corresponding author affiliation*", InnerHtml = manuscriptVm.CorrespondingAuthorAff, StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Other author(s)",      InnerHtml = GetOtherAuthors(manuscriptVm), StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Handling editor",      InnerHtml = manuscriptVm.HandlingEditor, StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Corresponding editor", InnerHtml = manuscriptVm.CorrespondingEditor, StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Assigned editor",      InnerHtml = manuscriptVm.AssignedEditor, StyleClass = StyleRow },

                new HtmlRowData() { LabelText = "Analytical Findings",  InnerHtml = string.Empty, StyleClass = StyleGroupRow },
                new HtmlRowData() { LabelText = "iThenticate %*",       InnerHtml =Convert.ToString(manuscriptVm.iThenticatePercentage), StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Highest iThenticate %*", InnerHtml =Convert.ToString(manuscriptVm.Highest_iThenticateFromSingleSrc), StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Cross check/ iThenticate result*", InnerHtml = _manuscriptDbRepositoryReadSide.GetMetrixLegendTitle(Convert.ToInt32(manuscriptVm.Crosscheck_iThenticateResultID)), StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Comment",              InnerHtml = manuscriptVm.Comments_Crosscheck_iThenticateResult, StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "English language Quality*", InnerHtml = _manuscriptDbRepositoryReadSide.GetMetrixLegendTitle(Convert.ToInt32(manuscriptVm.English_Lang_QualityID)), StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Comment",              InnerHtml = manuscriptVm.Comments_English_Lang_Quality , StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Ethics compliance*",   InnerHtml =_manuscriptDbRepositoryReadSide.GetMetrixLegendTitle(Convert.ToInt32(manuscriptVm.Ethics_ComplianceID)), StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Comment",              InnerHtml = manuscriptVm.Comments_Ethics_Compliance, StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Overall analysis*",    InnerHtml =_manuscriptDbRepositoryReadSide.GetMetrixLegendTitle(Convert.ToInt32(manuscriptVm.OverallAnalysisID)) , StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Comment",              InnerHtml = manuscriptVm.Comments_OverallAnalysis, StyleClass = StyleRow },
                new HtmlRowData() { LabelText = "Transfer report",      InnerHtml = (Convert.ToBoolean(manuscriptVm.HasTransferReport) ?"Yes":"No"), StyleClass = StyleRow },
            };
            var htmlPreview = msHtmlPreview.CreateHtmlPreview(htmlRowDataList, "", true, "table table-bordered");
            return htmlPreview;
        }

        private string GetOtherAuthors(ManuscripScreeningVM manuscriptVm)
        {
            var htmltable = "<Table class='table table-bordered'>";

            foreach (var otherauthor in manuscriptVm.OtherAuthors)
            {
                htmltable += "<TR>";
                htmltable += "<TD>";
                htmltable += otherauthor.AuthorName;
                htmltable += "</TD>";
                htmltable += "<TD>";
                htmltable += otherauthor.Affillation;
                htmltable += "</TD>";
                htmltable += "</TR>";
            }

            htmltable += "</Table>";

            return htmltable;

        }

        public ActionResult BookScreening(int? id)
        {
            var userId = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            var roleIds = _manuscriptDbRepositoryReadSide.GetUserRoles(userId);
            if (roleIds.Count() > 0)
            {
                ViewBag.RoleList = _manuscriptDbRepositoryReadSide.GetUserRoleList(roleIds);
                ViewBag.SearchList = _manuscriptDbRepositoryReadSide.GetSearchList("b");
                ViewBag.iThenticateResult = JsonConvert.SerializeObject(_manuscriptDbRepositoryReadSide.GetList(1));
                ViewBag.EnglishLangQuality = JsonConvert.SerializeObject(_manuscriptDbRepositoryReadSide.GetList(2));
                ViewBag.EthicsComplience = JsonConvert.SerializeObject(_manuscriptDbRepositoryReadSide.GetList(3));
                ViewBag.OverallAnalysis = JsonConvert.SerializeObject(_manuscriptDbRepositoryReadSide.GetList(4));
                ViewBag.BookTitleList = _manuscriptDbRepositoryReadSide.GetManuscriptBookTitle();
                var manuscriptBookId = id ?? default(int);

                if (_manuscriptService._manuscriptScreeningBL == null)
                    _manuscriptService._manuscriptScreeningBL = new BAL.Manuscript.ManuscriptScreeningBL();

                if (_manuscriptService._manuscriptScreeningBL._manuscriptDBRepositoryReadSide == null)
                    _manuscriptService._manuscriptScreeningBL._manuscriptDBRepositoryReadSide = _manuscriptDbRepositoryReadSide;

                var manuscriptBookVm = manuscriptBookId == 0 ? _manuscriptService.GetManuscriptBookScreeningDefaultVm() : _manuscriptService.GetManuscriptBookScreeningVm(manuscriptBookId);
                manuscriptBookVm.EmployeeName = _manuscriptDbRepositoryReadSide.EmployeeName(userId);
                return View("BookScreening", manuscriptBookVm);
            }
            return File("~/Views/Shared/Unauthorised.htm", "text/html");
        }
    }
}


