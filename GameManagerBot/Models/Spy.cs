using GameManagerBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameManagerBot.Models
{
    class Spy : IGame
    {
        public int PlayersCount { get; set; }
        List<string> _roles = new List<string>();
        string _place;
        string[] _place_arr;
        Random r = new Random();

        public Spy(int count)
        {
            PlayersCount = count;
        }

        public List<string> Start()
        {
            _place_arr = FileService.ReadData();
            _place = _place_arr[r.Next(0, _place_arr.Length)];
            InitRoles();
            _roles = _roles.OrderBy(a => Guid.NewGuid()).ToList();

            return _roles;
        }
        void InitRoles()
        {
            _roles.AddRange(Enumerable.Repeat("Вы шпион", 1));
            _roles.AddRange(Enumerable.Repeat($"Локация - {_place}", PlayersCount - 1));
        }
    }
}
