using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace ActorsGallery.Data.Utilities
{
    /// <summary>
    /// The default response body for all requests to this API with Status, Message and Data fields 
    /// </summary>
    public class ResponseBody
    {
        /// <summary>
        /// An one-word indication of whether the operation succeeded (normal) or failed (error)
        /// </summary>
        [Required]
        public string Status { get; set; }


        /// <summary>
        /// This dynamic property, if not null, contains the payload returned for a given request
        /// </summary>
        public dynamic Payload { get; set; }


        /// <summary>
        /// A one-sentence confirmation if the operation succeeded; otherwise a listing of error messages
        /// </summary>
        [Required]
        public string Message { get; set; }


        public override string ToString()
        {
            return JsonSerializer.Serialize(this, null);
        }
    }
}