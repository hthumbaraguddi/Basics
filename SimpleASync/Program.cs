using System.Runtime.InteropServices;

public class Program
{
    static async Task Main(string[] args)
    {
        //DateTime date= DateTime.Now;
        //new Breakfast().PrepareBreakfast();
        //DateTime date2 = DateTime.Now;
        //Console.WriteLine(date2 - date);

        var asyncBreakfast = new AsyncBreakfast().PrepareBreakfast();
        await asyncBreakfast;
        DateTime date3 = DateTime.Now;
        // Console.WriteLine(date3 - date2);

        Console.ReadLine();

    }
}

public class Breakfast
{
    public void PrepareBreakfast()
    {
        PourCoffee();
        HeatPan();
        FryEggs();
        HeatPan();
        FryBecon();
        ToadBread();
        JamOnBread();
        PourJuice();
    }

    private void PourCoffee()
    {
        Thread.Sleep(1000);
        Console.WriteLine("Coffee is Ready");
    }
    private void HeatPan()
    {
        Thread.Sleep(5000);
        Console.WriteLine("Pan is ready");
    }
    private void FryEggs()
    {
        Thread.Sleep(1000);
        Console.WriteLine("Eggs are Ready");
    }

    private void FryBecon()
    {
        Thread.Sleep(3000);
        Console.WriteLine("Beacon are Ready");
    }

    private void ToadBread()
    {
        Thread.Sleep(3000);
        Console.WriteLine("Breads are Ready");
    }

    private void JamOnBread()
    {
        Thread.Sleep(5000);
        Console.WriteLine("Breads with Jam are Ready");
    }

    private void PourJuice()
    {
        Thread.Sleep(1000);
        Console.WriteLine("Juice is Ready");
    }
}

public class AsyncBreakfast
{
    public async Task PrepareBreakfast()
    {
       Task coffee = PourCoffee();
       Task pan =HeatPan();
       Task Eggs =FryEggs();
       Task pan2=HeatPan();
       Task becon=FryBecon();
       Task bread=ToadBread();
       Task jam=JamOnBread();
       Task juice =PourJuice();
        Task[] firstSet = new Task[]
        {
            coffee,pan,bread,juice
        };

        Task[] SecondSet = new Task[]
        {
            Eggs,jam
        };
        await Task.WhenAll(firstSet).ConfigureAwait(true);
        Console.WriteLine("First set is completed");
        await Task.WhenAll(SecondSet);
        await pan2;
        await becon;

        Console.WriteLine("Breakfast is ready");

    }

    private async Task PourCoffee()
    {
        Console.WriteLine("1. Staring to Prepare Coffee");
        await Task.Delay(1000);
        Console.WriteLine("1. Coffee is Ready");
    }
    private async Task HeatPan()
    {
        Console.WriteLine("2. Heating Up pan");
        await Task.Delay(5000);
        Console.WriteLine("2. Pan is ready");
    }
    private async Task FryEggs()
    {
        Console.WriteLine("3. Starting Frying Eggs");
        await Task.Delay(2000);
        Console.WriteLine("3. Eggs are Ready");
    }

    private async Task FryBecon()
    {
        Console.WriteLine("4. Starting to Prepare Beacon");
        await Task.Delay(4000);
        Console.WriteLine("4. Beacon are Ready");
    }

    private async Task ToadBread()
    {
        Console.WriteLine("5. Starting to Prepare Bread");
        await Task.Delay(10000);
        Console.WriteLine("5. Breads are Ready");
    }

    private async Task JamOnBread()
    {
        Console.WriteLine("5. Starting to Prepare Jam");
        await Task.Delay(5000);
        Console.WriteLine("5. Breads with Jam are Ready");
    }

    private async Task PourJuice()
    {
        Console.WriteLine("6. Pouring Juice");
        await Task.Delay(1000);
        Console.WriteLine("6. Pouring juice is Done");
    }
}

