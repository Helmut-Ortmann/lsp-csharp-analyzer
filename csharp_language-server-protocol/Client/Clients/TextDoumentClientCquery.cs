using System;
using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Client.Utilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace OmniSharp.Extensions.LanguageServer.Client.Clients
{
    /// <summary>
    ///     Client for the LSP Command Text Document API.
    /// </summary>
    public partial class TextDocumentClient
    {

        /// <summary>
        ///     Request Commands CQUery Callers, Vars, .. for a textDocument position
        /// </summary>
        /// <param name="method">
        ///     The CQuery method to call like: '$cquery/callers', '$cquery/vars'
        /// </param>
        /// <param name="filePath">
        ///     The full file-system path of the text document.
        /// </param>
        /// <param name="line">
        ///     The target line (0-based).
        /// </param>
        /// <param name="column">
        ///     The target column (0-based).
        /// </param>
        /// <param name="cancellationToken">
        ///     An optional <see cref="CancellationToken"/> that can be used to cancel the request.
        /// </param>
        /// <returns>
        ///     A <see cref="Task{TResult}"/> that resolves to the hover information or <c>null</c> if no hover information is available at the specified position.
        /// </returns>
        public Task<LocationContainer> Cquery(string method,string filePath, int line, int column, 
            CancellationToken cancellationToken = default(CancellationToken))
        
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Argument cannot be null, empty, or entirely composed of whitespace: 'filePath'.", nameof(filePath));

            Uri documentUri = DocumentUri.FromFileSystemPath(filePath);
            
            return PositionalRequest<LocationContainer>(method, documentUri, line, column, cancellationToken);

        }
      
    }
}




