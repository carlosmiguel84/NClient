﻿using System;
using NClient.Common.Helpers;
using NClient.Standalone.ClientProxy.Building;
using NClient.Standalone.ClientProxy.Building.Context;

// ReSharper disable once CheckNamespace
namespace NClient
{
    /// <summary>
    /// The factory used to create the client with custom providers.
    /// </summary>
    internal class NClientFactory<TRequest, TResponse> : INClientFactory
    {
        private readonly BuilderContext<TRequest, TResponse> _builderContext;
        
        public string Name { get; set; }
        
        public NClientFactory(string name, BuilderContext<TRequest, TResponse> builderContext)
        {
            Name = name;
            _builderContext = builderContext;
        }
        
        public TClient Create<TClient>(Uri baseUri) where TClient : class
        {
            Ensure.IsNotNull(baseUri, nameof(baseUri));
            return new NClientOptionalBuilder<TClient, TRequest, TResponse>(_builderContext.WithBaseUri(baseUri)).Build();
        }
    }
}
