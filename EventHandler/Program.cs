
public class Program
{
    static void Main(string[] args)
    {
        ImageWaterMarker imageWaterMarker = new ImageWaterMarker(new Photo { Description = "SunSet-Photo" });
        ImageProcessor imageProcessor = new ImageProcessor(imageWaterMarker);
        imageWaterMarker.AddWaterMarker();

        imageProcessor = null;
        GC.Collect();

        //Though imageProcessor which is observer is null and called GC.collect also it is still in memory. 

        imageWaterMarker.AddWaterMarker();

        Console.ReadLine();
    } 
   
}

class ImageProcessor
{
    private ImageWaterMarker imageWaterMarker;

    public ImageProcessor(ImageWaterMarker imageWaterMarker)
    {
        this.imageWaterMarker = imageWaterMarker;

        this.imageWaterMarker.MessageDistributer += ImageWaterMarker_MessageDistributer;
    }

    private void ImageWaterMarker_MessageDistributer(object? sender, MessageEventArgs e)
    {
        Console.WriteLine($" received Message and Printing : {e.Message}");
    }
}

class ImageWaterMarker
{
    private Photo photo;

    //public delegate void MessageDistributer(string message);

    //public event MessageDistributer MessageDistributerEvent;

    // updated version is, event EventHandler with event Args
    // where we have removed deleage and then creating event for the delegate.
    public event EventHandler<MessageEventArgs> MessageDistributer;

    static int Count;
    public ImageWaterMarker(Photo photo)
    {
        this.photo = photo;
    }
    public void AddWaterMarker()
    {
        Thread.Sleep(2000); //Just to mimic the process        
        Count++;
        if(MessageDistributer!=null)
            MessageDistributer(this,new MessageEventArgs($"Done Adding water mark for {this.photo.Description} and Count is {Count}"));
    }

}

class Photo
{
    public string Description { get; set; }
}

class MessageEventArgs : EventArgs
{

    public MessageEventArgs(string message)
    {
        Message = message;
    }
    public string Message { get; set; }
}