using MVCImplement.Dtos;
using MVCImplement.Models;
using MVCImplement.Repositories;

namespace MVCImplement.Services.NewsService
{
    public class NewsService : INewsService
    {
        private readonly IRepository<News> _newsRepository = new Repository<News>();

        public NewsService(IRepository<News> newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public List<NewsDto> GetAllNews()
        {
            var newsQuery = _newsRepository.GetAll();
            return newsQuery.OrderByDescending(n => n.CreatedAt)
                           .Select(model => new NewsDto
                           {
                               Id = model.Id,
                               Title = model.Title,
                               Content = model.Content,
                               CreatedAt = model.CreatedAt
                           }).ToList();
        }

        public NewsDto GetNewsById(int id)
        {
            Console.WriteLine($"NewsService.GetNewsById called with ID: {id} at {DateTime.Now}");
            try
            {
                var newsQuery = _newsRepository.GetById(id);
                var newsModel = newsQuery.FirstOrDefault();
                Console.WriteLine($"NewsService.GetNewsById({id}): {(newsModel != null ? $"Found: {newsModel.Title}" : "Not found")} at {DateTime.Now}");
                if (newsModel == null) return null;
                return new NewsDto
                {
                    Id = newsModel.Id,
                    Title = newsModel.Title,
                    Content = newsModel.Content,
                    CreatedAt = newsModel.CreatedAt
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in NewsService.GetNewsById({id}): {ex.Message}, StackTrace: {ex.StackTrace}, Time: {DateTime.Now}");
                return null;
            }
        }

        public void AddNews(News news)
        {
            _newsRepository.Add(news);
        }

        public void UpdateNews(News news)
        {
            _newsRepository.Update(news);
        }

        public void DeleteNews(int id)
        {
            _newsRepository.Delete(id);
        }
    }
}
