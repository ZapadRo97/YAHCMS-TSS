using System;
using System.ComponentModel.DataAnnotations;

namespace YAHCMS.BlogService.Models
{
    public class Post
    {
        [Required]
        public long ID {get; set;}
        public DateTime Published {get; set;}
        public DateTime Updated {get; set;}
        public string Title {get; set;}
        public string Content{get; set;}
        public long BlogID{get; set;}

        public long? ArtistID {get; set;}
        public long? LocationID {get; set;}

        public Post()
        {
            //this.Blog = blog;
            //init dates
            Published = DateTime.Now;
            Updated = DateTime.Now;
        }

        public Post(string title, string content) : this()
        {
            this.Title = title;
            this.Content = content;
        }
    }
}