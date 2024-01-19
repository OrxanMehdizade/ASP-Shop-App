namespace ASP_Shop_App.Helpers
{
    public class UploadFileHelper
    {
        public static async Task<string> UploadFile(IFormFile formFile)
        {
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(formFile.FileName)}";
            string fullPath = Path.Combine("wwwroot", "Images", fileName);

            using var fileStream = new FileStream(fullPath, FileMode.Create);
            await formFile.CopyToAsync(fileStream);

            return fullPath;
        }
    }
}
