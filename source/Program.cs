namespace unknown_horizons_spriteconversion;
class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("You must specify the source and target directories!");
            Environment.Exit(0);
        }

        Converter converter = new Converter(args[0], args[1]);
        converter.ConvertImages();
    }
}
