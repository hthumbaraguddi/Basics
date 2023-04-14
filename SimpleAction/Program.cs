
public class Program
{
    static void Main(string[] args)
    {
        ImageWaterMarker imageWaterMarker = new ImageWaterMarker(new Photo { Description = "SunSet-Photo" });
        //Case 1:
        imageWaterMarker.AddWaterMarker(subscriber);

        //Case 2:
        imageWaterMarker.AddWaterMarker(
                                        (message) =>
                                        {
                                            Console.WriteLine("Inline " + message);
                                        }
                                        );

    }

    static void subscriber(string message)
    {
        Console.WriteLine(message);
    }
}


class ImageWaterMarker
{
    private Photo photo;

    //public delegate void MessageDistributer(string message);

    public ImageWaterMarker(Photo photo)
    {
        this.photo = photo;
    }
    public void AddWaterMarker(Action<string> action)
    {
        Thread.Sleep(2000); //Just to mimic the process
        action($"Done Adding water mark for {this.photo.Description} ");
    }

}

class Photo
{
    public string Description { get; set; }
}