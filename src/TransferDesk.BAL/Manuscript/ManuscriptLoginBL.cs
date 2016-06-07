using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.DTO;
using Repositories = TransferDesk.DAL.Manuscript.Repositories;
using DataContext = TransferDesk.DAL.Manuscript.DataContext;
using TransferDesk.DAL.Manuscript.Repositories;
using TransferDesk.DAL.Manuscript.UnitOfWork;
using TransferDesk.Contracts.Manuscript.Entities;
using TransferDesk.Contracts.Manuscript.ComplexTypes.UserRole;
using System.IO;
using System.Threading;

namespace TransferDesk.BAL.Manuscript
{
    public class ManuscriptLoginBL
    {

        private DataContext.ManuscriptDBContext context = null;
        public ManuscriptDBRepositoryReadSide _manuscriptDBRepositoryReadSide { get; set; }
        public ManuscriptLoginDBRepositoryReadSide _manuscriptLoginDBRepositoryReadSide { get; set; }
        public ManuscriptLoginDetailsRepository _manuscriptLoginDetailsRepository { get; set; }

        public ManuscriptBookLoginDetailsRepository _manuscriptBookLoginDetailsRepository { get; set; }

        public Impersonation.Impersonate impersonate = null;
        string conString;

        public ManuscriptLoginBL(string conString)
        {
            this.conString = conString;
            context = new DataContext.ManuscriptDBContext(conString);
            _manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);
            _manuscriptLoginDBRepositoryReadSide = new ManuscriptLoginDBRepositoryReadSide(conString);
            _manuscriptLoginDetailsRepository = new ManuscriptLoginDetailsRepository(conString);
            _manuscriptBookLoginDetailsRepository = new ManuscriptBookLoginDetailsRepository(conString);
        }

        public bool SaveManuscriptBookLogin(ManuscriptBookLoginDTO manuscriptBookLoginDTO, IDictionary<string, string> dataErrors)
        {
            var manuscriptStatus = _manuscriptDBRepositoryReadSide.GetManuscriptStatus();
            var manuscriptStatusId = (from q in manuscriptStatus
                                      where q.Description.ToLower() == "open"
                                      select q.ID).FirstOrDefault();
            manuscriptBookLoginDTO.manuscriptBookLogin.ManuscriptStatusID = Convert.ToInt32(manuscriptStatusId);
            ManuscriptLoginUnitOfWork manuscriptLoginUnitOfWork = null;
            try
            {
                int currentServiceTypeId = 0;
                int userRoleId = 0;
                var currentAssociateName = string.Empty;
                if (manuscriptBookLoginDTO.manuscriptBookLogin.ID != 0)
                {
                    var currentManuscriptBookLogin =context.ManuscriptBookLogin.Find(manuscriptBookLoginDTO.manuscriptBookLogin.ID);
                    currentServiceTypeId = _manuscriptBookLoginDetailsRepository.ServiceTypeId(manuscriptBookLoginDTO.manuscriptBookLogin.ID);
                    userRoleId =_manuscriptBookLoginDetailsRepository.GetAssociateUserRoleId(manuscriptBookLoginDTO.manuscriptBookLogin.ID);
                    if (userRoleId!=0)
                    {
                        currentAssociateName = _manuscriptBookLoginDetailsRepository.GetUserID(Convert.ToInt32(userRoleId)).UserID;
                        currentAssociateName =_manuscriptDBRepositoryReadSide.EmployeeName(currentAssociateName);
                    }
                    manuscriptBookLoginDTO.manuscriptBookLogin.CreatedBy = currentManuscriptBookLogin.CreatedBy;
                    manuscriptBookLoginDTO.manuscriptBookLogin.CreatedDate = currentManuscriptBookLogin.CreatedDate;
                }
                //add record in ManuscriptBookLogin table
                manuscriptLoginUnitOfWork = new ManuscriptLoginUnitOfWork(conString);
                manuscriptLoginUnitOfWork.manuscriptBookLoginDTO = manuscriptBookLoginDTO;
                manuscriptBookLoginDTO.manuscriptBookLogin.CrestID = CreateCrestId(manuscriptBookLoginDTO.manuscriptBookLogin.ID);
                manuscriptLoginUnitOfWork.SaveManuscriptBookLogin(manuscriptBookLoginDTO);
                if(manuscriptBookLoginDTO.manuscriptBookLogin.ID!=0)
                {
                    manuscriptBookLoginDTO.manuscriptBookLogin.CrestID = CreateCrestId(manuscriptBookLoginDTO.manuscriptBookLogin.ID);
                    manuscriptLoginUnitOfWork.SaveManuscriptBookLogin(manuscriptBookLoginDTO);
                }
                //check service type is changed
                SaveManuscriptBookDetails(manuscriptBookLoginDTO, manuscriptStatusId, manuscriptLoginUnitOfWork, currentServiceTypeId, currentAssociateName);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            //exception will be raised up in the call stack
            finally
            {
                if (manuscriptLoginUnitOfWork != null)
                {
                    manuscriptLoginUnitOfWork.Dispose();
                }
            }
            return true;
        }

        public string CreateCrestId(int id)
        {
            DateTime dtTodaysDateTime = DateTime.Now;
            string formatedID = "";
            formatedID = String.Format("{0:00000}", id);
            string crestId = "B" + Convert.ToString(dtTodaysDateTime.Year) + formatedID;
            return crestId;
        }

        public bool SaveManuscriptLogin(ManuscriptLoginDTO manuscriptLoginDTO, IDictionary<string, string> dataErrors)
        {
            var manuscriptStatus = _manuscriptDBRepositoryReadSide.GetManuscriptStatus();
            var manuscriptStatusId = (from q in manuscriptStatus
                                      where q.Description.ToLower() == "open"
                                      select q.ID).FirstOrDefault();
            manuscriptLoginDTO.manuscriptLogin.ManuscriptStatusId = Convert.ToInt32(manuscriptStatusId);
            if (manuscriptLoginDTO.IsRevision == true)
            {
                manuscriptLoginDTO.manuscriptLogin.Revision = Convert.ToInt32(_manuscriptLoginDBRepositoryReadSide.GetRevisionCount(manuscriptLoginDTO.manuscriptLogin.MSID));
                manuscriptLoginDTO.manuscriptLogin.RevisionParentId = _manuscriptLoginDBRepositoryReadSide.GetParentCrestId(manuscriptLoginDTO.manuscriptLogin.MSID);
                manuscriptLoginDTO.manuscriptLogin.MSID = manuscriptLoginDTO.manuscriptLogin.MSID + "_R" + Convert.ToString(_manuscriptLoginDBRepositoryReadSide.GetRevisionCount(manuscriptLoginDTO.manuscriptLogin.MSID));
            }

            ManuscriptLoginUnitOfWork manuscriptLoginUnitOfWork = null;
            try
            {
                manuscriptLoginUnitOfWork = new ManuscriptLoginUnitOfWork(conString);
                manuscriptLoginUnitOfWork.manuscriptLoginDTO = manuscriptLoginDTO;
                manuscriptLoginUnitOfWork.SaveManuscriptLogin();
                SaveManuscriptDetails(manuscriptLoginDTO, manuscriptStatusId, manuscriptLoginUnitOfWork);

                return true;
            }
            catch (Exception ex)
            {

            }
            //exception will be raised up in the call stack
            finally
            {
                if (manuscriptLoginUnitOfWork != null)
                {
                    manuscriptLoginUnitOfWork.Dispose();
                }
            }
            return true;
        }

        private bool SaveManuscriptBookDetails(ManuscriptBookLoginDTO manuscriptBookLoginDTO, int manuscriptStatusID, ManuscriptLoginUnitOfWork _manuscriptLoginUnitOfWork, int currentServiceTypeId, string currentAssociateName)
        {

            bool bookLoggedIn = IsBookLogedIn(manuscriptBookLoginDTO.manuscriptBookLogin.ID);
            if (bookLoggedIn)
            {
                if (currentServiceTypeId != manuscriptBookLoginDTO.manuscriptBookLogin.ServiceTypeID || currentAssociateName != manuscriptBookLoginDTO.AssociateName)
                {
                    SaveManuscriptBookLoginDetails(manuscriptBookLoginDTO, manuscriptStatusID);
                    //update record
                    var updateManuscriptBookLoginDetails = new ManuscriptBookLoginDetails();
                    updateManuscriptBookLoginDetails = _manuscriptBookLoginDetailsRepository.GetManuscriptBookLoginDetails(manuscriptBookLoginDTO.manuscriptBookLogin.ID, currentServiceTypeId);//_manuscriptLoginDBRepositoryReadSide.MSServiceTypeID()
                    updateManuscriptBookLoginDetails.SubmitedDate = DateTime.Now;
                    updateManuscriptBookLoginDetails.FetchedDate = DateTime.Now;
                    //update using query 
                    updateManuscriptBookLoginDetails.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "unallocate").Select(x => x.ID).FirstOrDefault();
                    //update using query
                    updateManuscriptBookLoginDetails.JobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "close").Select(x => x.ID).FirstOrDefault();
                    manuscriptBookLoginDTO.manuscriptBookLoginDetails.Add(updateManuscriptBookLoginDetails);
                }
            }
            else
            {
                SaveManuscriptBookLoginDetails(manuscriptBookLoginDTO, manuscriptStatusID);
            }
            _manuscriptLoginUnitOfWork.SaveManuscriptBookLoginDetails();
            return true;

        }

        private void SaveManuscriptBookLoginDetails(ManuscriptBookLoginDTO manuscriptBookLoginDTO, int manuscriptStatusID)
        {
            var msBookLoginDetails = new ManuscriptBookLoginDetails
            {
                AssignedDate = DateTime.Now,
                ServiceTypeStatusId = manuscriptBookLoginDTO.manuscriptBookLogin.ServiceTypeID,
                JobStatusId = Convert.ToInt32(manuscriptStatusID),
                ManuscriptBookLoginId = manuscriptBookLoginDTO.manuscriptBookLogin.ID,
                RoleId = _manuscriptLoginDBRepositoryReadSide.GetAssociateRole(),
                JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault()
            };
            if (!string.IsNullOrEmpty(manuscriptBookLoginDTO.AssociateName))
            {
                var prGetAssociateInfoResult = _manuscriptDBRepositoryReadSide.GetAssociateName(manuscriptBookLoginDTO.AssociateName).FirstOrDefault();
                msBookLoginDetails.UserRoleId = prGetAssociateInfoResult.ID;
                msBookLoginDetails.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();
            }
            manuscriptBookLoginDTO.manuscriptBookLoginDetails.Add(msBookLoginDetails);
        }

        private void SaveManuscriptDetails(ManuscriptLoginDTO manuscriptLoginDTO, int manuscriptStatusID, ManuscriptLoginUnitOfWork _manuscriptLoginUnitOfWork)
        {
            int crestID = _manuscriptLoginDBRepositoryReadSide.GetCrestID(manuscriptLoginDTO.manuscriptLogin.MSID);
            manuscriptLoginDTO.IsCrestIDPresent = _manuscriptLoginDBRepositoryReadSide.IsCrestIDPresent(crestID);
            bool IsBoth = _manuscriptLoginDBRepositoryReadSide.IsServiceTypeBoth(manuscriptLoginDTO.manuscriptLogin.ServiceTypeStatusId);
            #region["Logic for both service type"]
            if (IsBoth)
            {
                var msLoginDetails = new ManuscriptLoginDetails
                {
                    AssignedDate = DateTime.Now,
                    ServiceTypeStatusId = _manuscriptLoginDBRepositoryReadSide.MSServiceTypeID(),
                    JobStatusId = Convert.ToInt32(manuscriptStatusID),
                    CrestId = crestID,
                    RoleId = _manuscriptLoginDBRepositoryReadSide.GetAssociateRole(),
                    JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault()
                };
                if (!string.IsNullOrEmpty(manuscriptLoginDTO.AssociateName))
                {
                    var prGetAssociateInfoResult =
                        _manuscriptDBRepositoryReadSide.GetAssociateName(manuscriptLoginDTO.AssociateName)
                            .FirstOrDefault();
                    msLoginDetails.UserRoleId = prGetAssociateInfoResult.ID;
                    msLoginDetails.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();
                }
                manuscriptLoginDTO.manuscriptLoginDetails.Add(msLoginDetails);
                if (manuscriptLoginDTO.IsCrestIDPresent)
                {
                    var rsLoginDetails = new ManuscriptLoginDetails
                    {
                        AssignedDate = DateTime.Now,
                        CrestId = crestID,
                        JobStatusId = Convert.ToInt32(manuscriptStatusID),
                        RoleId = _manuscriptLoginDBRepositoryReadSide.GetAssociateRole(),
                        ServiceTypeStatusId = _manuscriptLoginDBRepositoryReadSide.RSServiceTypeID(),
                        JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault()
                    };
                    manuscriptLoginDTO.manuscriptLoginDetails.Add(rsLoginDetails);

                    //update record
                    var updateManuscriptLoginDetails = new ManuscriptLoginDetails();
                    updateManuscriptLoginDetails = _manuscriptLoginDBRepositoryReadSide.GetManuscriptLoginDetails(crestID, _manuscriptLoginDBRepositoryReadSide.GetServiceTypeStatusId(crestID));
                    updateManuscriptLoginDetails.SubmitedDate = DateTime.Now;
                    updateManuscriptLoginDetails.FetchedDate = DateTime.Now;
                    //update using query 
                    updateManuscriptLoginDetails.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "unallocate").Select(x => x.ID).FirstOrDefault();
                    //update using query
                    updateManuscriptLoginDetails.JobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "close").Select(x => x.ID).FirstOrDefault();
                    manuscriptLoginDTO.manuscriptLoginDetails.Add(updateManuscriptLoginDetails);

                    if (_manuscriptLoginDetailsRepository.GetOpenManuscriptCount(crestID) == 2)
                    {
                        var updateManuscriptLoginDetailsRS = _manuscriptLoginDBRepositoryReadSide.GetManuscriptLoginDetails(crestID, _manuscriptLoginDBRepositoryReadSide.RSServiceTypeID());
                        updateManuscriptLoginDetailsRS.SubmitedDate = DateTime.Now;
                        updateManuscriptLoginDetailsRS.FetchedDate = DateTime.Now;
                        //update using query
                        updateManuscriptLoginDetailsRS.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "unallocate").Select(x => x.ID).FirstOrDefault();
                        //update using query
                        updateManuscriptLoginDetailsRS.JobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "close").Select(x => x.ID).FirstOrDefault();
                        manuscriptLoginDTO.manuscriptLoginDetails.Add(updateManuscriptLoginDetailsRS);
                    }
                }

                if (!manuscriptLoginDTO.IsCrestIDPresent)
                {
                    var rsLoginDetails = new ManuscriptLoginDetails
                    {
                        AssignedDate = DateTime.Now,
                        CrestId = crestID,
                        JobStatusId = Convert.ToInt32(manuscriptStatusID),
                        RoleId = _manuscriptLoginDBRepositoryReadSide.GetAssociateRole(),
                        ServiceTypeStatusId = _manuscriptLoginDBRepositoryReadSide.RSServiceTypeID(),
                        JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault()
                    };
                    manuscriptLoginDTO.manuscriptLoginDetails.Add(rsLoginDetails);
                }
            }
            #endregion
            else if (_manuscriptLoginDetailsRepository.GetOpenManuscriptCount(crestID) == 2)
            {
                var msLoginDetails = new ManuscriptLoginDetails
                {
                    AssignedDate = DateTime.Now,
                    ServiceTypeStatusId = manuscriptLoginDTO.manuscriptLogin.ServiceTypeStatusId,
                    JobStatusId = Convert.ToInt32(manuscriptStatusID),
                    CrestId = crestID,
                    RoleId = _manuscriptLoginDBRepositoryReadSide.GetAssociateRole(),
                    JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault()
                };
                if (!string.IsNullOrEmpty(manuscriptLoginDTO.AssociateName))
                {
                    var prGetAssociateInfoResult = _manuscriptDBRepositoryReadSide.GetAssociateName(manuscriptLoginDTO.AssociateName).FirstOrDefault();
                    msLoginDetails.UserRoleId = prGetAssociateInfoResult.ID;
                    msLoginDetails.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();
                }



                manuscriptLoginDTO.manuscriptLoginDetails.Add(msLoginDetails);
                if (manuscriptLoginDTO.IsCrestIDPresent)
                {
                    //update record
                    var updateManuscriptLoginDetailsMS = new ManuscriptLoginDetails();
                    updateManuscriptLoginDetailsMS = _manuscriptLoginDBRepositoryReadSide.GetManuscriptLoginDetails(crestID, _manuscriptLoginDBRepositoryReadSide.MSServiceTypeID());
                    updateManuscriptLoginDetailsMS.SubmitedDate = DateTime.Now;
                    updateManuscriptLoginDetailsMS.FetchedDate = DateTime.Now;
                    //update using query 
                    updateManuscriptLoginDetailsMS.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "unallocate").Select(x => x.ID).FirstOrDefault();
                    //update using query
                    updateManuscriptLoginDetailsMS.JobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "close").Select(x => x.ID).FirstOrDefault();
                    manuscriptLoginDTO.manuscriptLoginDetails.Add(updateManuscriptLoginDetailsMS);

                    //update record  _manuscriptLoginDetailsRepository
                    var updateManuscriptLoginDetailsRS = _manuscriptLoginDBRepositoryReadSide.GetManuscriptLoginDetails(crestID, _manuscriptLoginDBRepositoryReadSide.RSServiceTypeID());
                    updateManuscriptLoginDetailsRS.SubmitedDate = DateTime.Now;
                    updateManuscriptLoginDetailsRS.FetchedDate = DateTime.Now;
                    //update using query
                    updateManuscriptLoginDetailsRS.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "unallocate").Select(x => x.ID).FirstOrDefault();
                    //update using query
                    updateManuscriptLoginDetailsRS.JobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "close").Select(x => x.ID).FirstOrDefault();
                    manuscriptLoginDTO.manuscriptLoginDetails.Add(updateManuscriptLoginDetailsRS);


                }
            }
            else
            {
                var manuscriptLoginDetails = new ManuscriptLoginDetails
                {
                    AssignedDate = DateTime.Now,
                    CrestId = crestID,
                    ServiceTypeStatusId = manuscriptLoginDTO.manuscriptLogin.ServiceTypeStatusId,
                    JobStatusId = Convert.ToInt32(manuscriptStatusID),
                    RoleId = _manuscriptLoginDBRepositoryReadSide.GetAssociateRole(),
                    JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault()
                };
                if (!string.IsNullOrEmpty(manuscriptLoginDTO.AssociateName))
                {
                    var prGetAssociateInfoResult =
                        _manuscriptDBRepositoryReadSide.GetAssociateName(manuscriptLoginDTO.AssociateName)
                            .FirstOrDefault();
                    manuscriptLoginDetails.UserRoleId = prGetAssociateInfoResult.ID;
                    manuscriptLoginDetails.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();
                }
                manuscriptLoginDTO.manuscriptLoginDetails.Add(manuscriptLoginDetails);
                if (manuscriptLoginDTO.IsCrestIDPresent)
                {
                    //update record  _manuscriptLoginDetailsRepository
                    var updateManuscriptLoginDetailsMS = _manuscriptLoginDBRepositoryReadSide.GetManuscriptLoginDetails(crestID, _manuscriptLoginDBRepositoryReadSide.GetServiceTypeStatusId(crestID));
                    updateManuscriptLoginDetailsMS.SubmitedDate = DateTime.Now;
                    updateManuscriptLoginDetailsMS.FetchedDate = DateTime.Now;
                    //update using query
                    updateManuscriptLoginDetailsMS.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "unallocate").Select(x => x.ID).FirstOrDefault();
                    //update using query
                    updateManuscriptLoginDetailsMS.JobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "close").Select(x => x.ID).FirstOrDefault();
                    manuscriptLoginDTO.manuscriptLoginDetails.Add(updateManuscriptLoginDetailsMS);
                }
            }
            _manuscriptLoginUnitOfWork.SaveManuscriptLoginDetails();
        }

        public bool ImpersonateUser()
        {
            impersonate = new Impersonation.Impersonate();
            pr_ImpersonateCredential_Result impersonateCredential = new pr_ImpersonateCredential_Result();
            ManuscriptLoginDBRepositoryReadSide ManuscriptLoginDBRepositoryReadSide = new ManuscriptLoginDBRepositoryReadSide(conString);
            impersonateCredential = ManuscriptLoginDBRepositoryReadSide.GetImpersonateCredential();
            return impersonate.StartImpersonation(impersonateCredential.ServerUserName, impersonateCredential.Domain, impersonateCredential.Password);
        }

        public IDictionary<string, string> CreateJournalFolderStructure(string fileServerIPPath, string manuscriptFilePath, IDictionary<string, string> dataErrors, ManuscriptLoginDTO manuscriptLoginDTO)
        {
            if (ImpersonateUser())
            {
                //Impersonation.Impersonate impersonate = new Impersonation.Impersonate();
                List<Journal> journalList = new List<Journal>();
                //ManuscriptLogin manuscriptLogin = new ManuscriptLogin();
                _manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);
                string journalFolderPath = string.Empty;
                string fileName = manuscriptFilePath.Substring(manuscriptFilePath.LastIndexOf("\\"));

                string revisionNo = string.Empty;
                journalList = _manuscriptDBRepositoryReadSide.GetJournalList();
                revisionNo = Convert.ToString(_manuscriptLoginDBRepositoryReadSide.GetRevisionCount(manuscriptLoginDTO.manuscriptLogin.MSID));
                var journalName = (from journal in journalList
                                   where journal.ID == manuscriptLoginDTO.manuscriptLogin.JournalId
                                   select journal.JournalTitle).FirstOrDefault();

                journalFolderPath = fileServerIPPath + "\\" + journalName + "\\" + manuscriptLoginDTO.manuscriptLogin.MSID + "\\Manuscript";
                if (manuscriptLoginDTO.IsRevision)
                    journalFolderPath = fileServerIPPath + "\\" + journalName + "\\" + manuscriptLoginDTO.manuscriptLogin.MSID + "\\Manuscript\\Revision " + revisionNo;
                else
                    journalFolderPath = fileServerIPPath + "\\" + journalName + "\\" + manuscriptLoginDTO.manuscriptLogin.MSID + "\\Manuscript\\Fresh";
                if (CreateDirectory(journalFolderPath))
                {
                    File.Copy(manuscriptFilePath, journalFolderPath + fileName, true);
                    File.Delete(manuscriptFilePath);
                    manuscriptLoginDTO.manuscriptLogin.ManuscriptFilePath = journalFolderPath;
                    impersonate.EndImpersonation();
                }
                else
                {
                    dataErrors.Add("Impersonation Error", "File is not uploaded file server.");
                    return dataErrors;
                }
            }
            else
            {
                dataErrors.Add("Impersonation Error", "File is not uploaded file server.");
                return dataErrors;
            }
            return dataErrors;
        }

        public bool CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
            if (Directory.Exists(path))
            {
                return true;
            }
            return false;
        }

        public bool IsBookLogedIn(int Id)
        {
            var manuscriptBookLoginDetailsCount = (from bookLoginDetails in context.ManuscriptBookLoginDetails
                                                   where bookLoginDetails.ManuscriptBookLoginId == Id
                                                   select bookLoginDetails.Id).Count();
            if (Convert.ToInt32(manuscriptBookLoginDetailsCount) == 0)
                return false;
            else
                return true;

        }

    }
}
