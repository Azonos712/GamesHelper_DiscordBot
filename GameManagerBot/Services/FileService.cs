using Newtonsoft.Json;
using System;
using System.IO;

namespace GameManagerBot.Services
{
    class FileService
    {
        static readonly string LIST_OF_USERS_PATH = $"{Environment.CurrentDirectory}\\ListOfUsers.txt";
        static readonly string LIST_OF_PLACES_PATH = $"{Environment.CurrentDirectory}\\ListOfPlaces.txt";
        static readonly string TOKEN_PATH = $"D:\\JustProjects\\DiscordBots\\token.txt";

        static public void SaveData(string output)
        {
            using (StreamWriter writer = File.CreateText(LIST_OF_USERS_PATH))
            {
                writer.Write(output);
            }
        }
        static public string ReadToken()
        {
            return File.ReadAllText(TOKEN_PATH);
        }
        static public string[] ReadData()
        {
            return File.ReadAllLines(LIST_OF_PLACES_PATH);
        } 
    }
}
