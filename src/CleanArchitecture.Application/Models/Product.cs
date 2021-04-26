using CleanArchitecture.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Models
{
    public class Product : AuditableEntity
    {
        public Guid ProductID { get; set; }
        public Guid ProductTypeID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
