using System;
using OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities;

// ReSharper disable CheckNamespace

namespace OmniSharp.Extensions.LanguageServer.Protocol
{
    public interface ITextDocumentSyncHandler : IDidChangeTextDocumentHandler, IDidOpenTextDocumentHandler, IDidCloseTextDocumentHandler, IDidSaveTextDocumentHandler
    {
        TextDocumentSyncOptions Options { get; }
        /// <summary>
        /// Returns the attributes for the document at the given URI.  This can return null.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        TextDocumentAttributes GetTextDocumentAttributes(Uri uri);
    }
}
