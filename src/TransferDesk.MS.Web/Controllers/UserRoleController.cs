using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.DAL.Manuscript;
using TransferDesk.DAL.Manuscript.DataContext;
using TransferDesk.DAL.Manuscript.Repositories;
using TransferDesk.Services.Manuscript.ViewModel;

namespace TransferDesk.MS.Web.Controllers
{
    public class UserRoleController : Controller
    {
        private UserRoleRepository _UserRoleRepository;
        private UserRoleVM userRoleVM;
        private ManuscriptDBRepositoryReadSide _manuscriptDBRepositoryReadSide;
        public UserRoleController()
        {
            string conString = Convert.ToString(ConfigurationManager.AppSettings["dbTransferDeskService"]);
            //ManuscriptContext  
            this._UserRoleRepository = new UserRoleRepository((new ManuscriptDBContext(conString)));
            _manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);
            userRoleVM = new UserRoleVM();
        }
        // GET: UserRole
        public ActionResult UserRole()
        {
            var userID = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            if(_UserRoleRepository.IsAdmin(userID.Trim()))
            {
                userRoleVM.Role = _UserRoleRepository.GetRoleByUserID(userID);
                userRoleVM.userRoles = _UserRoleRepository.GetUserRoleDetails();
                userRoleVM.EmployeeName = _manuscriptDBRepositoryReadSide.EmployeeName(userID);
                return View(userRoleVM);
            }
            else
                return File("~/Views/Shared/Unauthorised.htm", "text/html");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserRole(UserRoleVM userRoleVM)
        {
            if (ModelState.IsValid)
            {
                userRoleVM.UserID = userRoleVM.UserID.Trim();
                if (userRoleVM.ID == 0)
                {
                    if (_UserRoleRepository.IsUserIDAvailable(userRoleVM.UserID))
                    {
                        if (!_UserRoleRepository.IsUserRoleAvailable(userRoleVM.ID, userRoleVM.RollID, userRoleVM.UserID))
                        {
                            var _UserRole = new UserRoles();
                            _UserRole.ID = userRoleVM.ID;
                            _UserRole.IsActive = true;
                            _UserRole.UserID = userRoleVM.UserID;
                            _UserRole.RollID = userRoleVM.RollID;
                            _UserRole.DefaultRollID = userRoleVM.RollID;
                            _UserRole.Status = 1;
                            _UserRole.ModifiedDateTime = System.DateTime.Now;
                            _UserRoleRepository.AddUserRole(_UserRole);
                            _UserRoleRepository.SaveUserRole();
                            TempData["msg"] = "<script>alert('Record added succesfully');</script>";
                            return RedirectToAction("UserRole", "UserRole", userRoleVM);
                        }
                        else
                        {
                            TempData["msg"] = "<script>alert('Record is already present.');</script>";
                        }
                    }
                    else
                    {
                        TempData["msg"] = "<script>alert('User ID is not present in active directory.');</script>";
                    }
                }
                else
                {
                    if (_UserRoleRepository.IsUserIDAvailable(userRoleVM.UserID))
                    {
                        if (!_UserRoleRepository.IsUserRoleAvailable(userRoleVM.ID, userRoleVM.RollID, userRoleVM.UserID))
                        {
                            var _UserRole = new UserRoles();
                            _UserRole.IsActive = userRoleVM.IsActive;
                            _UserRole.UserID = userRoleVM.UserID;
                            _UserRole.RollID = userRoleVM.RollID;
                            _UserRole.ID = userRoleVM.ID;
                            _UserRole.DefaultRollID = userRoleVM.RollID;
                            _UserRole.Status = 1;
                            _UserRole.ModifiedDateTime = System.DateTime.Now;
                            _UserRoleRepository.UpdateUserRole(_UserRole);
                            _UserRoleRepository.SaveUserRole();
                            TempData["msg"] = "<script>alert('Record updated succesfully');</script>";
                            return RedirectToAction("UserRole", "UserRole", userRoleVM);
                        }
                        else
                        {
                            TempData["msg"] = "<script>alert('Record is already present.');</script>";
                        }
                    }
                    else
                    {
                        TempData["msg"] = "<script>alert('User ID is not present in active directory.');</script>";
                    }
                }
            }

            var userID = @System.Web.HttpContext.Current.User.Identity.Name.Replace("SPRINGER-SBM\\", "");
            userRoleVM.Role = _UserRoleRepository.GetRoleByUserID(userID);
            userRoleVM.userRoles = _UserRoleRepository.GetUserRoleDetails();
            userRoleVM.EmployeeName = _manuscriptDBRepositoryReadSide.EmployeeName(userID);
            return View(userRoleVM);
        }
    }
}