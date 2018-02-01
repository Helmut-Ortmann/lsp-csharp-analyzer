using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using OmniSharp.Extensions.LanguageServer.Client.Utilities;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace OmniSharp.Extensions.LanguageServer.Client.Clients
{
 /// <summary>
    ///     Client for the LSP Command Text Document API.
    /// </summary>
    public class CommandCqueryCallers
    {

        /// <summary>
        ///     Create a new <see cref="TextDocumentClient"/>.
        /// </summary>
        /// <param name="client">
        ///     The language client providing the API.
        /// </param>
        public CommandCqueryCallers(LanguageClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            Client = client;
        }

        /// <summary>
        ///     The language client providing the API.
        /// </summary>
        public LanguageClient Client { get; }
        /// <summary>
        ///     Request Command CQUery Callers for a textDocument position
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
        public async Task<LocationContainer> CqueryCallers(string filePath, int line, int column,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Argument cannot be null, empty, or entirely composed of whitespace: 'filePath'.", nameof(filePath));

            Uri documentUri = DocumentUri.FromFileSystemPath(filePath);


            
            var request = new TextDocumentPositionParams
            {
                TextDocument = new TextDocumentIdentifier
                {
                    Uri = documentUri
                },
                Position = new Position
                {
                    Line = line,
                    Character = column
                }
                
            };
            var command = new Command()
            {
                Name = @"cquery/callers",
                Title = "Callers",
                Arguments = (JArray)JToken.FromObject(request)
            };
            return await Client.SendRequest<LocationContainer>(@"command", command, cancellationToken).ConfigureAwait(false);

        }
      
    }
}




