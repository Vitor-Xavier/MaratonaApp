using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;

namespace TesteAppBackendService.DataObjects
{
    public class Cart : EntityData
    {
        public string Name { get; set; }

        public DateTime PlannedFor { get; set; }

        public bool Complete { get; set; }

        public string User { get; set; }

        public virtual ICollection<Item> Items { get; set; }

        public Cart()
        {
            Items = new List<Item>();
        }
    }
}