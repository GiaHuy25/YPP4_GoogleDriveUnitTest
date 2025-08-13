using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.FavoriteObjectRepo;
using GoogleDriveUnittestWithDapper.Services.FavoriteObjectService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Controller
{
    public class FavoriteObjectController
    {
        private readonly IFavoriteObjectService _favoriteObjectService;

        public FavoriteObjectController(IFavoriteObjectService favoriteObjectService)
        {
            _favoriteObjectService = favoriteObjectService;
        }

        public IEnumerable<FavoriteObjectOfUser> GetFavoritesByUserId(int userId)
        {
            return _favoriteObjectService.GetFavoritesByUserId(userId);
        }
    }
}
