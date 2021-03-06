﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTOs.Visitor
{
    public class InputUpdateVisitor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Cpf { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
