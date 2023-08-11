namespace ArticlProjects.Code
{
    public class FilesHelper
    {
        private readonly IWebHostEnvironment _webHost;

        public FilesHelper(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        //Upload File

        public string UploadFile(IFormFile file,string folder)
        {
            if (file != null)
            {
                var fileDir = Path.Combine(_webHost.WebRootPath,folder);
                var fileName = Guid.NewGuid() + "-" +file.FileName;
                var filePath = Path.Combine(fileDir,fileName);

                using FileStream fileStream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(fileStream);
                    return fileName;
                
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
