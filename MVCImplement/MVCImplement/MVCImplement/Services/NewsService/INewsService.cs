using MVCImplement.Dtos;

namespace MVCImplement.Services.NewsService
{
    public interface INewsService
    {
        List<NewsDto> GetAllNews();
        NewsDto GetNewsById(int id);
    }
}
