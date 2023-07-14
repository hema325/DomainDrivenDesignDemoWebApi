﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ICurrentUser
    {
        string? Id { get;}
        string? UserName { get; }
        string? Role { get; }
        bool IsAuthenticated { get; }
    }
}