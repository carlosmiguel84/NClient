﻿using System;
using Microsoft.AspNetCore.Mvc;
using NClient.Core.Attributes;
using NClient.Core.Attributes.Methods;
using NClient.Core.Attributes.Parameters;
using NClient.Core.Exceptions.Factories;

namespace NClient.AspNetProxy.Attributes
{
    public class NClientAttributeMapper : IAttributeMapper
    {
        public Attribute? TryMap(Attribute attribute)
        { 
            return attribute switch
            {
                PathAttribute x => new RouteAttribute(x.Template) { Order = x.Order },

                GetMethodAttribute x => new HttpGetAttribute(x.Template ?? "") { Order = x.Order },
                PostMethodAttribute x => new HttpPostAttribute(x.Template ?? "") { Order = x.Order },
                PutMethodAttribute x => new HttpPutAttribute(x.Template ?? "") { Order = x.Order },
                DeleteMethodAttribute x => new HttpDeleteAttribute(x.Template ?? "") { Order = x.Order },

                RouteParamAttribute => new FromRouteAttribute(),
                QueryParamAttribute => new FromQueryAttribute(),
                BodyParamAttribute => new FromBodyAttribute(),
                HeaderParamAttribute => new FromHeaderAttribute(),

                {} => null,
                _ => throw InnerExceptionFactory.NullArgument(nameof(attribute))
            };
        }
    }
}
