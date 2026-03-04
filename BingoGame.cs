using System;
using System.Collections.Generic;

namespace bingoMikes;

public static class BingoGame
{
    public static void Run()
    {
        // 3/4. Betöltés és számlálás (4. feladat)
        var players = BingoLoader.LoadPlayers();

        Console.WriteLine("4. feladat");
        Console.WriteLine($"Játékosok száma: {players.Count}");

        if (players.Count == 0)
        {
            Console.WriteLine("Nincs betöltött játékos. Kilépés.");
            return;
        }

        // 7. számhúzás nyertesig
        var nums = new List<int>();
        for (int i = 1; i <= 75; i++) nums.Add(i);
        var rnd = new Random();
        for (int i = nums.Count - 1; i > 0; i--)
        {
            int j = rnd.Next(i + 1);
            var tmp = nums[i]; nums[i] = nums[j]; nums[j] = tmp;
        }

        List<int> drawn = new List<int>();
        List<BingoJatekos> winners = new List<BingoJatekos>();

        // Számhúzások fejléc
        Console.WriteLine();
        Console.WriteLine("Számhúzások:");
        const int perLine = 10;
        for (int turn = 0; turn < nums.Count; turn++)
        {
            int value = nums[turn];
            drawn.Add(value);

            foreach (var p in players) p.SorsoltSzamotJelol(value);

            winners.Clear();
            foreach (var p in players) if (p.BingoEll()) winners.Add(p);

            // Húzások kiírása oszlopban: "sorsz. érték"
            int col = turn % perLine;
            string item = $"{turn + 1,2}. {value,2}";
            Console.Write(item.PadRight(8));
            if (col == perLine - 1) Console.WriteLine();

            if (winners.Count > 0) 
            {
                if (turn % perLine != perLine - 1) Console.WriteLine();
                break;
            }
        }

        // 8. nyertesek és kártyák, igazított
        Console.WriteLine();
        Console.WriteLine("8. feladat");
        if (winners.Count == 0)
        {
            Console.WriteLine("Nincs nyertes.");
            return;
        }

        Console.WriteLine("Nyertes(ek):");
        foreach (var w in winners) Console.WriteLine($" - {w.Nev}");

        foreach (var w in winners)
        {
            Console.WriteLine();
            Console.WriteLine(new string('-', 30));
            Console.WriteLine($"Név: {w.Nev}");
            Console.WriteLine("Jelölt számok (X = Joker), nem jelölt: 0");
            Console.WriteLine();

            var m = w.GetDisplayMatrix();
            int rows = m.GetLength(0);
            int cols = m.GetLength(1);

            // Oszlopfejlécek
            for (int j = 0; j < cols; j++) Console.Write($"{(j+1),4}");
            Console.WriteLine();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var cell = m[i, j] ?? "0";
                    // Replace empty or null with 0, keep X as is
                    if (string.IsNullOrWhiteSpace(cell)) cell = "0";
                    Console.Write(cell.PadLeft(4));
                }
                Console.WriteLine();
            }
            Console.WriteLine(new string('-', 30));
        }
    }
}

