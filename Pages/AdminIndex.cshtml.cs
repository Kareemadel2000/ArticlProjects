using ArticlProjects.Core.Entityes;
using ArticlProjects.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace ArticlProjects.Pages
{
    [Authorize]
    public class AdminIndexModel : PageModel
    {

        private readonly IDataHelper<AuthorPost> _authorPostHelper;

        public AdminIndexModel(IDataHelper<AuthorPost> authorPostHelper)
        {
            _authorPostHelper = authorPostHelper;
        }

        public int AllPost { get; set; }
        public int PostLastMonth { get; set; }
        public int PostThisYear { get; set; }
        public void OnGet()
        {
            var dataM = DateTime.Now.AddMonths(-1);
            var dataY = DateTime.Now.AddYears(-1);
            var userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            AllPost = _authorPostHelper.GetDataByUserId(userid).Count;
            PostLastMonth =_authorPostHelper.GetDataByUserId(userid).Where(x=>x.AddedDate>=dataM).Count();
            PostThisYear = _authorPostHelper.GetDataByUserId(userid).Where(x=>x.AddedDate>=dataY).Count();
        }
    }
}
