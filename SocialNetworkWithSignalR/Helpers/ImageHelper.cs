namespace SocialNetworkWithSignalR.Helpers
{
    public class ImageHelper
    {
        private readonly IWebHostEnvironment _webhost;

        public ImageHelper(IWebHostEnvironment webhost)
        {
            _webhost = webhost;
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            var saveImage=Path.Combine(_webhost.WebRootPath,"images",file.FileName);
            using (var img=new FileStream(saveImage,FileMode.OpenOrCreate))
            {
                await file.CopyToAsync(img);
            }
            return file.FileName.ToString();
        }
    }
}
