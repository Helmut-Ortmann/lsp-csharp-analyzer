using System;
using System.Collections.Generic;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace OmniSharp.Extensions.LanguageServer.Server
{
    public interface ILanguageServer : IResponseRouter
    {
        IDisposable AddHandler(string method, IJsonRpcHandler handler);
        IDisposable AddHandler(IJsonRpcHandler handler);
        IDisposable AddHandlers(IEnumerable<IJsonRpcHandler> handlers);
        IDisposable AddHandlers(params IJsonRpcHandler[] handlers);

        InitializeParams Client { get; }
        InitializeResult Server { get; }
    }
}
