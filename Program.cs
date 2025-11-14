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

        static int PromptMenu(string[] options)
        {
            Console.Clear();
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
                    Console.WriteLine("\nNije unesen pravilan odabir. Pokušajte ponovno.\a");
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

        static void BringCursorBackToPrompt(int promptLength, int userInputLength)
        {
            for (int i = 0; i < (promptLength + userInputLength) / Console.BufferWidth + 1; i++)
                Console.CursorTop--;
            Console.CursorLeft = promptLength;
            var savedPos = Console.GetCursorPosition();
            Console.Write(new string(' ', userInputLength));
            Console.SetCursorPosition(savedPos.Left, savedPos.Top);
            Console.Write('\a');
        }

        static DateTime OneLinePromptDate()
        {
            var prompt = "Unesite datum (YYYY-MM-DD): ";
            Console.Write(prompt);
            bool firstLoop = true;
            int lastEnteredLength = 0;
            while (true)
            {
                if (!firstLoop)
                    BringCursorBackToPrompt(prompt.Length, lastEnteredLength);
                else
                    firstLoop = false;

                var inputted = Console.ReadLine();
                if (inputted is null)
                    continue;
                lastEnteredLength = inputted.Length;

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

        static string OneLinePromptString(string prompt)
        {
            Console.Write(prompt);
            bool firstLoop = true;
            while (true)
            {
                if (!firstLoop)
                    BringCursorBackToPrompt(prompt.Length, 0);
                else
                    firstLoop = false;

                var inputted = Console.ReadLine();
                if (inputted is null)
                    continue;

                return inputted;
            }
        }

        static decimal OneLinePromptDecimal(string prompt)
        {
            Console.Write(prompt);
            bool firstLoop = true;
            int lastEnteredLength = 0;
            while (true)
            {
                if (!firstLoop)
                    BringCursorBackToPrompt(prompt.Length, lastEnteredLength);
                else
                    firstLoop = false;

                var inputted = Console.ReadLine();
                if (inputted is null)
                    continue;
                lastEnteredLength = inputted.Length;

                if (!(decimal.TryParse(inputted, out decimal parsed)))
                    continue;

                return parsed;
            }
        }

        static int OneLinePromptUser()
        {
            var prompt = "Odaberite korisnika (ime i prezime ili ID): ";
            Console.Write(prompt);
            bool firstLoop = true;
            int lastEnteredLength = 0;
            while (true)
            {
                if (!firstLoop)
                    BringCursorBackToPrompt(prompt.Length, lastEnteredLength);
                else
                    firstLoop = false;

                var inputted = Console.ReadLine();
                if (inputted is null)
                    continue;
                lastEnteredLength = inputted.Length;

                if (!(int.TryParse(inputted, out int userId)))
                {
                    var inputtedUserNameAndSurname = inputted.Split(' ');
                    if (inputtedUserNameAndSurname.Length != 2)
                        continue;
                    foreach (var surname in userSurnames)
                    {
                        if (surname.Value + userNames[surname.Key] == inputted)
                            return surname.Key;
                    }
                }
                else
                    return userId;
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

        static void GenerateInitialRandomData()
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

        static void PromptTrip()
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.Write("\n>>> Unos novog putovanja\n\nUnesite informacije.\nAko ikoja bude nevažeća, pokazivač će se vratiti na početak upita i konzola će zazvoniti.\n\n");
            StoreNewTrip(
                OneLinePromptUser(),
                OneLinePromptDate(),
                OneLinePromptDecimal("Unesite kilometražu: "),
                OneLinePromptDecimal("Unesite potrošeno gorivo (L): "),
                OneLinePromptDecimal("Unesite cijenu po litri: ")
            );
            Console.WriteLine("\nPutovanje uspješno dodano!");
            Thread.Sleep(2000);
            Console.Clear();
        }

        static void ShowAllTrips()
        {
            Console.Clear();
            Console.Write("{0}\n\n>>> Pregled svih putovanja\n\n", title);

            foreach (var userId in userIds)
            {
                Console.WriteLine("-- Korisnik: {0} {1} --\n", userNames[userId], userSurnames[userId]);
                foreach (var tripId in userTripIds[userId])
                {
                    Console.WriteLine("Putovanje #{0}", tripId);
                    Console.WriteLine("Datum: {0}", tripDates[tripId]);
                    Console.WriteLine("Kilometri: {0}", tripDistances[tripId]);
                    Console.WriteLine("Gorivo: {0} L", tripOilUsedUp[tripId]);
                    Console.WriteLine("Cijena po litri: {0} EUR", tripOilPrices[tripId]);
                    Console.WriteLine("Ukupno: {0} EUR", tripTotalSpendings[tripId]);
                    Console.WriteLine();
                }
            }

            Console.Write("Pritisnite bilo koju tipku za povratak...");
            Console.ReadKey();
        }

        static void ShowMenuTrip()
        {
            var choice = PromptMenu([
                "Unos novog putovanja",
                "Brisanje putovanja",
                "Uređivanje postojećeg putovanja",
                "Pregled svih putovanja",
                "Izvještaji i analize",
                "Povratak na glavni izbornik"
            ]);

            switch (choice)
            {
                case 1:
                    PromptTrip();
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    ShowAllTrips();
                    break;
                case 5:
                    break;
            }
        }
        static void ShowMenuUser()
        {
            var choice = PromptMenu([
                "Unos novog korisnika",
                "Brisanje korisnika",
                "Uređivanje korisnika",
                "Pregled svih korisnika",
                "Povratak na glavni izbornik"
            ]);

            switch (choice)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
            }
        }
        static void ShowMenuMain()
        {
            var choice = PromptMenu([
                "Korisnici",
                "Putovanja",
                "Izlaz iz aplikacije"
            ]);

            switch (choice)
            {
                case 1:
                    ShowMenuUser();
                    break;
                case 2:
                    ShowMenuTrip();
                    break;
            }
        }

        static void Main()
        {
            GenerateInitialRandomData();

            while (true)
                ShowMenuMain();
        }
    }
}
