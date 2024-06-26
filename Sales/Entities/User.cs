﻿using System.ComponentModel;

namespace Sales.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DefaultValue(false)]
        public bool IsEmployee { get; set; }
    }
}
