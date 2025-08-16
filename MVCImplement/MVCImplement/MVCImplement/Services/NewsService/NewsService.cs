using MVCImplement.Dtos;
using MVCImplement.Models;
using MVCImplement.Repositories;

namespace MVCImplement.Services.NewsService
{
    public class NewsService : INewsService
    {
        private readonly IRepository<NewsModel> _newsRepository = new Repository<NewsModel>();

        public NewsService(IRepository<NewsModel> newsRepository)
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
            var newsQuery = _newsRepository.GetById(id);
            var newsModel = newsQuery.FirstOrDefault();
            if (newsModel == null) return null;
            return new NewsDto
            {
                Id = newsModel.Id,
                Title = newsModel.Title,
                Content = newsModel.Content,
                CreatedAt = newsModel.CreatedAt
            };
        }
    }
}
