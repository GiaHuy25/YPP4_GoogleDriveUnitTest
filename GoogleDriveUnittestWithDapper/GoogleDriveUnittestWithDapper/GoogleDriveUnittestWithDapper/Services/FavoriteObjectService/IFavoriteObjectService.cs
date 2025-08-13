using GoogleDriveUnittestWithDapper.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Services.FavoriteObjectService
{
    public interface IFavoriteObjectService
    {
        IEnumerable<FavoriteObjectOfUser> GetFavoritesByUserId(int userId);
    }
}
