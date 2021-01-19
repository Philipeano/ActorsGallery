using System;
using System.Collections.Generic;
using System.Text;

namespace ActorsGallery.Data.Utilities
{
    public class Formatter
    {
        public ResponseBody Render(string message, dynamic payload)
        {
            ResponseBody response = new ResponseBody
            {
                Status = "normal",
                Payload = payload,
                Message = message
            };
            return response;
        }


        public ResponseBody Render(string errorMessage)
        {
            ResponseBody response = new ResponseBody
            {
                Status = "error",
                Payload = null,
                Message = errorMessage
            };
            return response;
        }
    }
}
