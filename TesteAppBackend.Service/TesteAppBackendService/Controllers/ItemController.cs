using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using TesteAppBackendService.DataObjects;
using TesteAppBackendService.Models;

namespace TesteAppBackendService.Controllers
{
    public class ItemController : TableController<Item>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            TesteAppBackendContext context = new TesteAppBackendContext();
            DomainManager = new EntityDomainManager<Item>(context, Request);
        }

        // GET tables/Item
        public IQueryable<Item> GetAllTodoItems()
        {
            return Query();
        }

        // GET tables/Item
        public IQueryable<Item> GetAllItems(string cartId)
        {
            return Query().Where(it => it.CartId == cartId);
        }

        // GET tables/Item/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Item> GetTodoItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Item/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Item> PatchTodoItem(string id, Delta<Item> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Item
        public async Task<IHttpActionResult> PostTodoItem(Item item)
        {
            Item current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Item/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTodoItem(string id)
        {
            return DeleteAsync(id);
        }
    }
}