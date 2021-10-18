﻿using System;
using System.Threading.Tasks;
using NClient.Abstractions.Serialization;

namespace NClient.Abstractions.Results
{
    // TODO: Typed
    public interface IResultBuilder<TResponse>
    {
        bool CanBuild(Type resultType, TResponse response);
        Task<object?> BuildAsync(Type resultType, TResponse response, ISerializer serializer);
    }
}
