namespace bingoMikes;
public class BingoJatekos
{
    // Azonosító
    public int Azonosito { get; private set; }

    // Név
    public string Nev { get; private set; }

    // Kártya
    public string[,] Kartya { get; private set; }

    // Találatok
    public bool[,] Talalatok { get; private set; }

    // Konstruktor
    public BingoJatekos(int azonosito, string nev, string[,] kartya)
    {
        Azonosito = azonosito;
        Nev = nev;
        Kartya = kartya;

        int sorok = kartya.GetLength(0);
        int oszlopok = kartya.GetLength(1);
        Talalatok = new bool[sorok, oszlopok];

        // Joker ("X") is always marked
        for (int i = 0; i < sorok; i++)
        {
            for (int j = 0; j < oszlopok; j++)
            {
                if (!string.IsNullOrWhiteSpace(Kartya[i, j]) && Kartya[i, j].Trim().ToUpper() == "X")
                {
                    Talalatok[i, j] = true;
                }
            }
        }
    }

    // Jelöli a kisorsolt számot
    public void SorsoltSzamotJelol(int szam)
    {
        int sorok = Kartya.GetLength(0);
        int oszlopok = Kartya.GetLength(1);

        for (int i = 0; i < sorok; i++)
        {
            for (int j = 0; j < oszlopok; j++)
            {
                if (int.TryParse(Kartya[i, j], out int val) && val == szam)
                {
                    Talalatok[i, j] = true;
                }
            }
        }
    }

    // Ellenőrzi, van-e bingó
    public bool BingoEll()
    {
        int sorok = Talalatok.GetLength(0);
        int oszlopok = Talalatok.GetLength(1);

        // sorok
        for (int i = 0; i < sorok; i++)
        {
            bool ok = true;
            for (int j = 0; j < oszlopok; j++)
            {
                if (!Talalatok[i, j]) { ok = false; break; }
            }
            if (ok) return true;
        }

        // oszlopok
        for (int j = 0; j < oszlopok; j++)
        {
            bool ok = true;
            for (int i = 0; i < sorok; i++)
            {
                if (!Talalatok[i, j]) { ok = false; break; }
            }
            if (ok) return true;
        }

        // átlók
        bool diag1 = true;
        for (int i = 0; i < sorok; i++) if (!Talalatok[i, i]) { diag1 = false; break; }
        if (diag1) return true;

        bool diag2 = true;
        for (int i = 0; i < sorok; i++) if (!Talalatok[i, oszlopok - 1 - i]) { diag2 = false; break; }
        if (diag2) return true;

        return false;
    }

    // Visszaadja a megjeleníthető mátrixot (marked numbers or X, else 0)
    public string[,] GetDisplayMatrix()
    {
        int sorok = Kartya.GetLength(0);
        int oszlopok = Kartya.GetLength(1);
        var disp = new string[sorok, oszlopok];
        for (int i = 0; i < sorok; i++)
        {
            for (int j = 0; j < oszlopok; j++)
            {
                if (Talalatok[i, j])
                {
                    disp[i, j] = Kartya[i, j];
                }
                else
                {
                    disp[i, j] = "0";
                }
            }
        }
        return disp;
    }
}
