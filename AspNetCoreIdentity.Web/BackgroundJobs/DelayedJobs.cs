using System.Drawing;

namespace AspNetCoreIdentity.Web.BackgroundJobs;

public class DelayedJobs
{
    public static void AddWatermarkJob(string filename, string watermarkText)
    {
        BackgroundJob.Schedule(() => ApplyWatermark(filename, watermarkText),
                           TimeSpan.FromSeconds(30));
    }

    public static void ApplyWatermark(string filename, string watermarkText)
    {
        var basePath = Directory.GetCurrentDirectory();
        var sourcePath = Path.Combine(basePath, "wwwroot/UserPhoto", filename);
        var outputPath = Path.Combine(basePath, "wwwroot/UserPhoto/UserPhotoWithWatermark", filename);

        using var originalImage = (Bitmap)Image.FromFile(sourcePath);
        using var watermarkedImage = new Bitmap(originalImage.Width, originalImage.Height);
        using var graphics = Graphics.FromImage(watermarkedImage);

        // Arkaplanı çiz
        graphics.DrawImage(originalImage, 0, 0);

        // Yazı ayarları
        using var font = new Font(FontFamily.GenericSerif, 10, FontStyle.Bold);
        using var brush = new SolidBrush(Color.FromArgb(255, 0, 0));

        var position = new Point(20, originalImage.Height - 50);

        // Watermark
        graphics.DrawString(watermarkText, font, brush, position);

        // Kaydet
        watermarkedImage.Save(outputPath);
    }
}
