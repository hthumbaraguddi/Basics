
public class Program
{
    static void Main(string[] args)
    {
        SimpleMessagePublisher publisher = new SimpleMessagePublisher();
        FormatedMessagePublisher formatedMessagePublisher = new FormatedMessagePublisher();

        MessageDistributor.MessageInvoke invoke= new MessageDistributor.MessageInvoke(publisher.SimpleMessage);
        invoke += formatedMessagePublisher.FormatedMessage;

        //Message is broadcasting & but we can retrive the last return message
        var str =invoke("Hello World");

        Console.WriteLine(str);

        foreach (MessageDistributor.MessageInvoke item in invoke.GetInvocationList())
        {
            Console.WriteLine(item("Simple Message"));
        }

        Console.ReadKey();
    }
}


class MessageDistributor
{
    public delegate string MessageInvoke(string message);   
}

class SimpleMessagePublisher
{
    public string SimpleMessage(string message)
    {
        Console.WriteLine("printing simple way of messaging : " + message);
        return message; 
    }
}

class FormatedMessagePublisher
{
    public string FormatedMessage(string message)
    {
        Console.WriteLine($"printing Formated@@$$$ {message}");
        return ($"Formated@@$$$ {message}");
    }
}
