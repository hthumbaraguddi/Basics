using System;

public class Program
{
    public static void Main()
    {       

        ImageWaterMarker imageWaterMarker = new ImageWaterMarker(new Photo { Description = "SunSet-Photo" });
        ImageProcessor imageProcessor = new ImageProcessor(imageWaterMarker);
        imageWaterMarker.AddWaterMarker();

        imageProcessor = null;
        GC.Collect();

        imageWaterMarker.AddWaterMarker();

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
    private WeakReference<EventHandler<MessageEventArgs>> _eventSubscriber;
    static int count;
    public event EventHandler<MessageEventArgs> MessageDistributer
    {
        add
        {
            if (_eventSubscriber == null)
            {
                _eventSubscriber = new WeakReference<EventHandler<MessageEventArgs>>(value);
            }
            else
            {
                throw new InvalidOperationException("Cannot subscribe more than one event handler");
            }
        }
        remove
        {
            EventHandler<MessageEventArgs> target;
            _eventSubscriber.TryGetTarget(out target);
            if (value == target)
            {
                _eventSubscriber = null;
            }
        }
    }
    public ImageWaterMarker(Photo photo)
    {
        this.photo = photo;
    }
    public void AddWaterMarker()
    {
        Thread.Sleep(2000); //Just to mimic the process
        count++;
        EventHandler<MessageEventArgs> handler;
        if (_eventSubscriber != null && _eventSubscriber.TryGetTarget(out handler))
        {
            handler(this, new MessageEventArgs($"Done Adding water mark for {this.photo.Description} and counter is {count}"));
        }
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
