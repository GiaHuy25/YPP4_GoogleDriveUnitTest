namespace GoogleDriveUnitTestWithADO.Database.Share
{
    public interface IShareRepository
    {
        int AddShare(Models.Share share);
        Models.Share GetShareById(int shareId);
        void UpdateShare(Models.Share share);
        void DeleteShare(int shareId);
    }
}
