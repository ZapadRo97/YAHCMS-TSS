using System.Collections.Generic;

namespace YAHCMS.WebApp.ViewModels
{

    public class SendAnswersViewModel
    {
        public long ID {get; set;}
        public ICollection<string> Answers {get; set;}
    }

}