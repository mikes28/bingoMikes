using System;
using System.Collections.Generic;
using System.IO;

namespace bingoMikes;
public static class BingoLoader
{
    public static List<BingoJatekos> LoadPlayers(string namesListPath = "adatok/nevek.text", string dataDir = "adatok")
    {
        var players = new List<BingoJatekos>();

        List<string> files = new List<string>();
        if (File.Exists(namesListPath))
        {
            foreach (var line in File.ReadAllLines(namesListPath))
            {
                var s = line.Trim();
                if (string.IsNullOrEmpty(s)) continue;
                files.Add(s);
            }
        }
        else
        {
            // fallback: use Andi.txt
            var fallback = Path.Combine(dataDir, "Andi.txt");
            if (File.Exists(fallback)) files.Add("Andi.txt");
        }

        if (files.Count == 0)
        {
            // try to list all txt files in dataDir
            if (Directory.Exists(dataDir))
            {
                foreach (var f in Directory.GetFiles(dataDir, "*.txt")) files.Add(Path.GetFileName(f));
            }
        }

        int id = 1;
        foreach (var fname in files)
        {
            if (players.Count >= 100) break;
            var path = Path.Combine(dataDir, fname);
            if (!File.Exists(path)) continue;

            var lines = File.ReadAllLines(path);
            var rows = lines.Length;
            // assume 5x5 structure
            var kartya = new string[5,5];
            for (int i = 0; i < 5 && i < rows; i++)
            {
                var parts = lines[i].Split(';');
                for (int j = 0; j < 5 && j < parts.Length; j++)
                {
                    kartya[i, j] = parts[j].Trim();
                }
            }

            var name = Path.GetFileNameWithoutExtension(fname);
            players.Add(new BingoJatekos(id++, name, kartya));
        }

        return players;
    }
}
