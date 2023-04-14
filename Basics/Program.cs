
public class Program
{
    static void Main(string[] args)
    {
        ImageWaterMarker imageWaterMarker = new ImageWaterMarker(new Photo { Description = "SunSet-Photo" });
        imageWaterMarker.AddWaterMarker(subscriber);
    }

    static void subscriber(string message)
    {
        Console.WriteLine(message);
    }
}


class ImageWaterMarker
{
    private Photo photo;

    public delegate void MessageDistributer(string message);

    public ImageWaterMarker(Photo photo)
    {
        this.photo = photo;
    }
    public void AddWaterMarker(MessageDistributer messageDistributer)
    {
        Thread.Sleep(2000); //Just to mimic the process
        messageDistributer($"Done Adding water mark for {this.photo.Description} ");
    }

}

class Photo
{
    public string Description { get; set; }
}