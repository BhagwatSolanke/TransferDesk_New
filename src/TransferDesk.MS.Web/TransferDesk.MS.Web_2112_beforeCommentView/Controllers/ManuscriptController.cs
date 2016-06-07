
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

namespace TransferDesk.MS.Web.Controllers
{
    public class ManuscriptController : Controller
    {
        private ManuscriptDBRepositoryReadSide _manuscriptDBRepositoryReadSide;
        private ManuscriptService _manuscriptService;


        public ManuscriptController()
        {
            //empty constructor
            string conString = "data source=192.168.84.149;initial catalog=dbTransferDeskService;user id=sa;password=admin@123;MultipleActiveResultSets=True;App=EntityFramework";
            _manuscriptService = new ManuscriptService(conString, conString);
            _manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);
        }

        public ActionResult Index()
        {
            //  var manuscripts = from manuscript in _manuscriptService.GetManuscript(0)
            //                    select manuscript;
            return View();
        }

        public ActionResult Edit(int id)
        {
            //DTOs.ManuscriptDTO  manuscript = _manuscriptBL.GetManuscript(id);
            //string conString = "data source=192.168.84.149;initial catalog=dbTransferDeskService;user id=sa;password=admin@123;MultipleActiveResultSets=True;App=EntityFramework";
            //_manuscriptService.SetServiceConfig(conString);
            //ManuscriptViewModel model = _manuscriptService.GetManuscript(id);
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Manuscript manuscript)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //string conString = "data source=192.168.84.149;initial catalog=dbTransferDeskService;user id=sa;password=admin@123;MultipleActiveResultSets=True;App=EntityFramework";
                    //this._manuscriptRepository = new ManuscriptRepository(conString);
                    //_manuscriptRepository.UpdateManuscript(manuscript);
                    //_manuscriptRepository.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator. [exception] " + ex.Message);
            }
            return View(manuscript);
        }
        [HttpGet]
        public ActionResult HomePage(int? id)
        {
            //string conString = "data source=192.168.84.149;initial catalog=dbTransferDeskService;user id=sa;password=admin@123;MultipleActiveResultSets=True;App=EntityFramework";
            //_manuscriptService = new ManuscriptService();
            //_manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);

            ViewBag.SearchList = _manuscriptDBRepositoryReadSide.GetSearchList();
            ViewBag.JournalList = _manuscriptDBRepositoryReadSide.GetJournalList();
            ViewBag.RoleList = _manuscriptDBRepositoryReadSide.GetRoleList();
            ViewBag.iThenticateResult = JsonConvert.SerializeObject(_manuscriptDBRepositoryReadSide.GetList(1));
            ViewBag.EnglishLangQuality = JsonConvert.SerializeObject(_manuscriptDBRepositoryReadSide.GetList(2));
            ViewBag.EthicsComplience = JsonConvert.SerializeObject(_manuscriptDBRepositoryReadSide.GetList(3));
            ViewBag.OverallAnalysis = JsonConvert.SerializeObject(_manuscriptDBRepositoryReadSide.GetList(4));
            int manuscriptID = id ?? default(int);

            ManuscripScreeningVM manuscriptVM;
            if (_manuscriptService._manuscriptScreeningBL == null)
            {
                _manuscriptService._manuscriptScreeningBL = new BAL.Manuscript.ManuscriptScreeningBL();
            }

            if (_manuscriptService._manuscriptScreeningBL._manuscriptDBRepositoryReadSide == null)
            {
                _manuscriptService._manuscriptScreeningBL._manuscriptDBRepositoryReadSide = _manuscriptDBRepositoryReadSide;
            }

            //if _manuscriptService._manuscriptScreeningBL

            if (manuscriptID == 0)
            {
                manuscriptVM = _manuscriptService.GetManuscriptScreeningDefaultVM();
            }
            else
            {
                manuscriptVM = _manuscriptService.GetManuscriptScreeningVM(manuscriptID);
            }

            //@System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "")
            //manuscriptVM.UserID = _manuscriptDBRepositoryReadSide.EmployeeName(manuscriptVM.UserID);


            int journalID = Convert.ToInt32(manuscriptVM.JournalID);
            ViewBag.ArticleList = _manuscriptDBRepositoryReadSide.GetArticleList(journalID);
            ViewBag.SectionList = _manuscriptDBRepositoryReadSide.GetSectionList(journalID);
            return View("HomePage", manuscriptVM);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult HomePage(ManuscripScreeningVM manuscriptVM)
        {
            try
            {
                manuscriptVM.OverallAnalysis = "Okay to proceed";
                IDictionary<string, string> dataErrors = new Dictionary<string, string>();
                if (manuscriptVM.ID == 0)
                {
                    if (!_manuscriptDBRepositoryReadSide.IsMSIDAvailable(manuscriptVM.MSID, manuscriptVM.ID))
                        TempData["MSIDError"] = "<script>alert('Manuscript Number is already present.');</script>";
                    else
                    {
                        _manuscriptService.SaveManuscriptScreeningVM(dataErrors, manuscriptVM);
                        TempData["msg"] = "<script>alert('Record added succesfully');</script>";
                        manuscriptVM = _manuscriptService.GetManuscriptScreeningVM(manuscriptVM.ID);
                    }
                }
                else
                {
                    if (_manuscriptDBRepositoryReadSide.IsMSIDAvailable(manuscriptVM.MSID, manuscriptVM.ID))
                    {
                        //ModelState.AddModelError("MSID", "Manuscript Number is already present.");
                        TempData["MSIDError"] = "<script>alert('Manuscript Number is already present.');</script>";
                    }
                    else
                    {
                        _manuscriptService.SaveManuscriptScreeningVM(dataErrors, manuscriptVM);
                        TempData["msg"] = "<script>alert('Record updated succesfully');</script>";
                        manuscriptVM = _manuscriptService.GetManuscriptScreeningVM(manuscriptVM.ID);
                    }
                }


                foreach (KeyValuePair<String, String> item in dataErrors)
                {
                    ModelState.AddModelError(item.Key, item.Value);
                }

                ViewBag.SearchList = _manuscriptDBRepositoryReadSide.GetSearchList();
                ViewBag.JournalList = _manuscriptDBRepositoryReadSide.GetJournalList();
                ViewBag.RoleList = _manuscriptDBRepositoryReadSide.GetRoleList();
                ViewBag.iThenticateResult = JsonConvert.SerializeObject(_manuscriptDBRepositoryReadSide.GetList(1));
                ViewBag.EnglishLangQuality = JsonConvert.SerializeObject(_manuscriptDBRepositoryReadSide.GetList(2));
                ViewBag.EthicsComplience = JsonConvert.SerializeObject(_manuscriptDBRepositoryReadSide.GetList(3));
                ViewBag.OverallAnalysis = JsonConvert.SerializeObject(_manuscriptDBRepositoryReadSide.GetList(4));
                //throw new Exception("test exception");
             
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator. [exception] " + ex.Message);
            }
            finally
            {
               
                int journalID = Convert.ToInt32(manuscriptVM.JournalID);
                ViewBag.ArticleList = _manuscriptDBRepositoryReadSide.GetArticleList(journalID);
                ViewBag.SectionList = _manuscriptDBRepositoryReadSide.GetSectionList(journalID);
                _manuscriptDBRepositoryReadSide.Dispose();

            }
            return View("HomePage", manuscriptVM);

        }


        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetSearchResult(string SelectedValue, string SearchBy)
        {
            return this.Json(_manuscriptDBRepositoryReadSide.GetSearchResult(SelectedValue, SearchBy), JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string GetArticleType(int journalMasterID)
        {
            String articleTypeList = String.Empty;
            if (journalMasterID > 0)
            {
                List<TransferDesk.Contracts.Manuscript.Entities.ArticleType> articleTypeMaster = _manuscriptDBRepositoryReadSide.GetArticleTypeList(journalMasterID);
                foreach (var articleType in articleTypeMaster)
                {
                    articleTypeList += Convert.ToString(articleType.ID) + "---" + Convert.ToString(articleType.ArticleTypeName) + "~";
                }
            }
            return articleTypeList;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public string GetSectionType(int journalMasterID)
        {
            String sectionTypeList = String.Empty;
            if (journalMasterID > 0)
            {
                List<TransferDesk.Contracts.Manuscript.Entities.Section> sectionTypeMaster = _manuscriptDBRepositoryReadSide.GetSectionMasterList(journalMasterID);
                foreach (var sectionType in sectionTypeMaster)
                {
                    sectionTypeList += Convert.ToString(sectionType.ID) + "---" + Convert.ToString(sectionType.SectionName) + "~";
                }
            }
            return sectionTypeList;
        }

        private bool disposed = false;

        //protected virtual void Dispose(bool disposing)
        //{
        //    if (!this.disposed)
        //    {
        //        if (disposing)
        //        {
        //            if (_manuscriptDBRepositoryReadSide != null )
        //            {
        //                _manuscriptDBRepositoryReadSide.Dispose();
        //            }
        //            //context.Dispose();
        //        }
        //    }
        //    this.disposed = true;
        //}

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_manuscriptDBRepositoryReadSide != null)
                {
                    _manuscriptDBRepositoryReadSide.Dispose();
                }
            }
            base.Dispose(disposing);
        }

    }
}


