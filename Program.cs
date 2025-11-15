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
        static List<string> existingNameSurnameCombos = [];
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

            Console.WriteLine("{0}\n", title);
            if (subtitle is not null)
                Console.WriteLine("{0}\n", subtitle);

            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine(
                    $"{(i != options.Length - 1 ? i + 1 : 0)} - {options[i]}"
                );
            }

            Console.WriteLine();
            return OneLinePromptIntInRange("Odabir: ", -1, options.Length);
        }

        static void BringCursorBackToPrompt(int promptLength, int userInputLength)
        {
            for (int i = 0; i < (promptLength + userInputLength) / Console.BufferWidth + 1; i++)
                Console.CursorTop--;
            Console.CursorLeft = promptLength;
            var savedPos = Console.GetCursorPosition();
            Console.Write(new string(' ', userInputLength));
            Console.SetCursorPosition(savedPos.Left, savedPos.Top);

            var invalidInputWarning = "Nevažeći unos!";
            Console.Write("\a\x1b[31mNevažeći unos!\x1b[0m");
            Thread.Sleep(1500);
            Console.Write(new string('\b', invalidInputWarning.Length));
            Console.Write(new string(' ', invalidInputWarning.Length));
            Console.Write(new string('\b', invalidInputWarning.Length));
        }

        static DateTime OneLinePromptDate([Optional] string customPrompt)
        {
            var prompt = customPrompt is not null ? customPrompt : "Unesite datum (YYYY-MM-DD): ";
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
                inputted = inputted.Trim();
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
                inputted = inputted.Trim();

                return inputted;
            }
        }

        static int OneLinePromptIntInRange(string prompt, int lower, int higher)
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
                inputted = inputted.Trim();
                lastEnteredLength = inputted.Length;

                if (!(int.TryParse(inputted, out int parsed)))
                    continue;
                else if (parsed > lower && parsed < higher)
                    return parsed;
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
                inputted = inputted.Trim();
                lastEnteredLength = inputted.Length;

                if (!(decimal.TryParse(inputted, out decimal parsed)))
                    continue;

                return parsed;
            }
        }

        static int OneLinePromptTrip(string prompt)
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
                inputted = inputted.Trim();
                lastEnteredLength = inputted.Length;

                if (!(int.TryParse(inputted, out int parsed)))
                    continue;
                else if (tripIds.Contains(parsed))
                    return parsed;
            }
        }

        static int OneLinePromptUser([Optional] bool onlyById)
        {
            var prompt = $"Odaberite korisnika ({(onlyById ? "" : "ime i prezime ili ")}ID): ";
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
                inputted = inputted.Trim().ToLower();
                lastEnteredLength = inputted.Length;

                if (!(int.TryParse(inputted, out int userId)))
                {
                    if (!onlyById)
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
                    else
                        continue;
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

        static void DeleteTripData(int tripId)
        {
            foreach (var pair in userTripIds)
                pair.Value.Remove(tripId);

            tripIds.Remove(tripId);
            tripDates.Remove(tripId);
            tripDistances.Remove(tripId);
            tripFuelUsedUp.Remove(tripId);
            tripFuelPrices.Remove(tripId);
            tripTotalSpendings.Remove(tripId);
        }

        static void DeleteUserData(int userId)
        {
            userIds.Remove(userId);
            userNames.Remove(userId);
            userSurnames.Remove(userId);
            userDatesOfBirth.Remove(userId);
            userTripIds.Remove(userId);

            if (userTripIds.TryGetValue(userId, out List<int>? value))
                foreach (var tripId in value)
                    DeleteTripData(tripId);
            userTripIds.Remove(userId);
        }

        static void GenerateInitialRandomData()
        {
            var stockNames = new string[] { "Ivan", "Stipe", "Mate", "Jozo", "Šimun", "Luka", "Kate", "Andrijana", "Lucija", "Antonia", "Lukrecija", "Jelena" };
            var stockSurnames = new string[] { "Ivić", "Babić", "Šimić", "Žarković", "Slapničar", "Geić" };

            var rand = new Random();
            while (userLatestId < 3)
            {
                var pickedName = stockNames[rand.Next(0, stockNames.Length)];
                var pickedSurname = stockSurnames[rand.Next(0, stockSurnames.Length)];
                var newCombo = $"{pickedName} {pickedSurname}";

                if (existingNameSurnameCombos.Contains(newCombo))
                    continue;
                else
                    existingNameSurnameCombos.Add(newCombo);

                StoreNewUser(
                    pickedName,
                    pickedSurname,
                    new DateTime(rand.Next(1960, 2007), rand.Next(1, 13), rand.Next(1, 29))
                );

                for (int x = 0; x < 5; x++)
                {
                    var randomTripDistance = Math.Round((decimal)rand.NextDouble() * 900, 2);
                    StoreNewTrip(
                        userLatestId,
                        new DateTime(rand.Next(userDatesOfBirth[userLatestId].Year + 19, 2007 + 19), rand.Next(1, 13), rand.Next(1, 29)),
                        randomTripDistance,
                        Math.Round(randomTripDistance * rand.Next(7, 15) / 100, 2),
                        Math.Round((decimal)rand.NextDouble() + 1, 2)
                    );
                }
            }
        }

        static void PromptTripNew()
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.Write("\n>>> Unos novog putovanja\n\nUnesite informacije.\n\n");
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

        static void PromptTripEdit(int tripId)
        {
            var choice = PromptMenu([
                "Datum",
                "Kilometražu",
                "Potrošeno gorivo",
                "Cijenu goriva",
                "Nazad na glavni izbornik"
            ], $">>> Uređivanje postojećeg putovanja\n\nOdaberite podatak koji želite izmjeniti za putovanje {tripId}.");

            Console.WriteLine();

            decimal promptAnswer = 0;
            var promptChoices = new string[] {
                "Da",
                "Ne (nazad na glavni izbornik)"
            };
            var promptSubtitle = $">>> Uređivanje postojećeg putovanja\n\nJeste li sigurni da želite urediti putovanje {tripId}?";

            switch (choice)
            {
                case 1:
                    var promptAnswerDate = OneLinePromptDate();
                    if (PromptMenu(promptChoices, promptSubtitle) == 1)
                        tripDates[tripId] = promptAnswerDate;
                    else
                        return;
                    break;
                case 2:
                    promptAnswer = OneLinePromptDecimal("Unesite kilometražu: ");
                    if (PromptMenu(promptChoices, promptSubtitle) == 1)
                        tripDistances[tripId] = promptAnswer;
                    else
                        return;
                    break;
                case 3:
                    promptAnswer = OneLinePromptDecimal("Unesite potrošeno gorivo (L): ");
                    if (PromptMenu(promptChoices, promptSubtitle) == 1)
                        tripFuelUsedUp[tripId] = promptAnswer;
                    else
                        return;
                    break;
                case 4:
                    promptAnswer = OneLinePromptDecimal("Unesite cijenu po litri: ");
                    if (PromptMenu(promptChoices, promptSubtitle) == 1)
                        tripFuelPrices[tripId] = promptAnswer;
                    else
                        return;
                    break;
                case 0:
                    return;
            }

            tripTotalSpendings[tripId] = tripFuelUsedUp[tripId] * tripFuelPrices[tripId];

            Console.Write("\nPutovanje uspješno uređeno!\n\n");
            Halt();
            Console.Clear();
        }

        static void PromptTripDeleteSpecific(int tripId)
        {
            var choice = PromptMenu([
                "Da (TRAJNO!)",
                "Ne (nazad na glavni izbornik)"
            ], $">>> Brisanje putovanja\n\nJeste li sigurni da želite izbrisati putovanje {tripId}?");

            if (choice == 1)
            {
                DeleteTripData(tripId);

                Console.Write("\nUspješno izbrisano putovanje {0}!\n\n", tripId);
                Halt();
            }
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
            Console.WriteLine("Datum: {0}", DateOnly.FromDateTime(tripDates[tripId]));
            Console.WriteLine("Kilometri: {0}", tripDistances[tripId]);
            Console.WriteLine("Gorivo: {0} L", tripFuelUsedUp[tripId]);
            Console.WriteLine("Cijena po litri: {0} EUR", tripFuelPrices[tripId]);
            Console.WriteLine("Ukupno: {0} EUR", tripTotalSpendings[tripId]);
            if (addExtraNewline) Console.WriteLine();
        }

        static void ShowSpecificUserData(int userId, [Optional] bool addExtraNewline)
        {
            Console.Write(userId + " - ");
            Console.Write(userNames[userId] + " - ");
            Console.Write(userSurnames[userId] + " - ");
            Console.Write(DateOnly.FromDateTime(userDatesOfBirth[userId]) + (addExtraNewline ? "\n" : ""));
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
                    foreach (var trip in from trip in tripTotalSpendings orderby trip.Value ascending select trip)
                        ShowSpecificTripData(trip.Key, true);
                    break;
                case 4:
                    Console.Write("trošku silazno\n\n");
                    foreach (var trip in from trip in tripTotalSpendings orderby trip.Value descending select trip)
                        ShowSpecificTripData(trip.Key, true);
                    break;
                case 5:
                    Console.Write("kilometraži uzlazno\n\n");
                    foreach (var trip in from trip in tripDistances orderby trip.Value ascending select trip)
                        ShowSpecificTripData(trip.Key, true);
                    break;
                case 6:
                    Console.Write("kilometraži silazno\n\n");
                    foreach (var trip in from trip in tripDistances orderby trip.Value descending select trip)
                        ShowSpecificTripData(trip.Key, true);
                    break;
                case 7:
                    Console.Write("datumu uzlazno\n\n");
                    foreach (var trip in from trip in tripDates orderby trip.Value ascending select trip)
                        ShowSpecificTripData(trip.Key, true);
                    break;
                case 8:
                    Console.Write("datumu silazno\n\n");
                    foreach (var trip in from trip in tripDates orderby trip.Value descending select trip)
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

        static void PromptUserNew()
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.Write("\n>>> Unos novog korisnika\n\nUnesite informacije.\n\n");

            var pickedName = OneLinePromptString("Ime (bez prezimena): ");
            var pickedSurname = OneLinePromptString("Prezime: ");
            var newCombo = $"{pickedName} {pickedSurname}";

            if (existingNameSurnameCombos.Contains(newCombo))
            {
                Console.Write("\nGreška: Korisnik s tim imenom i prezimenom već postoji.\n\n");
                Halt();
                Console.Clear();
                return;
            }

            StoreNewUser(
                pickedName,
                pickedSurname,
                OneLinePromptDate("Datum rođenja (YYYY-MM-DD): ")
            );
            Console.Write("\nKorisnik uspješno dodan!\n\n");
            Halt();
            Console.Clear();
        }

        static void PromptUserDelete(int userId)
        {
            var choice = PromptMenu([
                "Da (TRAJNO!)",
                "Ne (nazad na glavni izbornik)"
            ], $">>> Brisanje korisnika\n\nJeste li sigurni da želite izbrisati korisnika {userNames[userId]} {userSurnames[userId]} ({userId})?");

            if (choice == 1)
            {
                DeleteUserData(userId);

                Console.Write("\nUspješno izbrisan korisnik {0}!\n\n", userId);
                Halt();
            }
        }

        static void PromptUserEdit(int userId)
        {
            var userFormatted = $"{userNames[userId]} {userSurnames[userId]} ({userId})";

            var choice = PromptMenu([
                "Ime",
                "Prezime",
                "Datum rođenja",
                "Nazad na glavni izbornik"
            ], $">>> Uređivanje postojećeg korisnika\n\nOdaberite podatak koji želite izmjeniti za korisnika {userFormatted}.");

            Console.WriteLine();

            var promptAnswer = "";
            var promptChoices = new string[] {
                "Da",
                "Ne (nazad na glavni izbornik)"
            };
            var promptSubtitle = $">>> Uređivanje korisnika\n\nJeste li sigurni da želite urediti korisnika {userFormatted}?";

            switch (choice)
            {
                case 1:
                    promptAnswer = OneLinePromptString("Unesite ime (bez prezimena): ");
                    if (PromptMenu(promptChoices, promptSubtitle) == 1)
                        userNames[userId] = promptAnswer;
                    else
                        return;
                    break;
                case 2:
                    promptAnswer = OneLinePromptString("Unesite prezime: ");
                    if (PromptMenu(promptChoices, promptSubtitle) == 1)
                        userSurnames[userId] = promptAnswer;
                    else
                        return;
                    break;
                case 3:
                    var promptAnswerDate = OneLinePromptDate("Unesite datum rođenja (YYYY-MM-DD): ");
                    if (PromptMenu(promptChoices, promptSubtitle) == 1)
                        userDatesOfBirth[userId] = promptAnswerDate;
                    else
                        return;
                    break;
                case 0:
                    return;
            }

            Console.Write("\nKorisnik uspješno uređen!\n\n");
            Halt();
            Console.Clear();
        }

        static void ShowUsersSorted(int sort)
        {
            Console.Clear();
            Console.Write("{0}\n\n>>> Pregled svih korisnika", title);

            switch (sort)
            {
                case 1:
                    Console.Write("\n\n");
                    Console.WriteLine("ID - Ime - Prezime - Datum rođenja");
                    Console.WriteLine(new string('-', 50));
                    foreach (var userId in userIds)
                        ShowSpecificUserData(userId, true);
                    break;
                case 2:
                    Console.Write(" koji imaju više od 20 godina\n\n");
                    Console.WriteLine("ID - Ime - Prezime - Datum rođenja");
                    Console.WriteLine(new string('-', 50));
                    foreach (var query in from userDOBPair in userDatesOfBirth where (DateTime.Now.Year - userDOBPair.Value.Year > 20) select userDOBPair.Key)
                        ShowSpecificUserData(query, true);
                    break;
                case 3:
                    Console.Write(" koji imaju barem 2 putovanja\n\n");
                    Console.WriteLine("ID - Ime - Prezime - Datum rođenja");
                    Console.WriteLine(new string('-', 50));
                    foreach (var query in from userTripIdPair in userTripIds where userTripIds[userTripIdPair.Key].Count >= 2 select userTripIdPair.Key)
                        ShowSpecificUserData(query, true);
                    break;
            }

            Console.WriteLine();
            Halt();
        }

        static void ShowMenuShowUsers()
        {
            var choice = PromptMenu([
                "...redom kako su spremljena",
                "...svih onih koji imaju više od 20 godina",
                "...svih onih koji imaju barem 2 putovanja",
                "Povratak na glavni izbornik"
            ], ">>> Pregled svih korisnika");

            ShowUsersSorted(choice);
        }

        static void PromptTripDelete()
        {
            var choice = PromptMenu([
                "...po ID-u",
                "...svih putovanja skupljih od unesenog iznosa",
                "...svih putovanja jeftinijih od unesenog iznosa",
                "Povratak na glavni izbornik"
            ], ">>> Brisanje putovanja");

            var promptChoices = new string[] {
                "Da",
                "Ne (nazad na glavni izbornik)"
            };
            var promptSubtitle = $">>> Brisanje putovanja\n\nJeste li sigurni da želite izbrisati putovanja?";
            decimal amount = 0;

            switch (choice)
            {
                case 1:
                    Console.WriteLine();
                    PromptTripDeleteSpecific(OneLinePromptTrip("Upišite ID putovanja: "));
                    break;
                case 2:
                    Console.WriteLine();
                    amount = OneLinePromptDecimal("Upišite iznos (EUR): ");
                    if (PromptMenu(promptChoices, promptSubtitle) == 1)
                        foreach (var query in from tripCostPair in tripTotalSpendings where tripTotalSpendings[tripCostPair.Key] > amount select tripCostPair.Key)
                            DeleteTripData(query);
                    Console.Write("\nPutovanja uspješno izbrisana!\n\n");
                    Halt();
                    break;
                case 3:
                    Console.WriteLine();
                    amount = OneLinePromptDecimal("Upišite iznos (EUR): ");
                    if (PromptMenu(promptChoices, promptSubtitle) == 1)
                        foreach (var query in from tripCostPair in tripTotalSpendings where tripTotalSpendings[tripCostPair.Key] < amount select tripCostPair.Key)
                            DeleteTripData(query);
                    Console.Write("\nPutovanja uspješno izbrisana!\n\n");
                    Halt();
                    break;
            }
        }

        static void ShowReport(int userId)
        {
            Console.Clear();
            Console.Write("{0}\n\n>>> Izvještaji i analize\n\nKorisnik {1} {2} ({3})\n\n", title, userNames[userId], userSurnames[userId], userId);

            decimal totalFuelUsed = 0;
            decimal totalCost = 0;
            decimal totalKm = 0;
            foreach (var tripId in userTripIds[userId])
            {
                totalFuelUsed += tripFuelUsedUp[tripId];
                totalCost += tripTotalSpendings[tripId];
                totalKm += tripDistances[tripId];
            }
            decimal totalUsageInLPerKm = (totalFuelUsed / totalKm) * 100;

            Console.WriteLine("Ukupna potrošnja goriva: {0}", totalFuelUsed);
            Console.WriteLine("Ukupni troškovi goriva: {0}", totalCost);
            Console.WriteLine("Prosječna potrošnja goriva u L/100km: {0}", totalUsageInLPerKm);

            Console.WriteLine("Putovanje s najvećom potrošnjom goriva: {0}", (from tripId in userTripIds[userId] orderby tripFuelUsedUp[tripId] descending select tripId).First());

            Console.Write("\nSada odaberite ako želite pregled putovanja po određenom datumu. (1 za da, 0 za ne)\n\n");
            var choice = OneLinePromptIntInRange("Odabir: ", -1, 2);
            if (choice == 1)
            {
                Console.WriteLine();
                var chosenDate = OneLinePromptDate();
                Console.WriteLine();
                var foundSomething = false;
                foreach (var tripId in userTripIds[userId])
                {
                    if (tripDates[tripId] == chosenDate)
                    {
                        ShowSpecificTripData(tripId, true);
                        foundSomething = true;
                        break;
                    }
                }
                if (!foundSomething) Console.Write("Nije pronađeno niti jedno takvo putovanje.\n\n");
            }
            else
                Console.WriteLine();

            Halt();
        }

        static void ShowMenuTrip()
        {
            var choice = PromptMenu([
                "Unos novog putovanja",
                "Brisanje putovanja",
                "Uređivanje postojećeg putovanja",
                "Pregled svih putovanja",
                "Pregled specifičnog putovanja",
                "Izvještaji i analize",
                "Povratak na glavni izbornik"
            ]);

            switch (choice)
            {
                case 1:
                    PromptTripNew();
                    break;
                case 2:
                    Console.WriteLine();
                    PromptTripDelete();
                    break;
                case 3:
                    Console.WriteLine();
                    PromptTripEdit(OneLinePromptTrip("Upišite ID putovanja: "));
                    break;
                case 4:
                    ShowMenuShowTrips();
                    break;
                case 5:
                    Console.WriteLine();
                    var id = OneLinePromptTrip("Upišite ID putovanja: ");
                    Console.WriteLine();
                    ShowSpecificTripData(id, true);
                    Halt();
                    break;
                case 6:
                    Console.WriteLine();
                    ShowReport(OneLinePromptUser());
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
                    PromptUserNew();
                    break;
                case 2:
                    Console.WriteLine();
                    PromptUserDelete(OneLinePromptUser());
                    break;
                case 3:
                    Console.WriteLine();
                    PromptUserEdit(OneLinePromptUser(true));
                    break;
                case 4:
                    ShowMenuShowUsers();
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
