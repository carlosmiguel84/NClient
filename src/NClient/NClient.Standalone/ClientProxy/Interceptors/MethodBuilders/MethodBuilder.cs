﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NClient.Annotations;
using NClient.Core.Helpers;
using NClient.Exceptions;
using NClient.Invocation;
using NClient.Standalone.ClientProxy.Interceptors.MethodBuilders.Providers;

namespace NClient.Standalone.ClientProxy.Interceptors.MethodBuilders
{
    internal interface IMethodBuilder
    {
        Method Build(Type clientType, MethodInfo methodInfo);
    }

    internal class MethodBuilder : IMethodBuilder
    {
        private readonly IOperationAttributeProvider _operationAttributeProvider;
        private readonly IUseVersionAttributeProvider _useVersionAttributeProvider;
        private readonly IPathAttributeProvider _pathAttributeProvider;
        private readonly IHeaderAttributeProvider _headerAttributeProvider;
        private readonly IMethodParamBuilder _methodParamBuilder;

        public MethodBuilder(
            IOperationAttributeProvider operationAttributeProvider,
            IUseVersionAttributeProvider useVersionAttributeProvider,
            IPathAttributeProvider pathAttributeProvider,
            IHeaderAttributeProvider headerAttributeProvider,
            IMethodParamBuilder methodParamBuilder)
        {
            _operationAttributeProvider = operationAttributeProvider;
            _useVersionAttributeProvider = useVersionAttributeProvider;
            _pathAttributeProvider = pathAttributeProvider;
            _headerAttributeProvider = headerAttributeProvider;
            _methodParamBuilder = methodParamBuilder;
        }

        public Method Build(Type clientType, MethodInfo methodInfo)
        {
            var overridingMethods = new List<MethodInfo>();
            var isOverridingMethod = methodInfo.GetCustomAttribute<OverrideAttribute>() is not null;
            if (isOverridingMethod)
            {
                if (clientType.HasMultipleInheritance())
                    throw new ClientValidationException($"The {nameof(OverrideAttribute)} cannot be used if there is multiple inheritance.");

                var overridingMethodInfoEqualityComparer = new OverridingMethodInfoEqualityComparer();
                var allOverridingMethods = clientType.GetInterfaceMethods(inherit: true)
                    .Where(x => overridingMethodInfoEqualityComparer.Equals(x, methodInfo))
                    .Except(new[] { methodInfo })
                    .ToArray();
                overridingMethods.Add(allOverridingMethods.First());
                overridingMethods.AddRange(allOverridingMethods.Skip(1)
                    .TakeWhile(x => x.GetCustomAttribute<OverrideAttribute>() is not null)
                    .ToArray());
            }

            var methodAttribute = _operationAttributeProvider.Get(methodInfo, overridingMethods);
            var methodParams = _methodParamBuilder.Build(methodInfo, overridingMethods);

            return new Method(methodInfo.Name, clientType.Name, methodAttribute, methodParams)
            {
                PathAttribute = _pathAttributeProvider.Find(clientType),
                UseVersionAttribute = _useVersionAttributeProvider.Find(clientType, methodInfo, overridingMethods),
                MetadataAttributes = _headerAttributeProvider.Find(clientType, methodInfo, overridingMethods, methodParams)
            };
        }
    }
}
