using ArticlProjects.Core.Entityes;
using ArticlProjects.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArticlProjects.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IDataHelper<Category> _dataHelperforCategory;
        private readonly IDataHelper<AuthorPost> _dataHelperForAuthorPost;
        private readonly int NoOfItem;

        public IndexModel(
            ILogger<IndexModel> logger,
            IDataHelper<Category> dataHelperforCategory,
            IDataHelper<AuthorPost> dataHelperForAuthorPost
            )
        {
            _logger = logger;
            _dataHelperforCategory = dataHelperforCategory;
            _dataHelperForAuthorPost = dataHelperForAuthorPost;
            NoOfItem = 6;
            ListOfCategory = new List<Category>();
            ListOfPost = new List<AuthorPost>();
        }

        public List<Category> ListOfCategory { get; set; }
        public List<AuthorPost> ListOfPost { get; set; }
        public void OnGet(string LoadState, string CategoryName, string search)
        {
            GetAllCategory();
            if (LoadState == null || LoadState == "All")
            {
                GetAllPost();
            }
            else if (LoadState == "ByCategory")
            {
                GetDataByCategoryName(CategoryName);
            }
            else if (LoadState == "Search")
            {
                SearchData(search);
            }

        }

        private void GetAllCategory()
        {
            ListOfCategory = _dataHelperforCategory.GetAll();
        }
        private void GetAllPost()
        {
            ListOfPost = _dataHelperForAuthorPost.GetAll().Take(NoOfItem).ToList();
        }
        private void GetDataByCategoryName(string CategoryName)
        {
            ListOfPost = _dataHelperForAuthorPost.GetAll().Where(x => x.PostCategory == CategoryName).Take(NoOfItem).ToList();
        }
        private void SearchData(string SearchItem)
        {
            ListOfPost = _dataHelperForAuthorPost.Search(SearchItem);
        }
    }
}