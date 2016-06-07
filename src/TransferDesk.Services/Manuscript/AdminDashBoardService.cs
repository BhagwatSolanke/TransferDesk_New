using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.BAL.Manuscript;
using TransferDesk.Contracts.Manuscript.DTO;
using TransferDesk.Services.Manuscript.ViewModel;

namespace TransferDesk.Services.Manuscript
{
    public class AdminDashBoardService
    {
        //public String conString { get; set; }

        AdminDashBoardDTO adminDashBoardDTO { get; set; }
        AdminDashBoardBL adminDashBoardBL { get; set; }
        public AdminDashBoardService(String conString)
        {
            adminDashBoardDTO = new AdminDashBoardDTO();
            adminDashBoardBL = new AdminDashBoardBL(conString);
        }

        public bool AllocateMSIDToUser(AdminDasboardVM adminDasboardVM)
        {
            GetMSIDValues(adminDasboardVM);
            adminDashBoardDTO.AssociateName = adminDasboardVM.AssociateNameVM;
            return adminDashBoardBL.SaveManuscriptLoginDeatils(adminDashBoardDTO);
        }

        public bool UnallocateMSIDFromUser(AdminDasboardVM adminDasboardVM)
        {
            GetMSIDValues(adminDasboardVM);
            return adminDashBoardBL.updateManuscriptLoginDeatils(adminDashBoardDTO);

        }

        private void GetMSIDValues(AdminDasboardVM adminDasboardVM)
        {
            adminDashBoardDTO.CrestId = adminDasboardVM.CrestIdVM;
            adminDashBoardDTO.ServiceType = adminDasboardVM.ServiceTypeVM;
            adminDashBoardDTO.JobProcessingStatus = adminDasboardVM.JobProcessingStatusVM;
            adminDashBoardDTO.Role = adminDasboardVM.RoleVM;
        }
        public bool HoldMSID(AdminDasboardVM adminDasboardVM)
        {
            GetMSIDValues(adminDasboardVM);
            return adminDashBoardBL.updateManuscriptLoginDeatilsForHold(adminDashBoardDTO);

        }
    }

}
