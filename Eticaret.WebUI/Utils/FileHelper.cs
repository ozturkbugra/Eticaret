namespace Eticaret.WebUI.Utils
{
    public class FileHelper
    {
        public static async Task<string> FileLoaderAsync(IFormFile formFile, string filePath="/img/")
        {
            string fileName = "";
            if (formFile != null && formFile.Length > 0) {

                // Dosya adı ve uzantıyı ayırıyoruz
                string originalFileName = Path.GetFileNameWithoutExtension(formFile.FileName).ToLower();
                string extension = Path.GetExtension(formFile.FileName).ToLower();

                // Yeni dosya adını oluşturuyoruz
                fileName = $"{originalFileName}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{extension}";

                // Kayıt dizinini oluşturuyoruz
                string directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'), fileName);

                // Dosyayı kaydediyoruz
                using var stream = new FileStream(directory, FileMode.Create);
                await formFile.CopyToAsync(stream);
            }
            return fileName;
        }

        public static bool FileRemover(string fileName, string filePath = "/img/")
        {
            string directory = Directory.GetCurrentDirectory() + "/wwwroot" + filePath + fileName;
            if (File.Exists(directory))
            {
                File.Delete(directory);
                return true;
            }
            return false;
        }
    }
}
