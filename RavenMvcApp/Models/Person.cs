﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RavenMvcApp.Models
{
    public class Person
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Address Address { get; set; }
    }
}