﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Providers.Resilience;
using NClient.Providers.Transport;
using NClient.Standalone.ClientProxy.Building.Models;

// ReSharper disable once CheckNamespace
namespace NClient.Resilience
{
    internal class MethodResiliencePolicyProviderAdapter<TRequest, TResponse> : IMethodResiliencePolicyProvider<TRequest, TResponse>
    {
        private readonly IResiliencePolicyProvider<TRequest, TResponse>? _defaultResiliencePolicyProvider;
        private readonly IReadOnlyCollection<ResiliencePolicyPredicatePair<TRequest, TResponse>> _resiliencePolicyPredicates;

        public MethodResiliencePolicyProviderAdapter(
            IResiliencePolicyProvider<TRequest, TResponse> defaultResiliencePolicyProvider,
            IEnumerable<ResiliencePolicyPredicatePair<TRequest, TResponse>>? resiliencePolicyPredicates = null)
        {
            _defaultResiliencePolicyProvider = defaultResiliencePolicyProvider;
            _resiliencePolicyPredicates = resiliencePolicyPredicates?.ToArray() ?? Array.Empty<ResiliencePolicyPredicatePair<TRequest, TResponse>>();
        }

        public IResiliencePolicy<TRequest, TResponse> Create(MethodInfo methodInfo, IRequest transportRequest)
        {
            var resiliencePolicyPredicate = _resiliencePolicyPredicates.FirstOrDefault(x => x.Predicate(methodInfo, transportRequest));
            return resiliencePolicyPredicate?.Provider.Create() ?? _defaultResiliencePolicyProvider!.Create();
        }
    }
}