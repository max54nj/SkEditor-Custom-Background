using Avalonia.Media.Imaging;
using SkiaSharp;

namespace CustomBackgroundAddon.Utilities.Image;

public static class Blur
{
    /// <summary>
    /// Applies a blur effect to an image file and returns the blurred bitmap
    /// </summary>
    /// <param name="filePath">Path to the image file</param>
    /// <param name="blurAmount">Amount of blur to apply (0 = no blur)</param>
    /// <returns>Blurred Bitmap, or null if failed to load</returns>
    public static Bitmap? ApplyBlur(string filePath, float blurAmount)
    {
        
        using var originalBitmap = SKBitmap.Decode(filePath);
        if (originalBitmap == null)
            return null;

        var processedBitmap = ApplyBlur(originalBitmap, blurAmount);
        
        using var image = SKImage.FromBitmap(processedBitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        using var memoryStream = new MemoryStream();
        data.SaveTo(memoryStream);
        memoryStream.Position = 0;

        return new Bitmap(memoryStream);
    }

    /// <summary>
    /// Applies a blur effect to an existing SKBitmap
    /// </summary>
    /// <param name="originalBitmap">The bitmap to blur</param>
    /// <param name="blurAmount">Amount of blur to apply (0 = no blur)</param>
    /// <returns>Blurred SKBitmap</returns>
    public static SKBitmap ApplyBlur(SKBitmap originalBitmap, float blurAmount)
    {
        if (blurAmount <= 0)
            return originalBitmap.Copy();

        var blurPadding = (int)Math.Ceiling(blurAmount / 2) * 3;
        var paddedWidth = originalBitmap.Width + (blurPadding * 2);
        var paddedHeight = originalBitmap.Height + (blurPadding * 2);
        
        var paddedInfo = new SKImageInfo(paddedWidth, paddedHeight);
        using var paddedSurface = SKSurface.Create(paddedInfo);
        var paddedCanvas = paddedSurface.Canvas;
        
        using (var paint = new SKPaint())
        {
            paddedCanvas.DrawBitmap(originalBitmap, 
                new SKRect(0, 0, paddedWidth, paddedHeight), paint);
            
            paddedCanvas.DrawBitmap(originalBitmap, blurPadding, blurPadding, paint);
        }
        
        paddedCanvas.Flush();
        using var paddedSnapshot = paddedSurface.Snapshot();
        using var paddedBitmap = SKBitmap.FromImage(paddedSnapshot);
        
        var imageInfo = new SKImageInfo(paddedWidth, paddedHeight);
        using var surface = SKSurface.Create(imageInfo);
        var canvas = surface.Canvas;
        canvas.Clear(SKColors.Transparent);

        using (var paint = new SKPaint())
        {
            paint.IsAntialias = true;
            paint.ImageFilter = SKImageFilter.CreateBlur(blurAmount / 2, blurAmount / 2);
            canvas.DrawBitmap(paddedBitmap, 0, 0, paint);
        }
        
        canvas.Flush();

        using var blurredSnapshot = surface.Snapshot();
        using var blurredBitmap = SKBitmap.FromImage(blurredSnapshot);
        
        var processedBitmap = new SKBitmap(originalBitmap.Width, originalBitmap.Height);
        using (var cropCanvas = new SKCanvas(processedBitmap))
        {
            var sourceRect = new SKRect(blurPadding, blurPadding, 
                blurPadding + originalBitmap.Width, blurPadding + originalBitmap.Height);
            var destRect = new SKRect(0, 0, originalBitmap.Width, originalBitmap.Height);
            cropCanvas.DrawBitmap(blurredBitmap, sourceRect, destRect);
        }

        return processedBitmap;
    }
}