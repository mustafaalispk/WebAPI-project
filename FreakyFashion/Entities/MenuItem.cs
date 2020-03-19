using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HemTentan.Entities
{
    public class MenuItem
    {
        public int Id { get; protected set; }
        public string Name { get; protected set; }
        public Uri Link { get; protected set; }
        public int OrderBy { get; protected set; }
        public MenuItem(string name, Uri link, int orderBy)
        {
            Name = name;
            Link = link;
            OrderBy = orderBy;
        }
    }
}
