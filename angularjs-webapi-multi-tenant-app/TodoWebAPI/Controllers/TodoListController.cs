using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using TodoWebAPI.Models;

namespace TodoWebAPI.Controllers
{

    [Authorize]
    public class TodoListController : ApiController
    {
        //private TodoListServiceContext db = new TodoListServiceContext();
        //private List<Todo> todoItems = null;

        public TodoListController()
        {
            //todoItems = new List<Todo>();
        }

        // GET: api/TodoList
        public IEnumerable<Todo> Get()
        {
            string owner = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            Guid tenantId = new Guid(ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value);

            IEnumerable<Todo> currentUserToDos = TodoDatabase.TodoItems.Where(a => a.Owner == owner && a.TenantId.Equals(tenantId));
            return currentUserToDos;
        }

        // GET: api/TodoList/5
        public Todo Get(int id)
        {
            string owner = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            Todo todo = TodoDatabase.TodoItems.First(a => a.Owner == owner && a.ID == id);             
            return todo;
        }

        // POST: api/TodoList
        public void Post(Todo todo)
        {
            string owner = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            Guid tenantId = new Guid(ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value);

            todo.Owner = owner;
            todo.TenantId = tenantId;

            TodoDatabase.TodoItems.Add(todo);         
        }

        public void Put(Todo todo)
        {
            string owner = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            Todo xtodo = TodoDatabase.TodoItems.First(a => a.Owner == owner && a.ID == todo.ID);
            if (todo != null)
            {
                xtodo.Description = todo.Description;
            }
        }

        // DELETE: api/TodoList/5
        public void Delete(int id)
        {
            string owner = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            Todo todo = TodoDatabase.TodoItems.First(a => a.Owner == owner && a.ID == id);
            if (todo != null)
            {
                TodoDatabase.TodoItems.Remove(todo);
            }
        }        
    }
}
