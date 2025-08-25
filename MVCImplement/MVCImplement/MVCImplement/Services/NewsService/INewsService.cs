using MVCImplement.Dtos;
using MVCImplement.Models;

namespace MVCImplement.Services.NewsService
{
    public interface INewsService
    {
        List<NewsDto> GetAllNews();
        NewsDto GetNewsById(int id);
        void AddNews(News news);
        void UpdateNews(News news);
        void DeleteNews(int id);
    }
}
