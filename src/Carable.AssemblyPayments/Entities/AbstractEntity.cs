﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Carable.AssemblyPayments.Entities
{
    public abstract class AbstractEntity
    {
        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        [JsonProperty(PropertyName = "links")]
        public IDictionary<string, string> Links { get; set; }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the created at time.
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        public DateTime? CreatedAt { get; set; }
        /// <summary>
        /// Gets or sets the updated at time.
        /// </summary>
        [JsonProperty(PropertyName = "updated_at")]
        public DateTime? UpdatedAt { get; set; }

    }
}
