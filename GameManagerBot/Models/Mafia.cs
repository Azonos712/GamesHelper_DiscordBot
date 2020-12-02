using System;
using System.Collections.Generic;
using System.Linq;

namespace GameManagerBot.Models
{
    class Mafia : IGame
    {
        int PlayersCount { get; set; }
        List<string> _roles = new List<string>();
        readonly string _specialRoles;

        public Mafia(string r, int count)
        {
            _specialRoles = r;
            PlayersCount = count-1;
        }

        public List<string> Start()
        {
            InitRoles();

            if (_specialRoles != "")
                InitAdditionalRoles();

            _roles = _roles.OrderBy(a => Guid.NewGuid()).ToList();

            return _roles;
        }

        void InitRoles()
        {
            _roles.AddRange(Enumerable.Repeat("Мафия :levitate:", PlayersCount / 3));
            _roles.AddRange(Enumerable.Repeat("Мирный :person_standing:", PlayersCount - (PlayersCount / 3)));
        }

        void InitAdditionalRoles()
        {
            ReplaceRole("d", "Мафия :levitate:", "Дон Мафии :man_detective:");
            ReplaceRole("k", "Мирный :person_standing:", "Комиссар :police_officer:");
            ReplaceRole("v", "Мирный :person_standing:", "Врач :man_health_worker:");
            ReplaceRole("m", "Мирный :person_standing:", "Маньяк :knife:");
        }

        void ReplaceRole(string _contains, string _old, string _new)
        {
            if (_specialRoles.Contains(_contains))
                _roles[_roles.FindIndex(index => index.Equals(_old))] = _new;
        }
    }
}
