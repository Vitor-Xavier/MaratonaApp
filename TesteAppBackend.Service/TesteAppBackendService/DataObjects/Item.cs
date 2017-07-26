using Microsoft.Azure.Mobile.Server;

namespace TesteAppBackendService.DataObjects
{
    public class Item : EntityData
    {
        public Item() { }

        public string Text { get; set; }

        public bool Complete { get; set; }

        public string User { get; set; }

        public string CartId { get; set; }

        public virtual Cart Cart { get; set; }
    }
}