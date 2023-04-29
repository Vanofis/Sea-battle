using System;
using System.Collections.Generic;
using System.IO;

namespace SeaBattle
{
    class Menu
    {
        private int gamemodeNumber = 0;
        private int profileNumber = 0;

        private GameHandler gameHandler = new GameHandler();

        private List<string> profileNames = new List<string>()
        {
            "Profile1",
            "Profile2",
            "Profile3",
        };

        private PlayerProfile currentProfile = null;

        public void LaunchMenu()
        {
            TryCreateProfiles();

            Console.WriteLine("Welcome to Sea battle!");
            Console.WriteLine("Choose your profile");
            Console.WriteLine("Available profiles:");

            WriteAvailableProfiles();

            GetProfileInput();

            ChooseProfile();

            Console.WriteLine("Choose gamemode:");
            Console.WriteLine("Write 1 - Player VS AI");
            Console.WriteLine("Write 2 - Player VS Player");
            Console.WriteLine("Write 3 - AI VS AI (Spectate battle)");

            GetGamemodeInput();

            HandleInput();
        }
        private void WriteAvailableProfiles()
        {
            for (int i = 0; i < profileNames.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {profileNames[i]}");
            }
        }
        private void TryCreateProfiles()
        {
            if(!Directory.Exists(XMLManager.pathToProfiles))
                Directory.CreateDirectory(XMLManager.pathToProfiles);

            List<(string, bool)> profilesBase = new List<(string, bool)>();

            foreach(var name in profileNames)
            {
                profilesBase.Add((name, false));
            }

            profilesBase.Add(("Ai1", true));
            profilesBase.Add(("Ai2", true));

            foreach (var profileBase in profilesBase) 
            {
                TryCreateProfile(profileBase.Item1, profileBase.Item2);
            }
        }
        private void TryCreateProfile(string name, bool isAi)
        {
            if (!File.Exists(XMLManager.pathToProfiles + @"\" + name + ".xml"))
                CreateProfileXML(name, isAi);
        }
        private void CreateProfileXML(string profileName, bool isAi)
        {
            if(!File.Exists(XMLManager.pathToProfiles + @"\" + profileName + ".xml"))
            {
                PlayerProfile profile = new PlayerProfile(profileName, isAi);

                XMLManager.SerializeXML(profile, profileName + ".xml");
            }
        }
        private void GetProfileInput()
        {
            bool isCorrectInput = false;

            do
            {
                profileNumber = 0;

                isCorrectInput = int.TryParse(Console.ReadLine(), out profileNumber);

                if (profileNumber > profileNames.Count || profileNumber <= 0)
                    isCorrectInput = false;

                if (!isCorrectInput)
                    Console.WriteLine("Wrong profile index");

            } while (!isCorrectInput);
        }
        private void ChooseProfile()
        {
            currentProfile = XMLManager.DeserializeXML(profileNames[profileNumber - 1] + ".xml");
        }
        private void GetGamemodeInput()
        {
            bool isCorrectInput = false;

            do
            {
                gamemodeNumber = 0;

                isCorrectInput = int.TryParse(Console.ReadLine(), out gamemodeNumber);

                if(gamemodeNumber > 3 || gamemodeNumber < 1) 
                    isCorrectInput = false;

                if (!isCorrectInput) 
                    Console.WriteLine("Wrong input");
            }
            while (!isCorrectInput);
        }
        private void HandleInput()
        {
            switch (gamemodeNumber) 
            {
                case 1:
                    PlayerProfile aiProfile = XMLManager.DeserializeXML("Ai2.xml");

                    gameHandler.LaunchGame(currentProfile, aiProfile);
                    break;
                case 2:
                    //gameHandler.LaunchGame(false, false, currentProfile);
                    Console.WriteLine("There is no multiplayer puk-puk :(");
                    break;
                case 3:
                    PlayerProfile aiProfile1 = XMLManager.DeserializeXML("Ai1.xml");
                    PlayerProfile aiProfile2 = XMLManager.DeserializeXML("Ai2.xml");

                    gameHandler.LaunchGame(aiProfile1, aiProfile2);
                    break;
            }
        }
    }
}
