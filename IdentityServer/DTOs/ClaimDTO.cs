﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.DTOs
{
    public class ClaimDTO
    {
        public string? ClaimType { get; set; }
        public string? ClaimValue { get; set; }

    }
}
