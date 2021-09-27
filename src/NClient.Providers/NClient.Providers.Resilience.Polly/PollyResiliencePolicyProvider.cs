﻿using NClient.Abstractions.Resilience;
using NClient.Common.Helpers;
using Polly;

namespace NClient.Providers.Resilience.Polly
{
    /// <summary>
    /// The Polly based provider for a component that can create <see cref="IResiliencePolicy"/> instances.
    /// </summary>
    public class PollyResiliencePolicyProvider<TResponse> : IResiliencePolicyProvider<TResponse>
    {
        private readonly IAsyncPolicy<ResponseContext<TResponse>> _asyncPolicy;

        /// <summary>
        /// Creates the Polly based resilience policy provider.
        /// </summary>
        /// <param name="asyncPolicy">The asynchronous policy defining all executions available.</param>
        public PollyResiliencePolicyProvider(
            IAsyncPolicy<ResponseContext<TResponse>> asyncPolicy)
        {
            Ensure.IsNotNull(asyncPolicy, nameof(asyncPolicy));

            _asyncPolicy = asyncPolicy;
        }

        public IResiliencePolicy<TResponse> Create()
        {
            return new PollyResiliencePolicy<TResponse>(_asyncPolicy);
        }
    }
}
