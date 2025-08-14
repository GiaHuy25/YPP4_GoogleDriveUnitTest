using GoogleDriveUnittestWithDapper.Dto;
using GoogleDriveUnittestWithDapper.Repositories.UserSettingRepo;

namespace GoogleDriveUnittestWithDapper.Services.UserSettingService
{
    public class UserSettingService : IUserSettingService
    {
        private readonly IUserSettingRepository _userSettingRepository;

        public UserSettingService(IUserSettingRepository userSettingRepository)
        {
            _userSettingRepository = userSettingRepository;
        }

        public IEnumerable<UserSettingDto> GetUserSettings(int userId)
        {
            var userSettings = _userSettingRepository.GetUserSettingsByUserId(userId);
            var result = new List<UserSettingDto>();

            foreach (var us in userSettings)
            {
                var key = _userSettingRepository.GetAppSettingKeyById(us.AppSettingKeyId);
                var option = us.AppSettingOptionId.HasValue
                    ? _userSettingRepository.GetAppSettingOptionById(us.AppSettingOptionId.Value)
                    : null;

                if (key != null)
                {
                    result.Add(new UserSettingDto
                    {
                        SettingKey = key.SettingKey ?? string.Empty,
                        IsBoolean = key.IsBoolean ?? 0,
                        SettingValue = option?.SettingValue ?? string.Empty
                    });
                }
            }

            return result;
        }
    }
}
