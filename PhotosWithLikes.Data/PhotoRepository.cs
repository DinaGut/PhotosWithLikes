using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotosWithLikes.Data
{
    public class PhotoRepository
    {
        private string _connectionString;

        public PhotoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Photo> GetPhotos()
        {
            using var context = new PhotoDBContext(_connectionString);
            return context.Photos.ToList();
        }
        public void AddPhoto(Photo photo)
        {
            using var context = new PhotoDBContext(_connectionString);
            context.Photos.Add(photo);
            context.SaveChanges();
        }
        public Photo GetPhotoByID(int id)
        {
            using var context = new PhotoDBContext(_connectionString);

            return context.Photos.FirstOrDefault(i => i.Id == id);

        }
        public void UpdateLikes(int id)
        {
            using var context = new PhotoDBContext(_connectionString);
            context.Database.ExecuteSqlInterpolated($"UPDATE Photos SET Likes = likes + 1 WHERE Id = {id}");
        }
        public int GetLikes(int id)
        {
            using var context = new PhotoDBContext(_connectionString);
            return context.Photos.FirstOrDefault(i => i.Id == id).Likes;
        }

    }
}