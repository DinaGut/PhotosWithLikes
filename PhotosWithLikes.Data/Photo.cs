using System.Collections.Generic;

namespace PhotosWithLikes.Data
{
    public class Photo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public int Likes { get; set; }
        public DateTime DateUploaded { get; set; }

    }
}