
using Microsoft.AspNetCore.Mvc;
using PhotosWithLikes.Data;
using PhotosWithLikesHW.Models;
using System.Diagnostics;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace PhotosWithLikesHW.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString;
        private IWebHostEnvironment _environment;

        public HomeController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _environment = webHostEnvironment;
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public IActionResult Index()
        {
            var repo = new PhotoRepository(_connectionString);
            IndexViewModel ivm = new()
            {
                Photos = repo.GetPhotos()
            };
            return View(ivm);
        }


        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile imageFile, string title)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            string fullPath = Path.Combine(_environment.WebRootPath, "Upload", fileName);
            using var stream = new FileStream(fullPath, FileMode.CreateNew);
            imageFile.CopyTo(stream);
            Photo photo = new()
            {
                FileName = fileName,
                Title = title,
                DateUploaded = DateTime.Now,
            };
            var repo = new PhotoRepository(_connectionString);
            repo.AddPhoto(photo);
            return Redirect("/");
        }
        public IActionResult ViewPhoto(int id)
        {
            if (id == 0)
            {
                return Redirect("/home/index");
            }

            var repo = new PhotoRepository(_connectionString);
            var pvm = new PhotoViewModel()
            {
                photo = repo.GetPhotoByID(id),
                
            };
            var SessionIDs = HttpContext.Session.Get<List<int>>("SessionIDs");
            if (SessionIDs != null && SessionIDs.Contains(pvm.photo.Id))
            {
                pvm.Liked = true;
            }


            return View(pvm);
        }
        [HttpPost]
        public void UpdateLikes(int id)
        {
            var repo = new PhotoRepository(_connectionString);
            repo.UpdateLikes(id);
            var SessionIDs = HttpContext.Session.Get<List<int>>(" SessionIDs");
            if (SessionIDs == null)
            {
                SessionIDs = new List<int>();
            }

            SessionIDs.Add(id);
            HttpContext.Session.Set("SessionIDs", SessionIDs);


        }
        public IActionResult GetLikes(int id)
        {
            var repo = new PhotoRepository(_connectionString);
            int likes = repo.GetLikes(id);
            return Json(likes);
        }

    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonSerializer.Deserialize<T>(value);
        }
    }
}