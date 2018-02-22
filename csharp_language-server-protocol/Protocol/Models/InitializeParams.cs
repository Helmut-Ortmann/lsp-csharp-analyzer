﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Serialization;

namespace OmniSharp.Extensions.LanguageServer.Protocol.Models
{
    public class InitializeParams
    {
        /// <summary>
        /// The process Id of the parent process that started
        /// the server. Is null if the process has not been started by another process.
        /// If the parent process is not alive then the server should exit (see exit notification) its process.
        /// </summary>
        public long? ProcessId { get; set; }

        /// <summary>
        /// The rootPath of the workspace. Is null
        /// if no folder is open.
        ///
        /// @deprecated in favour of rootUri.
        /// </summary>
        [Optional]
        public string RootPath
        {
            get { return RootUri?.AbsolutePath; }
            set { RootUri = value == null ? null : new Uri($"file://{value}"); }
        }

        /// <summary>
        /// The rootUri of the workspace. Is null if no
        /// folder is open. If both `rootPath` and `rootUri` are set
        /// `rootUri` wins.
        /// </summary>
        public Uri RootUri { get; set; }

        /// <summary>
        /// User provided initialization options.
        /// </summary>
        public object InitializationOptions { get; set; }

        /// <summary>
        /// The capabilities provided by the client (editor or tool)
        /// </summary>
        public ClientCapabilities Capabilities { get; set; }

        /// <summary>
        /// The initial trace setting. If omitted trace is disabled ('off').
        /// </summary>
        [Optional]
        public InitializeTrace Trace { get; set; } = InitializeTrace.Off;
    }


    // ReSharper disable InconsistentNaming
    // ReSharper restore InconsistentNaming
}
