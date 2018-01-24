using System;
using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;

namespace OmniSharp.Extensions.LanguageServer.Client.Clients
{
    public partial class WorkspaceClient
    {
        /// <summary>
        ///     Request Symbol information for the workspace.
        /// </summary>
        /// <param name="query">The query string to find symbols</param>
        /// <param name="cancellationToken">
        ///     An optional <see cref="CancellationToken"/> that can be used to cancel the request.
        /// </param>
        /// <returns>
        ///     A <see cref="Task{TResult}"/> that resolves to the symbol information or <c>null</c> if no symbol information is available at the specified position.
        /// </returns>
        public async Task<SymbolInformationContainer> Symbol(string query, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Argument cannot be null, empty, or entirely composed of whitespace: 'query'.", nameof(query));

            var request = new WorkspaceSymbolParams
            {
                Query = query
            };

            return await Client.SendRequest<SymbolInformationContainer>(WorkspaceNames.Symbol, request, cancellationToken).ConfigureAwait(false);

        }
       

    }
}
