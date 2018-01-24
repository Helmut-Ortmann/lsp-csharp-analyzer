﻿namespace OmniSharp.Extensions.JsonRpc.Server.Messages
{
    public class InternalError : RpcError
    {
        public InternalError() : this(null) { }
        public InternalError(object id) : base(id, new ErrorMessage(-32602, "Internal Error")) { }
    }
}
