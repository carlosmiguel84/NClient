// ReSharper disable once CheckNamespace

namespace NClient
{
    public static class AsBuilderExtensions
    {
        public static INClientAdvancedApiBuilder<TClient> AsAdvanced<TClient>(
            this INClientApiBuilder<TClient> clientApiBuilder)
            where TClient : class
        {
            return (INClientAdvancedApiBuilder<TClient>)clientApiBuilder;
        }
        
        public static INClientApiBuilder<TClient> AsBasic<TClient>(
            this INClientAdvancedApiBuilder<TClient> clientApiBuilder)
            where TClient : class
        {
            return clientApiBuilder;
        }
        
        public static INClientAdvancedTransportBuilder<TClient> AsAdvanced<TClient>(
            this INClientTransportBuilder<TClient> clientApiBuilder)
            where TClient : class
        {
            return (INClientAdvancedTransportBuilder<TClient>)clientApiBuilder;
        }
        
        public static INClientTransportBuilder<TClient> AsBasic<TClient>(
            this INClientAdvancedTransportBuilder<TClient> clientApiBuilder)
            where TClient : class
        {
            return clientApiBuilder;
        }
        
        public static INClientAdvancedSerializerBuilder<TClient, TRequest, TResponse> AsAdvanced<TClient, TRequest, TResponse>(
            this INClientSerializerBuilder<TClient, TRequest, TResponse> clientApiBuilder)
            where TClient : class
        {
            return (INClientAdvancedSerializerBuilder<TClient, TRequest, TResponse>)clientApiBuilder;
        }
        
        public static INClientSerializerBuilder<TClient, TRequest, TResponse> AsBasic<TClient, TRequest, TResponse>(
            this INClientAdvancedSerializerBuilder<TClient, TRequest, TResponse> clientApiBuilder)
            where TClient : class
        {
            return clientApiBuilder;
        }
        
        public static INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> AsAdvanced<TClient, TRequest, TResponse>(
            this INClientOptionalBuilder<TClient, TRequest, TResponse> clientApiBuilder)
            where TClient : class
        {
            return (INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse>)clientApiBuilder;
        }
        
        public static INClientOptionalBuilder<TClient, TRequest, TResponse> AsBasic<TClient, TRequest, TResponse>(
            this INClientAdvancedOptionalBuilder<TClient, TRequest, TResponse> clientApiBuilder)
            where TClient : class
        {
            return clientApiBuilder;
        }
    }
}
