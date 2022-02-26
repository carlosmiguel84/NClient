﻿using System;
using System.Net.Http;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using NClient.Common.Helpers;
using NClient.Core.Helpers;

// ReSharper disable once CheckNamespace
namespace NClient.Extensions.DependencyInjection
{
    public static class AddRestNClientExtensions
    {
        internal static IGuidProvider GuidProvider { get; set; } = new GuidProvider();

        /// <summary>Adds a NClient client to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="host">The base address of URI used when sending requests.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IDiNClientBuilder<TClient, HttpRequestMessage, HttpResponseMessage> AddRestNClient<TClient>(
            this IServiceCollection serviceCollection,
            Uri host)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(host, nameof(host));
            
            var internalClientName = GuidProvider.Create().ToString();
            var httpClientBuilder = serviceCollection.AddHttpClient(internalClientName).ConfigureHttpClient(ConfigureDefaultHttpClient);
            serviceCollection.AddSingleton(serviceProvider =>
            {
                return new RestNClientBuilder(internalClientName, serviceProvider)
                    .For<TClient>(host)
                    .Build();
            });
            return new DiNClientBuilder<TClient, HttpRequestMessage, HttpResponseMessage>(httpClientBuilder);
        }
        
        /// <summary>Adds a NClient client to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IDiNClientBuilder<TClient, HttpRequestMessage, HttpResponseMessage> AddRestNClient<TClient>(
            this IServiceCollection serviceCollection,
            Func<IRestNClientBuilder, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            var internalClientName = GuidProvider.Create().ToString();
            var httpClientBuilder = serviceCollection.AddHttpClient(internalClientName).ConfigureHttpClient(ConfigureDefaultHttpClient);
            serviceCollection.AddSingleton(serviceProvider =>
            {
                return implementationFactory(new RestNClientBuilder(internalClientName, serviceProvider));
            });
            return new DiNClientBuilder<TClient, HttpRequestMessage, HttpResponseMessage>(httpClientBuilder);
        }
        
        /// <summary>Adds a NClient client to the DI container.</summary>
        /// <param name="serviceCollection"></param>
        /// <param name="implementationFactory">The action to configure NClient settings.</param>
        /// <typeparam name="TClient">The type of interface used to create the client.</typeparam>
        public static IDiNClientBuilder<TClient, HttpRequestMessage, HttpResponseMessage> AddRestNClient<TClient>(
            this IServiceCollection serviceCollection,
            Func<IServiceProvider, IRestNClientBuilder, TClient> implementationFactory)
            where TClient : class
        {
            Ensure.IsNotNull(serviceCollection, nameof(serviceCollection));
            Ensure.IsNotNull(implementationFactory, nameof(implementationFactory));
            
            var internalClientName = GuidProvider.Create().ToString();
            var httpClientBuilder = serviceCollection.AddHttpClient(internalClientName).ConfigureHttpClient(ConfigureDefaultHttpClient);
            serviceCollection.AddSingleton(serviceProvider =>
            {
                return implementationFactory(serviceProvider, new RestNClientBuilder(internalClientName, serviceProvider));
            });
            return new DiNClientBuilder<TClient, HttpRequestMessage, HttpResponseMessage>(httpClientBuilder);
        }

        private static void ConfigureDefaultHttpClient(HttpClient httpClient)
        {
            httpClient.Timeout = Timeout.InfiniteTimeSpan;
        }
    }
}
