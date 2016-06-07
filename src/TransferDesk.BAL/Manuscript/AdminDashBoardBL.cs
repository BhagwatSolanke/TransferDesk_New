using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferDesk.Contracts.Manuscript.DTO;
using TransferDesk.DAL.Manuscript.Repositories;
using TransferDesk.DAL.Manuscript.DataContext;
namespace TransferDesk.BAL.Manuscript
{
    public class AdminDashBoardBL
    {
        
        public AdminDashBoardDBReadSide _adminDashBoardDBReadSide { get; set; }


        public AdminDashBoardBL(string conString)
        {
            //this.conString = conString;
            _adminDashBoardDBReadSide = new AdminDashBoardDBReadSide(conString);
        }

        public bool SaveManuscriptLoginDeatils(AdminDashBoardDTO adminDashBoardDTO)
        {
            return _adminDashBoardDBReadSide.AllocateAssociateToMSID(adminDashBoardDTO);
            
        }

        public bool updateManuscriptLoginDeatils(AdminDashBoardDTO adminDashBoardDTO)
        {
            if(_adminDashBoardDBReadSide.UnallocateAssociateUser(adminDashBoardDTO))
                return true;
            else
                return false;
        }
        public bool updateManuscriptLoginDeatilsForHold(AdminDashBoardDTO adminDashBoardDTO)
        {
            if (_adminDashBoardDBReadSide.HoldMSIDForJob(adminDashBoardDTO))
                return true;
            else
                return false;
        }
    }
}
