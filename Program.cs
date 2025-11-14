#pragma warning disable IDE0044
#pragma warning disable IDE0059

namespace Internship_2_C_Sharp
{
    internal class Program
    {
        static readonly string title = "APLIKACIJA ZA EVIDENCIJU GORIVA";

        static int userLatestId = 0;
        static int tripLatestId = 0;
        static List<int> userIds = [];
        static Dictionary<int, string> userNames = [];
        static Dictionary<int, string> userSurnames = [];
        static Dictionary<int, DateTime> userDatesOfBirth = [];
        static Dictionary<int, List<int>> userTripIds = [];
        static List<int> tripIds = [];
        static Dictionary<int, DateTime> tripDates = [];
        static Dictionary<int, decimal> tripDistances = [];
        static Dictionary<int, decimal> tripOilUsedUp = [];
        static Dictionary<int, decimal> tripOilPrices = [];
        static Dictionary<int, decimal> tripTotalSpendings = [];

        static int ShowMenu(string[] options)
        {
            bool firstLoop = true;
            while (true)
            {
                Console.WriteLine("{0}\n", title);
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine(
                        $"{(i != options.Length - 1 ? i + 1 : 0)} - {options[i]}"
                    );
                }

                if (!firstLoop)
                    Console.WriteLine("\nNije unesen pravilan odabir. Pokušajte ponovno.");
                else
                    firstLoop = false;

                Console.Write("\nOdabir: ");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    if (!(choice > options.Length - 1 || choice < 0))
                        return choice;
                }

                Console.Clear();
            }
        }

        static DateTime GetDate()
        {
            bool firstLoop = true;
            while (true)
            {
                if (!firstLoop)
                    Console.WriteLine("\nNije unesen pravilan datum. Pokušajte ponovno.");
                else
                    firstLoop = false;

                Console.Write("\nUnesite datum (YYYY-MM-DD): ");

                var inputted = Console.ReadLine();
                if (inputted is null)
                    continue;

                var split = inputted.Split("-");
                if (split.Length != 3)
                    continue;

                if (!(int.TryParse(split[0], out int year) && int.TryParse(split[1], out int month) && int.TryParse(split[2], out int day)))
                    continue;

                if (year > DateTime.Now.Year)
                    continue;

                try
                {
                    return new DateTime(year, month, day);
                }
                catch
                {
                    continue;
                }
            }
        }

        static void StoreNewUser(string newUserName, string newUserSurname, DateTime newUserDateOfBirth)
        {
            userLatestId++;
            userIds.Add(userLatestId);
            userNames.Add(userLatestId, newUserName);
            userSurnames.Add(userLatestId, newUserSurname);
            userDatesOfBirth.Add(userLatestId, newUserDateOfBirth);
            userTripIds.Add(userLatestId, []);
        }

        static void StoreNewTrip(int userId, DateTime newTripDate, decimal newTripDistance, decimal newTripOilUsedUp, decimal newTripOilPrice)
        {
            tripLatestId++;
            tripIds.Add(tripLatestId);
            userTripIds[userId].Add(tripLatestId);

            tripDates.Add(tripLatestId, newTripDate);
            tripDistances.Add(tripLatestId, newTripDistance);
            tripOilUsedUp.Add(tripLatestId, newTripOilUsedUp);
            tripOilPrices.Add(tripLatestId, newTripOilPrice);
            tripTotalSpendings.Add(tripLatestId, newTripOilUsedUp * newTripOilPrice);
        }

        static void GenerateRandomData()
        {
            var stockNames = new string[] { "Ivan", "Stipe", "Mate", "Jozo", "Šimun", "Luka", "Kate", "Andrijana", "Lucija", "Antonia", "Lukrecija", "Jelena" };
            var stockSurnames = new string[] { "Ivić", "Babić", "Šimić", "Žarković", "Slapničar", "Geić" };

            var rand = new Random();
            while(userLatestId < 3)
            {
                StoreNewUser(
                    stockNames[rand.Next(0, stockNames.Length)],
                    stockSurnames[rand.Next(0, stockSurnames.Length)],
                    new DateTime(rand.Next(1960, 2007), rand.Next(1, 13), rand.Next(1, 29), rand.Next(0, 24), rand.Next(0, 60), rand.Next(0, 60))
                );

                for (int x = 0; x < 5; x++)
                {
                    var randomTripDistance = (decimal)rand.NextDouble() * 900;
                    StoreNewTrip(
                        userLatestId,
                        new DateTime(rand.Next(userDatesOfBirth[userLatestId].Year + 19, 2007 + 19), rand.Next(1, 13), rand.Next(1, 29), rand.Next(0, 24), rand.Next(0, 60), rand.Next(0, 60)),
                        randomTripDistance,
                        randomTripDistance * rand.Next(7, 15) / 100,
                        (decimal)rand.NextDouble() + 1
                    );
                }
            }
        }

        static void Main()
        {
            GenerateRandomData();

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

            var menuTrip = new string[]
            {
                "Unos novog putovanja",
                "Brisanje putovanja",
                "Uređivanje postojećeg putovanja",
                "Pregled svih putovanja",
                "Izvještaji i analize",
                "Povratak na glavni izbornik"
            };

            var menu = ShowMenu(menuMain);
            Console.Clear();

            switch (menu)
            {
                case 1:
                    ShowMenu(menuUser);
                    break;
                case 2:
                    ShowMenu(menuTrip);
                    break;
                case 0:
                    return;
                default:
                    return;
            }
        }
    }
}
