using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.ComplexTypes.AdminDashBoard;
using RepositoryInterfaces = TransferDesk.Contracts.Manuscript.Repositories;
using Entities = TransferDesk.Contracts.Manuscript.Entities;
using DataContexts = TransferDesk.DAL.Manuscript.DataContext;
using TransferDesk.Contracts.Manuscript.DTO;
using TransferDesk.Contracts.Manuscript.Entities;
using System.Data.SqlClient;

namespace TransferDesk.DAL.Manuscript.Repositories
{
    public class AdminDashBoardDBReadSide : IDisposable
    {
        private bool _disposed = false;
        private DataContexts.ManuscriptDBContext manuscriptDataContextRead;
        private ManuscriptLoginDetailsRepository _manuscriptLoginDetailsRepository;
        private ManuscriptDBRepositoryReadSide _manuscriptDBRepositoryReadSide;
        private ManuscriptLoginDBRepositoryReadSide _manuscriptLoginDBRepositoryReadSide;
        private int _serviceTypeId, _roleId;
         private int jobProcessStatusIdForHold;
        private int assignedstatus_id;
        private int fetchstatus_id;
        private int openstatus_id;
        //ManuscriptLoginDetails _updatemanuscriptLoginDetails = null;

        public AdminDashBoardDBReadSide(string conString)
        {
            this.manuscriptDataContextRead = new DataContexts.ManuscriptDBContext(conString);
            //_updatemanuscriptLoginDetails = new ManuscriptLoginDetails();
            _manuscriptLoginDetailsRepository = new ManuscriptLoginDetailsRepository(conString);
            _manuscriptDBRepositoryReadSide = new ManuscriptDBRepositoryReadSide(conString);
            _manuscriptLoginDBRepositoryReadSide = new ManuscriptLoginDBRepositoryReadSide(conString);
        }

        public bool IsAssociateAllocatedToMSID(AdminDashBoardDTO adminDashBoardDTO)
        {
            _serviceTypeId = GetServiceTypeId(adminDashBoardDTO.ServiceType);
            _roleId = GetRoleId(adminDashBoardDTO.Role);
            var jobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();
            var jobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
            var userRoleId = (from loginDetails in manuscriptDataContextRead.ManuscriptLoginDetails
                              where loginDetails.CrestId == adminDashBoardDTO.CrestId
                              && loginDetails.ServiceTypeStatusId == _serviceTypeId
                              && loginDetails.RoleId == _roleId
                              && loginDetails.JobProcessStatusId == jobProcessStatusId
                              && loginDetails.JobStatusId == jobStatusId
                              select loginDetails.UserRoleId).FirstOrDefault();

            if (Convert.ToInt32(userRoleId) == 0 || userRoleId.ToString().Trim() == "")
            {
                return true;
            }
            else
            {
                //dataErrors.Add("Error:", "MSID is already alloacted.");
                return false;
            }
        }

        public bool UnallocateAssociateUser(AdminDashBoardDTO _adminDashBoardDTO)
        {
            if (IsAssociateAllocateToMSID(_adminDashBoardDTO))
            {
                var jobProcessStatusId =_manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();
                var jobStatusId =_manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                var _updatemanuscriptLoginDetails = (from loginDetails in manuscriptDataContextRead.ManuscriptLoginDetails
                                                 where loginDetails.CrestId == _adminDashBoardDTO.CrestId
                                                       && loginDetails.RoleId == _roleId
                                                       && loginDetails.ServiceTypeStatusId == _serviceTypeId
                                                       && loginDetails.JobProcessStatusId == jobProcessStatusId
                                                       && loginDetails.JobStatusId == jobStatusId 
                                                 select loginDetails).FirstOrDefault();

                var manuscriptLoginDetails=new ManuscriptLoginDetails
                {
                    CrestId=_updatemanuscriptLoginDetails.CrestId,
                    JobStatusId=_updatemanuscriptLoginDetails.JobStatusId,
                    ServiceTypeStatusId = _updatemanuscriptLoginDetails.ServiceTypeStatusId,
                    RoleId=_updatemanuscriptLoginDetails.RoleId,
                    JobProcessStatusId = _updatemanuscriptLoginDetails.JobStatusId,
                };
                _adminDashBoardDTO.manuscriptLoginDetails.Add(manuscriptLoginDetails);

                _updatemanuscriptLoginDetails.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "unallocate").Select(x => x.ID).FirstOrDefault();
                _updatemanuscriptLoginDetails.FetchedDate= DateTime.Now;
                _updatemanuscriptLoginDetails.SubmitedDate= DateTime.Now;
                _updatemanuscriptLoginDetails.JobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "close").Select(x => x.ID).FirstOrDefault();
                _adminDashBoardDTO.manuscriptLoginDetails.Add(_updatemanuscriptLoginDetails);
                SaveManuscriptLoginDetails(_adminDashBoardDTO);

                return true;
            }
            else
            {

                return false;
            }
        }

        private bool IsAssociateAllocateToMSID(AdminDashBoardDTO _adminDashBoardDTO)
        {
            try
            {
                _serviceTypeId = GetServiceTypeId(_adminDashBoardDTO.ServiceType);
                _roleId = GetRoleId(_adminDashBoardDTO.Role);
                var jobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                var jobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();
                var MSIDAssociateToUser = (from MSD in manuscriptDataContextRead.ManuscriptLoginDetails
                                           where MSD.CrestId == _adminDashBoardDTO.CrestId
                                                 && MSD.RoleId == _roleId
                                                 && MSD.ServiceTypeStatusId == _serviceTypeId
                                                 && MSD.JobStatusId == jobStatusId
                                                 && MSD.JobProcessStatusId == jobProcessStatusId
                                           //&&  MSD.UserRoleId == null
                                           select MSD.UserRoleId).SingleOrDefault();
                if (Convert.ToString(MSIDAssociateToUser) == "")
                    return false;
                else
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        public bool AllocateJob_ForHoldJob(AdminDashBoardDTO _adminDashBoardDTO)
        {
            try
            {
                var prGetAssociateInfoResult = _manuscriptDBRepositoryReadSide.GetAssociateName(_adminDashBoardDTO.AssociateName).FirstOrDefault();
                var jobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "on hold").Select(x => x.ID).FirstOrDefault();
                var jobStatusIDvalue = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                var _updatemanuscriptLoginDetailsForAssigned = (from loginDetails in manuscriptDataContextRead.ManuscriptLoginDetails
                                                                where loginDetails.CrestId == _adminDashBoardDTO.CrestId
                                                                      && loginDetails.RoleId == _roleId
                                                                      && loginDetails.ServiceTypeStatusId == _serviceTypeId
                                                                      && loginDetails.JobStatusId == jobStatusIDvalue
                                                                      && loginDetails.JobProcessStatusId == jobProcessStatusId
                                                                select loginDetails).FirstOrDefault();
                var jobProcessStatusIdOfAssigned = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();

                var manuscriptLoginDetails = new ManuscriptLoginDetails
                {
                    CrestId = _adminDashBoardDTO.CrestId,
                    JobStatusId = _updatemanuscriptLoginDetailsForAssigned.JobStatusId,
                    ServiceTypeStatusId = _serviceTypeId,
                    RoleId = _updatemanuscriptLoginDetailsForAssigned.RoleId,
                    JobProcessStatusId = jobProcessStatusIdOfAssigned,
                    UserRoleId = prGetAssociateInfoResult.ID,
                    AssignedDate = DateTime.Now,
                };
                _adminDashBoardDTO.manuscriptLoginDetails.Add(manuscriptLoginDetails);
                _updatemanuscriptLoginDetailsForAssigned.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "on hold").Select(x => x.ID).FirstOrDefault();
                _updatemanuscriptLoginDetailsForAssigned.FetchedDate = DateTime.Now;
                _updatemanuscriptLoginDetailsForAssigned.SubmitedDate = DateTime.Now;
                _updatemanuscriptLoginDetailsForAssigned.ModifiedDate = DateTime.Now;
                _updatemanuscriptLoginDetailsForAssigned.JobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "close").Select(x => x.ID).FirstOrDefault();
                _adminDashBoardDTO.manuscriptLoginDetails.Add(_updatemanuscriptLoginDetailsForAssigned);
                SaveManuscriptLoginDetails(_adminDashBoardDTO);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool AllocateAssociateToMSID(AdminDashBoardDTO _adminDashBoardDTO)
        {
            try
            {
                if (IsAssociateAllocatedToMSID(_adminDashBoardDTO))
                {
                    if (_adminDashBoardDTO.JobProcessingStatus == "On Hold")
                    {
                        bool result = AllocateJob_ForHoldJob(_adminDashBoardDTO);
                        return result;
                    }
                    else
                    {
                        var prGetAssociateInfoResult = _manuscriptDBRepositoryReadSide.GetAssociateName(_adminDashBoardDTO.AssociateName).FirstOrDefault();
                        var jobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                        var jobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                        var _updatemanuscriptLoginDetails = (from loginDetails in manuscriptDataContextRead.ManuscriptLoginDetails
                                                             where loginDetails.CrestId == _adminDashBoardDTO.CrestId
                                                                   && loginDetails.RoleId == _roleId
                                                                   && loginDetails.ServiceTypeStatusId == _serviceTypeId
                                                                   && loginDetails.JobProcessStatusId == jobProcessStatusId
                                                                   && loginDetails.JobStatusId == jobStatusId
                                                             select loginDetails).SingleOrDefault();
                        _updatemanuscriptLoginDetails.UserRoleId = prGetAssociateInfoResult.ID;
                        _updatemanuscriptLoginDetails.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();
                        _updatemanuscriptLoginDetails.AssignedDate = DateTime.Now;
                        _adminDashBoardDTO.manuscriptLoginDetails.Add(_updatemanuscriptLoginDetails);
                        SaveManuscriptLoginDetails(_adminDashBoardDTO);
                        return true;
                    }
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }


        }

        public void SaveManuscriptLoginDetails(AdminDashBoardDTO _adminDashBoardDTO)
        {
            foreach (var item in _adminDashBoardDTO.manuscriptLoginDetails)
            {
                if (item.Id == 0 || item.Id == null)
                    _manuscriptLoginDetailsRepository.AddManuscriptLoginDetails(item);
                else
                {
                    _manuscriptLoginDetailsRepository.UpdateManuscriptLoginDetails(item);
                }

            }
            _manuscriptLoginDetailsRepository.SaveChanges();

        }


        private int GetJobStatusId()
        {
            var jobStatusId = (from jobStatus in manuscriptDataContextRead.StatusMaster
                               where jobStatus.Description == "assigned"
                               select jobStatus.ID).First();
            return Convert.ToInt32(jobStatusId);
        }

        private int GetRoleId(string role)
        {
            var roleId = (from roles in manuscriptDataContextRead.Roles
                         where roles.RoleName == role
                         select roles.ID).FirstOrDefault();
            return Convert.ToInt32(roleId);
        }

        private int GetServiceTypeId(string serviceType)
        {
            var serviceTypeId = (from statusMaster in manuscriptDataContextRead.StatusMaster
                                where statusMaster.Description == serviceType
                                select statusMaster.ID).FirstOrDefault();
            return Convert.ToInt32(serviceTypeId);
        }

       public bool HoldMSIDForJob(AdminDashBoardDTO _adminDashBoardDTO)
        {
            jobProcessStatusIdForHold = IsJobOpenMSID(_adminDashBoardDTO);
            assignedstatus_id = _manuscriptDBRepositoryReadSide.GetStatusID("assigned");
            fetchstatus_id = _manuscriptDBRepositoryReadSide.GetStatusID("Fetch");
            openstatus_id = _manuscriptDBRepositoryReadSide.GetStatusID("Open");
            try
            {
                if (jobProcessStatusIdForHold == assignedstatus_id || jobProcessStatusIdForHold == fetchstatus_id)
                {
                    var jobStatusIDvalue = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                    var _updatemanuscriptLoginDetailsForAssigned = (from loginDetails in manuscriptDataContextRead.ManuscriptLoginDetails
                                                                    where loginDetails.CrestId == _adminDashBoardDTO.CrestId
                                                                          && loginDetails.RoleId == _roleId
                                                                          && loginDetails.ServiceTypeStatusId == _serviceTypeId
                                                                          && loginDetails.JobStatusId == jobStatusIDvalue
                                                                          && loginDetails.JobProcessStatusId == jobProcessStatusIdForHold
                                                                    select loginDetails).FirstOrDefault();
                    var jobProcessStatusIdOfHold = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "on hold").Select(x => x.ID).FirstOrDefault();

                    var manuscriptLoginDetails = new ManuscriptLoginDetails
                    {
                        CrestId = _adminDashBoardDTO.CrestId,
                        JobStatusId = _updatemanuscriptLoginDetailsForAssigned.JobStatusId,
                        ServiceTypeStatusId = _serviceTypeId,
                        RoleId = _updatemanuscriptLoginDetailsForAssigned.RoleId,
                        JobProcessStatusId = jobProcessStatusIdOfHold,
                        AssignedDate = DateTime.Now,
                    };
                    _adminDashBoardDTO.manuscriptLoginDetails.Add(manuscriptLoginDetails);
                    _updatemanuscriptLoginDetailsForAssigned.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "unallocate").Select(x => x.ID).FirstOrDefault();
                    _updatemanuscriptLoginDetailsForAssigned.FetchedDate = DateTime.Now;
                    _updatemanuscriptLoginDetailsForAssigned.SubmitedDate = DateTime.Now;
                    _updatemanuscriptLoginDetailsForAssigned.ModifiedDate = DateTime.Now;
                    _updatemanuscriptLoginDetailsForAssigned.JobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "close").Select(x => x.ID).FirstOrDefault();
                    _adminDashBoardDTO.manuscriptLoginDetails.Add(_updatemanuscriptLoginDetailsForAssigned);
                    SaveManuscriptLoginDetails(_adminDashBoardDTO);
                    return true;

                }
                else if (jobProcessStatusIdForHold == openstatus_id)
                {
                    var jobStatusIDvalue =
                        _manuscriptLoginDBRepositoryReadSide.GetStatusMaster()
                            .Where(x => x.Description.ToLower() == "open")
                            .Select(x => x.ID)
                            .FirstOrDefault();
                    var _updatemanuscriptLoginDetailsForOpen = (from loginDetails in manuscriptDataContextRead.ManuscriptLoginDetails
                                                                where loginDetails.CrestId == _adminDashBoardDTO.CrestId
                                                                                               && loginDetails.RoleId == _roleId
                                                                                               && loginDetails.ServiceTypeStatusId == _serviceTypeId
                                                                                               && loginDetails.JobStatusId == jobStatusIDvalue
                                                                                               && loginDetails.JobProcessStatusId == jobProcessStatusIdForHold
                                                                select loginDetails).FirstOrDefault();

                    _updatemanuscriptLoginDetailsForOpen.JobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "on hold").Select(x => x.ID).FirstOrDefault();
                    _updatemanuscriptLoginDetailsForOpen.ModifiedDate = DateTime.Now;
                    _updatemanuscriptLoginDetailsForOpen.AssignedDate = DateTime.Now;
                    _updatemanuscriptLoginDetailsForOpen.ModifiedDate = DateTime.Now;
                    _updatemanuscriptLoginDetailsForOpen.JobStatusId = _updatemanuscriptLoginDetailsForOpen.JobStatusId;
                    _adminDashBoardDTO.manuscriptLoginDetails.Add(_updatemanuscriptLoginDetailsForOpen);
                    SaveManuscriptLoginDetails(_adminDashBoardDTO);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }


        private int IsJobOpenMSID(AdminDashBoardDTO _adminDashBoardDTO)
        {
            try
            {
                _serviceTypeId = GetServiceTypeId(_adminDashBoardDTO.ServiceType);
                _roleId = GetRoleId(_adminDashBoardDTO.Role);
                var jobStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open").Select(x => x.ID).FirstOrDefault();
                // var jobProcessStatusId = _manuscriptLoginDBRepositoryReadSide.GetStatusMaster().Where(x => x.Description.ToLower() == "open" || x.Description.ToLower() == "fetch" || x.Description.ToLower() == "assigned").Select(x => x.ID).FirstOrDefault();
                var IsJobOpen = (from MSD in manuscriptDataContextRead.ManuscriptLoginDetails
                                 where MSD.CrestId == _adminDashBoardDTO.CrestId
                                       && MSD.RoleId == _roleId
                                       && MSD.ServiceTypeStatusId == _serviceTypeId
                                       && MSD.JobStatusId == jobStatusId
                                 select MSD.JobProcessStatusId).SingleOrDefault();
                if ((Convert.ToInt32(IsJobOpen)) == 7 || (Convert.ToInt32(IsJobOpen)) == 11 || (Convert.ToInt32(IsJobOpen)) == 13)
                    return (Convert.ToInt32(IsJobOpen));
                else
                    return 0;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    manuscriptDataContextRead.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
