
using System.Collections.Generic;

namespace YAHCMS.WebApp.ViewModels
{
    public class ArtistWithPostViewModel
    {
        public ArtistWithPostViewModel(ArtistViewModel artist)
        {
            this.artist = artist;
        }

        public ArtistViewModel artist;
        public Dictionary<BlogViewModel, List<PostViewModel>> posts;
    }
}