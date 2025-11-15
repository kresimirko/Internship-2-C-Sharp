#pragma warning disable IDE0044
#pragma warning disable IDE0059

using System.Runtime.InteropServices;

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
        static Dictionary<int, decimal> tripFuelUsedUp = [];
        static Dictionary<int, decimal> tripFuelPrices = [];
        static Dictionary<int, decimal> tripTotalSpendings = [];

        static void Halt()
        {
            Console.Write("Pritisnite bilo koju tipku za povratak...");
            Console.ReadKey();
        }

        static int PromptMenu(string[] options, [Optional] string subtitle)
        {
            Console.Clear();
            bool firstLoop = true;
            while (true)
            {
                Console.WriteLine("{0}\n", title);
                if (subtitle is not null)
                    Console.WriteLine("{0}\n", subtitle);
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
                        if ($"{userNames[surname.Key]} {surname.Value}" == inputted)
                            return surname.Key;
                    }
                }
                else if (userIds.Contains(userId))
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

        static void StoreNewTrip(int userId, DateTime newTripDate, decimal newTripDistance, decimal newTripFuelUsedUp, decimal newTripFuelPrice)
        {
            tripLatestId++;
            tripIds.Add(tripLatestId);
            userTripIds[userId].Add(tripLatestId);

            tripDates.Add(tripLatestId, newTripDate);
            tripDistances.Add(tripLatestId, newTripDistance);
            tripFuelUsedUp.Add(tripLatestId, newTripFuelUsedUp);
            tripFuelPrices.Add(tripLatestId, newTripFuelPrice);
            tripTotalSpendings.Add(tripLatestId, newTripFuelUsedUp * newTripFuelPrice);
        }

        static void GenerateInitialRandomData()
        {
            var stockNames = new string[] { "Ivan", "Stipe", "Mate", "Jozo", "Šimun", "Luka", "Kate", "Andrijana", "Lucija", "Antonia", "Lukrecija", "Jelena" };
            var stockSurnames = new string[] { "Ivić", "Babić", "Šimić", "Žarković", "Slapničar", "Geić" };

            var rand = new Random();
            while (userLatestId < 3)
            {
                StoreNewUser(
                    stockNames[rand.Next(0, stockNames.Length)],
                    stockSurnames[rand.Next(0, stockSurnames.Length)],
                    new DateTime(rand.Next(1960, 2007), rand.Next(1, 13), rand.Next(1, 29), rand.Next(0, 24), rand.Next(0, 60), rand.Next(0, 60))
                );

                for (int x = 0; x < 5; x++)
                {
                    var randomTripDistance = Math.Round((decimal)rand.NextDouble() * 900, 2);
                    StoreNewTrip(
                        userLatestId,
                        new DateTime(rand.Next(userDatesOfBirth[userLatestId].Year + 19, 2007 + 19), rand.Next(1, 13), rand.Next(1, 29), rand.Next(0, 24), rand.Next(0, 60), rand.Next(0, 60)),
                        randomTripDistance,
                        Math.Round(randomTripDistance * rand.Next(7, 15) / 100, 2),
                        Math.Round((decimal)rand.NextDouble() + 1, 2)
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
            Console.Write("\nPutovanje uspješno dodano!\n\n");
            Halt();
            Console.Clear();
        }

        static void ShowSpecificTripData(int tripId, [Optional] bool addExtraNewline)
        {
            var user = "";
            foreach (var userTripIdsEntry in userTripIds)
            {
                if (userTripIdsEntry.Value.Contains(tripId))
                    user = $"{userNames[userTripIdsEntry.Key]} {userSurnames[userTripIdsEntry.Key]}";
            }
            Console.WriteLine("Putovanje #{0}", tripId);
            Console.WriteLine("Korisnik: {0}", user);
            Console.WriteLine("Datum: {0}", tripDates[tripId]);
            Console.WriteLine("Kilometri: {0}", tripDistances[tripId]);
            Console.WriteLine("Gorivo: {0} L", tripFuelUsedUp[tripId]);
            Console.WriteLine("Cijena po litri: {0} EUR", tripFuelPrices[tripId]);
            Console.WriteLine("Ukupno: {0} EUR", tripTotalSpendings[tripId]);
            if (addExtraNewline) Console.WriteLine();
        }

        static void ShowAllTripsInOrder()
        {
            Console.Clear();
            Console.Write("{0}\n\n>>> ...redom kako su spremljena\n\n", title);

            foreach (var tripId in tripIds)
                ShowSpecificTripData(tripId, true);

            Halt();
        }

        static void ShowTripsSorted(int choice)
        {
            Console.Clear();
            Console.Write("{0}\n\n>>>sortirana po ", title);

            switch (choice)
            {
                case 3:
                    Console.Write("trošku uzlazno\n\n");
                    foreach (var trip in (from trip in tripTotalSpendings orderby trip.Value ascending select trip))
                        ShowSpecificTripData(trip.Key, true);
                    break;
                case 4:
                    Console.Write("trošku silazno\n\n");
                    foreach (var trip in (from trip in tripTotalSpendings orderby trip.Value descending select trip))
                        ShowSpecificTripData(trip.Key, true);
                    break;
                case 5:
                    Console.Write("kilometraži uzlazno\n\n");
                    foreach (var trip in (from trip in tripDistances orderby trip.Value ascending select trip))
                        ShowSpecificTripData(trip.Key, true);
                    break;
                case 6:
                    Console.Write("kilometraži silazno\n\n");
                    foreach (var trip in (from trip in tripDistances orderby trip.Value descending select trip))
                        ShowSpecificTripData(trip.Key, true);
                    break;
                case 7:
                    Console.Write("datumu uzlazno\n\n");
                    foreach (var trip in (from trip in tripDates orderby trip.Value ascending select trip))
                        ShowSpecificTripData(trip.Key, true);
                    break;
                case 8:
                    Console.Write("datumu silazno\n\n");
                    foreach (var trip in (from trip in tripDates orderby trip.Value descending select trip))
                        ShowSpecificTripData(trip.Key, true);
                    break;
            }

            Halt();
        }

        static void ShowAllTripsGroupedByUsers()
        {
            Console.Clear();
            Console.Write("{0}\n\n>>> ...grupirana po korisnicima\n\n", title);

            foreach (var userId in userIds)
            {
                Console.WriteLine("-- Korisnik: {0} {1} --\n", userNames[userId], userSurnames[userId]);
                foreach (var tripId in userTripIds[userId])
                    ShowSpecificTripData(tripId, true);
            }

            Halt();
        }

        static void ShowMenuShowTrips()
        {
            var choice = PromptMenu([
                "...redom kako su spremljena",
                "...grupirana po korisnicima",
                "...sortirana po trošku uzlazno",
                "...sortirana po trošku silazno",
                "...sortirana po kilometraži uzlazno",
                "...sortirana po kilometraži silazno",
                "...sortirana po datumu uzlazno",
                "...sortirana po datumu silazno",
                "Povratak na glavni izbornik"
            ], ">>> Pregled svih putovanja");

            switch (choice)
            {
                case 1:
                    ShowAllTripsInOrder();
                    break;
                case 2:
                    ShowAllTripsGroupedByUsers();
                    break;
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    ShowTripsSorted(choice);
                    break;
            }
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
                    ShowMenuShowTrips();
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
                case 0:
                    Environment.Exit(0);
                    break;
            }
        }

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.GetEncoding(1200);
            Console.InputEncoding = System.Text.Encoding.GetEncoding(1200);

            GenerateInitialRandomData();

            while (true)
                ShowMenuMain();
        }
    }
}
