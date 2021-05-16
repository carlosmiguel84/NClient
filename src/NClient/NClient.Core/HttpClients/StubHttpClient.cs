﻿using System;
using System.Net;
using System.Threading.Tasks;
using NClient.Abstractions.HttpClients;

namespace NClient.Core.HttpClients
{
    internal class StubHttpClient : IHttpClient
    {
        public Task<HttpResponse> ExecuteAsync(HttpRequest request, Type? bodyType = null)
        {
            var response = new HttpResponse(request) { StatusCode = HttpStatusCode.OK };
            if (bodyType is null)
                return Task.FromResult(response);

            var genericResponse = typeof(HttpValueResponse<>).MakeGenericType(bodyType);
            return Task.FromResult((HttpResponse)Activator.CreateInstance(genericResponse, response, request, null));
        }
    }
}
