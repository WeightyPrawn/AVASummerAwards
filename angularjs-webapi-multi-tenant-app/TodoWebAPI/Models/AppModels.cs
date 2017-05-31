
using System;

namespace TodoWebAPI.Models
{
    public class Todo 
     { 
        public int ID { get; set; } 
        public string Description { get; set; }
        public string Owner { get; set; } 
        public Guid TenantId { get; set; }
     } 


}