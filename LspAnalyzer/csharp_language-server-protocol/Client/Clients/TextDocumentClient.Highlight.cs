using System;
using OmniSharp.Extensions.LanguageServer.Client.Utilities;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System.Threading;
using System.Threading.Tasks;

namespace OmniSharp.Extensions.LanguageServer.Client.Clients
{
     /// <summary>
    ///     Client for the LSP Text Document API.
    /// </summary>
    public partial class TextDocumentClient
    {
        /// <summary>
        ///     Request Highlight LocationInformation for a symbol location information at the specified document position.
        /// </summary>
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
        public async Task<DocumentHighlightContainer> Highlight(string filePath, int line, int column,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Argument cannot be null, empty, or entirely composed of whitespace: 'filePath'.", nameof(filePath));

            Uri documentUri = DocumentUri.FromFileSystemPath(filePath);

            var request = new TextDocumentPositionParams
            {
                TextDocument = new TextDocumentItem
                {
                    LanguageId = "C++",
                    Uri = documentUri
                },
                Position = new Position
                {
                    Line = line,
                    Character = column
                },
                
                
                
            };
            return await Client.SendRequest<DocumentHighlightContainer>(DocumentNames.DocumentHighlight, request, cancellationToken).ConfigureAwait(false);

        }
      
    }
}
