using System;

public class Program
{
    public static void Main()
    {

        ImageWaterMarker imageWaterMarker = new ImageWaterMarker(new Photo { Description = "SunSet-Photo" });
        ImageProcessor imageProcessor = new ImageProcessor(imageWaterMarker);
        ProcessManager processManager = new ProcessManager();
        imageWaterMarker.MessageDistributer += processManager.MessageReceiver;
        imageWaterMarker.AddWaterMarker();

        imageProcessor = null;
        GC.Collect();

        imageWaterMarker.AddWaterMarker();

    }


}

class ProcessManager
{
    public void MessageReceiver(object sender, MessageEventArgs e)
    {
        Console.WriteLine($"Message received in Process Manager : {e.Message}");
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
    private List<WeakReference<EventHandler<MessageEventArgs>>> _eventSubscriber = new List<WeakReference<EventHandler<MessageEventArgs>>>();
    
    public event EventHandler<MessageEventArgs> MessageDistributer
    {
        add
        {
            _eventSubscriber.Add(new WeakReference<EventHandler<MessageEventArgs>>(value));
        }
        remove
        {
            EventHandler<MessageEventArgs> target;
            foreach (var weakref in _eventSubscriber)
            {
                if (weakref.TryGetTarget(out var handler) && handler == value)
                {
                    _eventSubscriber.Remove(weakref);
                }
            }
        }
    }

    public int Count
    {        
        get
        {
            int count = 0;
            EventHandler<MessageEventArgs> handler;
            foreach (var weakRef in _eventSubscriber.ToList())
            {
                if (weakRef.TryGetTarget(out handler))
                {
                    count++;
                }
                else
                {
                    _eventSubscriber.Remove(weakRef);
                }
            }
            return count;
        }
    }
    public ImageWaterMarker(Photo photo)
    {
        this.photo = photo;
    }
    public void AddWaterMarker()
    {
        Thread.Sleep(2000); //Just to mimic the process       
        EventHandler<MessageEventArgs> handler;

        Console.WriteLine($"Total Number of subscribers are {Count}");
        foreach (var weakRef in _eventSubscriber.ToList())
        {
            if (weakRef.TryGetTarget(out handler))
            {
                Console.WriteLine($"Target is {handler.Target.ToString()} and Method is fire is {handler.Method.ToString()}");
            }
            else
            {
                _eventSubscriber.Remove(weakRef);
            }
        }

        Console.WriteLine($"Broadcasting messages to all subscribers");
        
        foreach (var weakRef in _eventSubscriber.ToList())
        {
            if (weakRef.TryGetTarget(out handler))
            {
                handler(this, new MessageEventArgs($"Done Adding water mark for {this.photo.Description}"));
            }
            else
            {
                _eventSubscriber.Remove(weakRef);
            }
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
