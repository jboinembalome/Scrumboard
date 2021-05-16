﻿using Scrumboard.Domain.Common;
using Scrumboard.Domain.Enums;
using Scrumboard.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Scrumboard.Domain.Entities
{
    public class Label : AuditableEntity, IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CustomColor CustomColor { get; set; }
        public Board Board { get; set; }
        public ICollection<Card> Cards { get; set; }
    }
}
