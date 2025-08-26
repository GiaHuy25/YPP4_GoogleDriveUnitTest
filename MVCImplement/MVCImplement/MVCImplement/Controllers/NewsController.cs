using MVCImplement.Dtos;
using MVCImplement.Models;
using MVCImplement.Services.AuthenService;
using MVCImplement.Services.NewsService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MVCImplement.Controllers
{
    public class NewsController : BaseController
    {
        private readonly INewsService _newsService;
        private readonly IAuthenService _authenService;

        public NewsController(INewsService newsService, IAuthenService authenService)
        {
            _newsService = newsService;
            _authenService = authenService;
        }

        public async Task GetAll(IHttpContextWrapper context)
        {
            var news = _newsService.GetAllNews();
            var json = JsonSerializer.Serialize(news, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await WriteResponse(context.Response, json, 200);
        }

        public async Task Get(IHttpContextWrapper context, int id)
        {
            var news = _newsService.GetNewsById(id);
            if (news == null)
            {
                await WriteResponse(context.Response, "{\"error\":\"News not found\"}", 404);
                return;
            }

            var json = JsonSerializer.Serialize(news, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });
            await WriteResponse(context.Response, json, 200);
        }

        public async Task Post(IHttpContextWrapper context, NewsDto newNews)
        {
            var news = new News
            {
                Title = newNews.Title,
                Content = newNews.Content,
                CreatedAt = DateTime.Now
            };
            _newsService.AddNews(news);
            await WriteResponse(context.Response, "News created successfully", 201);
        }

        public async Task Put(IHttpContextWrapper context, int id, NewsDto updatedNews)
        {
            var existingNews = _newsService.GetNewsById(id);
            if (existingNews == null)
            {
                await WriteResponse(context.Response, "News not found", 404);
                return;
            }

            _newsService.UpdateNews(new News
            {
                Id = id,
                Title = updatedNews.Title,
                Content = updatedNews.Content,
                CreatedAt = existingNews.CreatedAt 
            });

            await WriteResponse(context.Response, "News updated successfully", 200);
        }

        public async Task Delete(IHttpContextWrapper context, int id)
        {
            var existingNews = _newsService.GetNewsById(id);
            if (existingNews == null)
            {
                await WriteResponse(context.Response, "News not found", 404);
                return;
            }

            _newsService.DeleteNews(id);
            await WriteResponse(context.Response, "News deleted successfully", 200);
        }
    }
}
