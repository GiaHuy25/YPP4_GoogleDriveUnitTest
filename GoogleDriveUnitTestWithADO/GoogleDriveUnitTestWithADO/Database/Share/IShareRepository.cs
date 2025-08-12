using GoogleDriveUnitTestWithADO.Models;
namespace GoogleDriveUnitTestWithADO.Database.ShareRepo
{
    public interface IShareRepository
    {
        int AddShare(Share share);
        Share GetShareById(int shareId);
        void UpdateShare(Share share);
        void DeleteShare(int shareId);
    }
}
