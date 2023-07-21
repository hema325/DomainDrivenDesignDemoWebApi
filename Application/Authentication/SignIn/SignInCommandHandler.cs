﻿using Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authentication.SignIn
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand>
    {
        private readonly IAuthentication _auth;

        public SignInCommandHandler(IAuthentication auth)
        {
            _auth = auth;
        }

        public async Task<Unit> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var result = await _auth.SignInAsync(request.Email, request.Password, request.RememberMe);


            return Unit.Value;
        }
    }
}