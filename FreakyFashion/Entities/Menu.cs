using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HemTentan.Entities
{
    public class Menu
    {
        public int Id { get; protected set; }
        public string Name { get; protected set; }
        
        //Menu behöver en lista med Menuitems
        public List<MenuItem> Items { get; set; } = new List <MenuItem>();

        public Menu(int id,string name, List<MenuItem> items)
        {
            Id = id;
            Name = name;
            Items = items;
        }
        public Menu(string name)            
        {          
            Name = name;         
        }
    }
}
