using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MaratonaApp.Models
{
    public class Cart
    {
        private string id;
        private string name;
        private string id_user;
        private DateTime plannedFor;
        private bool completed;

        [JsonProperty("id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [JsonProperty("user")]
        public string Id_User
        {
            get { return id_user; }
            set { id_user = value; }
        }

        [JsonProperty("plannedFor")]
        public DateTime PlannedFor
        {
            get { return plannedFor; }
            set { plannedFor = value; }
        }

        [JsonProperty("complete")]
        public bool Completed
        {
            get { return completed; }
            set { completed = value; }
        }

    }
}
