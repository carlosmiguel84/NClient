﻿using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Execution;
using NClient.Providers.Transport;
using NClient.Standalone.Tests.Clients;
using NClient.Testing.Common.Apis;
using NClient.Testing.Common.Entities;
using NClient.Testing.Common.Helpers;
using NUnit.Framework;

namespace NClient.Tests.ClientTests
{
    [Parallelizable]
    public class HttpNClientTest
    {
        private static readonly Request RequestStub = new(Guid.Empty, new Uri("http://localhost:5000"), RequestType.Get);
        private static readonly Response ResponseStub = new(RequestStub);

        private IReturnClientWithMetadata _returnClient = null!;
        private ReturnApiMockFactory _returnApiMockFactory = null!;

        [SetUp]
        public void Setup()
        {
            _returnApiMockFactory = new ReturnApiMockFactory(port: 5013);
            _returnClient = NClientGallery.Clients
                .GetBasic()
                .For<IReturnClientWithMetadata>(_returnApiMockFactory.ApiUri.ToString())
                .Build();
        }

        [Test]
        public async Task GetAsync_ServiceReturnsEntity_HttpResponseWithValue()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockGetAsyncMethod(id, entity);

            var result = await _returnClient.AsHttp().GetTransportResponse(client => client.GetAsync(id));

            result.Should().BeEquivalentTo(new Response<BasicEntity>(ResponseStub, RequestStub, entity)
            {
                StatusCode = (int)HttpStatusCode.OK,
                Content = new Content(
                    Encoding.UTF8.GetBytes("{\"Id\":1,\"Value\":2}"),
                    new ContentHeaderContainer(new[]
                    {
                        new Header(HttpKnownHeaderNames.ContentType, "application/json"),
                        new Header(HttpKnownHeaderNames.ContentLength, "18")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK",
                IsSuccessful = true
            }, ExcludeInessentialFields);
        }

        [Test]
        public async Task GetAsync_ServiceReturnsEntity_HttpResponseWithValueWithoutError()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockGetAsyncMethod(id, entity);

            var result = await _returnClient.AsHttp()
                .GetTransportResponse<BasicEntity, Error>(client => client.GetAsync(id));

            result.Should().BeEquivalentTo(new ResponseWithError<BasicEntity, Error>(ResponseStub, RequestStub, entity, error: null)
            {
                StatusCode = (int)HttpStatusCode.OK,
                Content = new Content(
                    Encoding.UTF8.GetBytes("{\"Id\":1,\"Value\":2}"),
                    new ContentHeaderContainer(new[]
                    {
                        new Header(HttpKnownHeaderNames.ContentType, "application/json"),
                        new Header(HttpKnownHeaderNames.ContentLength, "18")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK",
                IsSuccessful = true
            }, ExcludeInessentialFields);
        }

        [Test]
        public void Get_ServiceReturnsEntity_HttpResponseWithValue()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockGetMethod(id, entity);

            var result = _returnClient.AsHttp().GetTransportResponse(client => client.Get(id));

            result.Should().BeEquivalentTo(new Response<BasicEntity>(ResponseStub, RequestStub, entity)
            {
                StatusCode = (int)HttpStatusCode.OK,
                Content = new Content(
                    Encoding.UTF8.GetBytes("{\"Id\":1,\"Value\":2}"),
                    new ContentHeaderContainer(new[]
                    {
                        new Header(HttpKnownHeaderNames.ContentType, "application/json"),
                        new Header(HttpKnownHeaderNames.ContentLength, "18")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK",
                IsSuccessful = true
            }, ExcludeInessentialFields);
        }

        [Test]
        public void Get_ServiceReturnsEntity_HttpResponseWithValueWithoutError()
        {
            const int id = 1;
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockGetMethod(id, entity);

            var result = _returnClient.AsHttp().GetTransportResponse<BasicEntity, Error>(client => client.Get(id));

            result.Should().BeEquivalentTo(new ResponseWithError<BasicEntity, Error>(ResponseStub, RequestStub, entity, error: null)
            {
                StatusCode = (int)HttpStatusCode.OK,
                Content = new Content(
                    Encoding.UTF8.GetBytes("{\"Id\":1,\"Value\":2}"),
                    new ContentHeaderContainer(new[]
                    {
                        new Header(HttpKnownHeaderNames.ContentType, "application/json"),
                        new Header(HttpKnownHeaderNames.ContentLength, "18")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK",
                IsSuccessful = true
            }, ExcludeInessentialFields);
        }

        [Test]
        public async Task PostAsync_ServiceReturnsOk_HttpResponseWithoutValue()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockPostAsyncMethod(entity);

            var httpResponse = await _returnClient.AsHttp().GetTransportResponse(client => client.PostAsync(entity));

            httpResponse.Should().BeEquivalentTo(new Response(RequestStub)
            {
                StatusCode = (int)HttpStatusCode.OK,
                Content = new Content(
                    Encoding.UTF8.GetBytes(""),
                    new ContentHeaderContainer(new[]
                    {
                        new Header(HttpKnownHeaderNames.ContentType, "application/json"),
                        new Header(HttpKnownHeaderNames.ContentLength, "0")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK",
                IsSuccessful = true
            }, ExcludeInessentialFields);
        }

        [Test]
        public async Task PostAsync_ServiceReturnsOk_HttpResponseWithoutValueWithoutError()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockPostAsyncMethod(entity);

            var httpResponse = await _returnClient.AsHttp().GetTransportResponse<Error>(client => client.PostAsync(entity));

            httpResponse.Should().BeEquivalentTo(new ResponseWithError<Error>(httpResponse, httpResponse.Request, error: null)
            {
                StatusCode = (int)HttpStatusCode.OK,
                Content = new Content(
                    Encoding.UTF8.GetBytes(""),
                    new ContentHeaderContainer(new[]
                    {
                        new Header(HttpKnownHeaderNames.ContentType, "application/json"),
                        new Header(HttpKnownHeaderNames.ContentLength, "0")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK",
                IsSuccessful = true
            }, ExcludeInessentialFields);
        }

        [Test]
        public void Post_ServiceReturnsOk_HttpResponseWithoutValue()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockPostMethod(entity);

            var httpResponse = _returnClient.AsHttp().GetTransportResponse(client => client.Post(entity));

            httpResponse.Should().BeEquivalentTo(new Response(RequestStub)
            {
                StatusCode = (int)HttpStatusCode.OK,
                Content = new Content(
                    Encoding.UTF8.GetBytes(""),
                    new ContentHeaderContainer(new[]
                    {
                        new Header(HttpKnownHeaderNames.ContentType, "application/json"),
                        new Header(HttpKnownHeaderNames.ContentLength, "0")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK",
                IsSuccessful = true
            }, ExcludeInessentialFields);
        }

        [Test]
        public void Post_ServiceReturnsOk_HttpResponseWithoutValueWithoutError()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockPostMethod(entity);

            var httpResponse = _returnClient.AsHttp().GetTransportResponse<Error>(client => client.Post(entity));

            httpResponse.Should().BeEquivalentTo(new ResponseWithError<Error>(httpResponse, httpResponse.Request, error: null)
            {
                StatusCode = (int)HttpStatusCode.OK,
                Content = new Content(
                    Encoding.UTF8.GetBytes(""),
                    new ContentHeaderContainer(new[]
                    {
                        new Header(HttpKnownHeaderNames.ContentType, "application/json"),
                        new Header(HttpKnownHeaderNames.ContentLength, "0")
                    })),
                ProtocolVersion = new Version("1.1"),
                StatusDescription = "OK",
                IsSuccessful = true
            }, ExcludeInessentialFields);
        }

        [Test]
        public void Post_ServiceReturnsInternalServerError_HttpResponseWithInternalServerError()
        {
            var entity = new BasicEntity { Id = 1, Value = 2 };
            using var api = _returnApiMockFactory.MockInternalServerError();

            var httpResponse = _returnClient.AsHttp().GetTransportResponse(client => client.Post(entity));

            using var assertionScope = new AssertionScope();
            httpResponse.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            httpResponse.IsSuccessful.Should().BeFalse();
            httpResponse.ErrorMessage.Should().NotBeNull();
            httpResponse.ErrorException.Should().NotBeNull();
        }

        private EquivalencyAssertionOptions<ResponseWithError<BasicEntity, Error>> ExcludeInessentialFields(
            EquivalencyAssertionOptions<ResponseWithError<BasicEntity, Error>> opts)
        {
            return opts
                .Excluding(x => x.Request)
                .Excluding(x => x.Headers)
                .Excluding(x => x.ResponseUri);
        }

        private EquivalencyAssertionOptions<ResponseWithError<Error>> ExcludeInessentialFields(
            EquivalencyAssertionOptions<ResponseWithError<Error>> opts)
        {
            return opts
                .Excluding(x => x.Request)
                .Excluding(x => x.Headers)
                .Excluding(x => x.ResponseUri);
        }

        private EquivalencyAssertionOptions<Response<BasicEntity>> ExcludeInessentialFields(
            EquivalencyAssertionOptions<Response<BasicEntity>> opts)
        {
            return opts
                .Excluding(x => x.Request)
                .Excluding(x => x.Headers)
                .Excluding(x => x.ResponseUri);
        }

        private EquivalencyAssertionOptions<Response> ExcludeInessentialFields(
            EquivalencyAssertionOptions<Response> opts)
        {
            return opts
                .Excluding(x => x.Request)
                .Excluding(x => x.Headers)
                .Excluding(x => x.ResponseUri);
        }
    }
}
