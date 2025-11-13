namespace Internship_2_C_Sharp
{
    internal class Program
    {
        static int ShowMenu(string[] options)
        {
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine(
                    $"{(i != options.Length - 1 ? i + 1 : 0)} - {options[i]}"
                );
            }

            bool enteredCorrectly = false;
            do
            {
                Console.Write("\nOdabir: ");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (!(choice > options.Length - 1 || choice < 0))
                    {
                        Console.WriteLine();
                        enteredCorrectly = true;
                        return choice;
                    }
                }
                Console.WriteLine("\nNije unesen pravilan odabir. Pokušajte ponovno.");
            } while (!enteredCorrectly);

            throw new Exception("ShowMenu je izašao iz petlje bez da je returnao value.");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("APLIKACIJA ZA EVIDENCIJU GORIVA");

            var menuMain = new string[]
            {
                "Korisnici",
                "Putovanja",
                "Izlaz iz aplikacije"
            };

            var menuUser = new string[]
            {
                "Unos novog korisnika",
                "Brisanje korisnika",
                "Uređivanje korisnika",
                "Pregled svih korisnika",
                "Povratak na glavni izbornik"
            };

            var menuTravel = new string[]
            {
                "Unos novog putovanja",
                "Brisanje putovanja",
                "Uređivanje postojećeg putovanja",
                "Pregled svih putovanja",
                "Izvještaji i analize",
                "Povratak na glavni izbornik"
            };

            switch (ShowMenu(menuMain))
            {
                case 1:
                    ShowMenu(menuUser);
                    break;
                case 2:
                    ShowMenu(menuTravel);
                    break;
                case 0:
                    return;
                default:
                    return;
            }
        }
    }
}
