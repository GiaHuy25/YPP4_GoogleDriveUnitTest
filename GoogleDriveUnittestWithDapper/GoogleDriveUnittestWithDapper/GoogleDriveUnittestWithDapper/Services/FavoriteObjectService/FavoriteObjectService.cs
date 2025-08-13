using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.FavoriteObjectRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleDriveUnittestWithDapper.Services.FavoriteObjectService
{
    public class FavoriteObjectService : IFavoriteObjectService
    {
        private readonly IFavoriteObjectRepository _favoriteObjectRepository;

        public FavoriteObjectService(IFavoriteObjectRepository favoriteObjectRepository)
        {
            _favoriteObjectRepository = favoriteObjectRepository;
        }

        public IEnumerable<FavoriteObjectOfUser> GetFavoritesByUserId(int userId)
        {
            return _favoriteObjectRepository.GetFavoritesByUserId(userId);
        }
    }
}
