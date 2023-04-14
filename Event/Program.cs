
public class Program
{
    static void Main(string[] args)
    {
        ImageWaterMarker imageWaterMarker = new ImageWaterMarker(new Photo { Description = "SunSet-Photo" });
        imageWaterMarker.MessageDistributerEvent += subscriber;
        imageWaterMarker.AddWaterMarker();
    }
 

    static void subscriber(string message)
    {
        Console.WriteLine($" received Message and Printing : {message}");
    }
}


class ImageWaterMarker
{
    private Photo photo;

    public delegate void MessageDistributer(string message);

    public event MessageDistributer MessageDistributerEvent;

    public ImageWaterMarker(Photo photo)
    {
        this.photo = photo;
    }
    public void AddWaterMarker()
    {
        Thread.Sleep(2000); //Just to mimic the process
        if(MessageDistributerEvent!=null)
            MessageDistributerEvent($"Done Adding water mark for {this.photo.Description} ");
    }

}

class Photo
{
    public string Description { get; set; }
}