class Program
{
    static void Main()
    {
        string source = "Programare Shannon";

        ShannonFano.Clase.ShannonFano shannonFano = new ShannonFano.Clase.ShannonFano();
        shannonFano.Encode(source);
        Console.WriteLine($"Cuvant introdus: {source}");
    }
}
