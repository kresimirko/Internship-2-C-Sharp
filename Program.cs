#pragma warning disable IDE0044
#pragma warning disable IDE0059

namespace Internship_2_C_Sharp
{
    internal class Program
    {
        static int userLatestId = 0;
        static int travelLatestId = 0;
        static List<int> userIds = [];
        static Dictionary<int, string> userNames = [];
        static Dictionary<int, string> userSurnames = [];
        static Dictionary<int, DateTime> userDatesOfBirth = [];
        static Dictionary<int, List<int>> userTravelIds = [];
        static List<int> travelIds = [];
        static Dictionary<int, DateTime> travelDates = [];
        static Dictionary<int, decimal> travelDistances = [];
        static Dictionary<int, decimal> travelOilUsedUp = [];
        static Dictionary<int, decimal> travelOilPrices = [];
        static Dictionary<int, decimal> travelTotalSpendings = [];

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

        static void StoreNewUser(string newUserName, string newUserSurname, DateTime newUserDateOfBirth)
        {
            userLatestId++;
            userIds.Add(userLatestId);
            userNames.Add(userLatestId, newUserName);
            userSurnames.Add(userLatestId, newUserSurname);
            userDatesOfBirth.Add(userLatestId, newUserDateOfBirth);
            userTravelIds.Add(userLatestId, []);
        }

        static void StoreNewTravel(int userId, DateTime newTravelDate, decimal newTravelDistance, decimal newTravelOilUsedUp, decimal newTravelOilPrice)
        {
            travelLatestId++;
            travelIds.Add(travelLatestId);
            userTravelIds[userId].Add(travelLatestId);

            travelDates.Add(travelLatestId, newTravelDate);
            travelDistances.Add(travelLatestId, newTravelDistance);
            travelOilUsedUp.Add(travelLatestId, newTravelOilUsedUp);
            travelOilPrices.Add(travelLatestId, newTravelOilPrice);
            travelTotalSpendings.Add(travelLatestId, newTravelOilUsedUp * newTravelOilPrice);
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
                    var randomTravelDistance = (decimal)rand.NextDouble() * 900;
                    StoreNewTravel(
                        userLatestId,
                        new DateTime(rand.Next(userDatesOfBirth[userLatestId].Year + 19, 2007 + 19), rand.Next(1, 13), rand.Next(1, 29), rand.Next(0, 24), rand.Next(0, 60), rand.Next(0, 60)),
                        randomTravelDistance,
                        randomTravelDistance * rand.Next(7, 15) / 100,
                        (decimal)rand.NextDouble() + 1
                    );
                }
            }
        }

        static void Main()
        {
            GenerateRandomData();

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
