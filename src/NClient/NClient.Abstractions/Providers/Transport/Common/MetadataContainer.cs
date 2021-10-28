﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace NClient.Providers.Transport
{
    public class MetadataContainer : IMetadataContainer
    {
        private readonly Dictionary<string, IEnumerable<IMetadata>> _metadatas;

        public MetadataContainer() : this(Array.Empty<IMetadata>())
        {
        }
        
        public MetadataContainer(IEnumerable<IMetadata> metadatas)
        {
            _metadatas = metadatas
                .GroupBy(x => x.Name)
                .ToDictionary(
                    g => g.Key,
                    g => (IEnumerable<IMetadata>)g.Select(x => x).ToList());
        }

        public IEnumerable<IMetadata> Get(string name)
        {
            return _metadatas[name];
        }

        public void Add(IMetadata metadata)
        {
            _metadatas.TryGetValue(metadata.Name, out var currentMetadata);
            _metadatas[metadata.Name] = currentMetadata?.Append(metadata) ?? new List<IMetadata> { metadata };
        }

        public IEnumerator<KeyValuePair<string, IEnumerable<IMetadata>>> GetEnumerator()
        {
            return _metadatas.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}