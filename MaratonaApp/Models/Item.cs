using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaratonaApp.Models
{
    public class Item : BaseModel
    {
        private string id;
        private string name;
        private string id_cart;
        private bool done;

        [JsonProperty("id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty("text")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [JsonProperty("cartId")]
        public string Id_Cart
        {
            get { return id_cart; }
            set { id_cart = value; }
        }

        [JsonProperty("complete")]
        public bool Done
        {
            get { return done; }
            set { SetProperty(ref done, value); }
        }

    }
}
