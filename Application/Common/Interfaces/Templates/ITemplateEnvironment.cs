﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Templates
{
    public interface ITemplateEnvironment
    {
        string EmailConfirmedTemplate { get; }
        string EmailConfirmationTemplate { get; }
    }
}
