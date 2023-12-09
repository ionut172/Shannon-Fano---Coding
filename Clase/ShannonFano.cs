using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace ShannonFano.Clase
{
    public class ShannonFano
    {
        private List<ShannonFanoNod> nodes;

        public ShannonFano()
        {
            nodes = new List<ShannonFanoNod>();
        }

        public void Encode(string source)
        {
            CalculateFrequencies(source);
            BuildTree(0, nodes.Count - 1);
            AssignCodes(nodes.First(), "");
            DisplayCodes();
            DisplayEncodedSource(source);
        }

        private string EncodeSource(string source)
        {
            // Utilizăm LINQ pentru a obține codul binar pentru întreaga sursă
            string binaryString = string.Join("", source.Select(c => nodes.Find(n => n.Symbol == c).Code));

            return binaryString;
        }

        public void DisplayEncodedSource(string source)
        {
            string encodedSource = EncodeSource(source);
            Console.WriteLine($"\nEncoded Source: {encodedSource}");
        }

        private void CalculateFrequencies(string source)
        {
            var frequencyDict = new Dictionary<char, int>();

            // Iterăm prin fiecare simbol din sursa de date
            foreach (char symbol in source)
            {
                // Actualizăm frecvența pentru simbol în dicționar
                if (frequencyDict.ContainsKey(symbol))
                {
                    frequencyDict[symbol]++;
                }
                else
                {
                    frequencyDict[symbol] = 1;
                }
            }

            // Adăugăm toate simbolurile într-un singur nod, sortate după frecvență în ordine crescătoare
            var distinctNodes = frequencyDict.Select(pair => new ShannonFanoNod
            {
                Symbol = pair.Key,
                Frequency = pair.Value
            }).OrderBy(node => node.Frequency).ToList();

            // Adăugăm nodurile distincte în lista de noduri
            nodes.AddRange(distinctNodes);
        }



        private void BuildTree(int start, int end)
        {
            if (start == end)
            {
                return;
            }

            int totalFrequency = nodes.GetRange(start, end - start + 1).Sum(node => node.Frequency);
            int runningTotal = 0;
            int splitIndex = start;

            while (runningTotal + nodes[splitIndex].Frequency <= totalFrequency / 2)
            {
                runningTotal += nodes[splitIndex].Frequency;
                splitIndex++;
            }

            for (int i = start; i <= end; i++)
            {
                nodes[i].Code += (i < splitIndex) ? '0' : '1';
            }

            BuildTree(start, splitIndex - 1);
            BuildTree(splitIndex, end);
        }

        private void AssignCodes(ShannonFanoNod node, string code)
        {
            node.Code += code;

            if (node.Symbol != default(char))
            {
                Console.WriteLine($"Symbol: {node.Symbol}, Code: {node.Code}");

            }
            else
            {
                AssignCodes(nodes.Find(n => n.Code == node.Code + '0'), "0");
                AssignCodes(nodes.Find(n => n.Code == node.Code + '1'), "1");
            }
        }

        private void DisplayCodes()
        {
            Console.WriteLine("\nShannon-Fano Codes:");
            foreach (var node in nodes.OrderBy(n => n.Code))
            {
                Console.WriteLine($"Symbol: {node.Symbol}, Code: {node.Code}");

            }

        }
    }
}
