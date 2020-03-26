
namespace YAHCMS.WebApp.ViewModels
{
    public class CompletePostViewModel
    {

        public CompletePostViewModel(PostViewModel post)
        {
            this.post = post;
        }
        public PostViewModel post;
        public LocationViewModel location;
        public ArtistViewModel artist;
    }
}