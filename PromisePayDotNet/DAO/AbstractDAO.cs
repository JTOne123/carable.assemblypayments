﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace PromisePayDotNet.DAO
{
    public abstract class AbstractDAO
    {
        [JsonExtensionData]
        public IDictionary<string, object> AdditionalData { get; set; }
    }
}