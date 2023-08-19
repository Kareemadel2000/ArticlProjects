using ArticlProjects.Core.Entityes;
using ArticlProjects.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArticlProjects.Pages
{
    public class ArticleModel : PageModel
    {
        private readonly IDataHelper<AuthorPost> _dataHelperForPost;
        public AuthorPost authorPost;
        public ArticleModel(IDataHelper<AuthorPost> dataHelperForPost)
        {
            _dataHelperForPost = dataHelperForPost;
            authorPost = new AuthorPost();
        }

      
        

        public void OnGet()
        {
            var id = HttpContext.Request.RouteValues["id"];
            authorPost = _dataHelperForPost.Find(Convert.ToInt32(id));
        }
    }
}
