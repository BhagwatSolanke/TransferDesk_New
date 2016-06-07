using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Text.RegularExpressions;

namespace TransferDesk.MS.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ManuscriptDBRepositoryReadSide _manuscriptDBRepositoryReadSide;
        private ManuscriptService _manuscriptService;
        private readonly JournalRepository _JournalRepository;
        private JournalVM journalvm;
        public JournalArticleTypeRepository JournalArticleTypeRepository;
        private readonly JournalArticleTypeVM journalarticletypevm;
        private JournalSectionTypeVM journalsectionvm;
        public JournalSectionTypeRepository _JournalSectionReposistory;
        public BookMasterRepository bookMasterRepository;

        public AdminController()
        {
            var conString = string.Empty;
            conString = Convert.ToString(ConfigurationManager.AppSettings["dbTransferDeskService"]);
            _manuscriptService = new ManuscriptService(conString, conString);
            _manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);
            _JournalRepository = new JournalRepository(conString);
            journalvm = new JournalVM();
            JournalArticleTypeRepository = new JournalArticleTypeRepository(conString);
            journalarticletypevm = new JournalArticleTypeVM();
            _JournalSectionReposistory = new JournalSectionTypeRepository(conString);
            journalsectionvm = new JournalSectionTypeVM();
            bookMasterRepository = new BookMasterRepository(conString);
        }

        public ActionResult Admin()
        {
            var userID = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            if (_JournalRepository.IsAdmin(userID.Trim()))
            {
                return View();
            }
            else
                return File("~/Views/Shared/Unauthorised.htm", "text/html");
        }

        [HttpGet]
        public ActionResult JournalMaster()
        {
            var jv = new JournalVM();
            jv.Journals = _JournalRepository.GetJournals();
            return View(jv);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JournalMaster(JournalVM journal)
        {
            if (ModelState.IsValid)
            {
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex("[ ]{2,}", options);
                var journaltitlename = regex.Replace(journal.JournalTitle.Trim(), " ");

                if (journal.ID != 0)
                {
                    if (_JournalRepository.IsJournalTitleStatusAvailable(journaltitlename, journal.IsActive, journal.ID))
                    {
                        string message = "Record for Journal Title : " + journaltitlename + " is already present";
                        TempData["msg"] = "<script>alert(\"" + message + "\");</script>";                   
                        return RedirectToAction("JournalMaster", "Admin", journal);
                    }
                    else
                    {
                        if (_JournalRepository.IsJournalStatusAvailable(journaltitlename, journal.IsActive))
                        {
                            string message = "Record for Journal Title : " + journaltitlename + " is already present";
                            TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                            return RedirectToAction("JournalMaster", "Admin", journal);
                        }
                        else
                        {
                            var _JournalData = new Journal();
                            _JournalData.ID = journal.ID;
                            _JournalData.JournalTitle = journaltitlename;
                            _JournalData.IsActive = journal.IsActive;
                            _JournalData.Link = journal.Link;
                            _JournalData.ModifiedDateTime = System.DateTime.Now;
                            _JournalRepository.UpdateJournal(_JournalData);
                            _JournalRepository.SaveChanges();
                            string message = "Record for Journal Title : " + journaltitlename + " is updated succesfully";
                            TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                            return RedirectToAction("JournalMaster", "Admin", journal);
                        }
                    }
                }
                else
                {
                    if (_JournalRepository.IsJournalTitleAvailable(journaltitlename))
                    {
                        string message = "Record for Journal Title : " + journaltitlename + " is already present";
                        TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                        return RedirectToAction("JournalMaster", "Admin", journal);
                    }
                    else
                    {
                        var _JournalData = new Journal();
                        _JournalData.IsActive = true;
                        _JournalData.JournalTitle = journaltitlename;
                        _JournalData.Link = journal.Link;
                        _JournalData.ModifiedDateTime = System.DateTime.Now;
                        _JournalRepository.AddJournal(_JournalData);
                        _JournalRepository.SaveChanges();
                        string message = "Record for Journal Title : " + journaltitlename + " is added succesfully";
                        TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                        return RedirectToAction("JournalMaster", "Admin", journal);
                    }
                }
            }
            return RedirectToAction("JournalMaster", "Admin");
        }

        [HttpGet]
        public ActionResult JournalSectionTypes()
        {
            ViewBag.JournalList = _manuscriptDBRepositoryReadSide.GetJournalList();
            int journalid = 0;
            journalsectionvm.sectiondetails = _JournalSectionReposistory.GetJournalSectionDetails(journalid);
            return View(journalsectionvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JournalSectionTypes(JournalSectionTypeVM journalsectiontype)
        {
            if (ModelState.IsValid)
            {
                var journalidvalue = _JournalSectionReposistory.JournalIDvalue(journalsectiontype.JournalTitleName.Trim());
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex("[ ]{2,}", options);
                var sectionname = regex.Replace(journalsectiontype.SectionName, " ").Trim();
                if (journalsectiontype.ID != 0)
                {
                    if (_JournalSectionReposistory.IsJournalSectionTypeAvailable(journalidvalue, journalsectiontype.SectionTypeID, sectionname, journalsectiontype.IsActive))
                    {
                        string message = "Record for Section : " + sectionname + " with Journal Title : " + journalsectiontype.JournalTitleName + " is updated successfully";
                        TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                        return RedirectToAction("JournalSectionTypes", "Admin", journalsectiontype);

                    }
                    else
                    {
                        if (_JournalSectionReposistory.IsSectionNameAvailable(sectionname))
                        {
                            int chksectionid = _JournalSectionReposistory.sectionID(sectionname);
                            if (chksectionid == journalsectiontype.SectionTypeID)
                            {
                                var _JournalSectionAdd = new JournalSections();
                                _JournalSectionAdd.ID = journalsectiontype.ID;
                                _JournalSectionAdd.JournalID = journalidvalue;
                                _JournalSectionAdd.SectionID = journalsectiontype.SectionTypeID;
                                _JournalSectionAdd.Status = 1;
                                _JournalSectionAdd.IsActive = journalsectiontype.IsActive;
                                _JournalSectionAdd.ModifiedDateTime = System.DateTime.Now;
                                _JournalSectionReposistory.UpdateJournalSection(_JournalSectionAdd);
                                _JournalSectionReposistory.SaveChanges();

                                string message = "Record for Section : " + sectionname + " with Journal Title : " + journalsectiontype.JournalTitleName + " is updated succesfully";
                                TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                                return RedirectToAction("JournalSectionTypes", "Admin", journalsectiontype);
                            }
                            else
                            {
                                if (_JournalSectionReposistory.IsSectionTypeAvailable(sectionname, journalidvalue))
                                {
                                    string message = "Record for Section : " + sectionname + " with Journal Title : " + journalsectiontype.JournalTitleName + " is already present";
                                    TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                                    return RedirectToAction("JournalSectionTypes", "Admin", journalsectiontype);
                                }
                                else
                                {
                                    if (_JournalSectionReposistory.AddJournalSectionData(journalidvalue, chksectionid))
                                    {
                                        string message = "Record for Section : " + sectionname + " with Journal Title : " + journalsectiontype.JournalTitleName + " is added succesfully";
                                        TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                                        return RedirectToAction("JournalSectionTypes", "Admin", journalsectiontype);
                                    }
                                }

                            }
                        }
                        else
                        {
                            _JournalSectionReposistory.AddSectionData(sectionname);
                            var sectionid = _JournalSectionReposistory.sectionID(sectionname);

                            if (_JournalSectionReposistory.AddJournalSectionData(journalidvalue, sectionid))
                            {
                                var message = "Record for Section : " + sectionname + " with Journal Title : " + journalsectiontype.JournalTitleName + " is added succesfully";
                                TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                                return RedirectToAction("JournalSectionTypes", "Admin", journalsectiontype);
                            }
                        }
                    }
                }
                else
                {
                    if (_JournalSectionReposistory.IsSectionTypeAvailable(sectionname, journalidvalue))
                    {
                        string message = "Record for Section : " + sectionname + " with Journal Title : " + journalsectiontype.JournalTitleName + " is already present";
                        TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                        return RedirectToAction("JournalSectionTypes", "Admin", journalsectiontype);
                    }
                    else
                    {
                        if (_JournalSectionReposistory.IsSectionNameAvailable(sectionname))
                        {
                            int sectionid = _JournalSectionReposistory.sectionID(sectionname);

                            if (_JournalSectionReposistory.AddJournalSectionData(journalidvalue, sectionid))
                            {
                                string message = "Record for Section : " + sectionname + " with Journal Title : " + journalsectiontype.JournalTitleName + " is added succesfully";
                                TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                                return RedirectToAction("JournalSectionTypes", "Admin", journalsectiontype);
                            }
                        }
                        else
                        {
                            _JournalSectionReposistory.AddSectionData(sectionname);
                            int sectionid = _JournalSectionReposistory.sectionID(sectionname);

                            if (_JournalSectionReposistory.AddJournalSectionData(journalidvalue, sectionid))
                            {
                                string message = "Record for Section : " + sectionname + " with Journal Title : " + journalsectiontype.JournalTitleName + " is added succesfully";
                                TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                                return RedirectToAction("JournalSectionTypes", "Admin", journalsectiontype);
                            }
                        }
                    }
                }
            }
            return RedirectToAction("JournalSectionTypes", "Admin");
        }

        [HttpGet]
        public ActionResult JournalArticleTypes()
        {

            ViewBag.JournalList = _manuscriptDBRepositoryReadSide.GetJournalList();
            int journalid = 0;
            journalarticletypevm.details = JournalArticleTypeRepository.GetJournalArticleDetails(journalid);

            return View(journalarticletypevm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JournalArticleTypes(JournalArticleTypeVM journalarticletype)
        {
            if (ModelState.IsValid)
            {
                var journalidvalue = JournalArticleTypeRepository.JournalIDvalue(journalarticletype.JournalTitleName.Trim());
                var options = RegexOptions.None;
                var regex = new Regex("[ ]{2,}", options);
                var articletypename = regex.Replace(journalarticletype.ArticleTypeName, " ").Trim();
                if (journalarticletype.ID != 0)
                {
                    if (JournalArticleTypeRepository.IsJournalArticleTypeAvailable(journalidvalue, journalarticletype.ArticleTypeID, articletypename, journalarticletype.IsActive))
                    {
                        string message = "Record for Article Type : " + articletypename + " with Journal Title : " + journalarticletype.JournalTitleName + " is updated successfully";
                        TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                        return RedirectToAction("JournalArticleTypes", "Admin", journalarticletype);
                    }
                    else
                    {
                        if (JournalArticleTypeRepository.IsArticleNameAvailable(articletypename))
                        {
                            int chkarcticleid = JournalArticleTypeRepository.articleID(articletypename);
                            if (chkarcticleid == journalarticletype.ArticleTypeID)
                            {
                                var _JournalArticleDetails = new JournalArticleTypes();
                                _JournalArticleDetails.ID = journalarticletype.ID;
                                _JournalArticleDetails.JournalID = journalidvalue;
                                _JournalArticleDetails.ArticleTypeID = journalarticletype.ArticleTypeID;
                                _JournalArticleDetails.Status = 1;
                                _JournalArticleDetails.IsActive = journalarticletype.IsActive;
                                _JournalArticleDetails.ModifiedDateTime = System.DateTime.Now;
                                JournalArticleTypeRepository.UpdateJournalArticle(_JournalArticleDetails);
                                JournalArticleTypeRepository.SaveChanges();
                                string message = "Record for Article Type : " + articletypename + " with Journal Title : " + journalarticletype.JournalTitleName + " is updated succesfully";
                                TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                                return RedirectToAction("JournalArticleTypes", "Admin", journalarticletype);
                            }
                            else
                            {
                                if (JournalArticleTypeRepository.IsArticleTypeAvailable(articletypename, journalidvalue))
                                {
                                    string message = "Record for Article Type : " + articletypename + " with Journal Title : " + journalarticletype.JournalTitleName + " is already present";
                                    TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                                    return RedirectToAction("JournalArticleTypes", "Admin", journalarticletype);
                                }
                                else
                                {
                                    if (JournalArticleTypeRepository.AddJournalArticleData(journalidvalue, chkarcticleid))
                                    {
                                        string message = "Record for Article Type : " + articletypename + " with Journal Title : " + journalarticletype.JournalTitleName + " is added succesfully";
                                        TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                                        return RedirectToAction("JournalArticleTypes", "Admin", journalarticletype);
                                    }
                                }
                            }
                        }
                        else
                        {
                            JournalArticleTypeRepository.AddArticleData(articletypename);
                            int articleid = JournalArticleTypeRepository.articleID(articletypename);
                            if (JournalArticleTypeRepository.AddJournalArticleData(journalidvalue, articleid))
                            {
                                string message = "Record for Article Type : " + articletypename + " with Journal Title : " + journalarticletype.JournalTitleName + " is added succesfully";
                                TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                                return RedirectToAction("JournalArticleTypes", "Admin", journalarticletype);
                            }

                        }
                    }
                }
                else
                {
                    if (JournalArticleTypeRepository.IsArticleTypeAvailable(articletypename, journalidvalue))
                    {
                        string message = "Record for Article Type : " + articletypename + " with Journal Title : " + journalarticletype.JournalTitleName + " is already present";
                        TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                        return RedirectToAction("JournalArticleTypes", "Admin", journalarticletype);
                    }
                    else
                    {
                        if (JournalArticleTypeRepository.IsArticleNameAvailable(articletypename))
                        {
                            int articleid = JournalArticleTypeRepository.articleID(articletypename);
                            if (JournalArticleTypeRepository.AddJournalArticleData(journalidvalue, articleid))
                            {
                                var message = "Record for Article Type : " + articletypename + " with Journal Title : " + journalarticletype.JournalTitleName + " is added succesfully";
                                TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                                return RedirectToAction("JournalArticleTypes", "Admin", journalarticletype);
                            }
                        }
                        else
                        {
                            JournalArticleTypeRepository.AddArticleData(articletypename);
                            var articleid = JournalArticleTypeRepository.articleID(articletypename);
                            if (JournalArticleTypeRepository.AddJournalArticleData(journalidvalue, articleid))
                            {
                                string message = "Record for Article Type : " + articletypename + " with Journal Title : " + journalarticletype.JournalTitleName + " is added succesfully";
                                TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                                return RedirectToAction("JournalArticleTypes", "Admin", journalarticletype);
                            }
                        }
                    }
                }
            }
                return RedirectToAction("JournalArticleTypes", "Admin");

        }


        [HttpGet]
        public ActionResult BookMaster()
        {
            var bookMasterVm = new BookMasterVm();
            bookMasterVm.BookMasterData = bookMasterRepository.GetBookMasterDetails();
            return View(bookMasterVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookMaster(BookMasterVm bookMasterVm)
        {
            if (ModelState.IsValid)
            {
                if (bookMasterVm.ID == 0)
                {
                    if (!bookMasterRepository.IsBookTitleAvailable(bookMasterVm.BookTitle.Trim(), bookMasterVm.ID))
                    {
                        BookMaster bookMaster = GetBookMasterEntity(bookMasterVm);
                        bookMasterRepository.AddBookMaster(bookMaster);
                        string message = "Record for book Title : " + bookMasterVm.BookTitle.Trim() + " is added succesfully";
                        TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                    }
                    else
                    {
                        string message = "Record for book Title : " + bookMasterVm.BookTitle.Trim() + " is already present";
                        TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                    }
                }
                else
                {
                    if (!bookMasterRepository.IsBookTitleAvailable(bookMasterVm.BookTitle.Trim(), bookMasterVm.ID))
                    {
                        BookMaster bookMaster;
                        bookMaster = bookMasterRepository.GetBookInfoById(bookMasterVm.ID);
                        bookMaster.BookTitle = bookMasterVm.BookTitle.Trim();
                        bookMaster.GPUInformation = bookMasterVm.GPUInformation;
                        bookMaster.FTPLink = bookMasterVm.FTPLink;
                        bookMaster.IsActive = bookMasterVm.IsActive;
                        bookMaster.ModifiedBy = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
                        bookMasterRepository.UpdateBookMaster(bookMaster);
                        string message = "Record for book Title : " + bookMasterVm.BookTitle.Trim() + " is updated succesfully";
                        TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                    }
                    else
                    {
                        string message = "Record for book Title : " + bookMasterVm.BookTitle.Trim() + " is already present";
                        TempData["msg"] = "<script>alert(\"" + message + "\");</script>";
                    }
                }
            }
            return RedirectToAction("BookMaster", "Admin");
        }

        private Contracts.Manuscript.Entities.BookMaster GetBookMasterEntity(BookMasterVm bookMasterVm)
        {
            var bookMaster = new BookMaster();
            bookMaster.BookTitle = bookMasterVm.BookTitle.Trim();
            bookMaster.GPUInformation = bookMasterVm.GPUInformation;
            bookMaster.FTPLink = bookMasterVm.FTPLink;
            bookMaster.IsActive = bookMasterVm.IsActive;
            bookMaster.CreatedBy = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            return bookMaster;
        }


        private bool disposed = false;

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

