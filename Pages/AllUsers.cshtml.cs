using ArticlProjects.Core.Entityes;
using ArticlProjects.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArticlProjects.Pages
{
    public class AllUsersModel : PageModel
    {


       
        public readonly int NoOfItem;
        private readonly IDataHelper<Core.Entityes.Author> _dataHelper;

        public AllUsersModel(IDataHelper<Core.Entityes.Author> dataHelper)
        {
            _dataHelper = dataHelper;
            NoOfItem = 6;
            ListOfAuthor = new List<Core.Entityes.Author>();
        }

        public List<Core.Entityes.Author> ListOfAuthor { get; set; }
        public void OnGet(string LoadState,string search, int id)
        {
           
            if (LoadState == null || LoadState == "All")
            {
                GetAllAuthor();
            }
            
            else if (LoadState == "Search")
            {
                SearchData(search);
            }
            else if (LoadState == "Next")
            {
                GetNaxtData(id);
            }
            else if (LoadState == "Prev")
            {
                GetNaxtData(id - NoOfItem);
            }

        }

        private void GetAllAuthor()
        {
            ListOfAuthor = _dataHelper.GetAll().Take(NoOfItem).ToList();
        }
        private void SearchData(string SearchItem)
        {
            ListOfAuthor = _dataHelper.Search(SearchItem);
        }

        private void GetNaxtData(int id)
        {
            ListOfAuthor = _dataHelper.GetAll().Where(x => x.Id > id).Take(NoOfItem).ToList();
        }
    }
}
