/*
 * Copyright (C) 2026 Santiago Hernandez M
 *
 * This file is part of Blessings & Curses.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * See the LICENSE file for details.
 */

using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using System;

namespace CL_ProyectoFinalPOO.Aspectos
{
    public class AuthInterceptor : IInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void Intercept(IInvocation invocation)
        {
            var context = _httpContextAccessor.HttpContext;

            if (context == null)
            {
                invocation.Proceed();
                return;
            }

            bool tieneSesion = context.Session.GetString("UsuarioSesion") != null;

            if (tieneSesion)
            {
                invocation.Proceed();
            }
            else
            {
                var returnType = invocation.Method.ReturnType;

                if (returnType == typeof(void))
                {
                    context.Response.Redirect("/Home/Login");
                }
                else if (returnType == typeof(bool))
                {
                    context.Response.Redirect("/Home/Login");
                    invocation.ReturnValue = false;
                }
                else if (returnType.IsClass || returnType.IsInterface)
                {
                    context.Response.Redirect("/Home/Login");
                    invocation.ReturnValue = null;
                }
                else
                {
                    throw new InvalidOperationException("Usuario no autenticado y tipo de retorno no controlado por el interceptor.");
                }
            }
        }
    }
}
