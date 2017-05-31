using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TodoWebAPI.Models;

namespace TodoWebAPI
{
    public class TodoDatabase
    {
        private static List<Todo> todoItems = new List<Todo>();
        public static List<Todo> TodoItems
        {
            get { return todoItems;}
        }
    }
}