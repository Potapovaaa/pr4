using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pr4
{
    internal class Zametka
    {
        public string Name { get; set; }
        public string Tip { get; set; }
        public string Date; 
        public int Money { get; set; }
        public bool Action { get; set; }

        public Zametka(string name, string tip, string date, int money, bool action) 
        {
            Name = name;
            Tip = tip;
            Date = date;
            Money = Math.Abs(money);
            Action = action;
        }
    }
}
